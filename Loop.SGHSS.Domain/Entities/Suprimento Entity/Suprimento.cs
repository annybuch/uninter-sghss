using Loop.SGHSS.Data.Entities.Suprimento_Entity;
using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Model.Suprimentos;

namespace Loop.SGHSS.Domain.Entities.Suprimento_Entity
{
    public class Suprimento : DefaultEntityIdModel
    {
        public Suprimento() { }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }

        public Instituicao? Instituicao { get; set; }
        public Guid? InstituicaoId { get; set; }

        public CategoriaSuprimento? Categoria { get; set; }
        public Guid? CategoriaId { get; set; }

        public virtual ICollection<Suprimento_Compra>? SuprimentosCompras { get; set; }

        public void Atualizar(SuprimentosModel model)
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
