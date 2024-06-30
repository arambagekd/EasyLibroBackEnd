
using Buisness_Logic_Layer.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Buisness_Logic_Layer.Interfaces
{
    public interface IResourceService
    {
        Task<IActionResult> AddResource(AddBookRequestDto book,HttpContext httpContext);
        Task<IActionResult> DeleteResource(string isbn);
        Task<IActionResult> SearchResources(SearchbookDto searchbookDto);
        Task<IActionResult> EditResource(AddBookRequestDto book);
        Task<IActionResult> AboutResource(string isbn);
        Task WeeklyBookUpdates();
    }
}
