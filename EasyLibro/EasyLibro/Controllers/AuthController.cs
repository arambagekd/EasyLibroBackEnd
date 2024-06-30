
using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace EasyLibro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService )
        {
           _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto request )
        {
           return await _authService.Login(request);
        }

        [HttpPost("mobilelogin")]
        public async  Task<IActionResult> MobileLogin([FromBody] AuthDto request)
        {
           return await _authService.MobileLogin(request);
        }
       
        public class UserType
        {
            public string userType { get; set;}
        }

        [HttpPost("selectusertype")]
        public async Task<IActionResult> SelectUserType([FromBody] UserType userType)
        {
           return await _authService.SelectUserType(userType.userType,HttpContext);  
        }

      //  [HttpGet("user")]
        //public async Task<IActionResult> User()
        //{
          //  try { 
            //var jwt = Request.Cookies["jwt"];
           // var token = _jwtService.Verify(jwt);
            //string userName = token.Issuer;
           // var user = _userService.GetById(userName);

            //return Ok(user);
           // }
            //catch (Exception ex)
            //{
              //  return Unauthorized();
            //}
        //}

        //[HttpPost("Logout")]
       // public async Task<IActionResult> LogOut()
        //{
           // Response.Cookies.Delete("jwt");
         //   return Ok(
             //   new
               // {
                 //   message = "logout"
                //}/
          //  );
        //}

      

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequest tokenRequest)
        {
            return await _authService.Refresh(tokenRequest);
        }




    }
}
