using Loop.SGHSS.Model._Enums.Genero;
using Loop.SGHSS.Model.Endereco;

namespace Loop.SGHSS.Model.Pacientes
{
    public class PacientesModel : EnderecoModel
    {
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? CPF { get; set; }
        public GeneroEnum? Genero { get; set; }
        public string? Foto { get; set; }
    }

    public class PacientesViewModel : EnderecoModel
    {
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? CPF { get; set; }
        public GeneroEnum? Genero { get; set; }
        public string? Foto { get; set; }
    }
}
