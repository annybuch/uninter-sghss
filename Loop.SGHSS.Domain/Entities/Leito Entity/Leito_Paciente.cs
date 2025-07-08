using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Patient_Entity;
using Loop.SGHSS.Model.Leitos;

namespace Loop.SGHSS.Domain.Entities.Leito_Entity
{
    public class Leito_Paciente : DefaultEntityIdModel
    {
        public Leito_Paciente() { }

        public Leito_Paciente(Paciente? paciente, Guid? pacienteId, Leito? leito, Guid? leitoId)
        {
            Paciente = paciente;
            PacienteId = pacienteId;
            Leito = leito;
            LeitoId = leitoId;
        }

        public DateTime? DataEntrada { get; set; }
        public ICollection<LeitoPacienteObservacao> Observacoes { get; set; } = new List<LeitoPacienteObservacao>();

        public Guid? LeitoId { get; set; }
        public Guid? PacienteId { get; set; }

        public Leito? Leito { get; set; }
        public Paciente? Paciente { get; set; }
    }    
}
