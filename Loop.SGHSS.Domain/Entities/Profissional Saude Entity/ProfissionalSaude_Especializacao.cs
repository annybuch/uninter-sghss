using Loop.SGHSS.Domain.Entities.Servicos_Entity;
using Loop.SGHSS.Domain.Entities.ProfessionalSaude_Entity;
using Loop.SGHSS.Domain.Defaults;

namespace Loop.SGHSS.Domain.Entities.Profissional_Saude_Entity
{
    public class ProfissionalSaude_Especializacao : DefaultEntityIdModel
    {
        public ProfissionalSaude_Especializacao() { }

        public ProfissionalSaude_Especializacao(Especializacoes? especializacao, Guid? especializacaoId, ProfissionalSaude? profissionalSaude, Guid? profissionalSaudeId)
        {
            Especializacoes = especializacao;
            EspecializacaoId = especializacaoId;
            ProfissionalSaude = profissionalSaude;
            ProfissionalSaudeId = profissionalSaudeId;
        }

        public Guid? EspecializacaoId { get; set; }
        public Guid? ProfissionalSaudeId { get; set; }

        public Especializacoes? Especializacoes { get; set; }
        public ProfissionalSaude? ProfissionalSaude { get; set; }
    }
}
