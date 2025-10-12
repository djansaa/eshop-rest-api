using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eshop_rest_api.DTOs
{
    /// <summary>
    /// Represents a set of pagination links for navigating through a paginated resource.
    /// </summary>
    /// <remarks>This DTO provides links to the current page, the first page, the last page, and optionally
    /// the previous and next pages. It is typically used in APIs to facilitate client-side navigation of paginated
    /// data.</remarks>
    /// <param name="Self"></param>
    /// <param name="First"></param>
    /// <param name="Previous"></param>
    /// <param name="Next"></param>
    /// <param name="Last"></param>
    public record PaginationLinksDTO([property: Required] string Self, [property: Required] string First, string? Previous, string? Next, [property: Required] string Last);

    /// <summary>
    /// Represents pagination details for a paginated response, including the current page, page size, total pages, and
    /// navigation links.
    /// </summary>
    /// <remarks>This record is typically used to encapsulate metadata about paginated data, such as the
    /// current page number, the number of items per page, the total number of pages available, and links for navigating
    /// between pages.</remarks>
    /// <param name="Page"></param>
    /// <param name="PageSize"></param>
    /// <param name="TotalPages"></param>
    /// <param name="Links"></param>
    public record PaginationDTO([property: Required] int Page, [property: Required] int PageSize, [property: Required] int TotalPages, [property: Required] PaginationLinksDTO Links);

    /// <summary>
    /// Represents a paginated response containing a collection of data items and associated pagination metadata.
    /// </summary>
    /// <remarks>This record is typically used to encapsulate a paginated result set, where the <see
    /// cref="Data"/> property contains the items for the current page, and the <see cref="Meta"/> property provides
    /// details about the pagination state, such as the current page number, total pages, and total items.</remarks>
    /// <typeparam name="T">The type of the data items contained in the paginated response.</typeparam>
    /// <param name="Data"></param>
    /// <param name="Meta"></param>
    public record PagedDTO<T>([property: Required] IEnumerable<T> Data, [property: Required] PaginationDTO Meta);
}
