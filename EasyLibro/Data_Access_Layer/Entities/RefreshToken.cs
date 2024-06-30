using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Access_Layer.Entities
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public string? Username { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }

        public virtual User User { get; set; }
    }
}
