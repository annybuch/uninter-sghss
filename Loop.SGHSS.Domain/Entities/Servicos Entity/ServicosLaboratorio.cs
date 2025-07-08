using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Exame_Entity;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Domain.Entities.Profissional_Saude_Entity;
using Loop.SGHSS.Model.ServicosPrestados;

namespace Loop.SGHSS.Domain.Entities.Servicos_Entity
{
    public class ServicosLaboratorio : DefaultEntityIdModel
    {
        public ServicosLaboratorio() { }

        public string? Titulo { get; set; }
        public string? Descricao { get; set; }

        /// <summary>
        /// Recomendações médicas importantes pré ou pós exame.
        /// </summary>
        public string? Recomendacao { get; set; }

        // --== Relacionamento
        public virtual ICollection<Exame>? Exames { get; set; }
        public virtual ICollection<Instituicao_ServicosLaboratorio>? InstituicaoServicosLaboratorio { get; set; }
        public virtual ICollection<ProfissionalSaude_ServicoLaboratorio>? ProfissionalSaudeServicoLaboratorio { get; set; }


        // --== Atualiza informações.
        public void Atualizar(ServicosLaboratorioModel model)
        {
            if (model != null)
            {
                if (model.Titulo != null)
                    this.Titulo = model.Titulo;
                if (model.Descricao != null)
                    this.Descricao = model.Descricao;
                if (model.Recomendacao != null)
                    this.Recomendacao = model.Recomendacao;
            }
        }
    }
}
