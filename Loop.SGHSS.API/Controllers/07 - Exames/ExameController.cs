using Loop.SGHSS.API.Default;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Consultas;
using Loop.SGHSS.Model.Exames;
using Loop.SGHSS.Model.Instituicoes;
using Loop.SGHSS.Model.ServicosPrestados;
using Loop.SGHSS.Services.Servicos_Prestados.Exames;
using Loop.SGHSS.Services.Servicos_Prestados.Laboratorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._07___Exames
{
    [ApiController, Route("api/v1/exame"),
    Tags("07. 💉 Exames, fluxo responsável por marcar um exame.")]
    public class ExameController : LoopContoller
    {
        private readonly IExameService _exameService;
        private readonly ILaboratorioService _laboratorioService;

        public ExameController(IExameService ExameService, ILaboratorioService laboratorioService)
        {
            _exameService = ExameService;
            _laboratorioService = laboratorioService;
        }

        /// <summary>
        /// 1 - Responsável por retornar todos os tipos de serviços de laboratório paginados.
        /// </summary>
        [HttpGet("bucar-servico-laboratorio")]
        [Authorize(Policy = "A01")]
        [ProducesResponseType(typeof(PagedResult<ServicosLaboratorioModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] ServicosLaboratorioQueryFilter filter)
        {
            return Ok(await _laboratorioService.ObterServicosLaboratoriosPaginados(filter));
        }

        /// <summary>
        /// 2 - Responsável por buscar instituições que tem disponibilidade para o serviço de laboratório escolhido.
        /// </summary>
        [HttpGet("buscar-instiruicoes")]
        [Authorize(Policy = "A01")]
        [ProducesResponseType(typeof(PagedResult<InstituicaoModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid servicoLaboratorioId)
        {
            return Ok(await _exameService.BuscarInstituicoesComDisponibilidade(servicoLaboratorioId));
        }

        /// <summary>
        /// 3 - Responsável por buscar todos os profissionais que possuem disponibilidade 
        /// naquela instituição retornando sua grade de horários.
        /// </summary>
        [HttpGet("buscar-grade")]
        [Authorize(Policy = "A01")]
        [ProducesResponseType(typeof(PagedResult<ProfissionalComHorariosModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid instituicaoId, Guid servicoLaboratorioId)
        {
            return Ok(await _exameService.ObterProfissionaisComHorarios(instituicaoId, servicoLaboratorioId));
        }

        /// <summary>
        /// 4 - Responsável por marcar um Exame em uma instituição.
        /// </summary>
        [HttpPost("marcar-exame")]
        [Authorize(Policy = "D02")]
        [Authorize(Policy = "E03")]
        public async Task<IActionResult> Post([FromBody] ExameModel model)
        {
            return await Created(_exameService.MarcarExame(model));
        }

        /// <summary>
        /// Responsável por iniciar um exame pelo profissional de saúde.
        /// </summary>
        [HttpPost("iniciar")]
        [Authorize(Policy = "C06")]
        public async Task<IActionResult> IniciarExame([FromQuery] Guid exameId)
        {
            return Ok(await _exameService.IniciarExame(exameId));
        }

        /// <summary>
        /// Responsável por finalizar um exame em andamento pelo profissional de saúde.
        /// </summary>
        [HttpPost("finalizar")]
        [Authorize(Policy = "C06")]
        public async Task<IActionResult> FinalizarExame([FromQuery] Guid exameId, [FromQuery] string? anotacoes = null)
        {
            return Ok(await _exameService.FinalizarExame(exameId, anotacoes));
        }


        /// <summary>
        /// Anexa um resultado de um exame na consulta.
        /// </summary>
        [HttpPost("anexar-resultado-exame")]
        [Authorize(Policy = "C08")]
        public async Task<IActionResult> AnexarResultado(Guid exameId, IFormFile arquivo)
        {
            using var ms = new MemoryStream();
            await arquivo.CopyToAsync(ms);
            var bytes = ms.ToArray();

            await _exameService.AnexarResultado(exameId, bytes);
            return Ok("Prescrição anexada com sucesso.");
        }

        /// <summary>
        /// Anexa um guia médica ao exame.
        /// </summary>
        [HttpPost("anexar-guia")]
        [Authorize(Policy = "C08")]
        public async Task<IActionResult> AnexarGuiaMedico(Guid exameId, IFormFile arquivo)
        {
            using var ms = new MemoryStream();
            await arquivo.CopyToAsync(ms);
            var bytes = ms.ToArray();

            await _exameService.AnexarGuiaMedico(exameId, bytes);
            return Ok("Guia médico anexada com sucesso.");
        }

        /// <summary>
        /// Responsável por marcar um exame como cancelado.
        /// </summary>
        [HttpPatch("cancelar-exame")]
        [Authorize(Policy = "E02")]
        public async Task<IActionResult> CancelarExame(Guid exameId)
        {
            return Ok(await _exameService.CancelarExame(exameId));
        }

        /// <summary>
        /// Responsável por retornar todos os exames de uma instituição, paginados para ser consumido no canlendário.
        /// </summary>
        /// <param name="filter"></param>
        /// <remarks>
        /// Podendo ser filtrado por período, instituição,
        /// e status do exame (pendente, EmAtendimento, concluída ou cancelada). Retornando as principais informações
        /// para formar o calendário, seja em lista ou em grade.
        /// </remarks>
        /// <returns></returns>
        [HttpGet("exames-calendario")]
        [Authorize(Policy = "E05")]
        [Authorize(Policy = "C07")]
        [ProducesResponseType(typeof(PagedResult<ExameGradeGeralModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] ExameGradeQueryFilter filter)
        {
            return Ok(await _exameService.ObterExamesGrade(filter));
        }

        /// <summary>
        /// Responsável por retornar todos os pacientes, com seus exames, podendo ser filtrado por profissionais e/ou instituições.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("pacientes-exames")]
        [Authorize(Policy = "E05")]
        [Authorize(Policy = "C07")]
        [ProducesResponseType(typeof(PagedResult<PacienteComExamesModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPacientes([FromQuery] ExameGradeQueryFilter filter)
        {
            return Ok(await _exameService.ObterPacientesComExames(filter));
        }

        /// <summary>
        /// Responsável por retornar um exame por id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{exameId}")]
        [Authorize(Policy = "E08")]
        [ProducesResponseType(typeof(ExameModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetId(Guid exameId)
        {
            return Ok(await _exameService.BuscarExamePorId(exameId));
        }
    }
}
