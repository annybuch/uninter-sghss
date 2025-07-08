using Loop.SGHSS.API.Default;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Permissao;
using Loop.SGHSS.Services.Permissoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._05___Permissoes
{
    [ApiController, Route("api/v1/permissoes"), Authorize,
    Tags("05. 🔒 Permissões, gestão de segurança")]
    public class PermissoesController : LoopContoller
    {
        private readonly IPermissaoService _permissaoService;

        public PermissoesController(IPermissaoService permissaoService)
        {
            _permissaoService = permissaoService;
        }

        /// <summary>
        /// Buscar permissão por ID.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Policy = "A05")]
        [ProducesResponseType(typeof(PermissaoModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _permissaoService.BuscarPermissaoPorId(id));
        }

        /// <summary>
        /// Listar permissões paginadas.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "A05")]
        [ProducesResponseType(typeof(PagedResult<PermissaoModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PermissaoQueryFilter filter)
        {
            return Ok(await _permissaoService.ObterPermissoesPaginadas(filter));
        }

        /// <summary>
        /// Cadastrar nova permissão.
        /// </summary>
        [HttpPost("Cadastrar")]
        [Authorize(Policy = "A05")]
        public async Task<IActionResult> Post([FromBody] PermissaoModel model)
        {
            return await Created(_permissaoService.CadastrarPermissao(model));
        }

        /// <summary>
        /// Vincular permissão a um profissional.
        /// </summary>
        [HttpPost("{permissaoId}/vincular-profissional/{profissionalId}")]
        [Authorize(Policy = "A05")]
        public async Task<IActionResult> VincularProfissional(Guid permissaoId, Guid profissionalId)
        {
            return await Ok(_permissaoService.VincularPermissaoProfissional(permissaoId, profissionalId));
        }

        /// <summary>
        /// Desvincular permissão de um profissional.
        /// </summary>
        [HttpDelete("{permissaoId}/desvincular-profissional/{profissionalId}")]
        [Authorize(Policy = "A05")]
        public async Task<IActionResult> DesvincularProfissional(Guid permissaoId, Guid profissionalId)
        {
            return await Ok(_permissaoService.DesvincularPermissaoProfissional(permissaoId, profissionalId));
        }

        /// <summary>
        /// Vincular permissão a um funcionário.
        /// </summary>
        [HttpPost("{permissaoId}/vincular-funcionario/{funcionarioId}")]
        [Authorize(Policy = "A05")]
        public async Task<IActionResult> VincularFuncionario(Guid permissaoId, Guid funcionarioId)
        {
            return await Ok(_permissaoService.VincularPermissaoFuncionario(permissaoId, funcionarioId));
        }

        /// <summary>
        /// Desvincular permissão de um funcionário.
        /// </summary>
        [HttpDelete("{permissaoId}/desvincular-funcionario/{funcionarioId}")]
        [Authorize(Policy = "A05")]
        public async Task<IActionResult> DesvincularFuncionario(Guid permissaoId, Guid funcionarioId)
        {
            return await Ok(_permissaoService.DesvincularPermissaoFuncionario(permissaoId, funcionarioId));
        }

        /// <summary>
        /// Vincular permissão a um paciente.
        /// </summary>
        [HttpPost("{permissaoId}/vincular-paciente/{pacienteId}")]
        [Authorize(Policy = "A05")]
        public async Task<IActionResult> VincularPaciente(Guid permissaoId, Guid pacienteId)
        {
            return await Ok(_permissaoService.VincularPermissaoPaciente(permissaoId, pacienteId));
        }

        /// <summary>
        /// Desvincular permissão de um paciente.
        /// </summary>
        [HttpDelete("{permissaoId}/desvincular-paciente/{pacienteId}")]
        [Authorize(Policy = "A05")]
        public async Task<IActionResult> DesvincularPaciente(Guid permissaoId, Guid pacienteId)
        {
            return await Ok(_permissaoService.DesvincularPermissaoPaciente(permissaoId, pacienteId));
        }

        /// <summary>
        /// Vincular permissão a um administrador.
        /// </summary>
        [HttpPost("{permissaoId}/vincular-adm/{admId}")]
        [Authorize(Policy = "A05")]
        public async Task<IActionResult> VincularAdm(Guid permissaoId, Guid admId)
        {
            return await Ok(_permissaoService.VincularPermissaoAdm(permissaoId, admId));
        }

        /// <summary>
        /// Desvincular permissão de um administrador.
        /// </summary>
        [HttpDelete("{permissaoId}/desvincular-adm/{admId}")]
        [Authorize(Policy = "A05")]
        public async Task<IActionResult> DesvincularAdm(Guid permissaoId, Guid admId)
        {
            return await Ok(_permissaoService.DesvincularPermissaoAdm(permissaoId, admId));
        }
    }
}
