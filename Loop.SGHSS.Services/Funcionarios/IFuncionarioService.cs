using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Funcionarios;
using Loop.SGHSS.Model.PassWord;

namespace Loop.SGHSS.Services.Funcionarios
{
    public interface IFuncionarioService
    {
        Task<PassModel> EditarSenhaFuncionario(PassModel model);
        Task<EnderecoModel> EditarFuncionarioEndereco(EnderecoModel model);
        Task<FuncionarioModel> EditarFuncionarioGeral(FuncionarioModel model);
        Task<FuncionarioModel> BuscarFuncionarioPorId(Guid id);
        Task<PagedResult<FuncionarioViewModel>> ObterFuncionariosPaginadas(FuncionarioQueryFilter filter);
        Task CadastrarFuncionario(FuncionarioModel model);
    }
}
