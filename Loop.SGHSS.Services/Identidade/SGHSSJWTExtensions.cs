using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class SGHSSJWTExtensions
{
    
    /// <summary>
    /// Converte uma string secreta em credenciais de segurança simétricas para a assinatura JWT.
    /// </summary>
    public static SigningCredentials ToSymmetricSecurity(this string chaveSecreta)
    {
        return new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(chaveSecreta)), SecurityAlgorithms.HmacSha256Signature);
    }
}