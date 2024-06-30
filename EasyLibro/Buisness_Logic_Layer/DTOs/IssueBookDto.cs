namespace Buisness_Logic_Layer.DTOs
{
    public class IssueBookFormRequestDto
    {
        public string ISBN { get; set; }
    }
    public class IssueBookFormResponseDto
    {
        public string ISBN { get; set; }
        public string URL { get; set; }
    }

    public class IssueBookRequestDto
{
        public string ISBN { get; set; }
        public string BorrowerID { get; set; }
        public string IssuedID {  get; set; }
        public string dueDate { get; set; }
        public bool Email { get; set;}
        
        public int requestId { get; set; }

    }
    public class IssueBookResponseDto
{
        public int ReservationId {  get; set; }
        public string ISBN { get; set; }
        public string BorrowerId {  get; set; } 
    }
}
