using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Adm_Entity;

namespace Loop.SGHSS.Domain.Entities.Permissao_Entity
{
    public class Permissao_Administrador : DefaultEntityIdModel
    {
        public Permissao_Administrador() { }
        public Permissao_Administrador(Permissao? permissao, Guid? permissaoId, Administrador? administrador, Guid? administradorId)
        {
            Permissao = permissao;
            PermissaoId = permissaoId;
            Administrador = administrador;
            AdministradorId = administradorId;
        }

        public Permissao? Permissao { get; set; }
        public Guid? PermissaoId { get; set; }

        public Administrador? Administrador { get; set; }
        public Guid? AdministradorId { get; set; }
    }
}
