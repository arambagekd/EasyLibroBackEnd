namespace Buisness_Logic_Layer.DTOs
{
    public class Dashboard
    {
        int Total {  get; set; }
        int Books { get; set; }
        int Ebooks { get; set; }
        int Journals { get; set; }
        int Users { get; set; }
        int Reservation { get; set; }
        int Requests { get; set; }
        int Overdue { get; set; }
        List<ReservationDto> OverdueList {  get; set; }

    }
}
