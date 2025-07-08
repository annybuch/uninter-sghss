using Loop.SGHSS.API.Default;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Consultas;
using Loop.SGHSS.Model.Exames;
using Loop.SGHSS.Services.Servicos_Prestados.Consultas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._06___Cosultas
{
    [ApiController]
    [Route("api/v1/consulta")]
    [Tags("06. 📝💻 Consultas, gestão completa de consultas.")]
    public class ConsultaController : LoopContoller
    {
        private readonly IConsultaService _consultaService;

        public ConsultaController(IConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        /// <summary>
        /// Responsável por iniciar uma consulta (presencial ou teleconsulta) pelo profissional de saúde.
        /// </summary>
        [HttpPost("iniciar")]
        [Authorize(Policy = "C02")]
        public async Task<IActionResult> IniciarConsulta([FromQuery] Guid consultaId)
        {
            return Ok(await _consultaService.IniciarConsulta(consultaId));
        }

        /// <summary>
        /// Responsável por finaliza uma consulta em andamento pelo profissional de saúde.
        /// </summary>
        [HttpPost("finalizar")]
        [Authorize(Policy = "C02")]
        public async Task<IActionResult> FinalizarConsulta([FromQuery] Guid consultaId, [FromQuery] string? anotacoes = null)
        {
            return Ok(await _consultaService.FinalizarConsulta(consultaId, anotacoes));
        }

        /// <summary>
        /// Gera link de acesso para o paciente e o profissional na teleconsulta.
        /// </summary>
        [HttpGet("gerar-link-paciente")]
        [Authorize(Policy = "C03")]
        public async Task<IActionResult> GerarLinkDeAcessoPaciente([FromQuery] Guid consultaId, [FromQuery] Guid pacienteId)
        {
            return Ok(await _consultaService.GerarLinkDeAcessoPaciente(consultaId, pacienteId));
        }

        /// <summary>
        /// Anexa uma receita médica à consulta.
        /// </summary>
        [HttpPost("anexar-receita")]
        [Authorize(Policy = "C08")]
        public async Task<IActionResult> AnexarReceita(Guid consultaId, IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                return BadRequest("Arquivo inválido.");

            using var stream = new MemoryStream();
            await arquivo.CopyToAsync(stream);
            var bytes = stream.ToArray();

            await _consultaService.AnexarReceita(consultaId, bytes);

            return Ok("Receita anexada com sucesso.");
        }

        /// <summary>
        /// Anexa uma prescrição médica à consulta.
        /// </summary>
        [HttpPost("anexar-prescricao")]
        [Authorize(Policy = "C08")]
        public async Task<IActionResult> AnexarPrescricao(Guid consultaId, IFormFile arquivo)
        {
            using var ms = new MemoryStream();
            await arquivo.CopyToAsync(ms);
            var bytes = ms.ToArray();

            await _consultaService.AnexarPrescricao(consultaId, bytes);
            return Ok("Prescrição anexada com sucesso.");
        }

        /// <summary>
        /// Anexa uma guia médica à consulta.
        /// </summary>
        [HttpPost("anexar-guia")]
        [Authorize(Policy = "C08")]
        public async Task<IActionResult> AnexarGuiaMedico(Guid consultaId, IFormFile arquivo)
        {
            using var ms = new MemoryStream();
            await arquivo.CopyToAsync(ms);
            var bytes = ms.ToArray();

            await _consultaService.AnexarGuiaMedico(consultaId, bytes);
            return Ok("Guia médico anexada com sucesso.");
        }

        /// <summary>
        /// Responsável por marcar uma consulta como cancelada.
        /// </summary>
        [HttpPatch("cancelar-consulta")]
        [Authorize(Policy = "E02")]
        public async Task<IActionResult> CancelarConsulta(Guid consultaId)
        {
            return Ok(await _consultaService.CancelarConsulta(consultaId));
        }

        /// <summary>
        /// Responsável por retornar todas as consultas de uma instituição, paginados para ser consumido no canlendário.
        /// </summary>
        /// <param name="filter"></param>
        /// <remarks>
        /// Podendo ser filtrado por período, instituição, tipo de consulta (HomeCare, Presencial ou TeleConsulta)
        /// e status da consulta (pensente, EmAtendimento, concluída ou cancelada). Retornando as principais informações
        /// para formar o calendário, seja em lista ou em grade.
        /// </remarks>
        /// <returns></returns>
        [HttpGet("consultas-calendario")]
        [Authorize(Policy = "E05")]
        [Authorize(Policy = "C07")]
        [ProducesResponseType(typeof(PagedResult<ConsultaGradeGeralModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] ConsultaPacienteQueryFilter filter)
        {
            return Ok(await _consultaService.ObterConsultasPorPacienteEProfissional(filter));
        }

        /// <summary>
        /// Responsável por retornar todos os pacientes, com suas consultas, podendo ser filtrado por profissionais e/ou instituições.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("pacientes-consultas")]
        [Authorize(Policy = "E05")]
        [Authorize(Policy = "C07")]
        [ProducesResponseType(typeof(PagedResult<PacienteComConsultasModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPacientes([FromQuery] ConsultaPacienteQueryFilter filter)
        {
            return Ok(await _consultaService.ObterPacientesComConsultas(filter));
        }

        /// <summary>
        /// Responsável por retornar uma consulta por id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{consultaId}")]
        [Authorize(Policy = "E08")]
        [ProducesResponseType(typeof(ConsultaModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetId(Guid consultaId)
        {
            return Ok(await _consultaService.BuscarConsultaPorId(consultaId));
        }
    }
}
