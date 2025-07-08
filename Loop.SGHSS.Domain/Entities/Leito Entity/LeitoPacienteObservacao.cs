using Loop.SGHSS.Domain.Defaults;

namespace Loop.SGHSS.Domain.Entities.Leito_Entity
{
    public class LeitoPacienteObservacao : DefaultEntityIdModel
    {
        public Guid LeitosPacientesId { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;


        public Leito_Paciente? LeitosPacientes { get; set; }
    }
}
