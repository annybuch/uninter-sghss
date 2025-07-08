using Loop.SGHSS.Model._Enums.Cargos;
using Loop.SGHSS.Model._Enums.Genero;
using Loop.SGHSS.Model.Endereco;

namespace Loop.SGHSS.Model.Adm
{
    public class AdministradorModel : EnderecoModel
    {
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? CPF { get; set; }
        public GeneroEnum? Genero { get; set; }
        public CargoFuncionario? CargoAdm { get; set; }
        public Guid? InstituicaoId { get; set; } = null;
    }

    public class AdministradorViewModel
    {
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public GeneroEnum? Genero { get; set; }
        public CargoFuncionario? CargoAdm { get; set; }
        public Guid? InstituicaoId { get; set; } = null;
    }
}
