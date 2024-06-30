using Buisness_Logic_Layer.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Buisness_Logic_Layer.Interfaces
{
    public interface ILocationService
    {
        Task<IActionResult> GetAllLocation(string cupboardname);
        Task<IActionResult> SearchResources(SearchbookcupDto request);
        Task<IActionResult> AddLocation(AddLocationDto location);
    }
}
