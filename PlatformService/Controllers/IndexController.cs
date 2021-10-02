using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace PlatformService.Controllers
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
