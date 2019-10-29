using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;


namespace MyPortfolioWebApp.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        [HttpPost]
        [Route("Authorize")]

        public IActionResult Authorize(string returnUri)
        {
            var properties = new AuthenticationProperties()
            {
                RedirectUri = returnUri
            };
            return Challenge(properties, "Facebook");
        }
        [HttpGet]
        [Route("Callback")]
        public IActionResult Callback(string returnUri)
        {
            return Redirect(returnUri);
        }
        [HttpPost]
        [Route("Signout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}
