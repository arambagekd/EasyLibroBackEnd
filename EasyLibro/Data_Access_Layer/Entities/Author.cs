using System.ComponentModel.DataAnnotations;

namespace Data_Access_Layer.Entities
{
    public class Author
    {
        [Key]
        public string AuthorName {  get; set; }
        public List<Resource> Resources { get; set; }
        

    }
}
