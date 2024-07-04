namespace Buisness_Logic_Layer.DTOs
{
    public class CreateUserRequestDto
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Image { get; set; }
        public string UserType { get; set; }
    }

    public class CreateUserResponseDto
    {
        public string UserName {  get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
      
    }

    public class EditUserRequestDto
    {
        public string FName { get; set; }
        public string LName { get; set; }

        public string DOB { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; } 
    
    }

}
