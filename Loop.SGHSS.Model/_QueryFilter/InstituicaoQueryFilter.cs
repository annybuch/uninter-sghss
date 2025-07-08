using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._Enums.Estados;

namespace Loop.SGHSS.Model._QueryFilter
{
    public class InstituicaoQueryFilter : IQueryFilter
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public EstadosEnum? Logradouro { get; set; }
        public string? Cidade { get; set; }

        public bool HasFilters =>
         Logradouro.HasValue ||
         !string.IsNullOrEmpty(Cidade);
    }
}
