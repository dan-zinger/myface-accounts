using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using Myface.Services;


namespace MyFace.Controllers

{
    [ApiController]
    [Route("/login")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authservice;
        private readonly IUsersRepo _users;
        public LoginController(IAuthService authservice, IUsersRepo users)
        {
            _authservice = authservice;
            _users = users;
        }

        [HttpGet("")]
        public ActionResult<LoginResponse> login(
            [FromHeader (Name = "Authorization")] string authorizationHeader )
            {
                
                if (!(_authservice.IsAuthenticated(authorizationHeader)))
                {
                    return new UnauthorizedResult();
                }
                
                var user = _users.GetUserByAuthorizationHeader(authorizationHeader);

                var isAdmin = _authservice.isAuthorizedAdmin(user) ? true : false;


                return new LoginResponse(user);
            }
    }

    
}