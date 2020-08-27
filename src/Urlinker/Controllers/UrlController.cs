using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Urlinker.Dto;
using Urlinker.Dto.Responses;
using Urlinker.Interfaces;
using Urlinker.ViewModels;

namespace Urlinker.Controllers
{
    public class UrlController : Controller
    {
        private readonly ILogger<UrlController> _logger;
        private readonly IUrlService _urlService;
        public UrlController(ILogger<UrlController> logger, IUrlService urlService)
        {
            _logger = logger;
            _urlService = urlService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateShortUrl(string newUrl)
        {
            if(!string.IsNullOrWhiteSpace(newUrl))
                return Content("The specified Url is wrong");

            //var response = new UrlAddResponse();
            var urlVm = new UrlViewModels();

            var response = await _urlService.AddUrlAsync(newUrl);
            if(response.IsSuccess == false)
            {
                urlVm.Message = response.Message; 
                return RedirectToAction("Index", urlVm);
            }
            urlVm = BuildUrlVm(response);
            urlVm.OriginalUrl =newUrl;
            return RedirectToAction("Index", urlVm);
        }

        [Route("{shortName}")]
        public async Task<IActionResult> UrlRedirect(string shortName)
        {
            if(!string.IsNullOrWhiteSpace(shortName))
            {
                //Error might occur on non instantiated variable e.g UrlGetResponse......
                var UrlToRedirect = await _urlService.GetOriginalUrlByNameAsync(shortName);
                if(UrlToRedirect.IsSuccess == true)
                {
                    return RedirectPermanent(UrlToRedirect.OriginalUrl);
                }       
                else
                {
                    var urlVm = new UrlViewModels{  Message = UrlToRedirect.Message };
                    return RedirectToAction("Index", urlVm);
                }
            }
            return RedirectToAction("Index");
        }

        private static UrlViewModels BuildUrlVm(UrlAddResponse response)
        {
            return new UrlViewModels
            {
                shortenedUrl = response.newShortenUrl,
                Message = response.Message
            };
        }
    }
}