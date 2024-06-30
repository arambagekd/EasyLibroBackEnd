using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Access_Layer.Entities
{
    public class FirebaseConnection
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public string? userName { get; set; }
        public string Token { get; set; }
        public virtual User User { get; set; }
    }
}
