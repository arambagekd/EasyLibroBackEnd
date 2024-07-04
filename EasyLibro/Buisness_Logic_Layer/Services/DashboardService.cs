using Buisness_Logic_Layer.AuthHelpers;
using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Data_Access_Layer;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Buisness_Logic_Layer.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly DataContext _dataContext;
        private readonly JWTService _jwt;

        public DashboardService(DataContext dataContext, JWTService jWTService)
        {
            _dataContext = dataContext;
            _jwt = jWTService;
        }

        public async Task<IActionResult> getDashboradData(HttpContext httpContext)
        {
            var count = _dataContext.Resources.Sum(e => e.Quantity) + _dataContext.Resources.Sum(e => e.Borrowed);
            var usertype = _jwt.GetUserType(httpContext);
            if (usertype == "admin")
            {
                var Statics = new AdminDashboardStatics
                {
                    Total = count,
                    IssueToday = await _dataContext.Reservations.Where(e => e.IssuedDate == DateOnly.FromDateTime(DateTime.Now)).CountAsync(),
                    ReturnToday = await _dataContext.Reservations.Where(e => e.ReturnDate == DateOnly.FromDateTime(DateTime.Now)).CountAsync(),
                    Locations = await _dataContext.Cupboard.CountAsync(),
                    Users = await _dataContext.Users.CountAsync(),
                    Reservations = await _dataContext.Reservations.CountAsync(),
                    Requests = await _dataContext.Requests.CountAsync(),
                    OverDue = await _dataContext.Reservations.Where(e => e.Status == "overdue").CountAsync()
                };

                return new OkObjectResult(Statics);
            }
            if (usertype == "patron")
            {
                var username = _jwt.GetUsername(httpContext);
                var user = await _dataContext.Users.FirstOrDefaultAsync(e => e.UserName == username);
                var Statics = new PatronDashboardStatics
                {
                    Status = user.Status,
                    myReservations = await _dataContext.Reservations.Where(e => e.BorrowerID == username).CountAsync(),
                    Requests = await _dataContext.Requests.Where(e => e.UserId == username).CountAsync(),
                    Penalty = 0
                };
                return new OkObjectResult(Statics);
            }
            return new BadRequestResult();
        }
        public async Task<IActionResult> getOverdueList(HttpContext httpContext)
        {
            var usertype = _jwt.GetUserType(httpContext);
            var username = _jwt.GetUsername(httpContext);

            var k = new List<Reservation>();
            if (usertype == "admin")
            {
                k = _dataContext.Reservations.Where(e => e.Status == "overdue").ToList();
            }
            else if (usertype == "patron")
            {
                k = _dataContext.Reservations.Where(e => e.BorrowerID == username).ToList();
            }

            List<ReservationDto> reservationlist = new List<ReservationDto>();
            foreach (var x in k)
            {
                var userob = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == x.BorrowerID);
                var res = new ReservationDto
                {
                    reservationNo = x.Id,
                    Resource = x.ResourceId,
                    BorrowerName = x.BorrowerID,
                    UserName = userob.FName + " " + userob.LName,
                    DueDate = x.DueDate,
                    Status = x.Status//need to look due or not
                };
                reservationlist.Add(res);
            }
            return new OkObjectResult(reservationlist);

        }

        public async Task<List<LastWeekReservations>> getLastWeekReservations()
        {
            var lastWeekList = new List<LastWeekReservations>();
            for (int x = 6; x >= 0; x--)
            {
                DateOnly issueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-x));
                int count = _dataContext.Reservations.Where(e => e.IssuedDate == issueDate).Count();

                var a = new LastWeekReservations
                {
                    day = issueDate.ToString("MM-dd"),
                    y = count,
                };
                lastWeekList.Add(a);
            }
            return lastWeekList;
        }


        public async Task<List<LastWeekReservations>> getLastWeekUsers()
        {
            var lastWeekList = new List<LastWeekReservations>();
            for (int x = 6; x >= 0; x--)
            {
                DateOnly issueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-x));
                int count = _dataContext.Users.Where(e => e.AddedDate == issueDate).Count();

                var a = new LastWeekReservations
                {
                    day = issueDate.ToString("MM-dd"),
                    y = count,
                };
                lastWeekList.Add(a);
            }
            return lastWeekList;
        }
        public async Task<List<LastWeekReservations>> getLastWeekResourses()
        {
            var lastWeekList = new List<LastWeekReservations>();
            for (int x = 6; x >= 0; x--)
            {
                DateTime issueDate = DateTime.Today.AddDays(-x);
                int count = _dataContext.Resources.Where(e => e.AddedOn.Date == issueDate.Date).Count();

                var a = new LastWeekReservations
                {
                    day = issueDate.ToString("MM-dd"),
                    y = count,
                };
                lastWeekList.Add(a);
            }
            return lastWeekList;
        }



        public async Task<IActionResult> getAnouncement(HttpContext httpContext)
        {
            var username = _jwt.GetUsername(httpContext);
            var notification = _dataContext.Notifications
                               .Where(s => _dataContext.NotificationUser.Any(e => e.UserName == username && e.NotificationId == s.Id && s.Type == "notice"))
                               .ToList().Select(e => e.Description);
            return new OkObjectResult(notification);
        }
    }
}
