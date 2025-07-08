namespace Loop.SGHSS.Model.Monitoramento.Recursos
{
    public class MonitoramentoSuprimentosModel
    {
        public Guid? InstituicaoId { get; set; }

        // --==  Custo total (soma do custo de todos os itens)
        public decimal CustoTotal { get; set; }

        // --==  Quantidade de itens com estoque crítico (abaixo do nível mínimo)
        public int ItensEstoqueCritico { get; set; }

        // --== Total de produtos comprados (quantidade de unidades compradas)
        public int TotalUnidadesCompradas { get; set; }

        // --== Total de produtos já usados (quantidade de unidades consumidas)
        public int TotalUnidadesUsadas { get; set; }

        // --== Detalhamento por produto
        public List<SuprimentoPorProdutoModel> SuprimentosPorProduto { get; set; } = new List<SuprimentoPorProdutoModel>();
    }

    public class SuprimentoPorProdutoModel
    {
        public string NomeProduto { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;

        // --== Quantidade total comprada na última compra
        public int QuantidadeComprada { get; set; }

        // --== Quantidade total já usada/consumida da última compra
        public int QuantidadeUsada { get; set; }

        // --== Quantidade atual em estoque (saldo)
        public int QuantidadeEmEstoque { get; set; }

        // --== Custo total da última compra.
        public decimal CustoTotalComprado { get; set; }

        // --== Custo por unidade da última compra
        public decimal CustoPorUnidade { get; set; }

        // --== Total de itens em estoque
        public int TotalItensEstoque { get; set; }
    }
}
