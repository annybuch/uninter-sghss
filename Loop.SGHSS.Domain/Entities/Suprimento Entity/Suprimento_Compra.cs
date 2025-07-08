using Loop.SGHSS.Domain.Defaults;
using Loop.SGHSS.Domain.Entities.Instituicao_Entity;
using Loop.SGHSS.Domain.Entities.Suprimento_Entity;
using Loop.SGHSS.Model.Suprimentos;

namespace Loop.SGHSS.Data.Entities.Suprimento_Entity
{
    public class Suprimento_Compra : DefaultEntityIdModel
    {
        public Suprimento_Compra() { }
        public string? Descricao { get; set; }
        public string? Codigo { get; set; }
        public string? Marca { get; set; }
        public decimal?  ValorPago { get; set; }
        public int? QuantidadeComprada { get; set; }
        public int? QuantidadeSaida { get; set; }

        public DateTime? DataComprada { get; set; }

        public Suprimento? Suprimento { get; set; }
        public Guid? SuprimentoId { get; set; }
        public Instituicao? Instituicao { get; set; }
        public Guid? InstituicaoId { get; set; }


        // --== Responsável por editar uma compra de suprimento.
        public void Atualizar(SuprimentosCompraModel model)
        {
            if (model != null)
            {
                if (model.Codigo != null)
                    this.Codigo = model.Codigo;
                if (model.Descricao != null)
                    this.Descricao = model.Descricao;
                if (model.Marca != null)
                    this.Marca = model.Marca;
                if (model.ValorPago != null)
                    this.ValorPago = model.ValorPago;
                if (model.QuantidadeComprada != null)
                    this.QuantidadeComprada = model.QuantidadeComprada;
                if (model.QuantidadeSaida != null)
                    this.QuantidadeSaida = model.QuantidadeSaida;
                if (model.SuprimentoId != null)
                    this.SuprimentoId = model.SuprimentoId;
            }
        }

        // --== Responsável por atualizar o estoque de compras.
        public void Estoque(SuprimentosCompraModel model)
        {
            if (model != null)
            {
                if (model.QuantidadeSaida != null)
                    this.QuantidadeSaida = model.QuantidadeSaida;
            }
        }
    }
}
