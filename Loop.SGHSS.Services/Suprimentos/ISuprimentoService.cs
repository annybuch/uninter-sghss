using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Suprimentos;

namespace Loop.SGHSS.Services.Suprimentos
{
    public interface ISuprimentoService
    {
        // --== Categorias
        Task<PagedResult<CategoriaComSuprimentosViewModel>> ObterCategoriasComSuprimentosPaginadas(CategoriaQueryFilter filter);
        Task<CategoriaSuprimentosModel> EditarCategoria(CategoriaSuprimentosModel model);
        Task<CategoriaSuprimentosModel> BuscarCategoriaPorId(Guid id);
        Task<PagedResult<CategoriaSuprimentosModel>> ObterCategoriasPaginadas(CategoriaQueryFilter filter);
        Task CadastrarCategoria(CategoriaSuprimentosModel model);

        // --== Supriemntos
        Task<SuprimentosModel> BuscarSuprimentoPorId(Guid id);
        Task<PagedResult<SuprimentosModel>> ObterSuprimentosPaginados(SuprimentoQueryFilter filter);
        Task CadastrarSuprimento(SuprimentosModel model);
        Task<SuprimentosModel> EditarSuprimento(SuprimentosModel model);

        // --== Compras de suprimentos
        Task CadastrarCompraSuprimento(SuprimentosCompraModel model);
        Task<PagedResult<SuprimentosCompraModel>> ObterCompraSuprimentosPaginados(SuprimentoQueryFilter filter);
        Task<List<CategoriaComSuprimentosViewModel>> ObterGradeCategoriasSuprimentos();
        Task<SuprimentosCompraModel> BuscarCompraSuprimentoPorId(Guid id);
        Task<SuprimentosCompraModel> EditarCompra(SuprimentosCompraModel model);
        Task ConsumirSuprimento(EstoqueModel model);
    }
}
