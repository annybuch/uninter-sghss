using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._Enums.Instituicao;
using Loop.SGHSS.Model._Enums.Leitos.Loop.SGHSS.Model.Enums;

namespace Loop.SGHSS.Model._QueryFilter
{
    public class LeitoQueryFilter : IQueryFilter
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public statusLeitoEnum? Status { get; set; }
        public TipoLeitoEnum? TipoLeito { get; set; }
        public Guid? InstituicaoId { get; set; }
        public int? Andar { get; set; }

        public bool HasFilters =>
            Status.HasValue ||
            TipoLeito.HasValue ||
            InstituicaoId.HasValue ||
            Andar.HasValue;
    }
}
