using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Domain.Entities.Patient_Entity;
using Loop.SGHSS.Domain.Entities.ProfessionalSaude_Entity;
using Loop.SGHSS.Domain.Entities.Servicos_Entity;
using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Model._Enums.Financas;
using Loop.SGHSS.Model._Enums.Consulta;

namespace Loop.SGHSS.Domain.Entities.Exame_Entity
{
    public class Exame : DefaultEntityIdModel
    {
        public Exame() { }
        /// <summary>
        /// Anotações médicas
        /// </summary>
        public string? Anotacoes { get; set; }

        /// <summary>
        /// Data marcada do exame
        /// </summary>
        public DateTime DataMarcada { get; set; }

        /// <summary>
        /// Data que realmente foi realizado o exame
        /// </summary>
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        /// <summary>
        /// Guia do médico que solicitou o procedimento.
        /// </summary>
        public byte[]? GuiaMedico { get; set; }

        /// <summary>
        /// Resultado do exame.
        /// </summary>
        public byte[]? Resultado { get; set; }

        /// <summary>
        /// Status do exame (pendente, realizada, emAtendimento, cancelada)
        /// </summary>
        public StatusConsultaEnum StatusExame{ get; set; }

        /// <summary>
        /// Valor do exame.
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Pagamento do exame.
        /// </summary>
        public StatusPagamentoEnum StatusPagamento { get; set; }
        public FormaDePagamentoEnum FormaDePagamento { get; set; }


        // --== Chaves estrangeiras (FKs) ==--
        public ServicosLaboratorio? servicoLaboratorio { get; set; }
        public Guid? servicoLaboratorioId { get; set; }

        public ProfissionalSaude? ProfissionalSaude { get; set; }
        public Guid? ProfissionalSaudeId { get; set; }

        public Instituicao? Instituicao { get; set; }
        public Guid? InstituicaoId { get; set; }

        public Paciente? Paciente { get; set; }
        public Guid? PacienteId { get; set; }
    }
}