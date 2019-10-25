using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyPortfolioWebApp.Attributes;
using MyPortfolioWebApp.DbContexts.BlogDbContext;
using MyPortfolioWebApp.Models.Blog;
using MyPortfolioWebApp.Services.CommentServices;
using System.Linq;
using System.Security.Claims;

namespace MyPortfolioWebApp.Controllers
{
    [Route("api/[controller]")]
    public class CommentsApiController : Controller
    {
        private readonly CommentManager _commentManager;
        private readonly IWebHostEnvironment _env;

        public CommentsApiController(CommentManager commentManager, IWebHostEnvironment env)
        {
            _commentManager = commentManager;
            _env = env;
        }
        [HttpPost]
        [AjaxOnly]
        [Route("Addcomment")]
        public IActionResult AddComment([FromBody]Comment comment)
        {
            var currentUserId = HttpContext.User
                .Claims?
                .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?
                .Value ?? string.Empty;

            if (currentUserId != comment.UserId)
            {
                return StatusCode(403, new { message = "Invalid user id"});
            }

            var (success, message) = _commentManager.AddComment(comment);

            if (success)
            {
                return Json(new
                {
                    commentId = comment.CommentId,
                    postedDate = comment.PostedDate
                });
            }
            return StatusCode(500, new { message });

        }
        [HttpPost]
        [AjaxOnly]
        [Route("Deletecomment/{commentId:int}")]
        public IActionResult DeleteComment(int commentId)
        {
            var currentUserId = HttpContext.User
              .Claims?
              .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?
              .Value ?? string.Empty;

            var commentOwnerId = _commentManager.GetCommentOwner(commentId);

            if (currentUserId != commentOwnerId)
            {
                return StatusCode(403, new { message = "Invalid user id"});
            }
            var (success, message) = _commentManager.DeleteComment(commentId);

            if (success)
            {
                return Ok(new { message });
            }
            return StatusCode(500, new { message });

        }
        
        [HttpPost]
        [AjaxOnly]
        [Route("Changecommenttext")]
        public IActionResult ChangeCommentText([FromBody] ChangeCommentTextModel model)
        {
            if (!this.ModelState.IsValid) { return BadRequest("Cannot update comment. Data are invalid"); }

            var currentUserId = HttpContext.User
               .Claims?
               .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?
               .Value ?? string.Empty;

            var commentOwnerId = _commentManager.GetCommentOwner(model.CommentId);

            if(commentOwnerId != currentUserId)
            {
                return StatusCode(403, new { message = "Invalid user id" });
            }

            var (success, message) = _commentManager
                .UpdateComment(model.CommentId, model.CommentText);

            if (success)
            {
                return Ok(new { message });
            }

            return BadRequest(new { message });
        }
    }
}

