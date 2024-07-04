using Buisness_Logic_Layer.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Buisness_Logic_Layer.Interfaces
{
    public interface IRequestService
    {
        Task<IActionResult> AddRequest(AddRequestDto request, HttpContext httpContex);
        Task<List<GetRequestDto>> GetRequestList(HttpContext httpContext);
        Task<IActionResult> RemoveRequestList(int id);
        Task DeleteExpiredRequests();
    }
}
