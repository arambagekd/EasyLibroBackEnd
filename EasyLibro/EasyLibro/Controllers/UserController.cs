using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EasyLibro.Controllers
{
    //  [Authorize]
    [Route("api/[controller]")]
    [ApiController]


    public class UserController : ControllerBase
    {
        //Create IUserService Field
        private readonly IUserService _userService;

        //Constructor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("AddUser")]
        public async Task<CreateUserResponseDto> AddUser(CreateUserRequestDto userdto)
        {
            var x = HttpContext;
            return await _userService.AddUser(userdto, x);
        }
        [HttpPut("EditUser")]
        public async Task<bool> EditUser(EditUserRequestDto edituser)
        {
            var x = HttpContext;
            return await _userService.EditUser(edituser, x);
        }
        [HttpPut("EditProfilePicture")]
        public async Task<bool> EditProfilePicture( string image)
        {
            return await _userService.EditProfilePicture(HttpContext, image);
        }
        [HttpGet("DeleteUser")]
        public async Task<bool> DeleteUser(string username)
        {
            return await _userService.DeleteUser(username);
        }
        [HttpPost("AboutUser")]
        public async Task<AboutUserDto> AboutUser(user user)
        {
            return await _userService.AboutUser(user.username);
        }
        [HttpPost("SearchUser")]
        public async Task<List<UserListDto>> SearchUsers(SearchUserDto searchUser)
        {

            return await _userService.SearchUser(searchUser);
        }
        [HttpPut("ChangePassword")]
        public async Task<bool> ChangePassword(ChangePasswordDto request)
        {
            var x = HttpContext;
            return await _userService.ChangePassword(request, x);
        }

        [HttpPost("GetMyData")]
        public async Task<AboutUserDto> GetMyData()
        {
            var x = HttpContext;
            return await _userService.GetMyData(x);
        }

        [HttpPost("GetEmail")]
        public async Task<String> GetEmail()
        {
            var x = HttpContext;
            return await _userService.GetEmail(x);
        }

        [HttpPut("ChangeEmail")]
        public async Task<bool> ChangeEmail(string newEmail)
        {
            var x = HttpContext;

            return await _userService.ChangeEmail(newEmail, x);
        }

        [HttpPut("ResetPassword")]
        public async Task<bool> ResetPassword(ChangePasswordDto request)
        {
            var x = HttpContext;

            return await _userService.ResetPassword(request, x);
        }

        public class email
        {
            public string emailaddress { get; set; }
        }
        [HttpPost("forgetPassword")]
        public async Task<bool> ForgetPassword(email email)
        {


            return await _userService.SendForgotPasswordEmail(email.emailaddress);
        }

       public class user
        {
            public string username { get; set; }
        }

        [HttpPost("AddAdmin")]
        public async Task<bool> AddAdmin()
        {
            return await _userService.AddAdmin();
        }
    }
    }
