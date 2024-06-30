using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EasyLibro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }



       [HttpPost("getDashboradData")]
        public async Task<IActionResult> getDashboradData()
        {
            var httpContext = HttpContext;
            return await _dashboardService.getDashboradData(httpContext);
        }

        [HttpPost("getOverDueList")]
        public async Task<IActionResult> getOverdueList()
        {
            var httpContext = HttpContext;
            return await _dashboardService.getOverdueList(httpContext);
        }

        [HttpPost("getLastWeekReservations")]
        public async Task<List<LastWeekReservations>> getLastWeekReservations()
        {
            return await _dashboardService.getLastWeekReservations();
        }
        [HttpPost("getLastWeekUsers")]
        public async Task<List<LastWeekReservations>> getLastWeekUsers()
        {
            return await _dashboardService.getLastWeekUsers();
        }
        [HttpPost("getAnouncement")]
        public async Task<IActionResult> getAnouncement()
        {
            var httpContext = HttpContext;
            return await _dashboardService.getAnouncement(httpContext);

        }


    }
}
