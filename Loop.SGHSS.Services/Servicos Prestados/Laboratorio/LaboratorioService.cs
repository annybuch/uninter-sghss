using Loop.SGHSS.Data;
using Loop.SGHSS.Extensions.Exceptions;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.ServicosPrestados;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Services.Servicos_Prestados.Laboratorio
{
    public class LaboratorioService : ILaboratorioService
    {
        private readonly LoopSGHSSDataContext _dbContext;
        private readonly IMapper _mapper;

        public LaboratorioService(LoopSGHSSDataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Cadastrar um novo serviço de laboratório.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CadastrarServicoLaboratorio(ServicosLaboratorioModel model)
        {
            // --== Validando se já existe um serviço de laboratório com o mesmo título
            bool jaExiste = await _dbContext.ServicosLaboratorios
                .AnyAsync(item => item.Titulo!.ToLower() == model.Titulo!.ToLower());

            if (jaExiste)
                throw new SGHSSBadRequestException("Serviço de laboratório informado já está cadastrado no sistema.");

            // --== Gerando um novo Identificador.
            model.Id = Guid.NewGuid();

            var entidade = _mapper.Map<Domain.Entities.Servicos_Entity.ServicosLaboratorio>(model);

            // --== Salvando entidade.
            _dbContext.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Listar todos os serviços de laboratórios de forma paginada, podendo filtrar também por Instituição.
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResult<ServicosLaboratorioModel>> ObterServicosLaboratoriosPaginados(ServicosLaboratorioQueryFilter filter)
        {
            // --== Iniciando a query.
            var query = _dbContext.ServicosLaboratorios.AsQueryable();

            if (filter.HasFilters)
            {
                // --== Filtro por nome.
                if (!string.IsNullOrEmpty(filter.Search))
                    query = query.Where(x => x.Titulo!.Contains(filter.Search));

                // --== Filtro por Instituição.
                if (filter.InstituicaoId.HasValue)
                {
                    query = query.Where(x =>
                        x.InstituicaoServicosLaboratorio!
                         .Any(il => il.InstituicaoId == filter.InstituicaoId));
                }
            }

            // --== Executando a query e paginação.
            var laboratorios = await query
                .OrderBy(x => x.SysDInsert)
                .Select(x => _mapper.Map<ServicosLaboratorioModel>(x))
                .ToPaged(filter);

            return laboratorios;
        }

        /// <summary>
        /// Buscar serviço laboratório por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServicosLaboratorioModel> BuscarServicoLaboratorioPorId(Guid id)
        {
            var entidade = await _dbContext.ServicosLaboratorios.FindAsync(id)
                ?? throw new SGHSSBadRequestException("Laboratório não encontrado no sistema.");

            return _mapper.Map<ServicosLaboratorioModel>(entidade);
        }

        /// <summary>
        /// Editar informações do serviço de laboratório.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServicosLaboratorioModel> EditarServicoLaboratorio(ServicosLaboratorioModel model)
        {
            // --== Validando laboratório.
            Domain.Entities.Servicos_Entity.ServicosLaboratorio laboratorio = _dbContext.ServicosLaboratorios
                .Where((item) => item.Id == model.Id)
                .FirstOrDefault() ?? throw new SGHSSBadRequestException("Laboratório informado não encontrado");

            // --== Atualizando entidade.
            laboratorio.Atualizar(model);

            // --== Salvando alterações.
            await _dbContext.SaveChangesAsync();

            return model;
        }
    }
}
