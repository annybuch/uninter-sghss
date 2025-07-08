using Loop.SGHSS.Model.Identidade;

namespace Loop.SGHSS.Services.Identidade
{
    public interface IIdentidadeService
    {
        Task<SGHSSAuthenticationTokenModel?> AuthenticateAndGenerateTokenAsync(SGHSSCredentialsModel credentials);
    }
}