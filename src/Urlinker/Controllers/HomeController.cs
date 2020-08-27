using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Urlinker.Models;
using Urlinker.Repos;
using Urlinker.ViewModels;
using Urlinker.Dto;

namespace Urlinker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILinker _linker;

        public HomeController(ILogger<HomeController> logger, ILinker linker)
        {
            _logger = logger;

            _linker = linker;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShortName(string urlName)
        {
            if (string.IsNullOrWhiteSpace(urlName)) return Content("Respect yourself pleaase....");

            var erl = new Uri(urlName);
            Console.WriteLine($"{erl.ToString()}");

            var urlString = new UrlRequestDto();
            urlString.OriginalUrl = erl.OriginalString;

            var urlink = new uLinker()
            {
                id = Guid.NewGuid(),
                UrlShortName = await _linker.GenerateShortName(),
                OriginalUrl = urlName
            };
            try
            {
                await _linker.AddUrl(urlink);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
            }

            //string baseUrl = string.Format("{0}://{1}", this.Request.Scheme, this.Request.Host.Value.ToString());

            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}";
            string bUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value.ToString()}";

            //const string domainUrl = @"http://localhost:5001";

            TempData["OriginalUrl"] = urlName;
            TempData["shortUrl"] = $"https://localhost:5001/{urlink.UrlShortName}";

            var urlVm = new UrlViewModels()
            {
                shortenedUrl = $"{bUrl}/{urlink.UrlShortName}",
                OriginalUrl = urlName
            };


            return RedirectToAction("Index", urlVm);
        }


        [Route("dev/{shortName}")]
        public async Task<IActionResult> UrlRedirect(string shortName)
        {
            if (!string.IsNullOrWhiteSpace(shortName))
            {
                var UrlToRedirect = await _linker.GetOriginalUrlByName(shortName);

                return RedirectPermanent(UrlToRedirect);
            }
            return RedirectToAction("Index");
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
