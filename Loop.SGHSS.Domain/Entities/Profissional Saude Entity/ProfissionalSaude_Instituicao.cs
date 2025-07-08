using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Domain.Entities.ProfessionalSaude_Entity;
using Loop.SGHSS.Model._Enums.Agenda;

namespace Loop.SGHSS.Domain.Entities.Profissional_Saude_Entity
{
    public class ProfissionalSaude_Instituicao : DefaultEntityIdModel
    {
        public ProfissionalSaude_Instituicao() { }
        public ProfissionalSaude_Instituicao(Instituicao? instituicao, Guid? instituicaoId, ProfissionalSaude? profissionalSaude, Guid? profissionalSaudeId)
        {
            Instituicao = instituicao;
            InstituicaoId = instituicaoId;
            ProfissionalSaude = profissionalSaude;
            ProfissionalSaudeId = profissionalSaudeId;
        }

        public Instituicao? Instituicao { get; set; }
        public ProfissionalSaude? ProfissionalSaude { get; set; }

        public Guid? ProfissionalSaudeId { get; set; }
        public Guid? InstituicaoId { get; set; }
    }
}
