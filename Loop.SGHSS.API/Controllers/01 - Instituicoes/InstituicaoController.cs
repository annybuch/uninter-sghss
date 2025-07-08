using Loop.SGHSS.API.Default;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Agenda;
using Loop.SGHSS.Model.Endereco;
using Loop.SGHSS.Model.Instituicoes;
using Loop.SGHSS.Services.Instituicoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._01___Instituicoes
{
    [ApiController, Route("api/v1/instituicao"),
    Tags("01. 🏥 Instituições, gestão de hospitais, clínicas e laboratórios")]
    public class InstituicaoController : LoopContoller
    {
        private readonly IInstituicaoService _instituiaoService;

        public InstituicaoController(IInstituicaoService instituicaoService)
        {
            _instituiaoService = instituicaoService;
        }

        /// <summary>
        /// Responsável por cadastrar uma nova instiruição de saúde na VidaPlus.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("cadastrar")]
        [Authorize(Policy = "A02")]
        public async Task<IActionResult> Post([FromBody] InstituicaoModel model)
        {
            return await Created(_instituiaoService.CadastrarInstituicao(model));
        }

        /// <summary>
        /// Responsável por cadastrar uma agenda para uma instituição.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("cadastrar-agenda")]
        [Authorize(Policy = "B10")]
        public async Task<IActionResult> Post([FromBody] InstituicaoAgendaModel model)
        {
            return await Created(_instituiaoService.CadastrarAgendaInstituicao(model));
        }

        /// <summary>
        /// Responsável por retornar todas as instituições de saúde paginadas.
        /// </summary>
        /// <param name="filter"></param>
        /// <remarks>
        /// Podendo também passar um filtro de Estado ou cidade, para facilitar o retorno de instituições.
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "A01")]
        [ProducesResponseType(typeof(PagedResult<InstituicaoModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] InstituicaoQueryFilter filter)
        {
            return Ok(await _instituiaoService.ObterInstituicoesPaginadas(filter));
        }

        /// <summary>
        /// Responsável por retornar uma instituição de saúde por id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{instituicaoId}")]
        [Authorize(Policy = "A01")]
        [ProducesResponseType(typeof(InstituicaoModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid instituicaoId)
        {
            return Ok(await _instituiaoService.BuscarInstituicaoPorId(instituicaoId));
        }

        /// <summary>
        /// Responsável por editar as informações gerais de uma instituição de saúde.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("editar-info")]
        [Authorize(Policy = "A03")]
        [ProducesResponseType(typeof(InstituicaoModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] InstituicaoModel model)
        {
            return Ok(await _instituiaoService.EditarInstituicaoGeral(model));
        }

        /// <summary>
        /// Responsável por editar apenas o endereço de uma instituição de saúde.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("editar-endereco")]
        [Authorize(Policy = "A03")]
        [ProducesResponseType(typeof(EnderecoModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] EnderecoModel model)
        {
            return Ok(await _instituiaoService.EditarInstituicaoEndereco(model));
        }

        /// <summary>
        /// Responsável por associar/contratar um profissional em uma instituição específica.
        /// </summary>
        /// <returns></returns>
        [HttpPatch("profissional/{instituicaoId}/{profissionalId}")]
        [Authorize(Policy = "B06")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Patch(Guid instituicaoId, Guid profissionalId)
        {
            return await Ok(_instituiaoService.VincularProfissional(instituicaoId, profissionalId));
        }

        /// <summary>
        /// Responsável por remover um profissional de uma instituição de saúde.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("profissional/{instituicaoId}/{profissionalId}")]
        [Authorize(Policy = "B06")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid instituicaoId, Guid profissionalId)
        {
            return await Ok(_instituiaoService.DesvincularProfissional(instituicaoId, profissionalId));
        }

        /// <summary>
        /// Responsável por associar uma especialização a uma instituição específica.
        /// </summary>
        /// <returns></returns>
        [HttpPatch("vincular-especializacao/{instituicaoId}/{especializacaoId}")]
        [Authorize(Policy = "A08")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PatchEspecializacao(Guid instituicaoId, Guid especializacaoId)
        {
            return await Ok(_instituiaoService.VincularEspecializacao(instituicaoId, especializacaoId));
        }

        /// <summary>
        /// Responsável por remover uma especialização de uma instituição de saúde.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("desvincular-especializacao/{instituicaoId}/{especializacaoId}")]
        [Authorize(Policy = "A08")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteEspecializacao(Guid instituicaoId, Guid especializacaoId)
        {
            return await Ok(_instituiaoService.DesvincularEspecializacao(instituicaoId, especializacaoId));
        }

        /// <summary>
        /// Responsável por associar um serviço de laboratório a uma instituição específica.
        /// </summary>
        /// <returns></returns>
        [HttpPatch("vincular-servico-laboratorio/{instituicaoId}/{servicoLaboratorioId}")]
        [Authorize(Policy = "A09")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PatchLaboratorio(Guid instituicaoId, Guid servicoLaboratorioId)
        {
            return await Ok(_instituiaoService.VincularServicoLaboratorio(instituicaoId, servicoLaboratorioId));
        }

        /// <summary>
        /// Responsável por remover um serviço de laboratório de uma instituição de saúde.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("desvincular-servico-laboratorio/{instituicaoId}/{servicoLaboratorioId}")]
        [Authorize(Policy = "A09")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteLaboratorio(Guid instituicaoId, Guid servicoLaboratorioId)
        {
            return await Ok(_instituiaoService.DesvincularServicoLaboratorio(instituicaoId, servicoLaboratorioId));
        }
    }
}
