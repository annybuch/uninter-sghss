using System.Text;

namespace Loop.SGHSS.Extensions.Paginacao
{
    public static class LoopSGHSSQueryFilterExtensions
    {
        public static string ToQueryParams<T>(this T model)
        {
            var props = model?.GetType().GetProperties();
            StringBuilder query = new();

            // --== Não há propriedades no modelo enviado
            if (props is null || props.Any()) return string.Empty;

            foreach (var prop in props.Select((val, i) => new { Index = i, Value = val }))
            {
                // --== Iniciando a pesquisa
                if (prop.Index.Equals(0))
                    query.Append("?");

                var value = prop.Value.GetValue(model);

                // --== Adicionando valor 
                if (value is not null)
                    query.Append($"{prop.Value.Name}={value}&");
            }

            // --== Retornando pesquisa
            return query.ToString();
        }
    }
}
