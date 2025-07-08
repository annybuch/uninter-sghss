using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Domain.Entities.ProfessionalSaude_Entity;
using Loop.SGHSS.Model._Enums.Agenda;
using Loop.SGHSS.Model._Enums.Consulta;

namespace Loop.SGHSS.Domain.Entities.Agenda_Entiity
{
    public class ProfissionalSaude_Agenda : DefaultEntityIdModel
    {
        public Instituicao? Instituicao { get; set; } = null;
        public ProfissionalSaude? ProfissionalSaude { get; set; }

        public Guid ProfissionalSaudeId { get; set; }
        public Guid? InstituicaoId { get; set; }

        // --== Tipo de consulta desta agenda (Presencial, HomeCare, Teleconsulta).
        public TipoConsultaEnum TipoConsulta { get; set; }

        // --== Dia da semana em que o profissional trabalha nesta instituição.
        public DiaSemanaEnum DiaSemana { get; set; }

        // --== Horário de início do expediente.
        public TimeSpan HoraInicio { get; set; }

        // --== Horário de fim do expediente.
        public TimeSpan HoraFim { get; set; }

        // --== Intervalo de almoço
        public TimeSpan? HoraInicioAlmoco { get; set; }
        public TimeSpan? HoraFimAlmoco { get; set; }
    }
}
