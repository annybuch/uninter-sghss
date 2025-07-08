using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._Enums.Consulta;

namespace Loop.SGHSS.Model._QueryFilter
{
    public class ConsultaPacienteQueryFilter : IQueryFilter
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public Guid? PacienteId { get; set; }
        public Guid? EspecializacaoId { get; set; }
        public Guid? InstituicaoId { get; set; }
        public Guid? ProfissionalSaudeId { get; set; }
        public StatusConsultaEnum? StatusConsulta { get; set; }
        public TipoConsultaEnum? TipoConsulta { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }

        public bool HasFilters =>
            EspecializacaoId.HasValue
            || InstituicaoId.HasValue
            || ProfissionalSaudeId.HasValue
            || StatusConsulta.HasValue
            || TipoConsulta.HasValue
            || PacienteId.HasValue
            || DataInicial.HasValue
            || DataFinal.HasValue;
    }
}
