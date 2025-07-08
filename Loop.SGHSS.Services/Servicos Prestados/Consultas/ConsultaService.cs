using Loop.SGHSS.Data;
using Loop.SGHSS.Domain.Entities.Agenda_Entiity;
using Loop.SGHSS.Domain.Entities.Consulta_Entity;
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

namespace Loop.SGHSS.Services.Servicos_Prestados.Consultas
{
    public class ConsultaService : IConsultaService
    {
        private readonly LoopSGHSSDataContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITeleConsultaService _teleConsultaService;

        public ConsultaService(LoopSGHSSDataContext dbContext, IMapper mapper, ITeleConsultaService teleConsultaService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _teleConsultaService = teleConsultaService;
        }


        #region Marcar Consulta
        // ---------------------------------------------------------------------------------------------------------------------
        // 🔍 Buscar instituições que possuem médicos com disponibilidade na especialização, somente em consultas presenciais
        // ---------------------------------------------------------------------------------------------------------------------
        public async Task<List<InstituicaoModel>> BuscarInstituicoesComDisponibilidade(Guid especializacaoId, TipoConsultaEnum tipoConsulta)
        {
            var dataReferencia = DateTime.Now;

            if (tipoConsulta != TipoConsultaEnum.Presencial)
                throw new Exception("Este fluxo é válido apenas para consultas presenciais.");

            var dataLimite = dataReferencia.AddMonths(2);

            var instituicoes = await _dbContext.InstituicoesEspecializacoes
                .Where(ie => ie.EspecializacaoId == especializacaoId)
                .Select(ie => ie.Instituicao)
                .Distinct()
                .ToListAsync();

            var resultado = new List<InstituicaoModel>();

            foreach (var inst in instituicoes)
            {
                var contexto = await ObterContextoDeAgendamento(
                    especializacaoId, dataReferencia, dataLimite, tipoConsulta, inst!.Id);

                foreach (var profissional in contexto.Profissionais)
                {
                    for (var data = dataReferencia.Date; data <= dataLimite.Date; data = data.AddDays(1))
                    {
                        var horarios = ObterHorarios(
                            profissional, data,
                            contexto.AgendasProfissionais, contexto.AgendasInstituicao,
                            contexto.ConsultasAgendadas, tipoConsulta, contexto.Instituicao);

                        if (horarios.Any())
                        {
                            resultado.Add(_mapper.Map<InstituicaoModel>(inst));
                            break;
                        }
                    }
                }
            }

            return resultado.Distinct().ToList();
        }

