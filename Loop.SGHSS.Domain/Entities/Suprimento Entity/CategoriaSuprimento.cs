using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Model.Suprimentos;

namespace Loop.SGHSS.Domain.Entities.Suprimento_Entity
{
    public class CategoriaSuprimento : DefaultEntityIdModel
    {
        public CategoriaSuprimento() { }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }

        // --== Relacionamentos
        public virtual ICollection<Suprimento>? Suprimentos { get; set; }


        // --== Atualiza informações.
        public void Atualizar(CategoriaSuprimentosModel model)
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
