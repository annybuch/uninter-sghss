using Loop.SGHSS.API.Default;
using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._QueryFilter;
using Loop.SGHSS.Model.Suprimentos;
using Loop.SGHSS.Services.Suprimentos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loop.SGHSS.API.Controllers._10___Suprimentos
{
    [ApiController, Route("api/v1/suprimento"),
    Tags("10. 💊 Suprimentos, gestão e cadastro de suprimentos")]
    public class SuprimentoController : LoopContoller
    {
        private readonly ISuprimentoService _suprimentoService;

        public SuprimentoController(ISuprimentoService suprimentoService)
        {
            _suprimentoService = suprimentoService;
        }


        #region 📦 Categorias

        /// <summary>
        /// Responsável por cadastrar uma nova categoria de suprimento na VidaPlus.
        /// </summary>
        [HttpPost("cadastrar-categoria")]
        [Authorize(Policy = "B07")]
        public async Task<IActionResult> PostCategoria([FromBody] CategoriaSuprimentosModel model)
        {
            return await Created(_suprimentoService.CadastrarCategoria(model));
        }

        /// <summary>
        /// Responsável por retornar todas as categorias de suprimentos paginadas.
        /// </summary>
        [HttpGet("categoria")]
        [Authorize(Policy = "B07")]
        [ProducesResponseType(typeof(PagedResult<CategoriaSuprimentosModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoria([FromQuery] CategoriaQueryFilter filter)
        {
            return Ok(await _suprimentoService.ObterCategoriasPaginadas(filter));
        }

        /// <summary>
        /// Responsável por retornar todas as categorias com seus respectivos suprimentos e quantidade.
        /// </summary>
        [HttpGet("categoria/com-suprimentos")]
        [Authorize(Policy = "B07")]
        [ProducesResponseType(typeof(PagedResult<CategoriaComSuprimentosViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoriaComSuprimentos([FromQuery] CategoriaQueryFilter filter)
        {
            return Ok(await _suprimentoService.ObterCategoriasComSuprimentosPaginadas(filter));
        }

        /// <summary>
        /// Responsável por retornar uma categoria de suprimento por id.
        /// </summary>
        [HttpGet("categoria/{categoriaId}")]
        [Authorize(Policy = "B07")]
        [ProducesResponseType(typeof(CategoriaSuprimentosModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoria(Guid categoriaId)
        {
            return Ok(await _suprimentoService.BuscarCategoriaPorId(categoriaId));
        }

        /// <summary>
        /// Responsável por editar as informações de uma categoria de suprimento.
        /// </summary>
        [HttpPut("editar-categoria")]
        [Authorize(Policy = "B07")]
        [ProducesResponseType(typeof(CategoriaSuprimentosModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> PutCategoria([FromBody] CategoriaSuprimentosModel model)
        {
            return Ok(await _suprimentoService.EditarCategoria(model));
        }

        #endregion


        #region 💊 Suprimentos

        /// <summary>
        /// Responsável por cadastrar um novo suprimento na VidaPlus.
        /// </summary>
        [HttpPost("cadastrar-suprimento")]
        [Authorize(Policy = "B07")]
        public async Task<IActionResult> PostSuprimento([FromBody] SuprimentosModel model)
        {
            return await Created(_suprimentoService.CadastrarSuprimento(model));
        }

        /// <summary>
        /// Responsável por retornar todos os suprimentos paginados.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "B07")]
        [ProducesResponseType(typeof(PagedResult<SuprimentosModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] SuprimentoQueryFilter filter)
        {
            return Ok(await _suprimentoService.ObterSuprimentosPaginados(filter));
        }

        /// <summary>
        /// Responsável por retornar um suprimento por id.
        /// </summary>
        [HttpGet("{suprimentoId}")]
        [Authorize(Policy = "B07")]
        [ProducesResponseType(typeof(SuprimentosModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid suprimentoId)
        {
            return Ok(await _suprimentoService.BuscarSuprimentoPorId(suprimentoId));
        }

        /// <summary>
        /// Responsável por editar as informações de um suprimento.
        /// </summary>
        [HttpPut("editar-suprimento")]
        [Authorize(Policy = "B07")]
        [ProducesResponseType(typeof(SuprimentosModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> PutSuprimento([FromBody] SuprimentosModel model)
        {
            return Ok(await _suprimentoService.EditarSuprimento(model));
        }

        #endregion]


        #region 💰 Compras de Suprimento

        [HttpPost("cadastrar-compra")]
        [Authorize(Policy = "D07")]
        public async Task<IActionResult> PostCompra([FromBody] SuprimentosCompraModel model)
        {
            return await Created(_suprimentoService.CadastrarCompraSuprimento(model));
        }

        [HttpGet("compra")]
        [Authorize(Policy = "D07")]
        [ProducesResponseType(typeof(PagedResult<SuprimentosCompraModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompras([FromQuery] SuprimentoQueryFilter filter)
        {
            return Ok(await _suprimentoService.ObterCompraSuprimentosPaginados(filter));
        }

        [HttpGet("compra/{compraId}")]
        [Authorize(Policy = "D07")]
        [ProducesResponseType(typeof(SuprimentosCompraModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompra(Guid compraId)
        {
            return Ok(await _suprimentoService.BuscarCompraSuprimentoPorId(compraId));
        }

        [HttpPut("editar-compra")]
        [Authorize(Policy = "D07")]
        [ProducesResponseType(typeof(SuprimentosCompraModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> PutCompra([FromBody] SuprimentosCompraModel model)
        {
            return Ok(await _suprimentoService.EditarCompra(model));
        }

        #endregion


        #region 🔻 Consumo de Suprimentos

        /// <summary>
        /// Atualiza a quantidade de saída de um suprimento.
        /// </summary>
        [HttpPut("consumir")]
        [Authorize(Policy = "D07")]
        public async Task<IActionResult> ConsumirSuprimento([FromBody] EstoqueModel model)
        {
            await _suprimentoService.ConsumirSuprimento(model);
            return Ok("Quantidade de saída atualizada com sucesso.");
        }

        #endregion


        #region 📊 Grade com categorias, suprimentos e compras

        [HttpGet("grade-completa")]
        [Authorize(Policy = "B07")]
        public async Task<IActionResult> GetGradeCompleta()
        {
            var resultado = await _suprimentoService.ObterGradeCategoriasSuprimentos();
            return Ok(resultado);
        }

        #endregion
    }
}
