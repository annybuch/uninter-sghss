using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Permissao;

namespace Loop.SGHSS.Services.Permissoes
{
    public interface IPermissaoService
    {
        Task<PermissaoModel?> BuscarPermissaoPorId(Guid id);
        Task<PagedResult<PermissaoModel>> ObterPermissoesPaginadas(PermissaoQueryFilter filter);
        Task CadastrarPermissao(PermissaoModel model);

        Task VincularPermissaoProfissional(Guid permissaoId, Guid profissionalId);
        Task DesvincularPermissaoProfissional(Guid permissaoId, Guid profissionalId);
        Task VincularPermissaoFuncionario(Guid permissaoId, Guid funcionarioId);
        Task DesvincularPermissaoFuncionario(Guid permissaoId, Guid funcionarioId);
        Task VincularPermissaoPaciente(Guid permissaoId, Guid pacienteId);
        Task DesvincularPermissaoPaciente(Guid permissaoId, Guid pacienteId);
        Task VincularPermissaoAdm(Guid permissaoId, Guid admId);
        Task DesvincularPermissaoAdm(Guid permissaoId, Guid admId);

        Task AtribuirPemissaoPadraoPaciente(Guid pacienteId);
        Task AtribuirPemissaoPadraoProfissionalSaude(Guid profissionalSaudeId);
        Task AtribuirPemissaoPadraoFuncionario(Guid funcionarioId);
        Task AtribuirPemissaoPadraoAdministrador(Guid administradorId, bool isGlobalAdmin);
    }
}
