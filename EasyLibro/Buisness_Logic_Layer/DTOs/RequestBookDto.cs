namespace Buisness_Logic_Layer.DTOs
{
    public class AddRequestDto
    {
        public string ISBN {  get; set; }

    }
    public class GetRequestDto
    {
        public int id {  get; set; }
        public string BorrowerID { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public DateOnly Date {  get; set; }

    }
}
