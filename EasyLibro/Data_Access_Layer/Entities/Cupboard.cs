using System.ComponentModel.DataAnnotations;

namespace Data_Access_Layer.Entities
{
    public class Cupboard
    {
        [Key]
        public int cupboardID { get; set; }
        public string name { get; set; }
    }
}
