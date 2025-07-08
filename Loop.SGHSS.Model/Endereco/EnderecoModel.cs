using Loop.SGHSS.Model._Enums.Estados;

namespace Loop.SGHSS.Model.Endereco
{
    public class EnderecoModel
    {
        public Guid Id { get; set; }
        public string? Cidade { get; set; }
        public EstadosEnum? Logradouro { get; set; }
        public string? Bairro { get; set; }
        public string? CEP { get; set; }
        public string? Numero { get; set; }
    }
}
