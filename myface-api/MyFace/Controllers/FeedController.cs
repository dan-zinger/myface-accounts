using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using Myface.Services;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("feed")]
    public class FeedController : ControllerBase
    {
        private readonly IPostsRepo _posts;
        private readonly IAuthService _authservice;

        public FeedController(IPostsRepo posts, IAuthService authservice)
        {
            _posts = posts;
            _authservice = authservice;
        }

        [HttpGet("")]
        public ActionResult<FeedModel> GetFeed(
            [FromQuery] FeedSearchRequest searchRequest, 
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (_authservice.isUnAuthorizedResult(authorizationHeader))
            {
                return new UnauthorizedResult();
            }
            
            var posts = _posts.SearchFeed(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return FeedModel.Create(searchRequest, posts, postCount);
        }
    }
}
