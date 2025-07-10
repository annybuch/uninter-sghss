using Loop.SGHSS.API.Default;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Agenda;
using Loop.SGHSS.Model.Consultas;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Exames;
using Loop.SGHSS.Model.PassWord;
using Loop.SGHSS.Model.ProfissionaisSaude;
using Loop.SGHSS.Services.Profissionais_Saude;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._04___ProfissionaisSaude
{
    [ApiController, Route("api/v1/profissional"),
    Tags("04. 🥼 Profissionais de saúde, gestão de médicos de instituições da VidaPlus")]
    public class ProfissionalSaudeController : LoopContoller
    {
        private readonly IProfissionalSaudeService _profissionalService;

        public ProfissionalSaudeController(IProfissionalSaudeService profissionalService)
        {
            _profissionalService = profissionalService;
        }

        /// <summary>
        /// Responsável por cadastrar um novo Profissional de saúde na VidaPlus.
        /// </summary>
        [HttpPost("cadastrar")]
        [Authorize(Policy = "B04")]
        public async Task<IActionResult> Post([FromBody] ProfissionalSaudeModel model)
        {
            return await Created(_profissionalService.CadastrarProfissional(model));
        }

        /// <summary>
        /// Responsável por retornar todos os Profissionals paginados.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "C07")]
        [ProducesResponseType(typeof(PagedResult<ProfissionalSaudeModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] ProfissionaisSaudeQueryFilter filter)
        {
            return Ok(await _profissionalService.ObterProfissionaisPaginados(filter));
        }

        /// <summary>
        /// Responsável por retornar todas as consultas de um profissional paginados e podendendo filtrar.
        /// </summary>
        [HttpGet("consultas-calendario")]
        [Authorize(Policy = "C07")]
        [ProducesResponseType(typeof(PagedResult<ConsultaGradeProfissionalSaudeModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] ConsultaProfissionalQueryFilter filter)
        {
            return Ok(await _profissionalService.ObterConsultasPorProfissional(filter));
        }

        /// <summary>
        /// Responsável por retornar todos os exames de um profissional paginados e podendendo filtrar.
        /// </summary>
        [HttpGet("exames-calendario")]
        [Authorize(Policy = "C07")]
        [ProducesResponseType(typeof(PagedResult<ExameGradeProfissionalSaudeModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExame([FromQuery] ExameGradeQueryFilter filter)
        {
            return Ok(await _profissionalService.ObterExamesPorProfissionalSaude(filter));
        }

        /// <summary>
        /// Responsável por retornar um Profissional por id.
        /// </summary>
        [HttpGet("{profissionalId}")]
        [Authorize(Policy = "C07")]
        [ProducesResponseType(typeof(ProfissionalSaudeModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid profissionalId)
        {
            return Ok(await _profissionalService.BuscarProfissionalPorId(profissionalId));
        }

        /// <summary>
        /// Responsável por editar as informações gerais de um Profissional.
        /// </summary>
        [HttpPut("editar-info")]
        [Authorize(Policy = "B05")]
        [ProducesResponseType(typeof(ProfissionalSaudeModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] ProfissionalSaudeModel model)
        {
            return Ok(await _profissionalService.EditarProfissionalGeral(model));
        }

        /// <summary>
        /// Responsável por editar apenas o endereço de um Profissional de saúde.
        /// </summary>
        [HttpPut("editar-endereco")]
        [Authorize(Policy = "B05")]
        [ProducesResponseType(typeof(EnderecoModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] EnderecoModel model)
        {
            return Ok(await _profissionalService.EditarProfissionalEndereco(model));
        }

        /// <summary>
        /// Responsável por alterar a senha do sistema do Profissional de saúde.
        /// </summary>
        [HttpPatch("trocar-senha")]
        [Authorize(Policy = "USUARIO_MUDAR_SENHA_PROPRIA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Patch(PassModel model)
        {
            return await Ok(_profissionalService.EditarSenhaProfissional(model));
        }

        /// <summary>
        /// Responsável por associar uma especialização a um profissional de saúde.
        /// </summary>
        [HttpPatch("vincular-especializacao/{profissionalId}/{especializacaoId}")]
        [Authorize(Policy = "B06")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PatchEspecializacao(Guid profissionalId, Guid especializacaoId)
        {
            return await Ok(_profissionalService.VincularEspecializacao(profissionalId, especializacaoId));
        }

        /// <summary>
        /// Responsável por remover uma especialização de um profissional de saúde.
        /// </summary>
        [HttpDelete("desvincular-especializacao/{profissionalId}/{especializacaoId}")]
        [Authorize(Policy = "B06")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteEspecializacao(Guid profissionalId, Guid especializacaoId)
        {
            return await Ok(_profissionalService.DesvincularEspecializacao(profissionalId, especializacaoId));
        }

        /// <summary>
        /// Responsável por associar um serviço de laboratório a um profissional de saúdea.
        /// </summary>
        [HttpPatch("vincular-servico-laboratorio/{profissionalId}/{servicoLaboratorioId}")]
        [Authorize(Policy = "B06")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PatchLaboratorio(Guid profissionalId, Guid servicoLaboratorioId)
        {
            return await Ok(_profissionalService.VincularServicoLaboratorio(profissionalId, servicoLaboratorioId));
        }

        /// <summary>
        /// Responsável por remover um serviço de laboratório de um profissional de saúde.
        /// </summary>
        [HttpDelete("desvincular-servico-laboratorio/{profissionalId}/{servicoLaboratorioId}")]
        [Authorize(Policy = "B06")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteLaboratorio(Guid profissionalId, Guid servicoLaboratorioId)
        {
            return await Ok(_profissionalService.DesvincularServicoLaboratorio(profissionalId, servicoLaboratorioId));
        }

        /// <summary>
        /// Responsável por cadastrar uma agenda para uma instituição, de um profissional.
        /// </summary>
        [HttpPost("cadastrar-agenda")]
        [Authorize(Policy = "C09")]
        public async Task<IActionResult> Post([FromBody] ProfissionalSaudeAgendaModel model)
        {
            return await Created(_profissionalService.CadastrarAgendaProfissional(model));
        }

        /// <summary>
        /// Responsável por inativar (remover logicamente) um profissional da saúde do sistema.
        /// </summary>
        [HttpDelete("remover/{profissionalId}")]
        [Authorize(Policy = "B06")]
        public async Task<IActionResult> RemoverProfissionalSaude(Guid profissionalId)
        {
            return await Ok(_profissionalService.Remover(profissionalId));
        }
    }
}
