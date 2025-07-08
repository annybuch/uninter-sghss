using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Consulta_Entity;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Domain.Entities.Profissional_Saude_Entity;
using Loop.SGHSS.Model.ServicosPrestados;

namespace Loop.SGHSS.Domain.Entities.Servicos_Entity
{
    public class Especializacoes : DefaultEntityIdModel
    {
        public Especializacoes() { }

        public string? Titulo { get; set; }
        public string? Descricao { get; set; }


        // --== Relacionamentos
        public virtual ICollection<Consulta>? Consultas { get; set; }
        public virtual ICollection<Instituicao_Especializacao>? InstituicaoEspecializacao { get; set; }
        public virtual ICollection<ProfissionalSaude_Especializacao>? ProfissionalSaudeEspecializacao { get; set; }


        // --== Atualiza informações.
        public void Atualizar(EspecializacoesModel model)
        {
            if (model != null)
            {
                if (model.Titulo != null)
                    this.Titulo = model.Titulo;
                if (model.Descricao != null)
                    this.Descricao = model.Descricao;
            }
        }
    }
}
