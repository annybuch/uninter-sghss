using Loop.SGHSS.Model.Pacientes;

namespace Loop.SGHSS.Model.Leitos
{
    public class LeitosPacientesLogModel
    {
        public Guid? Id { get; set; }
        public Guid? IdOriginal { get; set; }
        public DateTime? DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public ICollection<LeitosPacientesObservacaoModel> Observacoes { get; set; } = new List<LeitosPacientesObservacaoModel>();
        public Guid? PacienteId { get; set; }
        public Guid? LeitoId { get; set; }
    }


    public class LeitoComPacienteModel
    {
        public LeitosModel Leito { get; set; } = null!;
        public PacientesViewModel? Paciente { get; set; }
        public List<LeitoPacienteObservacaoModel>? Observacoes { get; set; }
    }

    public class LeitoPacienteObservacaoModel
    {
        public Guid Id { get; set; }
        public Guid LeitosPacientesId { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
    }
}
