namespace Buisness_Logic_Layer.DTOs
{
    public class ReservationDto
    {
        public int reservationNo { get; set; }
        public string Resource { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string BorrowerName { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly DueDate { get; set; }
        public string Status { get; set; }
    }

    public class SearchDetails
    {
        public string Keywords { get; set; }

        public string type { get; set; }
    }
}