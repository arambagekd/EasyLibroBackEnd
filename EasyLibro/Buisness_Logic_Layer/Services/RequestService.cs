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
    public class RequestService:IRequestService
    {
        private readonly DataContext _Context;
        private readonly JWTService _jwTService;
        private readonly IReservationService _reservationService;


        //Contructor of the RequestService
        public RequestService(DataContext Context,JWTService jwtService,IReservationService reservationService)
        {
            _Context = Context;
            _jwTService = jwtService;
            _reservationService = reservationService;
        }
        public async Task<IActionResult> AddRequest(AddRequestDto request,HttpContext httpContext)
        {
            var userName = _jwTService.GetUsername(httpContext);
            var resource = await _Context.Resources.FirstOrDefaultAsync(u => u.ISBN == request.ISBN);
            var borrower = await _Context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            var count = await _Context.Requests.Where(e => e.UserId == userName).CountAsync();

            if (resource.Quantity < 1) //If not enough resources
            {
                return new BadRequestObjectResult("No of Books not enough");
            }
            else if (borrower.Status == "Loan") //If User in a loan
            {
                return new BadRequestObjectResult("You are in a loan");
            }
            else if (count >= 3)
            {
                return new BadRequestObjectResult("You Exceed Request Count");
            }
            else
            {
                var req = await _Context.Requests.FirstOrDefaultAsync(e => e.UserId == userName && e.ResourceId == request.ISBN);
                if (req != null)
                {
                    throw new Exception("You are already requested this ");
                }
                var newrequest = new RequestResource
                {
                    ResourceId= request.ISBN,
                    UserId=userName,
                    Date= DateOnly.FromDateTime(DateTime.Now),
                    Time= TimeOnly.FromDateTime(DateTime.Now)
                };

                
                _Context.Requests.Add(newrequest);//Add the Reservation
                await _Context.SaveChangesAsync();

                return new OkObjectResult(true);
            }
        }
        public async Task<List<GetRequestDto>> GetRequestList(HttpContext httpContext)
        {
            var username = _jwTService.GetUsername(httpContext);
            var userType=_jwTService.GetUserType(httpContext);      
            var user = await _Context.Users.FirstOrDefaultAsync(e => e.UserName == username);
            var allrequest = new List<RequestResource>();
            if (userType == "admin")
            {
                allrequest = _Context.Requests.ToList();
            }
            else
            {
               allrequest=_Context.Requests.Where(e=>e.UserId==username).ToList();
            }
            List<GetRequestDto> requestlist = new List<GetRequestDto>();  
            if (allrequest != null)
            {
                foreach (var r in allrequest)
                {
                    var req = new GetRequestDto
                    {
                        id = r.Id,
                        BorrowerID = r.UserId,
                        ISBN = r.ResourceId,
                        Title = _Context.Resources.FirstOrDefault(e => e.ISBN == r.ResourceId).Title,
                        Date = r.Date
                    };
                    requestlist.Add(req);
                }
            }
            return requestlist;
        }
        public async Task<IActionResult> RemoveRequestList(int id)
        {
            var request = await _Context.Requests.FirstOrDefaultAsync(e => e.Id == id);
            if (request == null)
            {
                return new BadRequestObjectResult("No Request Found");
            }
            else
            {
               // var resource = await _Context.Resources.FirstOrDefaultAsync(e => e.ISBN == request.ResourceId);
                var user = await _Context.Users.FirstOrDefaultAsync(e => e.UserName == request.UserId);
                //resource.Quantity=resource.Quantity+1;
                //resource.Borrowed = resource.Borrowed-1;//need to fixed
                //user.Status = "free";
                _Context.Requests.Remove(request);
                 await _Context.SaveChangesAsync();
                 return new OkObjectResult(true);
            }
        }

        public async Task DeleteExpiredRequests()
        {
            var requests= await _Context.Requests.Where(e=>e.Date.AddDays(2)>= DateOnly.FromDateTime(DateTime.Today)).ToListAsync();
            _Context.RemoveRange(requests);
        }

    }
}
