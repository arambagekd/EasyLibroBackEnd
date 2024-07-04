
using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Buisness_Logic_Layer.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyLibro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }



        [HttpPost]
        [Route("generateReport")]
        public async Task<bool> generateReport()
        {
            return await _reportService.generateReport();

        }

 

        [HttpPost("reservationscount")]
        public async Task<rereservation> GetReservationsCountByDateRange([FromBody] Reservationreport data)
        {
            return await _reportService.GetReservationsCountByDateRangeAsync(data.StartDate1, data.EndDate1);
        }

        [HttpPost("usercount")]
        public async Task<userreport> GetUserCountByDateRange([FromBody] Reservationreport data)
        {
            return await _reportService.GetUserCountByDateRangeAsync(data.StartDate1, data.EndDate1);

        }
        [HttpPost("resoursecount")]
        public async Task<List<object[]>> GetEventCountByDateRangeAsync(Reports reports)
        {
            return await _reportService.GetEventCountByDateRangeAsync(reports.StartDate,reports.EndDate);
        }

        [HttpPost("GetAllLocation")]
        public async Task<List<LocationCountDTO>> GetAllLocation()
        {
            return await _reportService.GetAllLocation();
        }


    }
}
