using Loop.SGHSS.Model._Enums.Instituicao;
using Loop.SGHSS.Model._Enums.Leitos.Loop.SGHSS.Model.Enums;

namespace Loop.SGHSS.Model.Leitos
{
    public class LeitosModel
    {
        public Guid? Id { get; set; }
        public string? Titulo { get; set; }
        public string? Observacao { get; set; }
        public int? Andar { get; set; }
        public string? Sala { get; set; }
        public string? NumeroLeito { get; set; }
        public statusLeitoEnum? StatusLeito { get; set; }
        public TipoLeitoEnum? TipoLeito { get; set; }
        public Guid? InstituicaoId { get; set; }
    }

    public class LeitoStatus
    {
        public Guid? Id { get; set; }
        public statusLeitoEnum? StatusLeito { get; set; }
    }

    public class LeitosMassa
    {
        public int? NumeroInicial { get; set; }
        public int? NumeroFinal { get; set; }
        public int? Andar { get; set; }
        public List<int>? NumerosCancelados { get; set; }
        public Guid? InstituicaoId { get; set; }
        public TipoLeitoEnum? tipoLeitoEnum { get; set; }
        public statusLeitoEnum? StatusLeito { get; set; }
    }

    public class TicketCadastrarLeitoViewModel
    {
        public TicketCadastrarLeitoViewModel() { }

        /// <summary>
        /// Usado para cadastro de leito em massa
        /// </summary>
        public TicketCadastrarLeitoViewModel(string? numeroLeito, Guid? instutuicaoId, int? andar, TipoLeitoEnum? tipoLeitoEnum, statusLeitoEnum? statusLeitoEnum)
        {
            this.Titulo = $"Leito {numeroLeito}";
            this.NumeroLeito = numeroLeito;
            this.InstutuicaoId = instutuicaoId;
            this.Andar = Andar;
            this.tipoLeitoEnum = tipoLeitoEnum;
            this.StatusLeito = statusLeitoEnum;
        }

        public Guid? InstutuicaoId { get; set; }
        public string? NumeroLeito { get; set; }
        public string? Titulo { get; set; }
        public int? Andar { get; set; }
        public TipoLeitoEnum? tipoLeitoEnum { get; set; }
        public statusLeitoEnum? StatusLeito { get; set; }
    }
}
