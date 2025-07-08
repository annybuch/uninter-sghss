using Loop.SGHSS.API.Default;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Adm;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.PassWord;
using Loop.SGHSS.Services.Administrador;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._12___Administrador
{
    [ApiController]
    [Route("api/v1/adm")]
    [Tags("12. 🗝️ Administradores")]

    public class AdministradorController : LoopContoller
    {
        private readonly IAdministradorService _administradorService;

        public AdministradorController(IAdministradorService administradorService)
        {
            _administradorService = administradorService;
        }

        /// <summary>
        /// Responsável por cadastrar um novo adm, identificando se é local (em uma instituição específica), ou se é geral (VidaPlus).
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("cadastrar")]
        public async Task<IActionResult> Post([FromBody] AdministradorModel model)
        {
            return await Created(_administradorService.CadastrarAdministrador(model));
        }

        /// <summary>
        /// Responsável por retornar todos os adm paginados.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "A06")]
        [ProducesResponseType(typeof(PagedResult<AdministradorViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] AdministradorQueryFilter filter)
        {
            return Ok(await _administradorService.ObterAdmPaginados(filter));
        }

        /// <summary>
        /// Responsável por retornar um adm por id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{admId}")]
        [Authorize(Policy = "A06")]
        [ProducesResponseType(typeof(AdministradorModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid admId)
        {
            return Ok(await _administradorService.BuscarAdmPorId(admId));
        }

        /// <summary>
        /// Responsável por editar as informações gerais de um adm.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("editar-info")]
        [Authorize(Policy = "USUARIO_EDITAR_PERFIL_PROPRIO")]
        [ProducesResponseType(typeof(AdministradorModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] AdministradorModel model)
        {
            return Ok(await _administradorService.EditarAdmGeral(model));
        }

        /// <summary>
        /// Responsável por editar apenas o endereço de um adm.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("editar-endereco")]
        [Authorize(Policy = "USUARIO_EDITAR_PERFIL_PROPRIO")]
        [ProducesResponseType(typeof(EnderecoModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] EnderecoModel model)
        {
            return Ok(await _administradorService.EditarAdmEndereco(model));
        }

        /// <summary>
        /// Responsável por alterar a senha do sistema do adm.
        /// </summary>
        /// <returns></returns>
        [HttpPatch("trocar-senha")]
        [Authorize(Policy = "USUARIO_MUDAR_SENHA_PROPRIA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Patch(PassModel model)
        {
            return await Ok(_administradorService.EditarSenhaAdm(model));
        }
    }
}
