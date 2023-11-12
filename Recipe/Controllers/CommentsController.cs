using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

using Infrastructure.Exceptions;

using Application.DTOs.Request;
using Application.Interfaces.DomainServices;


namespace RecipeApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentService;
        public CommentsController(ICommentsService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetComments(
            [FromQuery] int recipeId, 
            [FromQuery] PaginatedRequest request
            )
        {
            return Ok(
                await _commentService.GetComments(recipeId, request)
                );
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostComment(
                [FromBody] CommentRequest request
            )
        {
            try
            {
                var res = await _commentService.AddComment(request);

                return new CreatedResult(nameof(PostComment), res);
            }
            catch (UnAuthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateComment(
            [FromRoute] int id,
            [FromBody] CommentRequest request
        )
        {
            try 
            { 
                var res = await _commentService.UpdateComment(id, request);

                return new CreatedResult(nameof(UpdateComment), res);
            }
            catch (UnAuthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteComment(
            [FromRoute] int id
            )
        {
            try 
            { 
                await _commentService.DeleteComment(id);

                return NoContent();
            }
            catch (UnAuthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// 
        /// ** Replies ** 
        ///

        [HttpPost("{id}/replies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostReply(
            [FromRoute] int id,
            [FromBody] ReplyRequest request
            )
        {
            try
            {
                var res = await _commentService.AddReply(id, request);

                return new CreatedResult(nameof(PostReply), res);
            }
            catch (UnAuthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/replies/{replyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateReply(
            [FromRoute] int id,
            [FromRoute] int replyId,
            [FromBody] ReplyRequest request
            )
        {
            try
            {
                var res = await _commentService.UpdateReply(replyId, request);

                return new CreatedResult(nameof(UpdateReply), res);
            }
            catch (UnAuthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/replies/{replyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteReply(
            [FromRoute] int id,
            [FromRoute] int replyId
            )
        {
            try
            {
                await _commentService.DeleteReply(replyId);

                return NoContent();
            }
            catch (UnAuthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
