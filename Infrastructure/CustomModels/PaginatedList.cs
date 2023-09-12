using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.CustomModels
{
    public class PaginatedList<T>
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public List<T> Items { get; private set; }

        public PaginatedList(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            Items = items;
        }

        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageNumber, int pageSize
            )
        {
            var totalCount = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, totalCount, pageNumber, pageSize);
        }
    }
}
