namespace Buisness_Logic_Layer.DTOs
{
    public class AdminDashboardStatics
    {
        public int Total { get; set; }
        public int ReturnToday { get; set; }
        public int IssueToday { get; set; }
        public int Locations { get; set; }
        public int Users { get; set; }
        public int Reservations { get; set; }
        public int Requests { get; set; }
        public int OverDue {  get; set; }
    }


    public class PatronDashboardStatics
    {
        public string Status { get; set; }
        public int myReservations { get; set; }
        public int Requests { get; set; }
        public int Penalty { get; set; }
    }
}
