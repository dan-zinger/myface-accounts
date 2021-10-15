using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using Myface.Services;


namespace MyFace.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepo _users;
        private readonly IAuthService _authservice;

        public UsersController(IUsersRepo users, IAuthService authservice)
        {
            _users = users;
            _authservice = authservice;
        }
        
        [HttpGet("")]
        public ActionResult<UserListResponse> Search(
            [FromQuery] UserSearchRequest searchRequest,
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (!(_authservice.IsAuthenticated(authorizationHeader)))
            {
                return new UnauthorizedResult();
            }
            var users = _users.Search(searchRequest);
            var userCount = _users.Count(searchRequest);
            return UserListResponse.Create(searchRequest, users, userCount);
        }

        [HttpGet("{id}")]
        public ActionResult<UserResponse> GetById(
            [FromRoute] int id,
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (!(_authservice.IsAuthenticated(authorizationHeader)))
            {
                return new UnauthorizedResult();
            }

            var user = _users.GetById(id);
            return new UserResponse(user);
        }

        [HttpPost("create")]
        public IActionResult Create(
            [FromBody] CreateUserRequest newUser,
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (!(_authservice.IsAuthenticated(authorizationHeader)))
            {
                return new UnauthorizedResult();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = _users.Create(newUser);

            var url = Url.Action("GetById", new { id = user.Id });
            var responseViewModel = new UserResponse(user);
            return Created(url, responseViewModel);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<UserResponse> Update(
            [FromRoute] int id, 
            [FromBody] UpdateUserRequest update,
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (!(_authservice.IsAuthenticated(authorizationHeader)))
            {
                return new UnauthorizedResult();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _users.Update(id, update);
            return new UserResponse(user);
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(
            [FromRoute] int id,
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (!(_authservice.IsAuthenticated(authorizationHeader)))
            {
                return new UnauthorizedResult();
            }

            var user = _users.GetUserByAuthorizationHeader(authorizationHeader);

            if (_authservice.isAuthorizedAdmin(user))
            {
                return StatusCode(403, "Access Denied");
            }
            _users.Delete(id);
            return Ok();
        }
    }
}