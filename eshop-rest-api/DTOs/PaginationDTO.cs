using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eshop_rest_api.DTOs
{
    public record PaginationLinksDTO(string? Self, string? First, string? Previous, string? Next, string? Last);
    public record PaginationDTO(int Page, int PageSize, int TotalPages, PaginationLinksDTO Links);
    public record PagedDTO<T>(IEnumerable<T> Data, PaginationDTO Meta);
}
