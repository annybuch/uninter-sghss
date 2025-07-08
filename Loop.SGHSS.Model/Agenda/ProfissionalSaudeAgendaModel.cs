using Loop.SGHSS.Model._Enums.Agenda;
using Loop.SGHSS.Model._Enums.Consulta;

namespace Loop.SGHSS.Model.Agenda
{
    public class ProfissionalSaudeAgendaModel
    {
        public Guid ProfissionalSaudeId { get; set; }
        public Guid InstituicaoId { get; set; }
        public DiaSemanaEnum DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public TimeSpan? HoraInicioAlmoco { get; set; }
        public TimeSpan? HoraFimAlmoco { get; set; }
        public TipoConsultaEnum TipoConsulta { get; set; }
    }
}
