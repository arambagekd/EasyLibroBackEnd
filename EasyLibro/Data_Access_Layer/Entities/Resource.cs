using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Access_Layer.Entities
{
    public class Resource
    {
        
        
        [Key]
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Title { get; set; }
        [ForeignKey(nameof(Author))]
        [Required]
        public string AuthorName {  get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        //  public int year { get; set; }
        public int Borrowed { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public int PageCount { get; set; }
        [Required]
        public DateTime AddedOn { get; set; }
        [Required]
        public string ImageURL { get; set; }
        [ForeignKey(nameof(AddedBy))]
        public string? AddedByID { get; set; }
        [Required]
        public int Year { get; set; }

        [ForeignKey(nameof(Location))]
        [Required]
        public string BookLocation { get; set; }



        public virtual User AddedBy { get; set; }
        public virtual Location Location { get; set; }
        public virtual Author Author { get; set; }
        
    }
}
