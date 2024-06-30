using Data_Access_Layer;
using Buisness_Logic_Layer.Interfaces;

namespace Buisness_Logic_Layer.Services
{
    public class ReportService : IReportService
    {

       private readonly DataContext _context;

       public ReportService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> generateReport()
        {

            return true;
        }
    }
}
