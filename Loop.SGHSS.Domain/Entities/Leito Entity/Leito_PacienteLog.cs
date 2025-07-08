using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Patient_Entity;
using Loop.SGHSS.Model.Leitos;

namespace Loop.SGHSS.Domain.Entities.Leito_Entity
{
    public class Leito_PacienteLog : DefaultEntityIdModel
    {
        public Leito_PacienteLog() { }

        public Leito_PacienteLog(Paciente? paciente, Guid? pacienteId, Leito? leito, Guid? leitoId)
        {
            Paciente = paciente;
            PacienteId = pacienteId;
            Leito = leito;
            LeitoId = leitoId;
        }

        public Guid? IdOriginal { get; set; }
        public DateTime? DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public ICollection<LeitoPacienteObservacao> Observacoes { get; set; } = new List<LeitoPacienteObservacao>();

        public Guid? LeitoId { get; set; }
        public Guid? PacienteId { get; set; }

        public Leito? Leito { get; set; }
        public Paciente? Paciente { get; set; }
    }
}
