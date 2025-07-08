using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Adm;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.PassWord;

namespace Loop.SGHSS.Services.Administrador
{
    public interface IAdministradorService
    {
        Task<PassModel> EditarSenhaAdm(PassModel model);
        Task<EnderecoModel> EditarAdmEndereco(EnderecoModel model);
        Task<AdministradorModel> EditarAdmGeral(AdministradorModel model);
        Task<AdministradorModel> BuscarAdmPorId(Guid id);
        Task<PagedResult<AdministradorViewModel>> ObterAdmPaginados(AdministradorQueryFilter filter);
        Task CadastrarAdministrador(AdministradorModel model);
    }
}
