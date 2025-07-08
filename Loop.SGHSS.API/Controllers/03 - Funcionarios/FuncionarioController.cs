using Loop.SGHSS.API.Default;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Funcionarios;
using Loop.SGHSS.Model.PassWord;
using Loop.SGHSS.Services.Funcionarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._03___Funcionarios
{
    [ApiController, Route("api/v1/funcionario"),
    Tags("03. 👥 Funcionários, gestão de funcionários de instituições da VidaPlus")]
    public class FuncionarioController : LoopContoller
    {
        private readonly IFuncionarioService _funcionarioService;

        public FuncionarioController(IFuncionarioService funcionarioService)
        {
            _funcionarioService = funcionarioService;
        }

        /// <summary>
        /// Responsável por cadastrar um novo funcionario na VidaPlus.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("cadastrar")]
        [Authorize(Policy = "D02")]
        public async Task<IActionResult> Post([FromBody] FuncionarioModel model)
        {
            return await Created(_funcionarioService.CadastrarFuncionario(model));
        }

        /// <summary>
        /// Responsável por retornar todos os funcionarios paginados, podendo filtrar por instiruição.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "D03")]
        [ProducesResponseType(typeof(PagedResult<FuncionarioModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] FuncionarioQueryFilter filter)
        {
            return Ok(await _funcionarioService.ObterFuncionariosPaginadas(filter));
        }

        /// <summary>
        /// Responsável por retornar um funcionario por id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{funcionarioId}")]
        [Authorize(Policy = "D03")]
        [ProducesResponseType(typeof(FuncionarioModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid funcionarioId)
        {
            return Ok(await _funcionarioService.BuscarFuncionarioPorId(funcionarioId));
        }

        /// <summary>
        /// Responsável por editar as informações gerais de um funcionario.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("editar-info")]
        [Authorize(Policy = "D03")]
        [ProducesResponseType(typeof(FuncionarioModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] FuncionarioModel model)
        {
            return Ok(await _funcionarioService.EditarFuncionarioGeral(model));
        }

        /// <summary>
        /// Responsável por editar apenas o endereço de um funcionario.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("editar-endereco")]
        [Authorize(Policy = "D03")]
        [ProducesResponseType(typeof(EnderecoModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] EnderecoModel model)
        {
            return Ok(await _funcionarioService.EditarFuncionarioEndereco(model));
        }

        /// <summary>
        /// Responsável por alterar a senha do sistema do funcionario.
        /// </summary>
        /// <returns></returns>
        [HttpPatch("trocar-senha")]
        [Authorize(Policy = "USUARIO_MUDAR_SENHA_PROPRIA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Patch(PassModel model)
        {
            return await Ok(_funcionarioService.EditarSenhaFuncionario(model));
        }
    }
}
