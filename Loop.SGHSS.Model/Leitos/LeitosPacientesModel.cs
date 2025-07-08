namespace Loop.SGHSS.Model.Leitos
{
    public class LeitosPacientesModel
    {
        public Guid? Id { get; set; }
        public DateTime? DataEntrada { get; set; }
        public ICollection<LeitosPacientesObservacaoModel> Observacoes { get; set; } = new List<LeitosPacientesObservacaoModel>();

        public Guid? PacienteId { get; set; }
        public Guid? LeitoId { get; set; }
    }

    public class LeitosPacientesObservacaoModel
    {
        public Guid? Id { get; set; } 
        public Guid? LeitosPacientesId { get; set; }
        public string? Observacao { get; set; } = string.Empty;
        public DateTime? DataCriacao { get; set; }
    }

    public class AddPacienteLeitoModel
    {
        public string Observacao { get; set; } = string.Empty;
        public Guid? PacienteId { get; set; }
        public Guid? LeitoId { get; set; }
    }
}
