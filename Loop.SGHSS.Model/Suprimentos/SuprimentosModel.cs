namespace Loop.SGHSS.Model.Suprimentos
{
    public class SuprimentosModel
    {
        public Guid? Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public Guid? CategoriaId { get; set; }
        public Guid? InstituicaoId { get; set; }
    }
}
