using Loop.SGHSS.Model._Enums.Consulta;
using Loop.SGHSS.Model._Enums.Financas;

namespace Loop.SGHSS.Model.Monitoramento.Financas
{
    public class MonitoramentoFinancasModel
    {
        public Guid? HospitalId { get; set; }

        // --== Receita total (consultas + exames)
        public decimal ReceitaTotal { get; set; }

        // --== Total arrecadado apenas com consultas
        public decimal ReceitaConsultas { get; set; }

        // --== Total arrecadado apenas com exames
        public decimal ReceitaExames { get; set; }

        // --== Total de consultas realizadas
        public int TotalConsultas { get; set; }

        // --== Total de exames realizados
        public int TotalExames { get; set; }

        // --== Receita média por dia (considerando o período filtrado)
        public decimal ReceitaMediaDiaria { get; set; }

        // --== Ticket médio por paciente (receita total / (consultas + exames))
        public decimal TicketMedio { get; set; }

        // --== Total de gastos com suprimentos
        public decimal TotalGastosSuprimentos { get; set; }

        // --== Gastos com suprimentos por categoria
        public List<GastoPorCategoria> GastosPorCategoria { get; set; } = new List<GastoPorCategoria>();

        // --== Receita por dia (para gráfico de linha de evolução diária)
        public List<ReceitaPorDia> ReceitaDiaria { get; set; } = new List<ReceitaPorDia>();

        // --== Gastos por tipo de consulta
        public List<GastoPorTipoConsulta> GastosPorTipoConsulta { get; set; } = new List<GastoPorTipoConsulta>();

        // --== Receita por status de pagamento
        public List<ReceitaPorStatusPagamento> ReceitaPorStatusPagamento { get; set; } = new List<ReceitaPorStatusPagamento>();

        // --== Receita por forma de pagamento
        public List<ReceitaPorFormaPagamento> ReceitaPorFormaPagamento { get; set; } = new List<ReceitaPorFormaPagamento>();
    }

    public class GastoPorCategoria
    {
        public string Categoria { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }

    public class ReceitaPorDia
    {
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
    }

    public class GastoPorTipoConsulta
    {
        public TipoConsultaEnum TipoConsulta { get; set; }
        public decimal Valor { get; set; }
    }

    public class ReceitaPorStatusPagamento
    {
        public StatusPagamentoEnum StatusPagamento { get; set; }
        public decimal Valor { get; set; }
    }

    public class ReceitaPorFormaPagamento
    {
        public FormaDePagamentoEnum FormaPagamento { get; set; }
        public decimal Valor { get; set; }
    }
}
