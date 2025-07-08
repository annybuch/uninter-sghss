using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.ProfessionalSaude_Entity;

namespace Loop.SGHSS.Domain.Entities.Permissao_Entity
{
    public class Permissao_ProfissionalSaude : DefaultEntityIdModel
    {
        public Permissao_ProfissionalSaude() { }
        public Permissao_ProfissionalSaude(Permissao? permissao, Guid? permissaoId, ProfissionalSaude? profissionalSaude, Guid? profissionalSaudeId)
        {
            Permissao = permissao;
            PermissaoId = permissaoId;
            ProfissionalSaude = profissionalSaude;
            ProfissionalSaudeId = profissionalSaudeId;
        }

        public Permissao? Permissao { get; set; }
        public Guid? PermissaoId { get; set; }

        public ProfissionalSaude? ProfissionalSaude { get; set; }
        public Guid? ProfissionalSaudeId { get; set; }
    }
}
