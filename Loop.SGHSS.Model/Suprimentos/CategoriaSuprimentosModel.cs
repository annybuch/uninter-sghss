namespace Loop.SGHSS.Model.Suprimentos
{
    public class CategoriaSuprimentosModel
    {
        public Guid? Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
    }

    public class CategoriaComSuprimentosViewModel
    {
        public Guid? Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }

        // --== Quantidade de suprimentos
        public int QtdSuprimentos { get; set; }

        public List<SuprimentoComComprasViewModel> Suprimentos { get; set; } = new();
    }


    public class SuprimentoComComprasViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descricao { get; set; }
        public List<SuprimentosCompraModel> Compras { get; set; } = new();
    }
}
