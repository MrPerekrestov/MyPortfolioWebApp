using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyPortfolioWebApp.Models.Blog;
using MyPortfolioWebApp.Services.BlogPostsRepository;
using MyPortfolioWebApp.Services.OperationsWithFiles;

namespace MyPortfolioWebApp.Controllers
{
    public class BlogController : Controller
    {      
        private readonly IBlogPostsRepository _repository;
        private readonly ILogger<BlogController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IBlogImageResolver _imageResolver;

        public BlogController(
            IBlogPostsRepository repository,
            ILogger<BlogController> logger,
            IWebHostEnvironment env,
            IBlogImageResolver imageResolver)
        {            
            _repository = repository;
            _logger = logger;
            _env = env;
            _imageResolver = imageResolver;
        }
        [HttpGet]
        [Route("Blog/{id:int}")]
        public IActionResult BlogPost(int id)
        {
            var blogPostHtml = _repository.GetBlogPostHtml(id);

            if (string.IsNullOrEmpty(blogPostHtml) || !_repository.BlogExists(id))
            {
                return RedirectToAction("About", "Home");
            }

            _imageResolver.Resolve(id);

            var returnUri = HttpContext.Request.Path;

            var authorized = HttpContext.User.Identity.IsAuthenticated;

            var currentUserId = HttpContext.User
                .Claims?
                .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?
                .Value??string.Empty;

            var currentUserName = HttpContext.User.Claims?
                .FirstOrDefault(claim => claim.Type == ClaimTypes.GivenName)?
                .Value??string.Empty;       

            var commentData = _repository.GetComments(id).ToList();
            var model = new BlogPostModel
            {
                BlogPostHtml = blogPostHtml,
                ReturnUri = returnUri,
                Authorized = authorized,
                CurrentUserId = currentUserId,
                CurrentUserName = currentUserName,
                CommentsData = commentData,
                PostId = id
            };
           
            return View("BlogPost", model);
        }
    }
}
