using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.ProfessionalSaude_Entity;
using Loop.SGHSS.Domain.Entities.Servicos_Entity;

namespace Loop.SGHSS.Domain.Entities.Profissional_Saude_Entity
{
    public class ProfissionalSaude_ServicoLaboratorio : DefaultEntityIdModel
    {
        public ProfissionalSaude_ServicoLaboratorio() {}
        public ProfissionalSaude_ServicoLaboratorio(ServicosLaboratorio? servicosLaboratorio, Guid? servicosLaboratorioId, ProfissionalSaude? profissionalSaude, Guid? profissionalSaudeId)
        {
            ServicosLaboratorio = servicosLaboratorio;
            ServicosLaboratorioId = servicosLaboratorioId;
            ProfissionalSaude = profissionalSaude;
            ProfissionalSaudeId = profissionalSaudeId;
        }

        public ServicosLaboratorio? ServicosLaboratorio { get; set; }
        public ProfissionalSaude? ProfissionalSaude { get; set; }

        public Guid? ProfissionalSaudeId { get; set; }
        public Guid? ServicosLaboratorioId { get; set; }
    }
}
