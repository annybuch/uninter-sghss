using Loop.SGHSS.Model._Enums.Consulta;
using Loop.SGHSS.Model._Enums.Financas;
using Loop.SGHSS.Model.Consultas;

namespace Loop.SGHSS.Model.Exames
{
    public class ExameModel
    {
        public Guid? Id { get; set; }
        public string? Anotacoes { get; set; }
        public DateTime DataMarcada { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public byte[]? GuiaMedico { get; set; }
        public byte[]? Resultado { get; set; }
        public StatusConsultaEnum StatusExame { get; set; }
        public decimal Valor { get; set; }
        public StatusPagamentoEnum StatusPagamento { get; set; }
        public FormaDePagamentoEnum FormaDePagamento { get; set; }
        public Guid? servicoLaboratorioId { get; set; }
        public Guid? ProfissionalSaudeId { get; set; }
        public Guid? InstituicaoId { get; set; }
        public Guid? PacienteId { get; set; }
    }

    public class ExameGradePacienteModel
    {
        public Guid? Id { get; set; }
        public DateTime DataMarcada { get; set; }
        public StatusConsultaEnum StatusExame { get; set; }
        public string? NomeServicoLaboratorio { get; set; }
        public string? NomeProfissionalSaude { get; set; }
        public string? NomeInstituicao { get; set; }
    }

    public class ExameGradeProfissionalSaudeModel
    {
        public Guid? Id { get; set; }
        public DateTime DataMarcada { get; set; }
        public StatusConsultaEnum StatusExame { get; set; }
        public string? NomeServicoLaboratorio { get; set; }
        public string? NomePaciente { get; set; }
        public string? NomeInstituicao { get; set; }
    }

    public class ExameGradeGeralModel
    {
        public Guid? Id { get; set; }
        public DateTime DataMarcada { get; set; }
        public StatusConsultaEnum StatusExame { get; set; }
        public string? NomeServicoLaboratorio { get; set; }
        public string? NomeProfissionalSaude { get; set; }
        public string? NomePaciente { get; set; }
        public string? NomeInstituicao { get; set; }
    }

    public class PacienteComConsultasModel
    {
        public Guid PacienteId { get; set; }
        public string? NomePaciente { get; set; } 
        public List<ConsultaGradePacienteModel> Consultas { get; set; } = new();
    }

    public class PacienteComExamesModel
    {
        public Guid PacienteId { get; set; }
        public string? NomePaciente { get; set; }
        public List<ExameGradePacienteModel> Exames { get; set; } = new();
    }

}
