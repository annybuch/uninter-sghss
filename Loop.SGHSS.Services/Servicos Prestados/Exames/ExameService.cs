using Loop.SGHSS.Data;
using Loop.SGHSS.Domain.Entities.Agenda_Entiity;
using Loop.SGHSS.Domain.Entities.Exame_Entity;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Domain.Entities.ProfessionalSaude_Entity;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._Enums.Agenda;
using Loop.SGHSS.Model._Enums.Consulta;
using Loop.SGHSS.Model._Enums.Financas;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Consultas;
using Loop.SGHSS.Model.Exames;
using Loop.SGHSS.Model.Instituicoes;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Services.Servicos_Prestados.Exames
{
    public  class ExameService : IExameService
    {
        private readonly LoopSGHSSDataContext _dbContext;
        private readonly IMapper _mapper;

        public ExameService(LoopSGHSSDataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // -------------------------------------------------------------------------------------
        // 🔍 Buscar instituições que possuem médicos com disponibilidade no laboratório
        // -------------------------------------------------------------------------------------
        public async Task<List<InstituicaoModel>> BuscarInstituicoesComDisponibilidade(Guid servicoLaboratorioId)
        {
            var dataReferencia = DateTime.Now;

            // --== Define o limite de data para a busca de disponibilidade (2 meses a partir da data de referência).
            var dataLimite = dataReferencia.AddMonths(2);

            // --== Busca todas as instituições que estão associadas ao serviço laboratório fornecida.
            var instituicoes = await _dbContext.InstituicoesServicosLaboratorio
                .Where(ie => ie.ServicosLaboratorioId == servicoLaboratorioId)
                .Select(ie => ie.Instituicao)
                .Distinct()
                .ToListAsync();

            // --== Cria uma lista para armazenar as instituições que têm disponibilidade.
            var resultado = new List<InstituicaoModel>();

            // --== Itera sobre cada instituição encontrada.
            foreach (var inst in instituicoes)
            {
                // --== Flag para controlar se foi encontrada disponibilidade para a instituição atual.
                bool instituicaoTemDisponibilidade = false;

                // --== Obtém o contexto completo de agendamento para a instituição, especialização e período definidos.
                // Isso inclui profissionais, suas agendas, agendas da instituição e consultas já agendadas.
                var contexto = await ObterContextoDeAgendamento(servicoLaboratorioId, inst!.Id, dataReferencia, dataLimite);

                // --== Itera sobre cada profissional de saúde associado ao contexto da instituição e especialização.
                foreach (var profissional in contexto.Profissionais)
                {
                    // --== Itera sobre cada dia no intervalo de datas (do dataReferencia ao dataLimite).
                    for (var data = dataReferencia.Date; data <= dataLimite.Date; data = data.AddDays(1))
                    {
                        // --== Chama o método auxiliar ObterHorarios para calcular os horários disponíveis
                        // para o profissional e a instituição na data atual, considerando agendas e consultas existentes.
                        var horarios = ObterHorarios(profissional, contexto.Instituicao, data,
                            contexto.AgendasProfissionais, contexto.AgendasInstituicao, contexto.ConsultasAgendadas);

                        if (horarios.Any())
                        {
                            // --== Adiciona a instituição à lista de resultados.
                            resultado.Add(_mapper.Map<InstituicaoModel>(inst));
                            instituicaoTemDisponibilidade = true;
                            break;
                        }
                    }

                    if (instituicaoTemDisponibilidade)
                        break;
                }
            }

            // --== Retorna a lista de instituições com disponibilidade, removendo duplicatas caso existam.
            return resultado.Distinct().ToList();
        }

        // ------------------------------------------------------------------------------------------------------
        // 👩‍⚕️ Obter profissionais e seus horários disponíveis para uma instituição e serviço de laboratório
        // ------------------------------------------------------------------------------------------------------
        public async Task<List<ProfissionalComHorariosModel>> ObterProfissionaisComHorarios(Guid instituicaoId, Guid servicoLaboratorioId)
        {
            var dataReferencia = DateTime.Now;

            var dataLimite = dataReferencia.AddMonths(2);

            // --== Obtem os profissionais filtrados pelos serviços de laboratório e instituição e também as agendas e consultas agendadas.
            var contexto = await ObterContextoDeAgendamento(
                servicoLaboratorioId, instituicaoId, dataReferencia, dataLimite);

            var profissionaisComHorarios = new List<ProfissionalComHorariosModel>();

            // --== Itera sobre cada profissional que atende na instituição e tem a especialização.
            foreach (var profissional in contexto.Profissionais)
            {
                var horariosPorData = new Dictionary<DateTime, List<TimeSpan>>();

                // --== Itera sobre cada dia no período de 2 meses para encontrar os horários.
                for (var dataAtual = dataReferencia.Date; dataAtual <= dataLimite.Date; dataAtual = dataAtual.AddDays(1))
                {
                    // --== Calcula os horários disponíveis para o profissional naquele dia específico.
                    var horariosDoDia = ObterHorarios(
                        profissional, contexto.Instituicao, dataAtual,
                        contexto.AgendasProfissionais, contexto.AgendasInstituicao, contexto.ConsultasAgendadas);

                    // --== Se houver horários, adiciona à coleção do dia.
                    if (horariosDoDia.Any())
                        horariosPorData.Add(dataAtual, horariosDoDia);
                }

                // --== Adiciona o profissional e seus horários disponiveis.
                if (horariosPorData.Any())
                {
                    profissionaisComHorarios.Add(new ProfissionalComHorariosModel
                    {
                        Id = profissional.Id,
                        Nome = profissional.Nome!,
                        HorariosDisponiveisPorData = horariosPorData
                    });
                }
            }

            return profissionaisComHorarios;
        }

        // -------------------------------------------------------------------------------------
        // 📅 Realizar o agendamento da consulta
        // -------------------------------------------------------------------------------------
        public async Task<ExameModel> MarcarExame(ExameModel model)
        {
            // --== Validações ==--
            if (model.PacienteId == null) throw new Exception("Paciente obrigatório.");
            if (model.ProfissionalSaudeId == null) throw new Exception("Profissional obrigatório.");
            if (model.InstituicaoId == null) throw new Exception("Instituição obrigatória.");
            if (model.servicoLaboratorioId == null) throw new Exception("Servico Laboratório obrigatória.");
            if (model.DataMarcada == default) throw new Exception("Data inválida.");

            // --== Obtém o contexto de agendamento necessário para a validação da disponibilidade do horário.
            var contexto = await ObterContextoDeAgendamento(
                model.servicoLaboratorioId!.Value, model.InstituicaoId!.Value,
                model.DataMarcada.Date, model.DataMarcada.Date);

            // --== Encontra o profissional no contexto.
            var profissional = contexto.Profissionais
                .FirstOrDefault(p => p.Id == model.ProfissionalSaudeId)
                ?? throw new Exception("Profissional não encontrado.");

            // --== Calcula os horários disponíveis para o profissional, instituição e data do exame.
            var horariosDisponiveis = ObterHorarios(
                profissional, contexto.Instituicao, model.DataMarcada,
                contexto.AgendasProfissionais, contexto.AgendasInstituicao, contexto.ConsultasAgendadas);

            // --== Verifica se o horário desejado para o exame está entre os horários disponíveis.
            if (!horariosDisponiveis.Contains(model.DataMarcada.TimeOfDay))
                throw new Exception("O horário não está disponível.");

            // --== Criação e Persistência da Exame ==--

            // --== Gera um novo GUID para a ID da exame.
            model.Id = Guid.NewGuid();

            // --== Define o status inicial do exame como Pendente.
            model.StatusExame = StatusConsultaEnum.Pendente;

            // --== Define o status inicial do pagamento como Pendente.
            model.StatusPagamento = StatusPagamentoEnum.Pendente;

            var entidade = _mapper.Map<Exame>(model);

            await _dbContext.Exames.AddAsync(entidade);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ExameModel>(entidade);
        }

        // =====================================================================================
        // 🔧 Método auxiliar que gera os horários disponíveis (Regras aplicadas)
        // =====================================================================================
        private List<TimeSpan> ObterHorarios(ProfissionalSaude profissional, Instituicao instituicao, DateTime data,
            List<ProfissionalSaude_Agenda> agendasProfissionais, List<Instituicao_Agenda> agendasInstituicao, List<AgendamentoSimples> consultasAgendadas)
        {
            // --== Determina o dia da semana da data fornecida.
            var diaSemana = (DiaSemanaEnum)data.DayOfWeek;

            // --== Busca a agenda específica do profissional para o dia da semana e instituição.
            var agendaProfissional = agendasProfissionais.FirstOrDefault(a =>
                a.ProfissionalSaudeId == profissional.Id &&
                a.InstituicaoId == instituicao.Id &&
                a.DiaSemana == diaSemana);

            if (agendaProfissional == null)
                return new List<TimeSpan>();

            // --== Busca a agenda específica da instituição para o dia da semana.
            var agendaInstituicao = agendasInstituicao.FirstOrDefault(ia =>
                ia.InstituicaoId == instituicao.Id && ia.DiaSemana == diaSemana);

            if (agendaInstituicao == null)
                return new List<TimeSpan>();

            var inicioEfetivo = new[] { agendaProfissional.HoraInicio, agendaInstituicao.HoraInicio }.Max();

            var fimEfetivo = new[] { agendaProfissional.HoraFim, agendaInstituicao.HoraFim }.Min();

            if (inicioEfetivo >= fimEfetivo)
                return new List<TimeSpan>();

            // --== Gera uma lista de todos os horários possíveis dentro do intervalo
            // de 'inicioEfetivo' e 'fimEfetivo', respeitando o intervalo de minutos da instituição
            // e excluindo o horário de almoço do profissional.
            var horariosPossiveis = GerarHorarios(
                inicioEfetivo, fimEfetivo, instituicao.IntervaloMinutos,
                agendaProfissional.HoraInicioAlmoco, agendaProfissional.HoraFimAlmoco);

            // --== Filtra as consultas agendadas para o profissional específico na data atual
            // e extrai apenas a parte da hora de cada agendamento, criando uma lista de horários que já estão ocupados.
            var horariosOcupados = consultasAgendadas
                .Where(c => c.ProfissionalSaudeId == profissional.Id && c.DataMarcada.Date == data.Date)
                .Select(c => c.DataMarcada.TimeOfDay)
                .ToList();

            // --== Calcula os horários disponíveis subtraindo os horários ocupados da lista de horários possíveis.
            // Retorna apenas os horários da grade que NÃO estão na lista de horários ocupados.
            return horariosPossiveis
                .Where(h => !horariosOcupados.Contains(h))
                .ToList();
        }

        // =====================================================================================
        // ⏰ Gera a grade de horários (considerando expediente, almoço e intervalo)
        // =====================================================================================
        private List<TimeSpan> GerarHorarios(TimeSpan horaInicio, TimeSpan horaFim, int? intervaloMinutos, TimeSpan? inicioAlmoco, TimeSpan? fimAlmoco)
        {
            var horarios = new List<TimeSpan>();
            var atual = horaInicio;
            int intervalo = intervaloMinutos ?? 30;

            while (atual < horaFim)
            {
                // --== Verifica se o horário atual está dentro do período de almoço do profissional.
                bool dentroDoAlmoco = inicioAlmoco.HasValue && fimAlmoco.HasValue &&
                     atual >= inicioAlmoco.Value && atual < fimAlmoco.Value;

                // --== Se o horário atual NÃO estiver dentro do período de almoço, adiciona-o à lista de horários.
                if (!dentroDoAlmoco)
                    horarios.Add(atual);

                // --== Avança para o próximo slot de horário, somando o intervalo definido.
                atual = atual.Add(TimeSpan.FromMinutes(intervalo));
            }

            return horarios;
        }

        // =====================================================================================
        // 📦 Monta todo o contexto de agendamento para reduzir duplicações
        // =====================================================================================
        private async Task<ContextoDeAgendamento> ObterContextoDeAgendamento(Guid servicoLaboratorioId, Guid instituicaoId,
            DateTime dataReferencia, DateTime dataLimite)
        {
            // --== Validar Instituição.
            var instituicao = await _dbContext.Instituicoes.FirstOrDefaultAsync(i => i.Id == instituicaoId)
                ?? throw new Exception("Instituição não encontrada.");

            // --== Busca os profissionais de saúde que atendem na instituição e possuem a especialização.
            var profissionais = await _dbContext.ProfissionaisSaude
                .Where(p =>
                    p.ProfissionalSaudeServicosLaboratorio!.Any(e => e.ServicosLaboratorioId == servicoLaboratorioId) &&
                    p.ProfissionalSaudeInstituicoes!.Any(i => i.InstituicaoId == instituicaoId))
                .ToListAsync();

            if (!profissionais.Any())
                throw new Exception("Não há profissionais disponíveis para este serviço de laboratório na instituição selecionada.");

            var profissionalIds = profissionais.Select(p => p.Id).ToList();

            var agendasProfissionais = await _dbContext.ProfissionalSaudeAgenda
                .Where(a => profissionalIds.Contains(a.ProfissionalSaudeId) && a.InstituicaoId == instituicaoId)
                .ToListAsync();

            var diasDaSemana = agendasProfissionais.Select(ap => ap.DiaSemana).Distinct().ToList();

            var agendasInstituicao = await _dbContext.InstituicaoAgenda
                .Where(ia => ia.InstituicaoId == instituicaoId && diasDaSemana.Contains(ia.DiaSemana))
                .ToListAsync();

            var consultasAgendadas = await _dbContext.Consultas
                .Where(c => profissionalIds.Contains(c.ProfissionalSaudeId) &&
                             c.InstituicaoId == instituicaoId &&
                             c.DataMarcada.Date >= dataReferencia.Date &&
                             c.DataMarcada.Date <= dataLimite.Date &&
                             c.StatusConsulta != StatusConsultaEnum.Cancelada)
                .Select(c => new AgendamentoSimples { ProfissionalSaudeId = c.ProfissionalSaudeId!, DataMarcada = c.DataMarcada })
                .ToListAsync();

            return new ContextoDeAgendamento
            {
                Instituicao = instituicao,
                Profissionais = profissionais,
                AgendasInstituicao = agendasInstituicao,
                AgendasProfissionais = agendasProfissionais,
                ConsultasAgendadas = consultasAgendadas
            };
        }


        /// <summary>
        /// Buscar exame por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExameModel?> BuscarExamePorId(Guid id)
        {
            var entidade = await _dbContext.Exames.FindAsync(id);

            if (entidade == null)
                return null;

            return _mapper.Map<ExameModel>(entidade);
        }

        /// <summary>
        /// Responsável por iniciar um exame.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ExameModel> IniciarExame(Guid exameId)
        {
            var exame = await _dbContext.Exames
                .FirstOrDefaultAsync(c => c.Id == exameId)
                ?? throw new Exception("Exame não encontrado.");

            if (exame.StatusExame != StatusConsultaEnum.Pendente)
                throw new Exception("Só é possível iniciar exames pendentes.");

            exame.DataInicio = DateTime.Now;
            exame.StatusExame = StatusConsultaEnum.EmAtendimento;

            _dbContext.Update(exame);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ExameModel>(exame);
        }

        /// <summary>
        /// Responsável por finalizar um exame realizado.
        /// </summary>
        /// <param name="anotacoes"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ExameModel> FinalizarExame(Guid exameId, string? anotacoes = null)
        {

            var exame = await _dbContext.Exames
                .FirstOrDefaultAsync(c => c.Id == exameId)
                ?? throw new Exception("Exame não encontrado.");

            if (exame.StatusExame != StatusConsultaEnum.EmAtendimento)
                throw new Exception("Só é possível finalizar exames que estão em andamento.");

            if (exame.StatusExame == StatusConsultaEnum.EmAtendimento)
            {
                exame.StatusExame = StatusConsultaEnum.Finalizada;
                exame.DataFim = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(anotacoes))
                    exame.Anotacoes = anotacoes;

                await _dbContext.SaveChangesAsync();
            }

            return _mapper.Map<ExameModel>(exame);
        }

        /// <summary>
        /// Responsável por anexar um resultado ao exame.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task AnexarResultado(Guid exameId, byte[] resultado)
        {
            var exame = await _dbContext.Exames
                .FirstOrDefaultAsync(c => c.Id == exameId)
                ?? throw new Exception("Exame não encontrado.");

            if (exame.StatusExame != StatusConsultaEnum.EmAtendimento &&
                exame.StatusExame != StatusConsultaEnum.Finalizada)
                throw new Exception("Só é possível anexar documentos em exames em andamento ou finalizados.");

            exame.Resultado = resultado;

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Responsável por anexar guia médico ao Exame.
        /// </summary>
        /// <param name="prescricao"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task AnexarGuiaMedico(Guid exameId, byte[] guiaMedico)
        {
            var exame = await _dbContext.Exames
                .FirstOrDefaultAsync(c => c.Id == exameId)
                ?? throw new Exception("Exame não encontrada.");

            if (exame.StatusExame != StatusConsultaEnum.EmAtendimento &&
                exame.StatusExame != StatusConsultaEnum.Finalizada)
                throw new Exception("Só é possível anexar documentos em exames em andamento ou finalizados.");

            exame.GuiaMedico = guiaMedico;

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Responsável por cancelar um exame.
        /// </summary>
        /// <param name="exameId">ID do exame a ser cancelado.</param>
        /// <returns>Dados do exame cancelado.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<ExameModel> CancelarExame(Guid exameId)
        {
            var exame = await _dbContext.Exames
                .FirstOrDefaultAsync(c => c.Id == exameId)
                ?? throw new Exception("Exame não encontrado.");

            if (exame.StatusExame == StatusConsultaEnum.Cancelada)
                throw new Exception("O exame já está cancelado.");

            if (exame.StatusExame == StatusConsultaEnum.Finalizada)
                throw new Exception("Não é possivel cancelar um exame já realizado.");

            exame.StatusExame = StatusConsultaEnum.Cancelada;

            _dbContext.Update(exame);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ExameModel>(exame);
        }

        /// <summary>
        /// Obter todos os exames de uma instituição, para ser utilizado no calendário por funcionários ou adms.
        /// </summary>
        public async Task<PagedResult<ExameGradeGeralModel>> ObterExamesGrade(ExameGradeQueryFilter filter)
        {
            var query = _dbContext.Exames
                .Include(x => x.servicoLaboratorio)
                .Include(x => x.ProfissionalSaude)
                .Include(x => x.Paciente)
                .Include(x => x.Instituicao)

                .AsQueryable();

            if (filter.StatusExame.HasValue)
                query = query.Where(x => x.StatusExame == filter.StatusExame);

            if (filter.ServicoLaboratorioId.HasValue)
                query = query.Where(x => x.servicoLaboratorioId == filter.ServicoLaboratorioId);

            if (filter.InstituicaoId.HasValue)
                query = query.Where(x => x.InstituicaoId == filter.InstituicaoId);

            if (filter.DataInicial.HasValue)
                query = query.Where(x => x.DataMarcada >= filter.DataInicial.Value);

            if (filter.DataFinal.HasValue)
                query = query.Where(x => x.DataMarcada <= filter.DataFinal.Value);

            query = query.OrderByDescending(x => x.DataMarcada);

            var resultado = await query
                .Select(x => new ExameGradeGeralModel
                {
                    Id = x.Id,
                    DataMarcada = x.DataMarcada,
                    StatusExame = x.StatusExame,
                    NomeServicoLaboratorio = x.servicoLaboratorio!.Titulo,
                    NomePaciente = x.Paciente!.Nome + " " + x.Paciente.Sobrenome,
                    NomeProfissionalSaude = x.ProfissionalSaude!.Nome + " " + x.ProfissionalSaude.Sobrenome,
                    NomeInstituicao = x.Instituicao != null ? x.Instituicao.RazaoSocial : null
                })
                .ToPaged(filter);

            return resultado;
        }

        /// <summary>
        /// Obter uma listagem de pacientes de um médico ou instituicao específica com seus exames, facilitando para obter seu histórico completo).
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<PacienteComExamesModel>> ObterPacientesComExames(ExameGradeQueryFilter filter)
        {
            var query = _dbContext.Exames
                .Include(x => x.Paciente)
                .Include(x => x.ProfissionalSaude)
                .Include(x => x.servicoLaboratorio)
                .Include(x => x.Instituicao)
                .AsQueryable();

            // --== Filtro por data (opcional)
            if (filter.DataInicial.HasValue)
                query = query.Where(x => x.DataMarcada >= filter.DataInicial.Value);

            if (filter.DataFinal.HasValue)
                query = query.Where(x => x.DataMarcada <= filter.DataFinal.Value);

            // --== Filtro por status
            if (filter.StatusExame.HasValue)
                query = query.Where(x => x.StatusExame == (StatusConsultaEnum)filter.StatusExame);

            // --== Filtro por instituição
            if (filter.InstituicaoId.HasValue)
                query = query.Where(x => x.InstituicaoId == filter.InstituicaoId);

            // --== Filtro por especialização
            if (filter.StatusExame.HasValue)
                query = query.Where(x => x.servicoLaboratorioId == filter.ServicoLaboratorioId);

            // --== Agrupar por paciente e projetar
            var pacientesComExames = await query
                .GroupBy(x => x.Paciente)
                .Select(g => new PacienteComExamesModel
                {
                    PacienteId = g.Key!.Id,
                    NomePaciente = g.Key.Nome + " " + g.Key.Sobrenome,
                    Exames = g
                        .OrderByDescending(c => c.DataMarcada)
                        .Select(c => new ExameGradePacienteModel
                        {
                            Id = c.Id,
                            DataMarcada = c.DataMarcada,
                            StatusExame = c.StatusExame,
                            NomeServicoLaboratorio = c.servicoLaboratorio!.Titulo,
                            NomeProfissionalSaude = c.ProfissionalSaude!.Nome + " " + c.ProfissionalSaude.Sobrenome,
                            NomeInstituicao = c.Instituicao != null ? c.Instituicao.RazaoSocial : null
                        })
                        .ToList()
                })
                .ToListAsync();

            return pacientesComExames;
        }

    }


    // ==========================================================
    // 🔹 Contexto de Agendamento 
    // ==========================================================
    public class ContextoDeAgendamento
    {
        public Instituicao Instituicao { get; set; } = null!;
        public List<Instituicao_Agenda> AgendasInstituicao { get; set; } = new();
        public List<ProfissionalSaude> Profissionais { get; set; } = new();
        public List<ProfissionalSaude_Agenda> AgendasProfissionais { get; set; } = new();
        public List<AgendamentoSimples> ConsultasAgendadas { get; set; } = new();
    }
}
