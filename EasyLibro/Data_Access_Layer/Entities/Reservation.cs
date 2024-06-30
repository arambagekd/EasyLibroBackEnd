using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Access_Layer.Entities
{
    public class Reservation
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ReservationNo { get; set; }
        [Required]
        public DateOnly IssuedDate { get; set; }
        [Required]
        public DateOnly DueDate {  get; set; }
        public DateOnly? ReturnDate { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public int Penalty { get; set; }
        
        
        [ForeignKey(nameof(Resource))]
        public string? ResourceId {  get; set; }
        
        [ForeignKey(nameof(IssuedBy))]
        public string? IssuedByID { get; set; }

        [ForeignKey(nameof(Borrower))]
        public string? BorrowerID { get; set; }



        public User Borrower { get; set; }
        public User IssuedBy { get; set; }
        public Resource Resource { get; set; }
        
        


    }
}
