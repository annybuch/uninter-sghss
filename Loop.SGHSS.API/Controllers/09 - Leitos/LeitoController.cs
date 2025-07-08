using Loop.SGHSS.API.Default;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Leitos;
using Loop.SGHSS.Services.Leitos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._09___Leitos
{
    [ApiController, Route("api/v1/leito"),
    Tags("09. 🛏️ Leitos, gestão e cadastro de leitos e pacientes associados")]
    public class LeitoController : LoopContoller
    {
        private readonly ILeitoService _leitoService;

        public LeitoController(ILeitoService leitoService)
        {
            _leitoService = leitoService;
        }

        /// <summary>
        /// Responsável por cadastrar um novo leito em uma instituição médica da VidaPlus.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("cadastrar")]
        [Authorize(Policy = "B09")]
        public async Task<IActionResult> Post([FromBody] LeitosModel model)
        {
            return await Created(_leitoService.CadastrarLeito(model));
        }

        /// <summary>
        /// Responsável por cadastrar vários leitos em massa em uma isntituição médica da VidaPlus.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("cadastrar-em-massa")]
        [Authorize(Policy = "B09")]
        public async Task<IActionResult> Post([FromBody] LeitosMassa model)
        {
            return await Created(_leitoService.Cadastrar_Leito_Em_Massa(model));
        }

        /// <summary>
        /// Responsável por retornar todos os leitos paginados.
        /// </summary>
        /// <param name="filter"></param>
        /// <remarks>
        /// Podendo também passar filtros de instituição, andar, tipo ou status.
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "A01")]
        [ProducesResponseType(typeof(PagedResult<LeitosModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] LeitoQueryFilter filter)
        {
            return Ok(await _leitoService.ObterLeitosPaginados(filter));
        }

        /// <summary>
        /// Responsável por retornar um leito por id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{leitoId}")]
        [Authorize(Policy = "A01")]
        [ProducesResponseType(typeof(LeitosModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid leitoId)
        {
            return Ok(await _leitoService.ObterLeitoPorId(leitoId));
        }

        /// <summary>
        /// Responsável por editar as informações gerais de um leito.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("editar-info")]
        [Authorize(Policy = "B09")]
        [ProducesResponseType(typeof(LeitosModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] LeitosModel model)
        {
            return Ok(await _leitoService.EditarLeitosGeral(model));
        }

        /// <summary>
        /// Responsável por atualizar o status de um leito.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("atualizar-status")]
        [Authorize(Policy = "B09")]
        [ProducesResponseType(typeof(LeitoStatus), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] LeitoStatus model)
        {
            return Ok(await _leitoService.AtualizarStatusLeito(model));
        }

        /// <summary>
        /// Responsável por associar um paciente a um leito específico.
        /// </summary>
        /// <returns></returns>
        [HttpPut("vincular-paciente")]
        [Authorize(Policy = "D09")]
        [ProducesResponseType(typeof(LeitosPacientesModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] AddPacienteLeitoModel model)
        {
            return await Ok(_leitoService.AdicionarPacienteLeito(model));
        }

        /// <summary>
        /// Responsável por adicionar uma observação ao paciente em um leito específico.
        /// </summary>
        /// <returns></returns>
        [HttpPut("adicionar-obs")]
        [Authorize(Policy = "D09")]
        [ProducesResponseType(typeof(LeitosPacientesModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Observacao([FromBody] AddPacienteLeitoModel model)
        {
            return await Ok(_leitoService.AdicionarObservacoesPacienteLeito(model));
        }

        /// <summary>
        /// Responsável por remover um paciente de um leito.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("liberar/{leitoId}/{pacienteId}")]
        [Authorize(Policy = "D09")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid leitoId, Guid pacienteId)
        {
            return await Ok(_leitoService.RemoverPacienteLeito(leitoId, pacienteId));
        }

        /// <summary>
        /// Obter um leito por ID. Se o leito estiver em uso, retorna também o paciente atual e suas observações.
        /// </summary>
        [HttpGet("{leitoId}/paciente-atual")]
        [Authorize(Policy = "B09")]
        [ProducesResponseType(typeof(LeitoComPacienteModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterLeitoComPacienteAtual(Guid leitoId)
        {
            return Ok(await _leitoService.ObterLeitoComPacienteAtual(leitoId));
        }

    }
}
