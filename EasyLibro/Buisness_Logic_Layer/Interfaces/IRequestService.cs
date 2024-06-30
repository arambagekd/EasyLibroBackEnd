using Buisness_Logic_Layer.DTOs;
using Microsoft.AspNetCore.Http;


namespace Buisness_Logic_Layer.Interfaces
{
    public interface IRequestService
    {
        Task<bool> AddRequest(AddRequestDto request, HttpContext httpContex);
        Task<List<GetRequestDto>> GetRequestList(HttpContext httpContext);
        Task<bool> RemoveRequestList(int id);
    }
}
