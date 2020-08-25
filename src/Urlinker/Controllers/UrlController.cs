using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Urlinker.Models;
using Urlinker.Repos;
using Urlinker.ViewModels;

namespace Urlinker.Controllers
{
    public class UrlController : Controller
    {
        private readonly ILogger<UrlController> _logger;
        private readonly ILinker _linker;

        public UrlController(ILogger<UrlController> logger)
        {
            _logger = logger;
            
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShortName(string urlName)
        {
            await Task.Delay(500);
            return RedirectToAction("Index");
        }

        [Route("{shortName}")]
        public async Task<IActionResult> UrlRedirect(string shortName)
        {
            await Task.Delay(100);
            return RedirectToAction("Index");
        }

    }
}