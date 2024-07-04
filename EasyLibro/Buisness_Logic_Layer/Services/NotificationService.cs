using Microsoft.EntityFrameworkCore;
using Data_Access_Layer.Entities;
using Buisness_Logic_Layer.Interfaces;
using Data_Access_Layer;
using Buisness_Logic_Layer.AuthHelpers;
using Buisness_Logic_Layer.DTOs;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using Buisness_Logic_Layer.EmailTemplates;


namespace Buisness_Logic_Layer.Services
{
    public class NotificationService:INotificationService
    {
        private readonly DataContext _Context;
       private readonly JWTService _jwtService;
        private readonly IEmailService _emailService;
    //    private readonly FirebaseApp _firebaseApp;


        public NotificationService(DataContext Context,JWTService jwtservice, IEmailService emailService)
        {
            _Context = Context;
            _jwtService = jwtservice;
            _emailService = emailService;
        }
        public async Task<bool> SetFireBaseToken(SetToken setToken)
        {   var user = await _Context.Users.FirstOrDefaultAsync(e => e.UserName == setToken.UserName);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var connection= await _Context.FirebaseConnections.FirstOrDefaultAsync(e => e.userName == setToken.UserName && e.Token==setToken.Token);
            if(connection != null)
            {
                return false;
            }
            if(setToken.Token==null || setToken.Token == "")
            {
                return false;
            }
            var newconnection = new FirebaseConnection
            {
                userName = setToken.UserName,
                Token = setToken.Token
            };
            await _Context.FirebaseConnections.AddAsync(newconnection);
            await _Context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveFireBaseToken(SetToken setToken)
        {
            var user = await _Context.Users.FirstOrDefaultAsync(e => e.UserName == setToken.UserName);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var connection = await _Context.FirebaseConnections.FirstOrDefaultAsync(e => e.userName == setToken.UserName && e.Token == setToken.Token);
            if (connection == null)
            {
                return false;
            }
            _Context.FirebaseConnections.Remove(connection);
            await _Context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> NewNotice(NoticeDto newnotice)
        {

            var notice = new Notifications
            {
                Title=newnotice.Subject,
                Description=newnotice.Description,
                Date= DateOnly.FromDateTime(DateTime.Now),
                time= TimeOnly.FromDateTime(DateTime.Now),
                ToUser = newnotice.UserName,
                Type="notice"
            };
            _Context.Notifications.Add(notice);
            await _Context.SaveChangesAsync();

           

            if (newnotice.UserName == "all"|| newnotice.UserName == "patron"|| newnotice.UserName == "admin")
            {
                
                var user =new List<User>();
                if (newnotice.UserName == "all")
                {
                     user = await _Context.Users.ToListAsync();
                }
                else
                {
                     user = await _Context.Users.Where(e => e.UserType == newnotice.UserName).ToListAsync();
                }

                var tokenlist = new List<string>();

                foreach (var x in user)
                {
                    var notificationuser = new NotificationUser
                    {
                        UserName = x.UserName,
                        NotificationId = notice.Id,
                        Status = "unread"
                    };

                    var tokens = await _Context.FirebaseConnections.Where(e => e.userName == x.UserName).Select(e => e.Token).ToListAsync();

                    tokenlist.AddRange(tokens);
                    _Context.NotificationUser.Add(notificationuser);
                }
                try
                {
                    var message = new MulticastMessage()
                    {
                        Tokens = tokenlist,
                        Notification = new Notification()
                        {
                            Title = newnotice.Subject,
                            Body = newnotice.Description
                        }
                    };
                    var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message) ;

                }catch(Exception e)
                {
                   
                }
               

            }
            else
            {
                var user = await _Context.Users.FirstOrDefaultAsync(e=>e.UserName==newnotice.UserName);
                if (user == null)
                {
                    throw new Exception("USer not found");
                }
                var notificationuser = new NotificationUser
                {
                    UserName = user.UserName,
                    NotificationId = notice.Id,
                    Status = "unread"
                };
                var tokens = await _Context.FirebaseConnections.Where(e => e.userName == user.UserName).Select(e=>e.Token).ToListAsync();
                try
                {
                    var message = new MulticastMessage()
                    {
                        Tokens = tokens,
                        Notification = new Notification()
                        {
                            Title = newnotice.Subject,
                            Body = newnotice.Description
                        }
                    };
                    var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                }catch(Exception e)
                {
                  
                }

                _Context.NotificationUser.Add(notificationuser);
            }

            await _Context.SaveChangesAsync();
            return true;
        }
        public async Task<List<NewNoticeDto>> GetNotification(SearchNotification searchnotification)
        {
            var notificationlist = new List<NewNoticeDto>();
            var notifications = new List<Notifications>();



            if (searchnotification.keyword == "")
            {
                notifications = await _Context.Notifications.ToListAsync();
            }

            else if (searchnotification.type == "all" )
            {
               
                
               
                    notifications = await _Context.Notifications.Where(e=>e.ToUser.Contains(searchnotification.keyword)|| e.Description.Contains(searchnotification.keyword)|| e.Title.Contains(searchnotification.keyword)).ToListAsync();
              
            }else if(searchnotification.type == "user")
            {
                notifications = await _Context.Notifications.Where(e => e.ToUser.Contains(searchnotification.keyword)).ToListAsync();
            }
            else if (searchnotification.type == "title")
            {
                notifications = await _Context.Notifications.Where(e => e.Title.Contains(searchnotification.keyword)).ToListAsync();
            }

            foreach (var x in notifications)
                {
                    var y = new NewNoticeDto
                    {
                        Id = x.Id,
                        UserName=x.ToUser,
                        Subject = x.Title,
                        Date = x.Date,
                        Description = x.Description,
                    };
                    notificationlist.Add(y);
                }
            
            return notificationlist;
        }
        public async Task<List<MyNotificationDto>> GetMyNotification(HttpContext httpContext)
        {
            var user = _jwtService.GetUsername(httpContext);
            var notifications = await _Context.Notifications.Where(s => _Context.NotificationUser.Any(e => e.UserName == user && e.NotificationId == s.Id)).ToListAsync();

            var mynotifications = new List<MyNotificationDto>();
            foreach(var x in notifications)
            {
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                // Subtract targetDate from currentDate to get a TimeSpan object
                TimeSpan difference = currentDate.ToDateTime(TimeOnly.MinValue) - x.Date.ToDateTime(TimeOnly.MinValue);
                int ago = (int)difference.TotalDays;
                var y = new MyNotificationDto
                {
                    Id = _Context.NotificationUser.FirstOrDefault(e => e.UserName == user && e.NotificationId == x.Id).Id,
                    UserName = x.ToUser,
                    Subject = x.Title,
                    ago = ago,
                    Description = x.Description,
                    Status =_Context.NotificationUser.FirstOrDefault(e => e.UserName == user && e.NotificationId == x.Id).Status
                };
                mynotifications.Add(y);

            }
            return mynotifications;

        }
        public async Task<bool> RemoveNotification(int id)
        {
            var notification = await _Context.Notifications.FirstOrDefaultAsync(e => e.Id == id);
            if (notification != null)
            {
                _Context.NotificationUser.RemoveRange(_Context.NotificationUser.Where(e => e.NotificationId == id));
                _Context.Notifications.Remove(notification);
                await _Context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> MarkAsRead(int id)
        {
            var notification = await _Context.NotificationUser.FirstOrDefaultAsync(e => e.Id == id);
            if (notification != null)
            {
                notification.Status = "read";
                await _Context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<int> UnreadCount(HttpContext httpContext)
        {
            var user = _jwtService.GetUsername(httpContext);
            var count = await _Context.NotificationUser.CountAsync(e => e.UserName == user && e.Status == "unread");
            return count;
        }
        public async Task<bool> SetRemind(Reservation reservation)
        {
            var Description = "The book " + reservation.ResourceId + " is overdue! Please return it by " + reservation.DueDate + " to avoid any fines.";
            var newNotification = new Notifications
            {
                Title = "Reservation Reminder",
                Description = Description,
                ToUser = reservation.BorrowerID,
                Date = DateOnly.FromDateTime(DateTime.Now),
                time = TimeOnly.FromDateTime(DateTime.Now),
                Type = "remind"
            };
            await _Context.Notifications.AddAsync(newNotification);
            await _Context.SaveChangesAsync();

            var newNotificationuser = new NotificationUser
            {
                UserName = reservation.BorrowerID,
                NotificationId = newNotification.Id,
                Status = "unread"
            };
            var tokens = await _Context.FirebaseConnections.Where(e => e.userName == reservation.BorrowerID).ToListAsync();
            foreach (var x in tokens)
            {
                try
                {
                    var message = new Message()
                    {
                        Token = x.Token,
                        Notification = new Notification()
                        {
                            Title = "Reservation Reminder",
                            Body = Description
                        }
                    };
                    string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                }
                catch (Exception e)
                {

                }
            }


            var borrower = await _Context.Users.FirstOrDefaultAsync(e => e.UserName == reservation.BorrowerID);
            var htmlBody = new EmailTemplate().RemindEmail(reservation);
            await _emailService.SendEmail(htmlBody, borrower.Email, "Your Reservation is Overdue");

            await _Context.NotificationUser.AddAsync(newNotificationuser);
            await _Context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> IssueNotification(int reservationNo)
        {
            var reservation = await _Context.Reservations.FirstOrDefaultAsync(e => e.Id == reservationNo);

            if (reservation != null)
            {

                var Description = "The book " + reservation.ResourceId + " has been successfully issued to you. Please return it by " + reservation.DueDate + " to avoid any fines.";
                var notification = new Notifications
                {
                    Title = "About Your ReservationNo : " + reservation.Id,
                    Description = Description,
                    ToUser = reservation.BorrowerID,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    time = TimeOnly.FromDateTime(DateTime.Now),
                    Type = "reservation"

                };
                _Context.Notifications.Add(notification);
                await _Context.SaveChangesAsync();

                var notificationuser = new NotificationUser
                {
                    UserName = reservation.BorrowerID,
                    NotificationId = notification.Id,
                    Status = "unread"
                };


                var tokens = await _Context.FirebaseConnections.Where(e => e.userName == reservation.BorrowerID).Select(e => e.Token).ToListAsync();
                try
                {
                    var message = new MulticastMessage()
                    {
                        Tokens = tokens,
                        Notification = new Notification()
                        {
                            Title = "Return Resource Successfully.ReservationNo : " + reservation.Id,
                            Body = Description
                        }
                    };

                    var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                }
                catch (Exception e)
                {

                }



                _Context.NotificationUser.Add(notificationuser);
                await _Context.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<bool> ReturnNotification(int reservationNo)
        {
            var reservation = await _Context.Reservations.FirstOrDefaultAsync(e => e.Id == reservationNo);
            if (reservation != null)
            {
                var Description = "The book " + reservation.ResourceId + " has been successfully returned by you. Thank you for returning it on time.";
                var notification = new Notifications
                {
                    Title = "Return Resource Successfully.ReservationNo : " + reservation.Id,
                    Description = Description,
                    ToUser = reservation.BorrowerID,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    time = TimeOnly.FromDateTime(DateTime.Now),
                    Type = "reservation"
                };
                await _Context.Notifications.AddAsync(notification);
                await _Context.SaveChangesAsync();

                var notificationuser = new NotificationUser
                {
                    UserName = reservation.BorrowerID,
                    NotificationId = notification.Id,
                    Status = "unread"
                };

                var tokens = await _Context.FirebaseConnections.Where(e => e.userName == reservation.BorrowerID).Select(e => e.Token).ToListAsync();
                try
                {
                    var message = new MulticastMessage()
                    {
                        Tokens = tokens,
                        Notification = new Notification()
                        {
                            Title = "Return Resource Successfully.ReservationNo : " + reservation.Id,
                            Body = Description
                        }
                    };

                    var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                }
                catch (Exception e)
                {

                }




                await _Context.NotificationUser.AddAsync(notificationuser);
                await _Context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<bool> BookAddedNotifications()
        {
            var Description = "Some new books had been added to the library. Check it out! by login to the website.";
            var notification = new Notifications
            {
                Title = "New Books Added",
                Description = Description,
                ToUser = "all",
                Date = DateOnly.FromDateTime(DateTime.Now),
                time = TimeOnly.FromDateTime(DateTime.Now),
                Type = "bookupdates"
            };
            await _Context.Notifications.AddAsync(notification);
            await _Context.SaveChangesAsync();
            var tokens = await _Context.FirebaseConnections.Select(e => e.Token).ToListAsync();
            try
            {
                var message = new MulticastMessage()
                {
                    Tokens = tokens,
                    Notification = new Notification()
                    {
                        Title = notification.Title,
                        Body = Description
                    }
                };
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            }
            catch (Exception e)
            {

            }
            return true;
        }
    }
}
