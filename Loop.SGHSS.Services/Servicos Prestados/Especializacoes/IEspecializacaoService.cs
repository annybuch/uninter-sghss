using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.ServicosPrestados;

namespace Loop.SGHSS.Services.Servicos_Prestados.Especializacoes
{
    public interface IEspecializacaoService
    {
        Task CadastrarEspecializacao(EspecializacoesModel model);
        Task<PagedResult<EspecializacoesModel>> ObterEspecializacoesPaginadas(EspecializacoesQueryFilter filter);
        Task<EspecializacoesModel> EditarEspecializacao(EspecializacoesModel model);
        Task<EspecializacoesModel> BuscarEspecializacaoPorId(Guid id);
    }
}
