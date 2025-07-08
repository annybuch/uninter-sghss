using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Patient_Entity;
using Loop.SGHSS.Domain.Entities.ProfessionalSaude_Entity;

namespace Loop.SGHSS.Domain.Entities.Permissao_Entity
{
    public class Permissao_Paciente : DefaultEntityIdModel
    {
        public Permissao_Paciente() { }
        public Permissao_Paciente(Permissao? permissao, Guid? permissaoId, Paciente? paciente, Guid? pacienteId)
        {
            Permissao = permissao;
            PermissaoId = permissaoId;
            Paciente = paciente;
            PacienteId = pacienteId;
        }

        public Permissao? Permissao { get; set; }
        public Guid? PermissaoId { get; set; }

        public Paciente? Paciente { get; set; }
        public Guid? PacienteId { get; set; }
    }
}
