using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Domain.Entities.Patient_Entity;
using Loop.SGHSS.Domain.Entities.ProfessionalSaude_Entity;
using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Servicos_Entity;
using Loop.SGHSS.Model._Enums.Consulta;
using Loop.SGHSS.Model._Enums.Financas;

namespace Loop.SGHSS.Domain.Entities.Consulta_Entity
{
    public class Consulta : DefaultEntityIdModel
    {
        public Consulta() { }
        /// <summary>
        /// Anotações médicas
        /// </summary>
        public string? Anotacoes { get; set; }

        /// <summary>
        /// Hora marcada da consulta
        /// </summary>
        public DateTime DataMarcada { get; set; }

        /// <summary>
        /// Data que realmente foi realizada a consulta
        /// </summary>
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        /// <summary>
        /// Documentos de prescições médicas da consulta.
        /// </summary>
        public byte[]? Prescricao { get; set; }
        public byte[]? Receita { get; set; }
        public byte[]? GuiaMedico { get; set; }

        /// <summary>
        /// Se a consulta é homeCare, presencial ou teleConsulta
        /// </summary>
        public TipoConsultaEnum TipoConsulta { get; set; }

        /// <summary>
        /// Caso seja uma TeleConsulta, o link da chamada.
        /// </summary>
        public string? UrlSalaVideo { get; set; }

        /// <summary>
        /// Status da consulta (pendente, realizada, emAtendimento, cancelada)
        /// </summary>
        public StatusConsultaEnum StatusConsulta { get; set; }

        /// <summary>
        /// Identifica se já foi enviado a notificação de 2 horas antes.
        /// </summary>
        public bool? Notificacao2horas { get; set; }

        /// <summary>
        /// Identifica se já foi enviado a notificação de 24 horas antes.
        /// </summary>
        public bool? Notificacao24horas { get; set; }

        /// <summary>
        /// Valor da consulta.
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Pagamento da consulta.
        /// </summary>
        public StatusPagamentoEnum StatusPagamento { get; set; }
        public FormaDePagamentoEnum FormaDePagamento { get; set; }


        // --== Chaves estrangeiras (FKs) ==--
        public Especializacoes? Especializacao { get; set; }
        public Guid? EspecializacaoId { get; set; }

        public ProfissionalSaude? ProfissionalSaude { get; set; }
        public Guid ProfissionalSaudeId { get; set; }

        public Instituicao? Instituicao { get; set; } = null;
        public Guid? InstituicaoId { get; set; }

        public Paciente? Paciente { get; set; }
        public Guid? PacienteId { get; set; }
    }
}
