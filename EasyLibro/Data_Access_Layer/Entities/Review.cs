using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(User))]
        public string? reviewer { get; set; }
        public int Stars { get; set; }
        [ForeignKey(nameof(Resource))]
        public string? ISBN {  get; set; }
        public DateOnly Date {  get; set; }

        public virtual User? User { get; set; }
        public virtual Resource? Resource { get; set; }
    }
}
