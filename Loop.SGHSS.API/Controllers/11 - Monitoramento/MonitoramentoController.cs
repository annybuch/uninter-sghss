using Loop.SGHSS.API.Default;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model.Adm;
using Loop.SGHSS.Model.Monitoramento.Financas;
using Loop.SGHSS.Model.Monitoramento.Recursos;
using Loop.SGHSS.Services.Monitoramento;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._11___Monitoramento
{
    [ApiController]
    [Route("api/v1/monitoramento")]
    [Tags("11. 📊 Monitoramento, gestão de suprimentos, leitos e finanças")]
    public class MonitoramentoController : LoopContoller
    {
        private readonly IMonitoramentoService _monitoramentoService;

        public MonitoramentoController(IMonitoramentoService monitoramentoService)
        {
            _monitoramentoService = monitoramentoService;
        }

        /// <summary>
        /// Obter dados do monitoramento financeiro (consultas, exames e suprimentos)
        /// </summary>
        [HttpGet("financas")]
        [Authorize(Policy = "B08")]
        [ProducesResponseType(typeof(MonitoramentoFinancasModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterFinancasDashboard([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim, [FromQuery] Guid? instituicaoId = null)
        {
            return Ok(await _monitoramentoService.ObterFinancasDashboard(dataInicio, dataFim, instituicaoId));
        }

        /// <summary>
        /// Obter dados do monitoramento de suprimentos (estoque, custos, itens críticos)
        /// </summary>
        [HttpGet("suprimentos")]
        [Authorize(Policy = "B08")]
        [ProducesResponseType(typeof(MonitoramentoSuprimentosModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterSuprimentosDashboard([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim, [FromQuery] Guid? instituicaoId = null)
        {
            return Ok(await _monitoramentoService.ObterMonitoramentoSuprimentos(dataInicio, dataFim, instituicaoId));
        }

        /// <summary>
        /// Obter dados do monitoramento de leitos (total, disponíveis, em uso e manutenção)
        /// </summary>
        [HttpGet("leitos")]
        [Authorize(Policy = "B08")]
        [ProducesResponseType(typeof(MonitoramentoLeitosModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterLeitosDashboard([FromQuery] Guid? instituicaoId = null)
        {
            return Ok(await _monitoramentoService.ObterMonitoramentoLeitos(instituicaoId));
        }
    }
}
