namespace Buisness_Logic_Layer.DTOs
{
    public class ReturnBookDto
    {
        public int reservationNo {  get; set; }
        public string returnby { get; set; }
        public string returnDate { get; set; }
        public bool email {  get; set; }

        public int Penalty { get; set; }
    }
}
