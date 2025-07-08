using Loop.SGHSS.Domain.Defaults;

namespace Loop.SGHSS.Domain.Entities.Permissao_Entity
{
    public class Permissao : DefaultEntityIdModel
    {
        public Permissao() { }
        public string? Codigo { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }

        // --== Relacionamentos
        public Permissao_Funcionario? PermissoesFuncionario { get; set; }
        public Permissao_ProfissionalSaude? PermissoesProfissionalSaude { get; set; }
        public Permissao_Administrador? PermissoesAdministrador { get; set; }
        public Permissao_Paciente? PermissoesPaciente { get; set; }
    }
}
