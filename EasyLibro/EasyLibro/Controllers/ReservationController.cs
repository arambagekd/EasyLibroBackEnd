
using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyLibro.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("Issuebook")]
        public async Task<IActionResult> IssueBook(IssueBookRequestDto request)
        {
            var httpContext = HttpContext;
            return await _reservationService.IssueBook(request,httpContext);
        }

        [HttpPost("Returnbook")]
        public async Task<IActionResult> ReturnBook(ReturnBookDto request)
        {
            var httpContext = HttpContext;
            return await _reservationService.ReturnBook(request,httpContext);
        }

        [HttpGet("LoadReservation")]
        public async Task<IActionResult> LoadIssueForm(string isbn)
        {
            return await _reservationService.LoadIssueForm(isbn);
        }

        [HttpPost("About")]
        public async Task<IActionResult> AboutReservation(int resId)
        {
            return await _reservationService.AboutReservation(resId);
        }

        [HttpPost("SearchReservation")]
        public async Task<IActionResult> SearchReservation(SearchDetails details)
        {
            var httpContext = HttpContext;
            return await _reservationService.SearchReservation(details,httpContext);
        }

        [HttpDelete("DeleteReservation")]
        public async Task<IActionResult> deleteReservation(int id)
        {
            return await _reservationService.deleteReservation(id);
        }

        [HttpPut("ExtendDue")]
        public async Task<IActionResult> extendDue(int id,string due)
        {
            return await _reservationService.extendDue(id,due);
        }

        [HttpGet("RemindOverdue")]
        public async Task<IActionResult> Remind()
        {
            return await _reservationService.Remind();
        }

    }
}
