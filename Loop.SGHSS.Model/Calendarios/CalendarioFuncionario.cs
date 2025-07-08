using Loop.SGHSS.Model._Enums.Consulta;

namespace Loop.SGHSS.Model.Calendarios
{
    public class CalendarioFuncionario
    {
        public Guid? ProfissionalSaudeId { get; set; }
        public string? ProfissionalSaude { get; set; }
        public Guid? InstituicaoSaudeId { get; set; }
        public string? InstituicaoSaude { get; set; }
        public DateOnly? DataMarcada { get; set; }
        public TimeOnly? HoraMarcada { get; set; }
        public TipoConsultaEnum? TipoConsulta { get; set; }
        public StatusConsultaEnum? StatusConsulta { get; set; }
    }
}
