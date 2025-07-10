using Loop.SGHSS.Data;
using Loop.SGHSS.Data.Entities.Suprimento_Entity;
using Loop.SGHSS.Domain.Entities.Suprimento_Entity;
using Loop.SGHSS.Extensions.Exceptions;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Suprimentos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Services.Suprimentos
{
    public class SuprimentoService : ISuprimentoService
    {
        private readonly LoopSGHSSDataContext _dbContext;
        private readonly IMapper _mapper;

        public SuprimentoService(LoopSGHSSDataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #region Categorias

        /// <summary>
        /// Cadastrar uma nova categoria de suprimento.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CadastrarCategoria(CategoriaSuprimentosModel model)
        {
            // --== Validando se já existe uma categoria com o mesmo título .
            bool jaExiste = _dbContext.CategoriasSuprimentos
                .Any(item => item.Titulo!.ToLower() == model.Titulo!.ToLower());

            if (jaExiste)
                throw new SGHSSBadRequestException("Categoria informada já existe no sistema");

            // --== Gerando um novo Identificador.
            model.Id = Guid.NewGuid();

            var entidade = _mapper.Map<CategoriaSuprimento>(model);

            // --== Salvando entidade.
            _dbContext.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Listar todas as caetgorias de suprimentos paginada.
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResult<CategoriaSuprimentosModel>> ObterCategoriasPaginadas(CategoriaQueryFilter filter)
        {
            // --== Iniciando a query.
            var query = _dbContext.CategoriasSuprimentos.Where(x => !x.SysIsDeleted).AsQueryable();

            // --== Executando a query e paginação.
            var categorias = await query
                .OrderBy(x => x.SysDInsert)
                .Select(x => _mapper.Map<CategoriaSuprimentosModel>(x))
                .ToPaged(filter);

            return categorias;
        }

        /// <summary>
        /// Obter todas as categorias com seus respectivos suprimentos e quantidade.
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResult<CategoriaComSuprimentosViewModel>> ObterCategoriasComSuprimentosPaginadas(CategoriaQueryFilter filter)
        {
            // --== Iniciando a query com Include para Suprimentos e suas Compras.
            var query = _dbContext.CategoriasSuprimentos
                .Include(c => c.Suprimentos!)
                    .ThenInclude(s => s.SuprimentosCompras!)
                .AsQueryable();

            // --== Aplicando filtros se houver.
            if (filter.HasFilters && !string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(x =>
                    x.Titulo!.ToLower().Contains(filter.Search.ToLower()) ||
                    (x.Descricao != null && x.Descricao.ToLower().Contains(filter.Search.ToLower()))
                );
            }

            // --== Mapeando os dados.
            var resultadoQuery = query
                .OrderBy(c => c.Titulo)
                .Select(categoria => new CategoriaComSuprimentosViewModel
                {
                    Id = categoria.Id,
                    Titulo = categoria.Titulo,
                    Descricao = categoria.Descricao,
                    QtdSuprimentos = categoria.Suprimentos != null ? categoria.Suprimentos.Count : 0,
                    Suprimentos = categoria.Suprimentos != null
                        ? categoria.Suprimentos.Select(s => new SuprimentoComComprasViewModel
                        {
                            Id = s.Id,
                            Titulo = s.Titulo ?? string.Empty,
                            Descricao = s.Descricao,
                            Compras = s.SuprimentosCompras != null
                                ? s.SuprimentosCompras.Select(sc => new SuprimentosCompraModel
                                {
                                    Id = sc.Id,
                                    Descricao = sc.Descricao,
                                    Codigo = sc.Codigo,
                                    Marca = sc.Marca,
                                    DataComprada = sc.DataComprada,
                                    ValorPago = sc.ValorPago,
                                    QuantidadeComprada = sc.QuantidadeComprada,
                                    QuantidadeSaida = sc.QuantidadeSaida,
                                    SuprimentoId = sc.SuprimentoId
                                }).ToList()
                                : new List<SuprimentosCompraModel>()
                        }).ToList()
                        : new List<SuprimentoComComprasViewModel>()
                });

            // --== Executando paginação.
            var resultado = await resultadoQuery.ToPaged(filter);

            return resultado;
        }

        /// <summary>
        /// Buscar categoria por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CategoriaSuprimentosModel> BuscarCategoriaPorId(Guid id)
        {
            var entidade = await _dbContext.CategoriasSuprimentos.FindAsync(id)
                ?? throw new SGHSSBadRequestException("Categoria não encontrada no sistema.");

            return _mapper.Map<CategoriaSuprimentosModel>(entidade);
        }

        /// <summary>
        /// Editar informações da categoria.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<CategoriaSuprimentosModel> EditarCategoria(CategoriaSuprimentosModel model)
        {
            // --== Validando categoria.
            CategoriaSuprimento categoria = _dbContext.CategoriasSuprimentos
                .Where((item) => item.Id == model.Id)
                .FirstOrDefault() ?? throw new SGHSSBadRequestException("Categoria informada não encontrada");

            // --== Atualizando entidade.
            categoria.Atualizar(model);

            // --== Salvando alterações.
            await _dbContext.SaveChangesAsync();

            return model;
        }

        #endregion


        #region Suprimento

        /// <summary>
        /// Cadastrar um novo suprimento.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CadastrarSuprimento(SuprimentosModel model)
        {
            // --== Gerando um novo Identificador.
            model.Id = Guid.NewGuid();

            var entidade = _mapper.Map<Suprimento>(model);

            // --== Salvando entidade.
            _dbContext.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Listar todos os suprimentos paginados.
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResult<SuprimentosModel>> ObterSuprimentosPaginados(SuprimentoQueryFilter filter)
        {
            // --== Iniciando a query.
            var query = _dbContext.Suprimentos.Where(x => !x.SysIsDeleted).AsQueryable();

            // --== Aplicando filtros se houver.
            if (filter.HasFilters)
            {
                if (!string.IsNullOrEmpty(filter.Search))
                {
                    query = query.Where(x =>
                        x.Titulo!.ToLower().Contains(filter.Search.ToLower()) ||
                        (x.Descricao != null && x.Descricao.ToLower().Contains(filter.Search.ToLower()))
                    );
                }
            }

            // --== Executando a query e paginação.
            var suprimento = await query
                .OrderBy(x => x.SysDInsert)
                .Select(x => _mapper.Map<SuprimentosModel>(x))
                .ToPaged(filter);

            return suprimento;
        }

        /// <summary>
        /// Buscar categoria por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SuprimentosModel> BuscarSuprimentoPorId(Guid id)
        {
            var entidade = await _dbContext.Suprimentos.FindAsync(id)
                ?? throw new SGHSSBadRequestException("Suprimento não encontrado no sistema.");

            return _mapper.Map<SuprimentosModel>(entidade);
        }

        /// <summary>
        /// Editar informações do suprimento.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SuprimentosModel> EditarSuprimento(SuprimentosModel model)
        {
            // --== Validando suprimento.
            Suprimento suprimento = _dbContext.Suprimentos
                .Where((item) => item.Id == model.Id)
                .FirstOrDefault() ?? throw new SGHSSBadRequestException("Suprimento informado não encontrado");

            // --== Atualizando entidade.
            suprimento.Atualizar(model);

            // --== Salvando alterações.
            await _dbContext.SaveChangesAsync();

            return model;
        }

        #endregion


        #region Comprar suprimento

        /// <summary>
        /// Cadastrar a compra de um novo suprimento 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CadastrarCompraSuprimento(SuprimentosCompraModel model)
        {
            // --== Gerando um novo Identificador.
            model.Id = Guid.NewGuid();

            var entidade = _mapper.Map<Suprimento_Compra>(model);

            // --== Salvando entidade.
            _dbContext.Add(entidade);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Listar todas as comprar paginadas.
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResult<SuprimentosCompraModel>> ObterCompraSuprimentosPaginados(SuprimentoQueryFilter filter)
        {
            // --== Iniciando a query.
            var query = _dbContext.Suprimentos.AsQueryable();

            // --== Aplicando filtros se houver.
            if (filter.HasFilters)
            {
                if (!string.IsNullOrEmpty(filter.Search))
                {
                    query = query.Where(x =>
                        x.Titulo!.ToLower().Contains(filter.Search.ToLower()) ||
                        (x.Descricao != null && x.Descricao.ToLower().Contains(filter.Search.ToLower()))
                    );
                }
            }

            // --== Executando a query e paginação.
            var suprimento = await query
                .OrderBy(x => x.SysDInsert)
                .Select(x => _mapper.Map<SuprimentosCompraModel>(x))
                .ToPaged(filter);

            return suprimento;
        }

        /// <summary>
        /// Listar todas as compras paginadas em grade, por categorias e suprimentos.
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoriaComSuprimentosViewModel>> ObterGradeCategoriasSuprimentos()
        {
            var categorias = await _dbContext.CategoriasSuprimentos
                .AsNoTracking()
                .Select(cat => new CategoriaComSuprimentosViewModel
                {
                    Id = cat.Id,
                    Titulo = cat.Titulo,
                    Descricao = cat.Descricao,
                    Suprimentos = _dbContext.Suprimentos
                        .Where(s => s.CategoriaId == cat.Id)
                        .Where(x => !x.SysIsDeleted)
                        .Select(s => new SuprimentoComComprasViewModel
                        {
                            Id = s.Id,
                            Titulo = s.Titulo!,
                            Descricao = s.Descricao,
                            Compras = _dbContext.SuprimentosCompras
                                .Where(c => c.SuprimentoId == s.Id)
                                .Where(x => !x.SysIsDeleted)
                                .Select(c => new SuprimentosCompraModel
                                {
                                    Id = c.Id,
                                    Descricao = c.Descricao,
                                    Codigo = c.Codigo,
                                    Marca = c.Marca,
                                    DataComprada = c.DataComprada,
                                    ValorPago = c.ValorPago,
                                    QuantidadeComprada = c.QuantidadeComprada,
                                    SuprimentoId = c.SuprimentoId
                                }).ToList()
                        }).ToList()
                })
                .ToListAsync();

            return categorias;
        }

        /// <summary>
        /// Buscar categoria por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SuprimentosCompraModel> BuscarCompraSuprimentoPorId(Guid id)
        {
            var entidade = await _dbContext.SuprimentosCompras.FindAsync(id)
                ?? throw new SGHSSBadRequestException("Compra de suprimento não encontrado no sistema.");

            return _mapper.Map<SuprimentosCompraModel>(entidade);
        }

        /// <summary>
        /// Editar informações de compra de um supriemnto.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SuprimentosCompraModel> EditarCompra(SuprimentosCompraModel model)
        {
            // --== Validando suprimento.
            Suprimento_Compra suprimento = _dbContext.SuprimentosCompras
                .Where((item) => item.Id == model.Id)
                .FirstOrDefault() ?? throw new SGHSSBadRequestException("Compra de Suprimento informado não encontrado");

            // --== Atualizando entidade.
            suprimento.Atualizar(model);

            // --== Salvando alterações.
            await _dbContext.SaveChangesAsync();

            return model;
        }

        /// <summary>
        /// Responsável por registras saídas de um suprimento.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="SGHSSBadRequestException"></exception>
        public async Task ConsumirSuprimento(EstoqueModel model)
        {
            if (model.SuprimentoCompraId is null || model.QuantidadeSaida is null)
                throw new SGHSSBadRequestException("Informações inválidas para consumo de suprimento");

            var compra = await _dbContext.SuprimentosCompras
                .FirstOrDefaultAsync(x => x.Id == model.SuprimentoCompraId);

            if (compra == null)
                throw new SGHSSBadRequestException("Compra de suprimento não encontrada");

            int saidaAtual = compra.QuantidadeSaida ?? 0;
            int saidaSolicitada = model.QuantidadeSaida.Value;
            int quantidadeComprada = compra.QuantidadeComprada ?? 0;

            // --== Verifica se a nova saída ultrapassa a quantidade comprada
            if (saidaAtual + saidaSolicitada > quantidadeComprada)
            {
                throw new SGHSSBadRequestException(
                    $"A quantidade solicitada ({saidaSolicitada}) somada à já consumida ({saidaAtual}) " +
                    $"ultrapassa o total comprado ({quantidadeComprada})."
                );
            }

            // --== Atualiza a saída somando com o valor já registrado
            compra.QuantidadeSaida = saidaAtual + saidaSolicitada;

            // --== Atualiza o estoque via método da entidade
            compra.Estoque(new SuprimentosCompraModel
            {
                QuantidadeSaida = compra.QuantidadeSaida
            });

            _dbContext.SuprimentosCompras.Update(compra);
            await _dbContext.SaveChangesAsync();
        }

        #endregion
    }
}
