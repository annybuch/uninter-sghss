using Loop.SGHSS.Extensions.Paginacao;

namespace Loop.SGHSS.Model._QueryFilter
{
    public class CategoriaQueryFilter : IQueryFilter
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public string? Search { get; set; }

        public bool HasFilters =>
            !string.IsNullOrEmpty(Search);
    }
}
