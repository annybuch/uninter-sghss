using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Model._Enums.Agenda;

namespace Loop.SGHSS.Domain.Entities.Agenda_Entiity
{
    public class Instituicao_Agenda : DefaultEntityIdModel
    {
        public Instituicao? Instituicao { get; set; } = null;
        public Guid? InstituicaoId { get; set; }

        // --== Dia da semana em que a instituição funciona.
        public DiaSemanaEnum DiaSemana { get; set; }

        // --== Horário de início do expediente.
        public TimeSpan HoraInicio { get; set; }

        // --== Horário de fim do expediente.
        public TimeSpan HoraFim { get; set; }
    }
}
