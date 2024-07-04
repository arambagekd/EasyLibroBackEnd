using Buisness_Logic_Layer.AuthHelpers;
using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Data_Access_Layer;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Buisness_Logic_Layer.Services
{
    public class AuthService: IAuthService
    {
        private readonly DataContext _Context;
        private readonly JWTService _jwtService;
        private readonly IUserService _userService;
        private readonly RefreshTokenService _refreshTokenService;

        public AuthService(DataContext context, JWTService jwt, IUserService userService, RefreshTokenService refreshTokenService)
        {
            _Context = context;
            _jwtService = jwt;
            _userService = userService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<IActionResult> Login( AuthDto request)
        {
           
            var user = await _Context.Users.FirstOrDefaultAsync(u => u.UserName == request.userName);

            if (user == null)
            {
                return new BadRequestObjectResult("User not found");
            }
             if (!(BCrypt.Net.BCrypt.Verify( request.password, user.Password)))
                if (request.password != user.Password)
                {
                    return new BadRequestObjectResult("Wrong Password");

                }
            var k = new AuthDto
            {
                userName = user.UserName,
                password = user.Password,
            };

            var permiss=await _Context.Permissions.FirstOrDefaultAsync(e=>e.userName== request.userName);

            if (!(permiss.permission))
            {
                return new BadRequestObjectResult("Permission Denied");
            }

            var jwt = _jwtService.Generate(user.UserName, user.UserType);
            var refreshtoken = _refreshTokenService.GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                Username = user.UserName,
                Token = refreshtoken,
                Expires = DateTime.Now.AddDays(7),
            };

            await _Context.RefreshTokens.AddAsync(refreshToken);
            await _Context.SaveChangesAsync();

          

            return new OkObjectResult(new
            {
                accessToken = jwt,
                refreshToken = refreshtoken,
                message = "success",
            });
        }

        public async Task<IActionResult> MobileLogin([FromBody] AuthDto request)
        {
            var user = await _Context.Users.FirstOrDefaultAsync(u => u.UserName == request.userName);

            if (user == null)
            {
                return new BadRequestObjectResult("User not found");
            }
            if (!(BCrypt.Net.BCrypt.Verify(request.password, user.Password)))
                //if (request.password == user.Password)
                if (request.password != user.Password)
                {
                    return new BadRequestObjectResult("Wrong Password");

                }
            var k = new AuthDto
            {
                userName = user.UserName,
                password = user.Password,
            };

            var permiss = await _Context.Permissions.FirstOrDefaultAsync(e => e.userName == request.userName);

            if (!(permiss.permission))
            {
                return new BadRequestObjectResult("Permission Denied");
            }

            var jwt = _jwtService.GenerateMobileJwt(user.UserName, user.UserType);
           
            
            return new OkObjectResult(new
            {
                accessToken = jwt,
                message = "success",
            });
        }
    

        public async Task<IActionResult> SelectUserType(string userType,HttpContext httpContext)
        {
            var userName = _jwtService.GetUsername(httpContext);
            var user = await _Context.Users.FirstOrDefaultAsync(e => e.UserName == userName);
            var jwt = "";

            if (user == null)
            {
                return new BadRequestObjectResult("User not found");
            }
            if (userType == "admin")
            {
                jwt = _jwtService.Generate(user.UserName, user.UserType);
               
            }
            if (userType == "patron")
            {
                jwt = _jwtService.Generate(user.UserName, "patron");
                
            }


            return new OkObjectResult(new
            {
                token = jwt,
                message = "success",
            });
        }
       
        public async Task<IActionResult> Refresh( TokenRequest tokenRequest)
        {
            if (tokenRequest is null)
            {
                return new BadRequestObjectResult("Invalid client request");
            }

            string accessToken = tokenRequest.accessToken;
            string refreshToken = tokenRequest.refreshToken;


            var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
            var roleClaim = principal.Claims.ToList().FirstOrDefault(c => c.Type == "role");
            var usernameClaim = principal.Claims.ToList().FirstOrDefault(c => c.Type == "username");

            var username = usernameClaim.Value;
            var role = roleClaim.Value;

            if(username == null)
            {
                return new BadRequestObjectResult("Invalid client request");
            }
            if (role == null)
            {
                return new BadRequestObjectResult("Invalid client request");
            }
            var permiss = await _Context.Permissions.FirstOrDefaultAsync(e => e.userName == username);
            if (!(permiss.permission))
            {
                return new BadRequestObjectResult("Permission Denied");
            }

            var savedRefreshToken = await _Context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.Username == username);

            if (savedRefreshToken == null || savedRefreshToken.Expires <= DateTime.Now)
            {
                return new BadRequestObjectResult("Invalid refresh token");
            }

            var newAccessToken = _jwtService.Generate(username, role);
            var newRefreshToken = _refreshTokenService.GenerateRefreshToken();

            savedRefreshToken.Token = newRefreshToken;
            _Context.RefreshTokens.Update(savedRefreshToken);
            await _Context.SaveChangesAsync();

            return new OkObjectResult(new TokenResponse
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }




    }
}
