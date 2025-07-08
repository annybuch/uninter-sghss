using Loop.SGHSS.Extensions.Paginacao;
using Loop.SGHSS.Model._Enums.Consulta;

namespace Loop.SGHSS.Model._QueryFilter
{
    public class ExameGradeQueryFilter : IQueryFilter
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public Guid? PacienteId { get; set; }
        public Guid? ProfissionalSaudeId { get; set; }
        public Guid? ServicoLaboratorioId { get; set; }
        public Guid? InstituicaoId { get; set; }
        public StatusConsultaEnum? StatusExame { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }

        public bool HasFilters =>
            ServicoLaboratorioId.HasValue
            || InstituicaoId.HasValue
            || ProfissionalSaudeId.HasValue
            || StatusExame.HasValue
            || PacienteId.HasValue
            || DataInicial.HasValue
            || DataFinal.HasValue;
    }
}
