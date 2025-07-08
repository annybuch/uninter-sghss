using Loop.SGHSS.Model._Enums.Cargos;

namespace Loop.SGHSS.Model.Identidade
{
    public abstract class SGHSSBaseUserTokenModel
    {
        public Guid UserId { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; } 
        public string? UserType { get; set; } 
        public List<string>? Permissions { get; set; }
    }

    public class SGHSSUserTokenFuncionarioModel : SGHSSBaseUserTokenModel
    {
        public CargoFuncionario? Cargo { get; set; }
        public Guid? InstituicaoId { get; set; } 
    }

    public class SGHSSUserTokenProfissionalSaudeModel : SGHSSBaseUserTokenModel
    {
        public CargoProfissionalSaude? Cargo { get; set; }
    }

    public class SGHSSUserTokenPacienteModel : SGHSSBaseUserTokenModel
    {
        public string? CPF { get; set; } 
    }

    public class SGHSSUserTokenAdministradorModel : SGHSSBaseUserTokenModel
    {
        public Guid? InstituicaoId { get; set; }
    }
}
