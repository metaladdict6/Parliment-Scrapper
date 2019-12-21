using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parliment_Scrapper.Services;

namespace Parliment_Scrapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScrappingController : ControllerBase
    {
        string TestUrl = "https://www.tweedekamer.nl/kamerstukken/plenaire_verslagen/detail/2e878452-e1c4-4235-a100-d3552688b56a#iddaeb907a";

        ScrappingService ScrappingService;
        public ScrappingController()
        {
            ScrappingService = new ScrappingService();
        }

        [HttpGet("{url}")]
        public async Task ScrapUrlAsync(string url)
        {
            var document = await ScrappingService.GetUrlDocument(TestUrl);
            ScrappingService.ScrapePageForText(document);
        }

        [HttpGet("All")]
        public async Task ScrappingParliment()
        {
            var parlimentUrl = "https://www.tweedekamer.nl/kamerstukken/plenaire_verslagen";
            List<string> urls = new List<string>();
            var document = await ScrappingService.GetUrlDocument(parlimentUrl);
            await ScrappingService.GetAllNotesUrlsFromParlimentAsync(document, urls);
            Console.WriteLine("Amount of Notes: " + urls.Count);
            ScrappingService.ScrapeAllPagesForText(urls);
        }
    }
}