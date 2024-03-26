using System.Text.Json;
using ProjectP.Helpers.Paginations;

namespace ProjectP.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemPerPage, int totalItems,
        int totalPages)
    {
        var paginationHeader = new PaginationHeader(currentPage,itemPerPage,totalItems,totalPages);
        response.Headers.Add("Pagination",JsonSerializer.Serialize(paginationHeader));
        response.Headers.Add("Access-Control-Expose-Headers","Pagination");
    }
}