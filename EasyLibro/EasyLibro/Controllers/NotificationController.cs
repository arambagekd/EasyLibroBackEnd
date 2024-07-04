using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EasyLibro.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        [HttpPost("SetFireBaseToken")]
        public async Task<bool> SetFireBaseToken(SetToken setToken)
        {
            return await _notificationService.SetFireBaseToken(setToken);
        }

        [HttpPost("NewNotice")]
        public async Task<bool> NewNotice(NoticeDto newnotice)
        {
            return await _notificationService.NewNotice(newnotice);
        }

        [HttpPost("SetRemind")]
        public async Task<bool> SetRemind(Reservation reservation)
        {
            return await _notificationService.SetRemind(reservation);
        }

        [HttpPost("GetNotificatons")]
        public async Task<List<NewNoticeDto>> GetNotification(SearchNotification searchnotification)
        {
          
           return await _notificationService.GetNotification(searchnotification);
        }
        [HttpPost("GetMyNotificatons")]
        public async Task<List<MyNotificationDto>> GetMyNotification()
        {
            var httpContext = HttpContext;
            return await _notificationService.GetMyNotification(httpContext);
        }

        [HttpDelete("RemoveNotification")]
        public async Task<bool> RemoveNotification(int id)
        {
            return await _notificationService.RemoveNotification(id);
        }
        [HttpGet("MarkAsRead")]
        public async Task<bool> MarkAsRead(int id)
        {
            return await _notificationService.MarkAsRead(id);
        }
        [HttpPost("UnreadCount")]
        public async Task<int> UnreadCount()
        {
            var httpContext = HttpContext;
            return await _notificationService.UnreadCount(httpContext);
        }

        [HttpPost("RemoveFireBaseToken")]
        public async Task<bool> RemoveFireBaseToken(SetToken setToken)
        {
            return await _notificationService.RemoveFireBaseToken(setToken);
        }
    }
}
