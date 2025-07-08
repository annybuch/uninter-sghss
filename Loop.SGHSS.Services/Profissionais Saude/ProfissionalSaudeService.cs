using Loop.SGHSS.Data;
using Loop.SGHSS.Domain.Entities.Agenda_Entiity;
using Loop.SGHSS.Domain.Entities.ProfessionalSaude_Entity;
using Loop.SGHSS.Domain.Entities.Profissional_Saude_Entity;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Extensions.Seguranca;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Agenda;
using Loop.SGHSS.Model.Consultas;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Exames;
using Loop.SGHSS.Model.PassWord;
using Loop.SGHSS.Model.ProfissionaisSaude;
using Loop.SGHSS.Services.Permissoes;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Services.Profissionais_Saude
{
    public class ProfissionalSaudeService : IProfissionalSaudeService
    {
        private readonly LoopSGHSSDataContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPermissaoService _permissaoService;

        public ProfissionalSaudeService(LoopSGHSSDataContext dbContext, IMapper mapper, IPermissaoService permissaoService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _permissaoService = permissaoService;
        }

        #region CRUD Profissionais de saúde

        /// <summary>
        /// Cadastrar um novo profissional da saúde.
        /// </summary>
        public async Task CadastrarProfissional(ProfissionalSaudeModel model)
        {
            // --== Verifica se já existe um profissional com esse CPF.
            bool jaExiste = await _dbContext.ProfissionaisSaude
                .AnyAsync(x => x.CPF == model.CPF);
            if (jaExiste)
                throw new Exception("Profissional já cadastrado no sistema.");

            // --== Gerar novo ID.
            model.Id = Guid.NewGuid();

            // --== Validar e gerar hash da senha
            if (string.IsNullOrWhiteSpace(model.PasswordHash))
                throw new Exception("Senha é obrigatória para o profissional.");

            model.PasswordHash = PasswordHelper.GerarHashSenha(model.PasswordHash!);

            // --== Mapear e persistir
            var entidade = _mapper.Map<ProfissionalSaude>(model);
            _dbContext.Add(entidade);
            await _dbContext.SaveChangesAsync();

            // --== ATRIBUIR PERMISSÕES PADRÃO AO NOVO PROFISSIONAL
            await _permissaoService.AtribuirPemissaoPadraoProfissionalSaude(entidade.Id);
        }

        /// <summary>
        /// Obter profissionais paginados, com filtro opcional por Instituição, Especializações e gênero.
        /// </summary>
        public async Task<PagedResult<ProfissionalSaudeViewModel>> ObterProfissionaisPaginados(ProfissionaisSaudeQueryFilter filter)
        {
            var query = _dbContext.ProfissionaisSaude.AsQueryable();

            if (filter.HasFilters)
            {
                if (filter.InstituicaoId.HasValue)
                    query = query.Where(p => p.ProfissionalSaudeInstituicoes!.Any(pi => pi.InstituicaoId == filter.InstituicaoId));

                if (filter.InstituicaoId.HasValue)
                    query = query.Where(p => p.ProfissionalSaudeServicosLaboratorio!.Any(pi => pi.ServicosLaboratorioId == filter.ServicoLaboratorioId));

                if (filter.InstituicaoId.HasValue)
                    query = query.Where(p => p.ProfissionalSaudeEspecializacoes!.Any(pi => pi.EspecializacaoId == filter.EspecializacaoId));
            }

            var resultado = await query
                .OrderBy(x => x.SysDInsert)
                .Select(x => _mapper.Map<ProfissionalSaudeViewModel>(x))
                .ToPaged(filter);

            return resultado;
        }

        /// <summary>
        /// Buscar profissional por Id.
        /// </summary>
        public async Task<ProfissionalSaudeModel> BuscarProfissionalPorId(Guid id)
        {
            var entidade = await _dbContext.ProfissionaisSaude.FindAsync(id)
                ?? throw new Exception("Profissional não encontrado.");

            return _mapper.Map<ProfissionalSaudeModel>(entidade);
        }

        /// <summary>
        /// Editar dados gerais do profissional.
        /// </summary>
        public async Task<ProfissionalSaudeModel> EditarProfissionalGeral(ProfissionalSaudeModel model)
        {
            var entidade = _dbContext.ProfissionaisSaude
                .FirstOrDefault(x => x.Id == model.Id)
                ?? throw new Exception("Profissional não encontrado.");

            entidade.AtualizarGeral(model);

            await _dbContext.SaveChangesAsync();

            return model;
        }

        /// <summary>
        /// Editar endereço do profissional.
        /// </summary>
        public async Task<EnderecoModel> EditarProfissionalEndereco(EnderecoModel model)
        {
            var entidade = _dbContext.ProfissionaisSaude
                .FirstOrDefault(x => x.Id == model.Id)
                ?? throw new Exception("Profissional não encontrado.");

            entidade.AtualizarEndereco(model);

            await _dbContext.SaveChangesAsync();

            return model;
        }

        /// <summary>
        /// Editar senha do profissional.
        /// </summary>
        public async Task<PassModel> EditarSenhaProfissional(PassModel model)
        {
            var entidade = _dbContext.ProfissionaisSaude
                .FirstOrDefault(x => x.Id == model.Id)
                ?? throw new Exception("Profissional não encontrado.");

            if (string.IsNullOrWhiteSpace(model.PasswordHash) || model.PasswordHash.Length < 6)
                throw new Exception("A senha deve ter pelo menos 6 caracteres.");

            if (PasswordHelper.VerificarSenha(model.PasswordHash, entidade.PasswordHash))
                throw new Exception("A nova senha não pode ser igual à senha anterior.");

            var novaSenhaHash = PasswordHelper.GerarHashSenha(model.PasswordHash);

            model.PasswordHash = novaSenhaHash;

            entidade.AtualizarSenha(model);

            await _dbContext.SaveChangesAsync();

            return model;
        }
        #endregion


        #region agenda

        /// <summary>
        /// Cadastrar agenda de um profissional de saúde em uma instituição.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task CadastrarAgendaProfissional(ProfissionalSaudeAgendaModel model)
        {
            // --== Verifica se o profissional existe
            var profissionalExiste = await _dbContext.ProfissionaisSaude
                .AnyAsync(x => x.Id == model.ProfissionalSaudeId);
            if (!profissionalExiste)
                throw new Exception("Profissional informado não encontrado.");

            // --== Cadastra a agenda
            var agenda = new ProfissionalSaude_Agenda
            {
                Id = Guid.NewGuid(),
                ProfissionalSaudeId = model.ProfissionalSaudeId,
                InstituicaoId = model.InstituicaoId,
                TipoConsulta = model.TipoConsulta,
                DiaSemana = model.DiaSemana,
                HoraInicio = model.HoraInicio,
                HoraFim = model.HoraFim,
                HoraInicioAlmoco = model.HoraInicioAlmoco,
                HoraFimAlmoco = model.HoraFimAlmoco
            };

            _dbContext.ProfissionalSaudeAgenda.Add(agenda);
            await _dbContext.SaveChangesAsync();
        }

        #endregion


        #region Consultas profissionais - Calendário.

        /// <summary>
        /// Obter todas as consultas de um profissional de saúde com filtros e paginação.
        /// </summary>
        public async Task<PagedResult<ConsultaGradeProfissionalSaudeModel>> ObterConsultasPorProfissional(ConsultaProfissionalQueryFilter filter)
        {
            // --== Query base.
            var query = _dbContext.Consultas
                .Include(x => x.Especializacao)
                .Include(x => x.Paciente)
                .Include(x => x.Instituicao)
                .Where(x => x.ProfissionalSaudeId == filter.ProfissionalSaudeId)
                .AsQueryable();

            // --== Filtro por status (Pendente, EmAtendimento, Realizada, Cancelada).
            if (filter.StatusConsulta.HasValue)
                query = query.Where(x => x.StatusConsulta == filter.StatusConsulta);

            // --== Filtro por tipo de consulta (HomeCare, Presencial, TeleConsulta).
            if (filter.TipoConsulta.HasValue)
                query = query.Where(x => x.TipoConsulta == filter.TipoConsulta);

            // --== Filtro por especialização.
            if (filter.EspecializacaoId.HasValue)
                query = query.Where(x => x.EspecializacaoId == filter.EspecializacaoId);

            // --== Filtro por instituição.
            if (filter.InstituicaoId.HasValue)
                query = query.Where(x => x.InstituicaoId == filter.InstituicaoId);

            // --== Filtro por período (DataMarcada).
            if (filter.DataInicial.HasValue)
                query = query.Where(x => x.DataMarcada >= filter.DataInicial.Value);

            if (filter.DataFinal.HasValue)
                query = query.Where(x => x.DataMarcada <= filter.DataFinal.Value);

            // --== Ordenação por data marcada, mais recentes primeiro.
            query = query.OrderByDescending(x => x.DataMarcada);

            // --== Execução da paginação.
            var resultado = await query
                .Select(x => new ConsultaGradeProfissionalSaudeModel
                {
                    Id = x.Id,
                    DataMarcada = x.DataMarcada,
                    TipoConsulta = x.TipoConsulta,
                    StatusConsulta = x.StatusConsulta,
                    NomeEspecializacao = x.Especializacao!.Titulo,
                    NomePaciente = x.Paciente!.Nome + " " + x.Paciente.Sobrenome,
                    NomeInstituicao = x.Instituicao != null ? x.Instituicao.RazaoSocial : null
                })
                .ToPaged(filter);

            return resultado;
        }


        /// <summary>
        /// Obter todos os exames de um profissional de saúde com filtros e paginação.
        /// </summary>
        public async Task<PagedResult<ExameGradeProfissionalSaudeModel>> ObterExamesPorProfissionalSaude(ExameGradeQueryFilter filter)
        {
            var query = _dbContext.Exames
                .Include(x => x.servicoLaboratorio)
                .Include(x => x.Paciente)
                .Include(x => x.Instituicao)
                .Where(x => x.ProfissionalSaudeId == filter.ProfissionalSaudeId)
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
                .Select(x => new ExameGradeProfissionalSaudeModel
                {
                    Id = x.Id,
                    DataMarcada = x.DataMarcada,
                    StatusExame = x.StatusExame,
                    NomeServicoLaboratorio = x.servicoLaboratorio!.Titulo,
                    NomePaciente = x.Paciente!.Nome + " " + x.Paciente.Sobrenome,
                    NomeInstituicao = x.Instituicao != null ? x.Instituicao.RazaoSocial : null
                })
                .ToPaged(filter);

            return resultado;
        }

        #endregion


        #region Relacionamento Especializacao.

        /// <summary>
        /// Vincular profissional à especialização.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task VincularEspecializacao(Guid profissionalId, Guid especializacaoId)
        {
            ProfissionalSaude profissional = await _dbContext.ProfissionaisSaude
                .FirstOrDefaultAsync(x => x.Id == profissionalId)
                ?? throw new Exception("Profissional informado não encontrado.");
            var relacionamento = await _dbContext.ProfissionaisSaudeEspecializacao
                .FirstOrDefaultAsync(x => x.EspecializacaoId == especializacaoId && x.ProfissionalSaudeId == profissionalId);

            if (relacionamento != null)
                throw new Exception("Este profissional já está associado a esta especialização.");

            var entidade = new ProfissionalSaude_Especializacao
            {
                Id = Guid.NewGuid(),
                ProfissionalSaudeId = profissionalId,
                EspecializacaoId = especializacaoId,
            };

            _dbContext.ProfissionaisSaudeEspecializacao.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Desvincular profissional da especialização.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DesvincularEspecializacao(Guid profissionalId, Guid especializacaoId)
        {
            var entidade = await _dbContext.ProfissionaisSaudeEspecializacao
                .FirstOrDefaultAsync(x => x.EspecializacaoId == especializacaoId && x.ProfissionalSaudeId == profissionalId)
                ?? throw new Exception("Este profissional não está associado a esta especialidade");

            _dbContext.ProfissionaisSaudeEspecializacao.Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }

        #endregion


        #region Relacionamento Serviço laboratório

        /// <summary>
        /// Vincular profissional ao serviço de laboratório.
        /// </summary>
        /// <param name="instituicaoId"></param>
        /// <param name="especializacaoId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task VincularServicoLaboratorio(Guid profissionalId, Guid servicoLaboratorioId)
        {

            ProfissionalSaude profissional = await _dbContext.ProfissionaisSaude
                .FirstOrDefaultAsync(x => x.Id == profissionalId)
                ?? throw new Exception("Profissional informado não encontrado.");
            var relacionamento = await _dbContext.ProfissionaisSaudeServicosLaboratorio
                .FirstOrDefaultAsync(x => x.ServicosLaboratorioId == servicoLaboratorioId && x.ProfissionalSaudeId == profissionalId);

            if (relacionamento != null)
                throw new Exception("Este profissional já está associado a este serviço de laboratório.");

            var entidade = new ProfissionalSaude_ServicoLaboratorio
            {
                Id = Guid.NewGuid(),
                ProfissionalSaudeId = profissionalId,
                ServicosLaboratorioId = servicoLaboratorioId,
            };

            _dbContext.ProfissionaisSaudeServicosLaboratorio.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Desvincular profissional do serviço de laboratório.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DesvincularServicoLaboratorio(Guid profissionalId, Guid servicoLaboratorioId)
        {
            var entidade = await _dbContext.ProfissionaisSaudeServicosLaboratorio
              .FirstOrDefaultAsync(x => x.ServicosLaboratorioId == servicoLaboratorioId && x.ProfissionalSaudeId == profissionalId)
              ?? throw new Exception("Este profissional não está associado a este serviço de laboratório.");

            _dbContext.ProfissionaisSaudeServicosLaboratorio.Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
