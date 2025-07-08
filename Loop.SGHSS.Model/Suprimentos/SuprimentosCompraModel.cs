namespace Loop.SGHSS.Model.Suprimentos
{
    public class SuprimentosCompraModel
    {
        public Guid? Id { get; set; }
        public string? Descricao { get; set; }
        public string? Codigo { get; set; }
        public string? Marca { get; set; }
        public DateTime? DataComprada { get; set; }
        public decimal? ValorPago { get; set; }
        public int? QuantidadeComprada { get; set; }
        public int? QuantidadeSaida { get; set; }
        public Guid? SuprimentoId { get; set; }
    }

    public class EstoqueModel
    {
        public Guid? SuprimentoCompraId { get; set; }
        public int? QuantidadeSaida { get; set; }
    }
}
