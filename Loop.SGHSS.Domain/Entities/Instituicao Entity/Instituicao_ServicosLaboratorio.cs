using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Servicos_Entity;

namespace Loop.SGHSS.Domain.Entities.Instituicao_Entity
{
    public class Instituicao_ServicosLaboratorio : DefaultEntityIdModel
    {
        public Instituicao_ServicosLaboratorio() { }
        public Instituicao_ServicosLaboratorio(Guid? instituicaoId, Guid? servicosLaboratorioId, Instituicao? instituicao, ServicosLaboratorio? servicosLaboratorio)
        {
            InstituicaoId = instituicaoId;
            ServicosLaboratorioId = servicosLaboratorioId;
            Instituicao = instituicao;
            ServicosLaboratorio = servicosLaboratorio;
        }

        public Guid? InstituicaoId { get; set; }
        public Guid? ServicosLaboratorioId { get; set; }

        // --== Relacionamentos
        public Instituicao? Instituicao { get; set; }
        public ServicosLaboratorio? ServicosLaboratorio { get; set; }
    }
}
