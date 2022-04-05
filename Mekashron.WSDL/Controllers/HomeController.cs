using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ServiceReference1;
using System.Threading.Tasks;

namespace Mekashron.WSDL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IToastifyService _toastyf;
        private readonly ILogger<HomeController> _logger;
        private readonly IICUTech _webService;
        public HomeController(ILogger<HomeController> logger,
            IToastifyService toastyf, 
            IICUTech webService)
        {
            _logger = logger;
            _toastyf = toastyf;
            _webService = webService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            var request = new LoginRequest(username, password, "string");
            var response = await _webService.LoginAsync(request);

            var jObject = JObject.Parse(response.@return);

            if (jObject.ContainsKey("ResultCode"))
            {
                _toastyf.Error("Error: " + jObject["ResultMessage"]);
            }
            else
            {
                _toastyf.Success("Success: EntityId: " + jObject["EntityId"] + " FirstName: " + jObject["FirstName"] + " LastName: " + jObject["LastName"]);
            }

            return View();
        }

    }
}
