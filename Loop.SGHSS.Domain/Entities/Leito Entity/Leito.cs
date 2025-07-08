using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Model._Enums.Instituicao;
using Loop.SGHSS.Model._Enums.Leitos.Loop.SGHSS.Model.Enums;
using Loop.SGHSS.Model.Leitos;

namespace Loop.SGHSS.Domain.Entities.Leito_Entity
{
    public class Leito : DefaultEntityIdModel
    {
        public Leito() { }
        public string? Titulo { get; set; }
        public string? Observacao { get; set; }
        public int? Andar { get; set; }
        public string? Sala { get; set; }
        public string? NumeroLeito { get; set; }
        public statusLeitoEnum? StatusLeito { get; set; }
        public TipoLeitoEnum? TipoLeito { get; set; }
        public Guid? InstutuicaoId { get; set; }

        public Instituicao? Instituicao { get; set; }


        // --== Atualiza informações Gerais do leito.
        public void AtualizarGeral(LeitosModel model)
        {
            if (model != null)
            {
                if (model.TipoLeito != null)
                    this.TipoLeito = model.TipoLeito;
                if (model.Titulo != null)
                    this.Titulo = model.Titulo;
                if (model.Observacao != null)
                    this.Observacao = model.Observacao;
                if (model.Andar != null)
                    this.Andar = model.Andar;
                if (model.Sala != null)
                    this.Sala = model.Sala;
                if (model.NumeroLeito != null)
                    this.NumeroLeito = model.NumeroLeito;
            }
        }

        // --== Atualizar o status do leito.
        public void  AtualizarStatus(LeitoStatus model)
        {
            if (model != null)
            {
                if (model.StatusLeito != null)
                    this.StatusLeito = model.StatusLeito;
            }
        }
    }
}
