using Loop.SGHSS.Model._Enums.Agenda;

namespace Loop.SGHSS.Model.Agenda
{
    public class InstituicaoAgendaModel
    {
        public Guid? InstituicaoId { get; set; }
        public DiaSemanaEnum DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
    }
}
