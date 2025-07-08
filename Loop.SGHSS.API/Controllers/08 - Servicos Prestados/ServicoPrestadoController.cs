using Loop.SGHSS.API.Default;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.ServicosPrestados;
using Loop.SGHSS.Services.Servicos_Prestados.Especializacoes;
using Loop.SGHSS.Services.Servicos_Prestados.Laboratorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._08___Servicos_Prestados
{
    [ApiController, Route("api/v1/servico"),
    Tags("08. 🩺💉 Serviços Prestados, gestão e cadastro de especializações e serviços de laboratório.")]
    public class ServicoPrestadoController : LoopContoller
    {
        private readonly IEspecializacaoService _especializacaoService;
        private readonly ILaboratorioService _laboratorioService;

        public ServicoPrestadoController(ILaboratorioService laboratorioService, IEspecializacaoService especializacaoService)
        {
            _especializacaoService = especializacaoService;
            _laboratorioService = laboratorioService;
        }

        /// <summary>
        /// Responsável por cadastrar uma nova especialização na VidaPlus.
        /// </summary>
        [HttpPost("especializacao/cadastrar")]
        [Authorize(Policy = "A08")]
        public async Task<IActionResult> Post([FromBody] EspecializacoesModel model)
        {
            return await Created(_especializacaoService.CadastrarEspecializacao(model));
        }

        /// <summary>
        /// Responsável por cadastrar uma nova especialização de serviço de laboratório na VidaPlus.
        /// </summary>
        [HttpPost("servico-laboratorio/cadastrar")]
        [Authorize(Policy = "A09")]
        public async Task<IActionResult> Post([FromBody] ServicosLaboratorioModel model)
        {
            return await Created(_laboratorioService.CadastrarServicoLaboratorio(model));
        }

        /// <summary>
        /// Responsável por retornar todos as especializações paginadas e podendo filtrar por instituição.
        /// </summary>
        [HttpGet("especializacao")]
        [Authorize(Policy = "A01")]
        [ProducesResponseType(typeof(PagedResult<EspecializacoesModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] EspecializacoesQueryFilter filter)
        {
            return Ok(await _especializacaoService.ObterEspecializacoesPaginadas(filter));
        }

        /// <summary>
        /// Responsável por retornar todos os tipos de serviços de laboratório paginados e podendo filtrar por instituição.
        /// </summary>
        [HttpGet("servico-laboratorio")]
        [Authorize(Policy = "A01")]
        [ProducesResponseType(typeof(PagedResult<ServicosLaboratorioModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] ServicosLaboratorioQueryFilter filter)
        {
            return Ok(await _laboratorioService.ObterServicosLaboratoriosPaginados(filter));
        }

        /// <summary>
        /// Responsável por retornar uma especialização por id.
        /// </summary>
        [HttpGet("especializacao/{especializacaoId}")]
        [Authorize(Policy = "A01")]
        [ProducesResponseType(typeof(EspecializacoesModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEspecializacao(Guid especializacaoId)
        {
            return Ok(await _especializacaoService.BuscarEspecializacaoPorId(especializacaoId));
        }

        /// <summary>
        /// Responsável por retornar um serviço de laboratório por id.
        /// </summary>
        [HttpGet("servico-laboratorio/{servicoLaboratorioId}")]
        [Authorize(Policy = "A01")]
        [ProducesResponseType(typeof(ServicosLaboratorioModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLaboratorio(Guid servicoLaboratorioId)
        {
            return Ok(await _laboratorioService.BuscarServicoLaboratorioPorId(servicoLaboratorioId));
        }

        /// <summary>
        /// Responsável por editar as informações da especialização.
        /// </summary>
        [HttpPut("especializacao/editar")]
        [Authorize(Policy = "A08")]
        [ProducesResponseType(typeof(EspecializacoesModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] EspecializacoesModel model)
        {
            return Ok(await _especializacaoService.EditarEspecializacao(model));
        }

        /// <summary>
        /// Responsável por editar as informações de um serviço de laboratório.
        /// </summary>
        [HttpPut("servico-laboratorio/editar")]
        [Authorize(Policy = "A09")]
        [ProducesResponseType(typeof(ServicosLaboratorioModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] ServicosLaboratorioModel model)
        {
            return Ok(await _laboratorioService.EditarServicoLaboratorio(model));
        }
    }
}
