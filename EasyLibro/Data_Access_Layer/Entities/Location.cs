using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Access_Layer.Entities
{
    public class Location
    {
        [Key]
        public string LocationNo { get; set; }
        [ForeignKey(nameof(cupboard))]
        public int CupboardId { get; set; }
        public int ShelfNo { get; set; }
        public List<Resource> resources { get; set; }
        public  Cupboard cupboard { get; set; }
    }


}
