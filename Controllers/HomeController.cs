using DatabaseConnect.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;


namespace DatabaseConnect.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile configFile)
        {
            var result = new ConnectionInputs();
            if (configFile != null && configFile.Length>0) {
                try
                {
                    string content;
                    using (var reader = new StreamReader(configFile.OpenReadStream()))
                    {
                        content = await reader.ReadToEndAsync();
                    }
                    var json = JObject.Parse(content);
                    string connectionString = json["ConnectionString"].ToString();
                    string password = json["Password"].ToString();
                    if (!connectionString.ToLower().Contains("password"))
                        connectionString += $";Password={password}";
                    using (var conn = new SqlConnection(connectionString))
                    {
                        await conn.OpenAsync();
                        result.success = true;
                        result.Message = "Connection Successsful";
                    }
                }
                catch (Exception ex) { 
                result.success = false;
                    result.Message = ex.Message;
                }

            }
            else
            {
                result.success = false;
                result.Message = "No File";
            }
            return View(result);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
