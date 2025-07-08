using Loop.SGHSS.API.Default;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._Enums.Consulta;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Consultas;
using Loop.SGHSS.Model.Instituicoes;
using Loop.SGHSS.Model.ServicosPrestados;
using Loop.SGHSS.Services.Servicos_Prestados.Consultas;
using Loop.SGHSS.Services.Servicos_Prestados.Especializacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._06___Cosultas
{
    [ApiController, Route("api/v1/consulta-home"),
    Tags("06.1. 💻 Consultas home, fluxo responsável por marcar uma TELECONSULTA ou uma HOMECARE.")]
    public class ConsultaHomeController : LoopContoller
    {
        private readonly IConsultaService _consultaService;
        private readonly IEspecializacaoService _especializacaoService;

        public ConsultaHomeController(IConsultaService consultaService, IEspecializacaoService especializacaoService)
        {
            _consultaService = consultaService;
            _especializacaoService = especializacaoService;
        }

        /// <summary>
        /// 1 - Responsável por retornar todos as especializações da Vida+ paginadas.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("buscar-especializacoes")]
        [Authorize(Policy = "A08")]
        [ProducesResponseType(typeof(PagedResult<EspecializacoesModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] EspecializacoesQueryFilter filter)
        {
            return Ok(await _especializacaoService.ObterEspecializacoesPaginadas(filter));
        }

        /// <summary>
        /// 2 - Responsável por buscar todos os profissionais que possuem disponibilidade 
        /// naquela modalidade (TeleConsulta ou HomeCare) retornando sua grade de horários.
        /// </summary>
        /// <param name="especializacaoId"></param>
        /// <param name="tipoConsulta"></param>
        /// <returns></returns>
        [HttpGet("buscar-grade")]
        [Authorize(Policy = "B08")]
        [ProducesResponseType(typeof(PagedResult<ProfissionalComHorariosModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid especializacaoId, TipoConsultaEnum tipoConsulta)
        {
            return Ok(await _consultaService.ObterProfissionaisComHorarios(especializacaoId, tipoConsulta));
        }

        /// <summary>
        /// 3 - Responsável por marcar uma consulta em uma instituição.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("marcar-consulta")]
        [Authorize(Policy = "E01")]
        public async Task<IActionResult> Post([FromBody] MarcarConsultaModel model)
        {
            return await Created(_consultaService.MarcarConsulta(model));
        }
    }
}
