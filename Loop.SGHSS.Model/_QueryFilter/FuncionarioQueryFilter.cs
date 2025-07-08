using Loop.SGHSS.Extensions.Paginacao;

namespace Loop.SGHSS.Model._QueryFilter
{
    public class FuncionarioQueryFilter : IQueryFilter
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public Guid? InstituicaoId { get; set; }

        public bool HasFilters => InstituicaoId.HasValue;
    }
}