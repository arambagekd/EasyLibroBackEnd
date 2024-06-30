using Buisness_Logic_Layer.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Buisness_Logic_Layer.Interfaces
{
    public interface IDashboardService
    {
        Task<IActionResult> getDashboradData(HttpContext httpContext);
        Task<IActionResult> getOverdueList(HttpContext httpContext);
        Task<IActionResult> getAnouncement(HttpContext httpContext);
        Task<List<LastWeekReservations>> getLastWeekReservations();
        Task<List<LastWeekReservations>> getLastWeekUsers();
        
    }
}
