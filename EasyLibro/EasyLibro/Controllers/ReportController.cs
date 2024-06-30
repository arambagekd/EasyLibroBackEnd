using Buisness_Logic_Layer.Interfaces;
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

    }
}
