using Loop.SGHSS.API.Default;
using Loop.SGHSS.Model.Identidade;
using Loop.SGHSS.Services.Identidade;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._00___Identidade
{
    [ApiController, Route("api/v1/identidade"),
    Tags("00. 🔐 Identidade, Login e autenticação")]
    public class IdentidadeController : LoopContoller
    {
        private readonly IIdentidadeService _identidadeService;

        public IdentidadeController(IIdentidadeService identidadeService)
        {
            _identidadeService = identidadeService;
        }

        /// <summary>
        /// Realiza o login de um usuário e gera um token de autenticação JWT.
        /// </summary>
        /// <param name="credentials">As credenciais do usuário (Identifier pode ser Email ou CPF, e Password).</param>
        /// <returns>Um token JWT e informações básicas do usuário se a autenticação for bem-sucedida.</returns>
        /// <response code="200">Retorna o token de autenticação e dados do usuário.</response>
        /// <response code="401">Se as credenciais forem inválidas.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(SGHSSAuthenticationTokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] SGHSSCredentialsModel credentials)
        {
            // --== Tenta autenticar o usuário e gerar o token usando o serviço de identidade
            var authResult = await _identidadeService.AuthenticateAndGenerateTokenAsync(credentials);

            if (authResult == null)
                return Unauthorized(new { Message = "Credenciais inválidas. Verifique seu identificador e senha." });

            return Ok(authResult);
        }
    }
}
