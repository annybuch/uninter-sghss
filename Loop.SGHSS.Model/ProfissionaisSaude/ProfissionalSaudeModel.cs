using Loop.SGHSS.Model._Enums.Cargos;
using Loop.SGHSS.Model._Enums.Genero;
using Loop.SGHSS.Model.Endereco;

namespace Loop.SGHSS.Model.ProfissionaisSaude
{
    public class ProfissionalSaudeModel : EnderecoModel
    {
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? NumeroRegistro { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? CPF { get; set; }
        public GeneroEnum? Genero { get; set; }
        public CargoProfissionalSaude? CargoProfissionalSaude { get; set; }
        public string? Foto { get; set; }
    }

    public class ProfissionalSaudeViewModel : EnderecoModel
    {
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? NumeroRegistro { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? CPF { get; set; }
        public GeneroEnum? Genero { get; set; }
        public CargoProfissionalSaude? CargoProfissionalSaude { get; set; }
        public string? Foto { get; set; }
    }
}
