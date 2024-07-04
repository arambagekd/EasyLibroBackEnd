using Buisness_Logic_Layer.DTOs;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Http;


namespace Buisness_Logic_Layer.Interfaces
{
    public interface INotificationService
    {
        Task<bool> NewNotice(NoticeDto newnotice);
        Task<List<NewNoticeDto>> GetNotification(SearchNotification searchnotification);
        Task<List<MyNotificationDto>> GetMyNotification(HttpContext httpContext);
        Task<bool> SetRemind(Reservation reservation);
        Task<bool> IssueNotification(int reservationNo);
        Task<bool> ReturnNotification(int reservationNo);
        Task<bool> RemoveNotification(int reservationNo);
        Task<bool> SetFireBaseToken(SetToken setToken);
        Task<bool> RemoveFireBaseToken(SetToken setToken);
        Task<bool> MarkAsRead(int id);
        Task<int> UnreadCount(HttpContext httpContext);
        Task<bool> BookAddedNotifications();
    }
}
