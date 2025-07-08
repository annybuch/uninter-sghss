using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._Enums.Consulta;

namespace Loop.SGHSS.Model._QueryFilter
{
    public class ConsultaProfissionalQueryFilter : IQueryFilter
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public Guid? ProfissionalSaudeId { get; set; }
        public Guid? EspecializacaoId { get; set; }
        public Guid? InstituicaoId { get; set; }
        public StatusConsultaEnum? StatusConsulta { get; set; }
        public TipoConsultaEnum? TipoConsulta { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }

        public bool HasFilters =>
            EspecializacaoId.HasValue
            || InstituicaoId.HasValue
            || StatusConsulta.HasValue
            || TipoConsulta.HasValue
            || ProfissionalSaudeId.HasValue
            || DataInicial.HasValue
            || DataFinal.HasValue;
    }
}
