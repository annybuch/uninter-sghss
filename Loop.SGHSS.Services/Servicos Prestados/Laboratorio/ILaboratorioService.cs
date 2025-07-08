using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.ServicosPrestados;

namespace Loop.SGHSS.Services.Servicos_Prestados.Laboratorio
{
    public interface ILaboratorioService
    {
        Task<ServicosLaboratorioModel> EditarServicoLaboratorio(ServicosLaboratorioModel model);
        Task<ServicosLaboratorioModel> BuscarServicoLaboratorioPorId(Guid id);
        Task<PagedResult<ServicosLaboratorioModel>> ObterServicosLaboratoriosPaginados(ServicosLaboratorioQueryFilter filter);
        Task CadastrarServicoLaboratorio(ServicosLaboratorioModel model);
    }
}
