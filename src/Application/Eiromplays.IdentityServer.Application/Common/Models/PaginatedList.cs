using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Application.Common.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; } = new();
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        private PaginatedList(List<T> items, int count, int? pageIndex, int? pageSize)
        {
            PageIndex = pageIndex ?? 1;
            TotalPages = (int)Math.Ceiling(count / (double)(pageSize ?? 10));
            TotalCount = count;
            Items = items;
        }

        public PaginatedList()
        {
            
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int? pageIndex, int? pageSize)
        {
            var pageIndexValue = pageIndex ?? 1;
            var pageSizeValue = pageSize ?? 10;
            
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndexValue - 1) * pageSizeValue).Take(pageSizeValue).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
