namespace Buisness_Logic_Layer.DTOs
{
    public class LocationListDto
    {
        public string CupboardId { get; set; }
        public string CupboardName { get; set; }
        public List<string> ShelfNo { get; set;}
        public int count { get; set; }
        public int remain { get; set; }
        public int borrowed { get; set; }
    }

    public class LocationDto
    {
       public string CupboardName { get; set; }
    }

    public class AddLocationDto
    {
        public string CupboardName { get; set; }
        public int ShelfNo { get; set; }
    }
}
