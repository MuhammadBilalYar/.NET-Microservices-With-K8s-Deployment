using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    public class IndexController : Controller
    {
        [Route(""), HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]

        public RedirectResult Index()
        {
            return Redirect("/swagger/");
        }
    }
}
