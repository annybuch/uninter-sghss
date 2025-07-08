using Loop.SGHSS.Extensions.Paginacao;

namespace Loop.SGHSS.Model._QueryFilter
{
    public class PacienteQueryFilter : IQueryFilter
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public bool HasFilters { get; set; }
    }
}
