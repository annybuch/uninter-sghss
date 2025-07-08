using Loop.SGHSS.Data;
using Loop.SGHSS.Domain.Entities.Agenda_Entiity;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Domain.Entities.Profissional_Saude_Entity;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Agenda;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Instituicoes;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Services.Instituicoes
{
    public class InstituicaoService : IInstituicaoService
    {
        private readonly LoopSGHSSDataContext _dbContext;
        private readonly IMapper _mapper;

        public InstituicaoService(LoopSGHSSDataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        #region CRUD Instituiçções

        /// <summary>
        /// Cadastrar uma nova instituição.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CadastrarInstituicao(InstituicaoModel model)
        {
            // --== Verifica se já existe uma instituição com esse CNPJ
            bool existe = await _dbContext.Instituicoes
                .AnyAsync(item => item.CNPJ == model.CNPJ);

            if (existe)
                throw new Exception("Instituição informada já cadastrada no sistema.");

            // --== Gerando um novo Identificador.
            model.Id = Guid.NewGuid();

            var entidade = _mapper.Map<Instituicao>(model);

            // --== Salvando entidade.
            _dbContext.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Responsável por cadastrar uma agenda para a instituição.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task CadastrarAgendaInstituicao(InstituicaoAgendaModel model)
        {
            // --== Verifica se a instituição existe.
            var instituicaoExiste = await _dbContext.Instituicoes
                .AnyAsync(x => x.Id == model.InstituicaoId);
            if (!instituicaoExiste)
                throw new Exception("Instituição informada não encontrada.");

            // --== Cadastra a agenda
            var agenda = new Instituicao_Agenda
            {
                Id = Guid.NewGuid(),
                InstituicaoId = model.InstituicaoId,
                DiaSemana = model.DiaSemana,
                HoraInicio = model.HoraInicio,
                HoraFim = model.HoraFim
            };

            _dbContext.InstituicaoAgenda.Add(agenda);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Listar todas as instituições paginadas e/ou filtradas por local.
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResult<InstituicaoModel>> ObterInstituicoesPaginadas(InstituicaoQueryFilter filter)
        {
            var query = _dbContext.Instituicoes.AsQueryable();

            if (filter.HasFilters)
            {
                if (filter.Logradouro.HasValue)
                    query = query.Where(x => x.Logradouro == filter.Logradouro);

                if (!string.IsNullOrEmpty(filter.Cidade))
                    query = query.Where(x => x.Cidade == filter.Cidade);
            }

            var instituicoes = await query
                .OrderBy(x => x.SysDInsert)
                .Select(x => _mapper.Map<InstituicaoModel>(x))
                .ToPaged(filter);

            return instituicoes;
        }

        /// <summary>
        /// Buscar instituição por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InstituicaoModel?> BuscarInstituicaoPorId(Guid id)
        {
            var entidade = await _dbContext.Instituicoes.FindAsync(id);

            if (entidade == null)
                return null;

            return _mapper.Map<InstituicaoModel>(entidade);
        }

        /// <summary>
        /// Editar informações gerais da instituição.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<InstituicaoModel> EditarInstituicaoGeral(InstituicaoModel model)
        {
            // --== Obtendo instituição.
            Instituicao instituicao = _dbContext.Instituicoes.Where((item) => item.Id == model.Id)
                .FirstOrDefault() ?? throw new Exception ("Instituição informada não encontrada");

            // --== Atualizando entidade.
            instituicao.AtualizarGeral(model);

            // --== Salvando alterações.
            await _dbContext.SaveChangesAsync();

            return model;
        }

        /// <summary>
        /// Editar apenas o endereço da instituição.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<EnderecoModel> EditarInstituicaoEndereco(EnderecoModel model)
        {
            // --== Obtendo instituição.
            Instituicao instituicao = _dbContext.Instituicoes.Where((item) => item.Id == model.Id)
                .FirstOrDefault() ?? throw new Exception("Instituição informada não encontrada");

            // --== Atualizando entidade.
            instituicao.AtualizarEndereco(model);

            // --== Salvando alterações.
            await _dbContext.SaveChangesAsync();

            return model;
        }

        #endregion


        #region Relacionamento com profissionais.

        /// <summary>
        /// Vincular profissional a uma instituição
        /// </summary>
        /// <param name="instituicaoId"></param>
        /// <param name="especialidadeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task VincularProfissional(Guid instituicaoId, Guid profissionalId)
        {
            Instituicao instituicao = await _dbContext.Instituicoes
                .FirstOrDefaultAsync(x => x.Id == instituicaoId)
                ?? throw new Exception("Instituição informada não encontrada.");

            var relacionamento = await _dbContext.ProfissionaisSaudeInstituicoes
                .FirstOrDefaultAsync(x => x.InstituicaoId == instituicaoId && x.ProfissionalSaudeId == profissionalId);

            if (relacionamento != null)
                throw new Exception("Este profissional já está associado a esta instituição.");

            var entidade = new ProfissionalSaude_Instituicao
            {
                Id = Guid.NewGuid(),
                ProfissionalSaudeId = profissionalId,
                InstituicaoId = instituicaoId,
            };

            _dbContext.ProfissionaisSaudeInstituicoes.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Desvincular um profissional de uma instituição.
        /// </summary>
        /// <param name="instituicaoId"></param>
        /// <param name="profissionalId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DesvincularProfissional(Guid instituicaoId, Guid profissionalId)
        {

            var entidade = await _dbContext.ProfissionaisSaudeInstituicoes
                .FirstOrDefaultAsync(x => x.InstituicaoId == instituicaoId && x.ProfissionalSaudeId == profissionalId)
                ?? throw new Exception("Este profissional não está associado a esta instituição");

            _dbContext.ProfissionaisSaudeInstituicoes.Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }
        #endregion


        #region Relacionamento Especializacao.

        /// <summary>
        /// Vincular Especialização à Instituição
        /// </summary>
        /// <param name="instituicaoId"></param>
        /// <param name="especializacaoId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task VincularEspecializacao(Guid instituicaoId, Guid especializacaoId)
        {
            Instituicao instituicao = await _dbContext.Instituicoes
                .FirstOrDefaultAsync(x => x.Id == instituicaoId)
                ?? throw new Exception("Instituição informada não encontrada.");

            var relacionamento = await _dbContext.InstituicoesEspecializacoes
                .FirstOrDefaultAsync(x => x.InstituicaoId == instituicaoId && x.EspecializacaoId == especializacaoId);

            if (relacionamento != null)
                throw new Exception("Esta especialização já está associado a esta instituição.");

            var entidade = new Instituicao_Especializacao
            {
                Id = Guid.NewGuid(),
                EspecializacaoId = especializacaoId,
                InstituicaoId = instituicaoId,
            };

            _dbContext.InstituicoesEspecializacoes.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Desvincular Especialização da Instituição
        /// </summary>
        /// <param name="instituicaoId"></param>
        /// <param name="especializacaoId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DesvincularEspecializacao(Guid instituicaoId, Guid especializacaoId)
        {

            var entidade = await _dbContext.InstituicoesEspecializacoes
                .FirstOrDefaultAsync(x => x.InstituicaoId == instituicaoId && x.EspecializacaoId == especializacaoId)
                ?? throw new Exception("Essa especialização não está associada a esta instituição");

            _dbContext.InstituicoesEspecializacoes.Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }

        #endregion


        #region Relacionamento Serviço laboratório
        /// <summary>
        /// Vincular serviço de laboratório à Instituição
        /// </summary>
        /// <param name="instituicaoId"></param>
        /// <param name="especializacaoId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task VincularServicoLaboratorio(Guid instituicaoId, Guid servicoLaboratorioId)
        {
            Instituicao instituicao = await _dbContext.Instituicoes
                .FirstOrDefaultAsync(x => x.Id == instituicaoId)
                ?? throw new Exception("Instituição informada não encontrada.");

            var relacionamento = await _dbContext.InstituicoesServicosLaboratorio
                .FirstOrDefaultAsync(x => x.InstituicaoId == instituicaoId && x.ServicosLaboratorioId == servicoLaboratorioId);

            if (relacionamento != null)
                throw new Exception("Esta especialização de laboratório já está associado a esta instituição.");

            var entidade = new Instituicao_ServicosLaboratorio
            {
                Id = Guid.NewGuid(),
                ServicosLaboratorioId = servicoLaboratorioId,
                InstituicaoId = instituicaoId,
            };

            _dbContext.InstituicoesServicosLaboratorio.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Desvincular serviço de laboratório da Instituição.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DesvincularServicoLaboratorio(Guid instituicaoId, Guid servicoLaboratorioId)
        {

            var entidade = await _dbContext.InstituicoesServicosLaboratorio
                .FirstOrDefaultAsync(x => x.InstituicaoId == instituicaoId && x.ServicosLaboratorioId == servicoLaboratorioId)
                ?? throw new Exception("Essa especialização não está associada a esta instituição");

            _dbContext.InstituicoesServicosLaboratorio.Remove(entidade);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

    }
}

