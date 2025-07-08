using Microsoft.EntityFrameworkCore;

namespace Loop.SGHSS.Extensions.Paginacao
{
    public interface IQueryFilter
    {
        int? Page { get; set; }
        int? PageSize { get; set; }
        bool HasFilters { get; }
    }

    public static class Pagination
    {
        public static async Task<PagedResult<T>> ToPaged<T>(this IQueryable<T> query, IQueryFilter queryFilter)
            where T : class
        {
            PagedResult<T> result = new();
            result.PageIndex = queryFilter.Page ??= 1;
            result.PageSize = queryFilter.PageSize ??= 10;
            result.TotalResults = query.Count();
            if (queryFilter.PageSize == 0)
            {
                result.List = await query.AsNoTracking().ToListAsync();
                result.TotalPages = 1;
            }
            else
            {
                result.TotalPages = (int)Math.Ceiling(((double)result.TotalResults / ((double)queryFilter.PageSize)));


                result.List = query.Skip((((int)queryFilter.Page - 1) * (int)queryFilter.PageSize))
                    .Take((int)queryFilter.PageSize);
            }
            return result;
        }
    }

    public class PartialEntityPagedResult<T>
    {
        public IQueryable<T>? List { get; set; }
        public int? TotalResults { get; set; }
        public int? TotalPages { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? Query { get; set; }
    }


    public class PagedResult<T>
    {
        public PagedResult() { List = new List<T>(); }
        public IEnumerable<T>? List { get; set; }
        public int? TotalResults { get; set; }
        public int? TotalPages { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? Query { get; set; }
    }
}
