using Loop.SGHSS.Model._Enums.Consulta;

namespace Loop.SGHSS.Model.Calendarios
{
    public class CalendarioProfissionalSaude
    {
        public Guid? PacienteId { get; set; }
        public string? Paciente { get; set; }
        public DateOnly? DataMarcada { get; set; }
        public TimeOnly? HoraMarcada { get; set; }
        public TipoConsultaEnum? TipoConsulta { get; set; }
        public StatusConsultaEnum? StatusConsulta { get; set; }
    }
}
