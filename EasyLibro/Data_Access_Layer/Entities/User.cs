using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Data_Access_Layer.Entities
{
    public class User
    {

        [Key]
        public string UserName {  get; set; }
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateOnly DOB {  get; set; }
        [Required]
        public string Address {  get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Status {  get; set; }
        public string? Image { get; set; }

        [Required]
        public string UserType {  get; set; }
        public string? AddedById { get; set; }
    

        public virtual User AddedBy { get; set; }

        public virtual DateOnly AddedDate { get; set; }

        public virtual List<Reservation> Reservations { get; set; }

        public virtual List<RequestResource> requests { get; set; }

        public virtual List<NotificationUser> NotificationUser { get; set;}
        
        public virtual List<FirebaseConnection> FirebaseConnections { get; set; }
    }

    

   
}


