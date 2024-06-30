namespace Data_Access_Layer.Entities { 
    public class RequestResource
    {
        public int Id { get; set; }
        public string ResourceId { get; set; }
        public string UserId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public User User { get; set; }
        public Resource Resource { get; set; }
    }
}
