
using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Data_Access_Layer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Buisness_Logic_Layer.Services
{
    public class ReportService : IReportService
    {

        private readonly DataContext _context;
        private object _Context;

        public ReportService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> generateReport()
        {

            return true;
        }

        //public async Task<ResoursereportDTO> GetEventCountByDateRangeAsync(DateTime startDate, DateTime endDate)
        //{
        //var Resoursere = new ResoursereportDTO
        //{
        //Total = _context.Resources.Where(e => (e.AddedOn >= startDate && e.AddedOn <= endDate)).Count(),
        // Novels = _context.Resources.Where(e => (e.AddedOn >= startDate && e.AddedOn <= endDate) && e.Type == "Novels").Count(),
        //Journals = _context.Resources.Where(e => (e.AddedOn >= startDate && e.AddedOn <= endDate) && e.Type == "Journals").Count(),
        // Ebooks = _context.Resources.Where(e => (e.AddedOn >= startDate && e.AddedOn <= endDate) && e.Type == "Ebooks").Count(),
        // Rbooks = _context.Resources.Where(e => (e.AddedOn >= startDate && e.AddedOn <= endDate) && e.Type == "Referencesbooks").Count(),
        // Others = _context.Resources.Where(e => (e.AddedOn >= startDate && e.AddedOn <= endDate) && e.Type == "Others").Count(),
        // };
        //return Resoursere;
        //}

        public async Task<List<object[]>> GetEventCountByDateRangeAsync(DateTime startDate, DateTime endDate)

            {
                var bookTypeCounts = await _context.Resources
                    .Where(r => r.AddedOn >= startDate && r.AddedOn <= endDate)
                    .GroupBy(r => r.Type)
                    .Select(g => new object[]
                    {
                    g.Key,
                    g.Count()
                    })
                    .ToListAsync();

            bookTypeCounts.Insert(0, new object[] { "Book Type", "Quantity" });

            return bookTypeCounts;
            }


        public async Task<List<LocationCountDTO>> GetAllLocation()
        {
            var locations = await _context.Locations.ToListAsync();

            // Group by CupboardId and process each group
            var locationListDtos = new List<LocationCountDTO>();

            foreach (var group in locations.GroupBy(cs => cs.CupboardId))
            {
                // Fetch the cupboard info asynchronously
                var cupboard = await _context.Cupboard.FirstOrDefaultAsync(e => e.cupboardID == group.Key);

                // Create the LocationListDto object

                var Quantity = _context.Cupboard
                .Where(c => c.cupboardID == cupboard.cupboardID)
                .Join(
                    _context.Locations,
                    c => c.cupboardID,
                    l => l.CupboardId,
                    (c, l) => new { Cupboard = c, Locations = l })
                .Join(
                    _context.Resources,
                    cl => cl.Locations.LocationNo,
                    r => r.BookLocation,
                    (cl, r) => r.Quantity)
                .Sum();

                var Borrow = _context.Cupboard
              .Where(c => c.cupboardID == cupboard.cupboardID)
              .Join(
                  _context.Locations,
                  c => c.cupboardID,
                  l => l.CupboardId,
                  (c, l) => new { Cupboard = c, Locations = l })
              .Join(
                  _context.Resources,
                  cl => cl.Locations.LocationNo,
                  r => r.BookLocation,
                  (cl, r) => r.Borrowed)
              .Sum();

                var a = new LocationCountDTO
                {
                    day = cupboard.name,
                    y = Borrow+Quantity,
                };

                locationListDtos.Add(a);
            }

            return locationListDtos;
        }



        public async Task<rereservation> GetReservationsCountByDateRangeAsync(DateOnly startDate1, DateOnly endDate1)
        {
            var Reservations = new rereservation
            {
                Total = _context.Reservations.Where(e => e.IssuedDate >= startDate1 && e.IssuedDate <= endDate1).Count(),
                Due = _context.Reservations.Where(e => (e.IssuedDate >= startDate1 && e.IssuedDate <= endDate1) && e.Status == "Overdue").Count(),
                Reserved = _context.Reservations.Where(e => (e.IssuedDate >= startDate1 && e.IssuedDate <= endDate1) && e.Status == "Received").Count(),
                Borrowed = _context.Reservations.Where(e => (e.IssuedDate >= startDate1 && e.IssuedDate <= endDate1) && e.Status == "Borrowed").Count(),

            };
            return Reservations;
        }

        public async Task<userreport> GetUserCountByDateRangeAsync(DateOnly startDate1, DateOnly endDate1)
        {
            var User = new userreport
            {
                Total = _context.Users.Where(e => e.AddedDate >= startDate1 && e.AddedDate <= endDate1).Count(),
                Free = _context.Users.Where(e => (e.AddedDate >= startDate1 && e.AddedDate <= endDate1) && e.Status == "free").Count(),
                Loan = _context.Users.Where(e => (e.AddedDate >= startDate1 && e.AddedDate <= endDate1) && e.Status == "loan").Count(),

            };
            return User;
        }
    }
}
