using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._Enums.Consulta;

namespace Loop.SGHSS.Model._QueryFilter
{
    public class ValoresServicosQueryFilter : IQueryFilter
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public Guid? InstituicaoId { get; set; }
        public Guid? EspecializacaoId { get; set; }
        public Guid? ServicoLaboratorioId { get; set; }
        public TipoConsultaEnum? TipoConsulta { get; set; }

        public bool HasFilters =>
            InstituicaoId.HasValue ||
            EspecializacaoId.HasValue ||
            ServicoLaboratorioId.HasValue ||
            TipoConsulta.HasValue;
    }
}
