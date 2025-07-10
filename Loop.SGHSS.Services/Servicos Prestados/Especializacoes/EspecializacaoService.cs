using Loop.SGHSS.Data;
using Loop.SGHSS.Extensions.Exceptions;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.ServicosPrestados;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Services.Servicos_Prestados.Especializacoes
{
    public class EspecializacaoService : IEspecializacaoService
    {
        private readonly LoopSGHSSDataContext _dbContext;
        private readonly IMapper _mapper;

        public EspecializacaoService(LoopSGHSSDataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Cadastrar uma nova especialização.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CadastrarEspecializacao(EspecializacoesModel model)
        {
            // --== Validando se já existe especialização com o mesmo título (case insensitive)
            bool jaExiste = await _dbContext.Especializacoes
                .AnyAsync(item => item.Titulo!.ToLower() == model.Titulo!.ToLower());

            if (jaExiste)
                throw new SGHSSBadRequestException("Especialização informada já está cadastrada no sistema.");

            // --== Gerando um novo Identificador.
            model.Id = Guid.NewGuid();

            // --== Mapeando e salvando entidade.
            var entidade = _mapper.Map<Domain.Entities.Servicos_Entity.Especializacoes>(model);

            _dbContext.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Listar todas as especializações de forma paginada, podendo filtrar também por Instituição.
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResult<EspecializacoesModel>> ObterEspecializacoesPaginadas(EspecializacoesQueryFilter filter)
        {
            // --== Iniciando a query.
            var query = _dbContext.Especializacoes.Where(x => !x.SysIsDeleted).AsQueryable();

            if (filter.HasFilters)
            {
                // --== Filtro por título.
                if (!string.IsNullOrEmpty(filter.Search))
                    query = query.Where(x => x.Titulo!.Contains(filter.Search));

                // --== Filtro por Instituição.
                if (filter.InstituicaoId.HasValue)
                {
                    query = query.Where(x =>
                        x.InstituicaoEspecializacao!
                         .Any(ie => ie.InstituicaoId == filter.InstituicaoId));
                }
            }

            // --== Executando a query e paginação.
            var especializacoes = await query
                .OrderBy(x => x.SysDInsert)
                .Select(x => _mapper.Map<EspecializacoesModel>(x))
                .ToPaged(filter);

            return especializacoes;
        }

        /// <summary>
        /// Buscar especialização por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EspecializacoesModel> BuscarEspecializacaoPorId(Guid id)
        {
            var entidade = await _dbContext.Especializacoes.FindAsync(id)
                ?? throw new SGHSSBadRequestException("Especialização não encontrada no sistema.");

            return _mapper.Map<EspecializacoesModel>(entidade);
        }

        /// <summary>
        /// Editar informações gerais da especialização.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<EspecializacoesModel> EditarEspecializacao(EspecializacoesModel model)
        {
            // --== Validando especialização.
            Domain.Entities.Servicos_Entity.Especializacoes especializacao = _dbContext.Especializacoes.Where((item) => item.Id == model.Id)
                .FirstOrDefault() ?? throw new SGHSSBadRequestException("Especialização informada não encontrada");

            // --== Atualizando entidade.
            especializacao.Atualizar(model);

            // --== Salvando alterações.
            await _dbContext.SaveChangesAsync();

            return model;
        }
    }
}
