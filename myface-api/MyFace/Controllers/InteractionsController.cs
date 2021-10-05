using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using Myface.Services;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/interactions")]
    public class InteractionsController : ControllerBase
    {
        private readonly IInteractionsRepo _interactions;
        private readonly IAuthService _authservice;

        public InteractionsController(IInteractionsRepo interactions, IAuthService authservice)
        {
            _interactions = interactions;
            _authservice = authservice;
        }
    
        [HttpGet("")]
        public ActionResult<ListResponse<InteractionResponse>> Search(
            [FromQuery] SearchRequest search, 
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (_authservice.isUnAuthorizedResult(authorizationHeader))
            {
                return new UnauthorizedResult();
            }
            
            var interactions = _interactions.Search(search);
            var interactionCount = _interactions.Count(search);
            return InteractionListResponse.Create(search, interactions, interactionCount);
        }

        [HttpGet("{id}")]
        public ActionResult<InteractionResponse> GetById(
            [FromRoute] int id, 
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (_authservice.isUnAuthorizedResult(authorizationHeader))
            {
                return new UnauthorizedResult();
            }
            
            var interaction = _interactions.GetById(id);
            return new InteractionResponse(interaction);
        }

        [HttpPost("create")]
        public IActionResult Create(
            [FromBody] CreateInteractionRequest newUser, 
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (_authservice.isUnAuthorizedResult(authorizationHeader))
            {
                return new UnauthorizedResult();
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        
            var interaction = _interactions.Create(newUser);

            var url = Url.Action("GetById", new { id = interaction.Id });
            var responseViewModel = new InteractionResponse(interaction);
            return Created(url, responseViewModel);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(
            [FromRoute] int id, 
            [FromHeader (Name = "Authorization")] string authorizationHeader)
        {
            if (_authservice.isUnAuthorizedResult(authorizationHeader))
            {
                return new UnauthorizedResult();
            }
            
            _interactions.Delete(id);
            return Ok();
        }
    }
}
