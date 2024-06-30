namespace Buisness_Logic_Layer.DTOs
{
    public class AboutReservationDto
    {
        public int ResId {  get; set; }
        public string? ISBN { get; set; }
        public string? BookTitle {  get; set; }
        public string? UserName {  get; set; }
        public DateOnly DateIssue { get; set; }
        public DateOnly DueDate { get; set; }
        public string? Issuer {  get; set; }
        public DateOnly? ReturnDate { get; set; }
        public string Status { get; set; }
        public int Penalty { get; set; }
       public string ImagePath { get; set; }

    }
}
