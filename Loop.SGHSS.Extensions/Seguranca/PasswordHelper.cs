using Microsoft.AspNetCore.Identity;

namespace Loop.SGHSS.Extensions.Seguranca
{
    public static class PasswordHelper
    {
        private static readonly PasswordHasher<object> hasher = new();

        // Gera o hash seguro da senha
        public static string GerarHashSenha(string senha)
        {
            return hasher.HashPassword(null, senha);
        }

        // Verifica se a senha informada corresponde ao hash salvo
        public static bool VerificarSenha(string senhaInformada, string senhaHashSalva)
        {
            var result = hasher.VerifyHashedPassword(null, senhaHashSalva, senhaInformada);
            return result == PasswordVerificationResult.Success;
        }
    }
}

