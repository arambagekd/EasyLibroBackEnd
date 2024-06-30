using Buisness_Logic_Layer.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Buisness_Logic_Layer.Interfaces
{
    public interface IAuthService
    {

        Task<IActionResult> Login(AuthDto request);
        Task<IActionResult> Refresh(TokenRequest tokenRequest);
        Task<IActionResult> MobileLogin(AuthDto request);
        Task<IActionResult> SelectUserType(string userType, HttpContext httpContext);

    }
}
