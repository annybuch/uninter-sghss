using Loop.SGHSS.Model._Enums.Consulta;
using Loop.SGHSS.Model._Enums.Financas;

namespace Loop.SGHSS.Model.Consultas
{
    public class ConsultaModel
    {
        public Guid? Id { get; set; }
        public string? Anotacoes { get; set; }
        public DateTime DataMarcada { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public byte[]? Prescricao { get; set; }
        public byte[]? Receita { get; set; }
        public byte[]? GuiaMedico { get; set; }
        public TipoConsultaEnum TipoConsulta { get; set; }
        public string? UrlSalaVideo { get; set; }
        public StatusConsultaEnum StatusConsulta { get; set; }
        public decimal Valor { get; set; }
        public StatusPagamentoEnum StatusPagamento { get; set; }
        public FormaDePagamentoEnum FormaDePagamento { get; set; }
        public Guid? EspecializacaoId { get; set; }
        public Guid? ProfissionalSaudeId { get; set; }
        public Guid? InstituicaoId { get; set; }
        public Guid? PacienteId { get; set; }
    }

    public class ConsultaGradePacienteModel
    {
        public Guid? Id { get; set; }
        public DateTime DataMarcada { get; set; }
        public TipoConsultaEnum TipoConsulta { get; set; }
        public StatusConsultaEnum StatusConsulta { get; set; }
        public string? NomeEspecializacao { get; set; }
        public string? NomeProfissionalSaude { get; set; }
        public string? NomeInstituicao { get; set; }
    }

    public class ConsultaGradeProfissionalSaudeModel
    {
        public Guid? Id { get; set; }
        public DateTime DataMarcada { get; set; }
        public TipoConsultaEnum TipoConsulta { get; set; }
        public StatusConsultaEnum StatusConsulta { get; set; }
        public string? NomeEspecializacao { get; set; }
        public string? NomePaciente { get; set; }
        public string? NomeInstituicao { get; set; }
    }

    public class ConsultaGradeGeralModel
    {
        public Guid? Id { get; set; }
        public DateTime DataMarcada { get; set; }
        public TipoConsultaEnum TipoConsulta { get; set; }
        public StatusConsultaEnum StatusConsulta { get; set; }
        public string? NomeEspecializacao { get; set; }
        public string? NomeProfissionalSaude { get; set; }
        public string? NomePaciente { get; set; }
        public string? NomeInstituicao { get; set; }
    }

    public class MarcarConsultaModel
    {
        public DateTime DataMarcada { get; set; }
        public TipoConsultaEnum TipoConsulta { get; set; }
        public decimal Valor { get; set; }
        public FormaDePagamentoEnum FormaDePagamento { get; set; }
        public Guid? EspecializacaoId { get; set; }
        public Guid? ProfissionalSaudeId { get; set; }
        public Guid? InstituicaoId { get; set; }
        public Guid? PacienteId { get; set; }
    }

    public class AgendamentoSimples
    {
        public Guid ProfissionalSaudeId { get; set; }
        public DateTime DataMarcada { get; set; }
    }

    public class ProfissionalComHorariosModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public Dictionary<DateTime, List<TimeSpan>> HorariosDisponiveisPorData { get; set; } = new();
    }

    public class DailyRoomResponse
    {
        public string name { get; set; } = "";
        public string url { get; set; } = "";
    }
}
