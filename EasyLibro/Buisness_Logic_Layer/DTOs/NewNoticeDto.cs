namespace Buisness_Logic_Layer.DTOs
{
    public class NewNoticeDto
    {
        public int Id { get; set; }
        public string UserName {  get; set; }
        public string Subject {  get; set; }
        public string Description { get; set; }
        public DateOnly Date { get; set; }
    }


    public class MyNotificationDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int ago { get; set; }
        public string Status { get; set; }
    }

    public class NoticeDto
    {
        public string UserName { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        
    }

    public class SetToken
    {
        public string Token { get; set; }
        public string UserName { get; set; }
    }

    public class SearchNotification
    {
        public string keyword { get; set; }
        public string type { get; set; }
    }
}
