using Loop.SGHSS.Model._Enums.Cargos;
using Loop.SGHSS.Model._Enums.Genero;
using Loop.SGHSS.Model.Endereco;

namespace Loop.SGHSS.Model.Funcionarios
{
    public class FuncionarioModel : EnderecoModel
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
        public CargoFuncionario? CargoFuncionario { get; set; }
        public Guid? InstituicaoId { get; set; }
    }

    public class FuncionarioViewModel : EnderecoModel
    {
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? CPF { get; set; }
        public GeneroEnum? Genero { get; set; }
        public string? Foto { get; set; }
        public CargoFuncionario? CargoFuncionario { get; set; }
        public Guid? InstituicaoId { get; set; }
    }
}
