namespace Loop.SGHSS.Model._Configuration
{
    public class LoopSGHSSConfiguration
    {
        public LoopSGHSSConfigurationConnectionStrings? ConnectionStrings { get; set; }
        public LoopSGHSSConfigurationDailyApi? DailyApi { get; set; }
        public ConfiguracaoEmail? Email { get; set; }

        public class LoopSGHSSConfigurationConnectionStrings
        {
            public string? ConnectionMySQL { get; set; }
            public string? ConnectionMongo { get; set; }
        }

        public class LoopSGHSSConfigurationDailyApi
        {
            public string? ApiKey { get; set; }
            public string? ApiUrl { get; set; }
        }

        public class ConfiguracaoEmail
        {
            public string ServidorSmtp { get; set; } = null!;
            public int Porta { get; set; }
            public string NomeRemetente { get; set; } = null!;
            public string EmailRemetente { get; set; } = null!;
            public string Usuario { get; set; } = null!;
            public string Senha { get; set; } = null!;
        }
    }
}
