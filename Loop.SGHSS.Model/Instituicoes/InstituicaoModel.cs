using Loop.SGHSS.Model._Enums.Instituicao;
using Loop.SGHSS.Model.Endereco;

namespace Loop.SGHSS.Model.Instituicoes
{
    public class InstituicaoModel : EnderecoModel
    {
        public string? NomeFantasia { get; set; }
        public string? RazaoSocial { get; set; }
        public string? CNPJ { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public int? IntervaloMinutos { get; set; }
        public TipoInstituicaoEnum? TipoInstituicao { get; set; }
    }
}