        // -------------------------------------------------------------------------------------------
        // 👩‍⚕️ Obter profissionais e seus horários disponíveis para uma instituição e especialização
        // -------------------------------------------------------------------------------------------
        public async Task<List<ProfissionalComHorariosModel>> ObterProfissionaisComHorarios(
        Guid especializacaoId, TipoConsultaEnum tipoConsulta, Guid? instituicaoId = null)
        {
            var dataReferencia = DateTime.Now;

            var dataLimite = dataReferencia.AddMonths(2);

            var contexto = await ObterContextoDeAgendamento(
                especializacaoId, dataReferencia, dataLimite, tipoConsulta, instituicaoId);

            var profissionaisComHorarios = new List<ProfissionalComHorariosModel>();

            foreach (var profissional in contexto.Profissionais)
            {
                var horariosPorData = new Dictionary<DateTime, List<TimeSpan>>();

                for (var dataAtual = dataReferencia.Date; dataAtual <= dataLimite.Date; dataAtual = dataAtual.AddDays(1))
                {
                    var horariosDoDia = ObterHorarios(
                        profissional, dataAtual,
                        contexto.AgendasProfissionais, contexto.AgendasInstituicao,
                        contexto.ConsultasAgendadas, tipoConsulta, contexto.Instituicao);

                    if (horariosDoDia.Any())
                        horariosPorData.Add(dataAtual, horariosDoDia);
                }

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
        public async Task<ConsultaModel> MarcarConsulta(MarcarConsultaModel model)
        {
            if (model.PacienteId == null) throw new Exception("Paciente obrigatório.");
            if (model.ProfissionalSaudeId == null) throw new Exception("Profissional obrigatório.");
            if (model.EspecializacaoId == null) throw new Exception("Especialização obrigatória.");
            if (model.DataMarcada == default) throw new Exception("Data inválida.");
            if (model.TipoConsulta == default) throw new Exception("Tipo de consulta obrigatório.");

            if (model.TipoConsulta == TipoConsultaEnum.Presencial && model.InstituicaoId == null)
                throw new Exception("Instituição obrigatória para consultas presenciais.");

            var contexto = await ObterContextoDeAgendamento(
                model.EspecializacaoId!.Value,
                model.DataMarcada.Date,
                model.DataMarcada.Date,
                model.TipoConsulta,
                model.InstituicaoId);

            var profissional = contexto.Profissionais
                .FirstOrDefault(p => p.Id == model.ProfissionalSaudeId)
                ?? throw new Exception("Profissional não encontrado.");

            var horariosDisponiveis = ObterHorarios(
                profissional, model.DataMarcada,
                contexto.AgendasProfissionais, contexto.AgendasInstituicao,
                contexto.ConsultasAgendadas, model.TipoConsulta, contexto.Instituicao);

            if (!horariosDisponiveis.Contains(model.DataMarcada.TimeOfDay))
                throw new Exception("O horário não está disponível.");


            var consulta = _mapper.Map<ConsultaModel>(model);

            consulta.Id = Guid.NewGuid();
            consulta.StatusConsulta = StatusConsultaEnum.Pendente;
            consulta.StatusPagamento = StatusPagamentoEnum.Pendente;

            var entidade = _mapper.Map<Consulta>(model);

            await _dbContext.Consultas.AddAsync(entidade);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ConsultaModel>(entidade);
        }


        // =====================================================================================
        // 🔧 Método auxiliar que gera os horários disponíveis (Regras aplicadas)
        // =====================================================================================
        private List<TimeSpan> ObterHorarios(ProfissionalSaude profissional, DateTime data,
        List<ProfissionalSaude_Agenda> agendasProfissionais, List<Instituicao_Agenda> agendasInstituicao, List<AgendamentoSimples> consultasAgendadas, TipoConsultaEnum tipoConsulta, Instituicao? instituicao = null)
        {
            var diaSemana = (DiaSemanaEnum)data.DayOfWeek;

            var agendaProfissional = agendasProfissionais.FirstOrDefault(a =>
                a.ProfissionalSaudeId == profissional.Id &&
                a.DiaSemana == diaSemana);

            if (agendaProfissional == null)
                return new List<TimeSpan>();

            TimeSpan inicioEfetivo = agendaProfissional.HoraInicio;
            TimeSpan fimEfetivo = agendaProfissional.HoraFim;

            // 📌 Se for presencial, considera também a agenda da instituição
            if (tipoConsulta == TipoConsultaEnum.Presencial)
            {
                var agendaInstituicao = agendasInstituicao.FirstOrDefault(ia =>
                    ia.InstituicaoId == instituicao!.Id && ia.DiaSemana == diaSemana);

                if (agendaInstituicao == null)
                    return new List<TimeSpan>();

                inicioEfetivo = new[] { inicioEfetivo, agendaInstituicao.HoraInicio }.Max();
                fimEfetivo = new[] { fimEfetivo, agendaInstituicao.HoraFim }.Min();

                if (inicioEfetivo >= fimEfetivo)
                    return new List<TimeSpan>();
            }

            var horariosPossiveis = GerarHorarios(
                inicioEfetivo, fimEfetivo, instituicao?.IntervaloMinutos ?? 30,
                agendaProfissional.HoraInicioAlmoco, agendaProfissional.HoraFimAlmoco);

            var horariosOcupados = consultasAgendadas
                .Where(c => c.ProfissionalSaudeId == profissional.Id && c.DataMarcada.Date == data.Date)
                .Select(c => c.DataMarcada.TimeOfDay)
                .ToList();

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
        private async Task<ContextoDeAgendamento> ObterContextoDeAgendamento(Guid especializacaoId,
        DateTime dataReferencia, DateTime dataLimite, TipoConsultaEnum tipoConsulta, Guid? instituicaoId = null)
        {
            // 🔍 Validação de instituição apenas para presencial
            Instituicao? instituicao = null;

            if (tipoConsulta == TipoConsultaEnum.Presencial)
            {
                if (instituicaoId == null)
                    throw new Exception("Instituição obrigatória para consultas presenciais.");

                instituicao = await _dbContext.Instituicoes
                    .FirstOrDefaultAsync(i => i.Id == instituicaoId)
                    ?? throw new Exception("Instituição não encontrada.");
            }

            // 🔍 Busca profissionais que possuem a especialização e agenda do tipo correto
            var profissionais = await _dbContext.ProfissionaisSaude
                .Where(p =>
                    p.ProfissionalSaudeEspecializacoes!.Any(e => e.EspecializacaoId == especializacaoId) &&
                    (
                        tipoConsulta != TipoConsultaEnum.Presencial
                        || p.ProfissionalSaudeInstituicoes!.Any(i => i.InstituicaoId == instituicaoId)
                    ) &&
                    p.ProfissionalSaudeAgenda!.Any(a => a.TipoConsulta == tipoConsulta)
                )
                .ToListAsync();

            if (!profissionais.Any())
                throw new Exception("Nenhum profissional disponível para este tipo de consulta.");

            var profissionalIds = profissionais.Select(p => p.Id).ToList();

            var agendasProfissionais = await _dbContext.ProfissionalSaudeAgenda
                .Where(a => profissionalIds.Contains(a.ProfissionalSaudeId) && a.TipoConsulta == tipoConsulta)
                .ToListAsync();

            // 🔍 Para presencial, carrega agenda da instituição
            var diasDaSemana = agendasProfissionais.Select(ap => ap.DiaSemana).Distinct().ToList();

            var agendasInstituicao = tipoConsulta == TipoConsultaEnum.Presencial
                ? await _dbContext.InstituicaoAgenda
                    .Where(ia => ia.InstituicaoId == instituicaoId && diasDaSemana.Contains(ia.DiaSemana))
                    .ToListAsync()
                : new List<Instituicao_Agenda>();

            // 🔍 Busca consultas já agendadas
            var consultasAgendadas = await _dbContext.Consultas
                .Where(c =>
                    profissionalIds.Contains(c.ProfissionalSaudeId) &&
                    (tipoConsulta != TipoConsultaEnum.Presencial || c.InstituicaoId == instituicaoId) &&
                    c.DataMarcada.Date >= dataReferencia.Date &&
                    c.DataMarcada.Date <= dataLimite.Date &&
                    c.TipoConsulta == tipoConsulta &&
                    c.StatusConsulta != StatusConsultaEnum.Cancelada
                )
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

        #endregion


        #region Atendimento da consulta

        /// <summary>
        /// Buscar consulta por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ConsultaModel?> BuscarConsultaPorId(Guid id)
        {
            var entidade = await _dbContext.Consultas.FindAsync(id);

            if (entidade == null)
                return null;

            return _mapper.Map<ConsultaModel>(entidade);
        }

        /// <summary>
        /// Responsável por iniciar uma consulta.
        /// </summary>
        /// <param name="consultaId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ConsultaModel> IniciarConsulta(Guid consultaId)
        {
            var consulta = await _dbContext.Consultas
                .FirstOrDefaultAsync(c => c.Id == consultaId)
                ?? throw new Exception("Consulta não encontrada.");

            if (consulta.StatusConsulta != StatusConsultaEnum.Pendente)
                throw new Exception("Só é possível iniciar consultas pendentes.");

            consulta.DataInicio = DateTime.Now;
            consulta.StatusConsulta = StatusConsultaEnum.EmAtendimento;

            if (consulta.TipoConsulta == TipoConsultaEnum.TeleConsulta)
            {
                var nomeSala = $"consulta-{consulta.Id}";

                // --== Cria sala no Daily
                var urlSala = await _teleConsultaService.CriarSala(nomeSala);

                // --==  Gera token JWT
                var token = _teleConsultaService.GerarTokenAcesso(nomeSala, "Profissional", "owner");

                consulta.UrlSalaVideo = $"{urlSala}?t={token}";
            }

            _dbContext.Update(consulta);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ConsultaModel>(consulta);
        }

        /// <summary>
        /// Responsável por gerar o link de acesso do paciete.
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="nomePaciente"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GerarLinkDeAcessoPaciente(Guid consultaId, Guid pacienteId)
        {
            var consulta = await _dbContext.Consultas
                .FirstOrDefaultAsync(c => c.Id == consultaId)
                ?? throw new Exception("Consulta não encontrada.");

            var paciente = await _dbContext.Pacientes
                .FirstOrDefaultAsync(c => c.Id == pacienteId)
                ?? throw new Exception("Paciente não encontrado.");

            if (consulta.TipoConsulta != TipoConsultaEnum.TeleConsulta)
                throw new Exception("Consulta não é do tipo teleconsulta.");

            var nomeSala = $"consulta-{consulta.Id}";

            var token = _teleConsultaService.GerarTokenAcesso(nomeSala, paciente.Nome!, "user");

            return $"{consulta.UrlSalaVideo!.Split("?")[0]}?t={token}";
        }

        /// <summary>
        /// Responsável por finalizar uma consulta realizada.
        /// </summary>
        /// <param name="consultaId"></param>
        /// <param name="anotacoes"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ConsultaModel> FinalizarConsulta(Guid consultaId, string? anotacoes = null)
        {

            var consulta = await _dbContext.Consultas
                .FirstOrDefaultAsync(c => c.Id == consultaId)
                ?? throw new Exception("Consulta não encontrada.");

            if (consulta.TipoConsulta == TipoConsultaEnum.TeleConsulta)
            {
                var nomeSala = $"consulta-{consulta.Id}";
                await _teleConsultaService.EncerrarSala(nomeSala);
            }

            if (consulta.StatusConsulta != StatusConsultaEnum.EmAtendimento)
                throw new Exception("Só é possível finalizar consultas que estão em andamento.");

            if (consulta.StatusConsulta == StatusConsultaEnum.EmAtendimento)
            {
                consulta.StatusConsulta = StatusConsultaEnum.Finalizada;
                consulta.DataFim = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(anotacoes))
                    consulta.Anotacoes = anotacoes;

                await _dbContext.SaveChangesAsync();
            }

            return _mapper.Map<ConsultaModel>(consulta);
        }

        /// <summary>
        /// Responsável por anexar uma receita a consulta.
        /// </summary>
        /// <param name="consultaId"></param>
        /// <param name="receita"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task AnexarReceita(Guid consultaId, byte[] receita)
        {
            var consulta = await _dbContext.Consultas
                .FirstOrDefaultAsync(c => c.Id == consultaId)
                ?? throw new Exception("Consulta não encontrada.");

            if (consulta.StatusConsulta != StatusConsultaEnum.EmAtendimento &&
                consulta.StatusConsulta != StatusConsultaEnum.Finalizada)
                throw new Exception("Só é possível anexar documentos em consultas em andamento ou finalizadas.");

            consulta.Receita = receita;

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Responsável por anexar uma prescrição médica a uma consulta.
        /// </summary>
        /// <param name="consultaId"></param>
        /// <param name="prescricao"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task AnexarPrescricao(Guid consultaId, byte[] prescricao)
        {
            var consulta = await _dbContext.Consultas
                .FirstOrDefaultAsync(c => c.Id == consultaId)
                ?? throw new Exception("Consulta não encontrada.");

            if (consulta.StatusConsulta != StatusConsultaEnum.EmAtendimento &&
                consulta.StatusConsulta != StatusConsultaEnum.Finalizada)
                throw new Exception("Só é possível anexar documentos em consultas em andamento ou finalizadas.");

            consulta.Prescricao = prescricao;

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Responsável por anexar guia médico a uma consulta.
        /// </summary>
        /// <param name="consultaId"></param>
        /// <param name="prescricao"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task AnexarGuiaMedico(Guid consultaId, byte[] guiaMedico)
        {
            var consulta = await _dbContext.Consultas
                .FirstOrDefaultAsync(c => c.Id == consultaId)
                ?? throw new Exception("Consulta não encontrada.");

            if (consulta.StatusConsulta != StatusConsultaEnum.EmAtendimento &&
                consulta.StatusConsulta != StatusConsultaEnum.Finalizada)
                throw new Exception("Só é possível anexar documentos em consultas em andamento ou finalizadas.");

            consulta.GuiaMedico = guiaMedico;

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Responsável por cancelar uma consulta.
        /// </summary>
        /// <param name="consultaId">ID da consulta a ser cancelado.</param>
        /// <returns>Dados da consulta cancelada.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<ConsultaModel> CancelarConsulta(Guid consultaId)
        {
            var consulta = await _dbContext.Consultas
                .FirstOrDefaultAsync(c => c.Id == consultaId)
                ?? throw new Exception("Consulta não encontrada.");

            if (consulta.StatusConsulta == StatusConsultaEnum.Cancelada)
                throw new Exception("A consulta já está cancelada.");

            if (consulta.StatusConsulta == StatusConsultaEnum.Finalizada)
                throw new Exception("Não é possivel cancelar uma consulta já realizada.");

            consulta.StatusConsulta = StatusConsultaEnum.Cancelada;

            _dbContext.Update(consulta);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ConsultaModel>(consulta);
        }

        /// <summary>
        /// Obter todas as consultas de uma instituição, para ser utilizado por funcionários ou adms.
        /// </summary>
        public async Task<PagedResult<ConsultaGradeGeralModel>> ObterConsultasPorPacienteEProfissional(ConsultaPacienteQueryFilter filter)
        {
            var query = _dbContext.Consultas
                .Include(x => x.Especializacao)
                .Include(x => x.ProfissionalSaude)
                .Include(x => x.Paciente)
                .Include(x => x.Instituicao)

                .AsQueryable();

            if (filter.StatusConsulta.HasValue)
                query = query.Where(x => x.StatusConsulta == filter.StatusConsulta);

            if (filter.TipoConsulta.HasValue)
                query = query.Where(x => x.TipoConsulta == filter.TipoConsulta);

            if (filter.EspecializacaoId.HasValue)
                query = query.Where(x => x.EspecializacaoId == filter.EspecializacaoId);

            if (filter.InstituicaoId.HasValue)
                query = query.Where(x => x.InstituicaoId == filter.InstituicaoId);

            if (filter.DataInicial.HasValue)
                query = query.Where(x => x.DataMarcada >= filter.DataInicial.Value);

            if (filter.DataFinal.HasValue)
                query = query.Where(x => x.DataMarcada <= filter.DataFinal.Value);

            query = query.OrderByDescending(x => x.DataMarcada);

            var resultado = await query
                .Select(x => new ConsultaGradeGeralModel
                {
                    Id = x.Id,
                    DataMarcada = x.DataMarcada,
                    TipoConsulta = x.TipoConsulta,
                    StatusConsulta = x.StatusConsulta,
                    NomeEspecializacao = x.Especializacao!.Titulo,
                    NomePaciente = x.Paciente!.Nome + " " + x.Paciente.Sobrenome,
                    NomeProfissionalSaude = x.ProfissionalSaude!.Nome + " " + x.ProfissionalSaude.Sobrenome,
                    NomeInstituicao = x.Instituicao != null ? x.Instituicao.RazaoSocial : null
                })
                .ToPaged(filter);

            return resultado;
        }

        /// <summary>
        /// Obter uma listagem de pacientes de um médico ou instituicao específica com suas consultas, facilitando para obter seu histórico completo).
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<PacienteComConsultasModel>> ObterPacientesComConsultas(ConsultaPacienteQueryFilter filter)
        {
            var query = _dbContext.Consultas
                .Include(x => x.Paciente)
                .Include(x => x.ProfissionalSaude)
                .Include(x => x.Especializacao)
                .Include(x => x.Instituicao)
                .AsQueryable();

            // --== Filtro por data (opcional)
            if (filter.DataInicial.HasValue)
                query = query.Where(x => x.DataMarcada >= filter.DataInicial.Value);

            if (filter.DataFinal.HasValue)
                query = query.Where(x => x.DataMarcada <= filter.DataFinal.Value);

            // --== Filtro por status
            if (filter.StatusConsulta.HasValue)
                query = query.Where(x => x.StatusConsulta == (StatusConsultaEnum)filter.StatusConsulta);

            // --== Filtro por instituição
            if (filter.InstituicaoId.HasValue)
                query = query.Where(x => x.InstituicaoId == filter.InstituicaoId);

            // --== Filtro por especialização
            if (filter.StatusConsulta.HasValue)
                query = query.Where(x => x.EspecializacaoId == filter.EspecializacaoId);

            // --== Agrupar por paciente e projetar
            var pacientesComConsultas = await query
                .GroupBy(x => x.Paciente)
                .Select(g => new PacienteComConsultasModel
                {
                    PacienteId = g.Key!.Id,
                    NomePaciente = g.Key.Nome + " " + g.Key.Sobrenome,
                    Consultas = g
                        .OrderByDescending(c => c.DataMarcada)
                        .Select(c => new ConsultaGradePacienteModel
                        {
                            Id = c.Id,
                            DataMarcada = c.DataMarcada,
                            TipoConsulta = c.TipoConsulta,
                            StatusConsulta = c.StatusConsulta,
                            NomeEspecializacao = c.Especializacao!.Titulo,
                            NomeProfissionalSaude = c.ProfissionalSaude!.Nome + " " + c.ProfissionalSaude.Sobrenome,
                            NomeInstituicao = c.Instituicao != null ? c.Instituicao.RazaoSocial : null
                        })
                        .ToList()
                })
                .ToListAsync();

            return pacientesComConsultas;
        }
        #endregion
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