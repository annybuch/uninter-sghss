using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Funcionario_Entity;

namespace Loop.SGHSS.Domain.Entities.Permissao_Entity
{
    public class Permissao_Funcionario : DefaultEntityIdModel
    {
        public Permissao_Funcionario() { }
        public Permissao_Funcionario(Permissao? permissao, Guid? permissaoId, Funcionario? funcionario, Guid? funcionarioId)
        {
            Permissao = permissao;
            PermissaoId = permissaoId;
            Funcionario = funcionario;
            FuncionarioId = funcionarioId;
        }

        public Permissao? Permissao { get; set; }
        public Guid? PermissaoId { get; set; }

        public Funcionario? Funcionario { get; set; }
        public Guid? FuncionarioId { get; set; }
    }
}
