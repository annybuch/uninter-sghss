namespace Loop.SGHSS.Model.Monitoramento.Recursos
{
    public class MonitoramentoLeitosModel
    {
        public Guid? InstituicaoId { get; set; }

        // --== Total de leitos no hospital
        public int TotalLeitos { get; set; }

        // --== Leitos atualmente liberados
        public int LeitosDisponiveis { get; set; }

        // --== Leitos atualmente ocupados (em uso por pacientes)
        public int LeitosEmUso { get; set; }

        // --== Leitos que estão em manutenção
        public int LeitosEmManutencao { get; set; }

        // --== Detalhamento por tipo de leito (ex: UTI, Enfermaria, etc)
        public List<LeitosPorTipoModel> LeitosPorTipo { get; set; } = new List<LeitosPorTipoModel>();
    }

    public class LeitosPorTipoModel
    {
        public string TipoLeito { get; set; } = string.Empty;
        public int Total { get; set; }
        public int Disponiveis { get; set; }
        public int EmUso { get; set; }
        public int EmManutencao { get; set; }
    }
}
