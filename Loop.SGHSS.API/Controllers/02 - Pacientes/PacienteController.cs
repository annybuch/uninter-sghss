using Loop.SGHSS.API.Default;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Consultas;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Exames;
using Loop.SGHSS.Model.Pacientes;
using Loop.SGHSS.Model.PassWord;
using Loop.SGHSS.Services.Pacientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._02___Pacientes
{
    [ApiController, Route("api/v1/paciente"),
    Tags("02. 🩺 Pacientes, gestão de pacientes da VidaPlus")]
    public class PacienteController : LoopContoller
    {
        private readonly IPacienteService _pacienteService;

        public PacienteController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        /// <summary>
        /// Responsável por cadastrar um novo paciente na VidaPlus.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("cadastrar")]
        public async Task<IActionResult> Post([FromBody] PacientesModel model)
        {
            return await Created(_pacienteService.CadastrarPaciente(model));
        }

        /// <summary>
        /// Responsável por retornar todos os pacientes paginados.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "D04")]
        [ProducesResponseType(typeof(PagedResult<PacientesModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PacienteQueryFilter filter)
        {
            return Ok(await _pacienteService.ObterPacientesPaginados(filter));
        }

        /// <summary>
        /// Responsável por retornar todas as consultas de um paciente, paginados para ser consumido no canlendário.
        /// </summary>
        /// <param name="filter"></param>
        /// <remarks>
        /// Podendo ser filtrado por período, instituição, tipo de consulta (HomeCare, Presencial ou TeleConsulta)
        /// e status da consulta (pendente, EmAtendimento, concluída ou cancelada). Retornando as principais informações
        /// para formar o calendário, seja em lista ou em grade.
        /// </remarks>
        /// <returns></returns>
        [HttpGet("consultas-calendario")]
        [Authorize(Policy = "E05")]
        [ProducesResponseType(typeof(PagedResult<ConsultaGradePacienteModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] ConsultaPacienteQueryFilter filter)
        {
            return Ok(await _pacienteService.ObterConsultasPorPaciente(filter));
        }


        /// <summary>
        /// Responsável por retornar todos os exames de um paciente paginados e podendendo filtrar para ser consumido por calendário..
        /// </summary>
        [HttpGet("exames-calendario")]
        [Authorize(Policy = "E05")]
        [ProducesResponseType(typeof(PagedResult<ExameGradePacienteModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExame([FromQuery] ExameGradeQueryFilter filter)
        {
            return Ok(await _pacienteService.ObterExamesPorPaciente(filter));
        }

        /// <summary>
        /// Responsável por retornar um paciente por id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{pacienteId}")]
        [Authorize(Policy = "D04")]
        [ProducesResponseType(typeof(PacientesModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid pacienteId)
        {
            return Ok(await _pacienteService.BuscarPacientePorId(pacienteId));
        }

        /// <summary>
        /// Responsável por editar as informações gerais de um paciente.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("editar-info")]
        [Authorize(Policy = "D06")]
        [ProducesResponseType(typeof(PacientesModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] PacientesModel model)
        {
            return Ok(await _pacienteService.EditarPacienteGeral(model));
        }

        /// <summary>
        /// Responsável por editar apenas o endereço de um paciente.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("editar-endereco")]
        [Authorize(Policy = "D06")]
        [ProducesResponseType(typeof(EnderecoModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] EnderecoModel model)
        {
            return Ok(await _pacienteService.EditarPacienteEndereco(model));
        }

        /// <summary>
        /// Responsável por alterar a senha do sistema do paciente.
        /// </summary>
        /// <returns></returns>
        [HttpPatch("trocar-senha")]
        [Authorize(Policy = "USUARIO_MUDAR_SENHA_PROPRIA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Patch(PassModel model)
        {
            return await Ok(_pacienteService.EditarSenhaPaciente(model));
        }
    }
}

