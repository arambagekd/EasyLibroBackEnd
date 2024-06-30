using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Access_Layer.Entities
{
    public class Notifications
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ToUser { get; set; }
        public DateOnly Date {  get; set; }
        public TimeOnly time { get; set; }
        public string Type {  get; set; }
        public List<NotificationUser> NotificationUser { get; set; }

    }
}
