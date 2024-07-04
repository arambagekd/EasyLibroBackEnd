using Buisness_Logic_Layer.AuthHelpers;
using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.EmailTemplates;
using Buisness_Logic_Layer.Interfaces;
using Data_Access_Layer;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Resources;

namespace Buisness_Logic_Layer.Services
{
    public class ReservationService : IReservationService
    {

        private readonly DataContext _Context;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        private readonly JWTService _jwtService;

        //Contructor of the ReservationService
        public ReservationService(DataContext Context, INotificationService notificationService, IEmailService emailService, JWTService jwtService)
        {
            _Context = Context;
            _notificationService = notificationService;
            _emailService = emailService;
            _jwtService = jwtService;
        }
        public async Task<IActionResult> LoadIssueForm(string isbn)
        {

            var resource = await _Context.Resources.FirstOrDefaultAsync(u => u.ISBN == isbn);

            if (resource == null)  //Check is there any resource
            {
                return new BadRequestObjectResult("ResourceNotFound");
            }
            else //If Resource found pass data to port
            {
                var resourcedto=new IssueBookFormResponseDto
                {
                    ISBN=resource.ISBN,
                    URL = resource.ImageURL,
                };
                return new OkObjectResult(resourcedto); 
            }
        }
        public async Task<IActionResult> IssueBook(IssueBookRequestDto request,HttpContext httpContext)
        {

            var resource = await _Context.Resources.FirstOrDefaultAsync(u => u.ISBN == request.ISBN);
            var borrower = await _Context.Users.FirstOrDefaultAsync(u => u.UserName == request.BorrowerID);
            if (borrower == null) //If User not found
            {
                return new BadRequestObjectResult("Wrong Borrower Name");
            }
            var per = await _Context.Permissions.FirstOrDefaultAsync(u => u.userName == borrower.UserName);
            if (!per.permission)
            {
                return new BadRequestObjectResult("User was blocked..");
            }
            //Decrease Quantity of resource by 1
            resource.Quantity = resource.Quantity - 1;
            resource.Borrowed=resource.Borrowed + 1;
            await _Context.SaveChangesAsync();

            
            if (resource.Quantity < 0) //If not enough resources
            {
                resource.Quantity = resource.Quantity + 1;
                resource.Borrowed = resource.Borrowed - 1;
                await _Context.SaveChangesAsync();
                return new BadRequestObjectResult("No of Books not enough");
            }
            else if (borrower.Status == "Loan") //If User in a loan
            {
                resource.Quantity = resource.Quantity + 1;
                resource.Borrowed = resource.Borrowed - 1;
                await _Context.SaveChangesAsync();
                return new BadRequestObjectResult("User in a loan");
            }
            else  //If User Can borrow the book
            {
                var issuer=_jwtService.GetUsername(httpContext);
                var reservation = new Reservation
                {
                    ReservationNo="123456",
                    ResourceId=request.ISBN,
                    BorrowerID = request.BorrowerID,
                    Status = "borrowed",
                    IssuedByID = issuer,
                    IssuedDate = DateOnly.FromDateTime(DateTime.Today),
                    DueDate = DateOnly.Parse(request.dueDate),
                    Penalty=0,
                    
                };

                _Context.Reservations.Add(reservation);//Add the Reservation
                borrower.Status = "Loan";
                if (request.requestId != 0)
                {
                    var req=await _Context.Requests.FirstOrDefaultAsync(e => e.Id == request.requestId);
                    _Context.Requests.Remove(req);
                }
                await _Context.SaveChangesAsync();

                var responsefromform = new IssueBookResponseDto //Intialize response dto
                {
                    BorrowerId = reservation.BorrowerID,
                    ISBN = resource.ISBN,
                    ReservationId = reservation.Id,
                };

                try
                {
                    await _notificationService.IssueNotification(reservation.Id);
                }
                catch
                {
                    return new OkObjectResult("Issue Book Succeessfully..Notification not sent");
                }

                if(request.Email == true) //If Email is true
                {
                    try
                    {
                        var htmlBody = new EmailTemplate().IssueBookEmail(reservation,borrower.FName+" "+borrower.LName,resource);
                        await _emailService.SendEmail(htmlBody, borrower.Email, "Successfully Issue the Book" + " " + reservation.ResourceId);
                    }
                    catch
                    {
                        return new OkObjectResult("Issue Book Succeessfully..Email not sent");
                    }
                    }
                return new OkObjectResult("Issue Book Succeessfully"); // return response dto to port
            }

        }
        public async Task<IActionResult> AboutReservation(int resId)
        {

            var reservation = await _Context.Reservations.FirstOrDefaultAsync(u => u.Id == resId);
            

            if (reservation == null)  //Check is there any resource
            {
                return new BadRequestObjectResult("ReservationNotFound");
            }
            else //If Resource found past data to port
            {
                var resource = await _Context.Resources.FirstOrDefaultAsync(u => u.ISBN == reservation.ResourceId);
                var reservationdto = new AboutReservationDto
                {
                    ResId = reservation.Id,
                    ISBN = resource!=null?reservation.ResourceId:"resource removed",
                    BookTitle = resource != null ? resource.Title : "resource removed",
                    UserName = reservation.BorrowerID,
                    
                    DateIssue = DateOnly.FromDateTime(DateTime.Now),
                    DueDate = reservation.DueDate,
                    Issuer = reservation.IssuedByID,
                    ReturnDate = reservation.ReturnDate,
                    Status = reservation.Status,
                    ImagePath = resource != null ? resource.ImageURL:null,
                    Penalty=reservation.Penalty
                    

    };
                return new OkObjectResult(reservationdto);
            }

        }
        public async Task<IActionResult> ReturnBook(ReturnBookDto request, HttpContext httpContext)
        {
            var reservation= await _Context.Reservations.FirstOrDefaultAsync(u => u.Id == request.reservationNo);
            

            if (reservation == null)
            {
                return new BadRequestObjectResult("Reservation not found");
            }
            else
            {
                if (reservation.Status == "borrowed"|| reservation.Status == "overdue")
                {
                    var resource = await _Context.Resources.FirstOrDefaultAsync(u => u.ISBN == reservation.ResourceId);
                    var borrower = await  _Context.Users.FirstOrDefaultAsync(u => u.UserName == reservation.BorrowerID);
                    borrower.Status = "free";
                    if(resource != null)
                    {
                        resource.Quantity = resource.Quantity + 1;
                        resource.Borrowed = resource.Borrowed - 1;
                    }
                    reservation.Status = "received";
                    reservation.ReturnDate = DateOnly.Parse(request.returnDate);
                    reservation.Penalty = reservation.Penalty + request.Penalty;
                    await _Context.SaveChangesAsync();
                   

                    try
                    {
                        await _notificationService.ReturnNotification(reservation.Id);
                       
                    }
                    catch
                    {
                        return new OkObjectResult("Return Book Succeessfully..Notification not sent");
                    }
                    
                    if (request.email == true) { 
                        try
                        {
                            
                            var htmlBody = new EmailTemplate().ReturnBookEmail(reservation, borrower.FName + " " + borrower.LName, resource);
                            await _emailService.SendEmail(htmlBody,borrower.Email,"Successfully Return the Book"+" "+reservation.ResourceId);
                            
                        }
                        catch
                        {
                            return new OkObjectResult("Email not sent");
                        }
                    }

                    return new OkObjectResult("Return Book Succeessfully");
                }
                else
                {
                    return new OkObjectResult("Already Returned");
                }
            }
        }
        public async Task<IActionResult> SearchReservation(SearchDetails details,HttpContext httpContext)
        {
            var userName = _jwtService.GetUsername(httpContext);
            var userType = _jwtService.GetUserType(httpContext);


            var k = new List<Reservation>();

            if (userType == "admin")
                k = _Context.Reservations.ToList();
            if (userType == "patron")
                k = _Context.Reservations.Where(e => e.BorrowerID == userName).ToList();

            if (details.Keywords == "")
            {

            }
            else if (details.type=="all")
            {

                int number;
                bool success = int.TryParse(details.Keywords, out number);
                if (success)
                {
                    var filteredReservations = from r in _Context.Reservations
                                               join res in _Context.Resources on r.ResourceId equals res.ISBN
                                               where res.Title.ToLower().Contains(details.Keywords.ToLower())
                                               select r;
                    var l = filteredReservations.ToList();
                    k = k.Where(e =>
                    e.BorrowerID.ToLower().Contains(details.Keywords.ToLower())
                    || e.ResourceId.ToLower().Contains(details.Keywords.ToLower())
                    || e.Id.Equals(number)).ToList();

                    k = k.Union(l).ToList();
                }
                else
                {
                    var filteredReservations = from r in _Context.Reservations
                                               join res in _Context.Resources on r.ResourceId equals res.ISBN
                                               where res.Title.ToLower().Contains(details.Keywords.ToLower())
                                               select r;
                    var l = filteredReservations.ToList();

                    k = k.Where(e =>
                    e.BorrowerID.ToLower().Contains(details.Keywords.ToLower())
                    || e.ResourceId.ToLower().Contains(details.Keywords.ToLower())).ToList();

                    k = k.Union(l).ToList();
                }
            }
            else if (details.type=="userId")
            {
                k = k.Where(e => e.BorrowerID.ToLower().Contains(details.Keywords.ToLower())).ToList();
            }
            else if (details.type== "resourceId")
            {
                var filteredReservations = from r in _Context.Reservations
                                           join res in _Context.Resources on r.ResourceId equals res.ISBN
                                           where res.Title.ToLower().Contains(details.Keywords.ToLower())
                                           select r;
               var z = k.Where(e => e.ResourceId.ToLower().Contains(details.Keywords.ToLower())).ToList();
               k=filteredReservations.ToList();
                k = k.Union(z).ToList(); 
                
            }
            else if (details.type== "reservationId")
            {
                int number;
                bool success = int.TryParse(details.Keywords, out number);

                if (success)
                {
                    k = _Context.Reservations.Where(e => e.Id.Equals(number)).ToList();
                }
            }
            else {      
                return new OkObjectResult("Invalid Search");
            }



            List<ReservationDto> reservationlist = new List<ReservationDto>();
            foreach(var x in k) {
                var userob = await _Context.Users.FirstOrDefaultAsync(u => u.UserName == x.BorrowerID);
                var resource = await _Context.Resources.FirstOrDefaultAsync(u => u.ISBN == x.ResourceId);
                var res = new ReservationDto
                {
                    reservationNo = x.Id,
                    Resource=x.ResourceId!=null?x.ResourceId:"resource removed",
                    BorrowerName=x.BorrowerID,
                    UserId=x.BorrowerID,
                    UserName=userob.FName+" "+ userob.LName,
                    DueDate=x.DueDate,
                    IssueDate=x.IssuedDate,
                    Status=x.Status,
                    ResourceTitle=x.ResourceId!=null?resource.Title:"resources removed"
                };
                reservationlist.Add(res);
            }
            return new OkObjectResult(reservationlist);


        }
        public async Task<IActionResult> deleteReservation(int id)
        {
            var reservation = await _Context.Reservations.FirstOrDefaultAsync(e => e.Id == id);
            if (reservation == null)
                return new OkObjectResult(false);
            else
            {
                if (reservation.Status != "reserved")
                {
                    var user = await _Context.Users.FirstOrDefaultAsync(e => e.UserName == reservation.BorrowerID);
                    var resource = await _Context.Resources.FirstOrDefaultAsync(u => u.ISBN == reservation.ResourceId);
                    if (resource != null)
                    {
                        resource.Quantity = resource.Quantity + 1;
                        resource.Borrowed = resource.Borrowed - 1;
                    }
                    user.Status = "free";
                    await _Context.SaveChangesAsync();
                }
                _Context.Remove(reservation);
                await _Context.SaveChangesAsync();
                return new OkObjectResult(true);
            }
                
        }
        public async Task<IActionResult> extendDue(int id, string due)
        {
            var reservation = await _Context.Reservations.FirstOrDefaultAsync(e => e.Id == id);
            if(reservation == null)
            {
               return new OkObjectResult( false);
            }
            else
            {
                reservation.DueDate = DateOnly.Parse(due);
                if (DateOnly.FromDateTime(DateTime.Now) < reservation.DueDate)
                {
                    reservation.Status = "borrowed";
                }
                await _Context.SaveChangesAsync();
                return new OkObjectResult(true);
            }

        }
        public async Task setOverdue()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Today);
            var overdueRecords =await _Context.Reservations
               .Where(e => e.Status == "borrowed" && e.DueDate == currentDate)
               .ToListAsync();

            foreach (var record in overdueRecords)
            {
                record.Status = "overdue";
                await _notificationService.SetRemind(record);
            }

            await _Context.SaveChangesAsync();

            Console.WriteLine("Recurring job scheduled to set overdue reservations.");
        }

        public async Task<IActionResult> Remind()
        {
            var overduereservaton = await _Context.Reservations.Where(e => e.Status == "overdue").ToListAsync();
            foreach(var record in overduereservaton)
            {
                await _notificationService.SetRemind(record);
            }
            return new OkObjectResult("Remind All");

        } 

        public async Task addPenalty()
        {
            var overduelist=await _Context.Reservations.Where(e => e.Status == "overdue").ToListAsync();
            
            foreach(var x in overduelist)
            {
                var penalty = DateOnly.FromDateTime(DateTime.Now).DayNumber - x.DueDate.DayNumber; // x.DueDate should be of type DateOnly
                x.Penalty = penalty;
            }

            await _Context.SaveChangesAsync();
        }

    }
}
