using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
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

        //break Highlight list into lists of 10
        public static List<List<Highlight>> SplitList(List<Highlight> highlights)
        {
            var list = new List<List<Highlight>>();

            for (int i = 0; i < highlights.Count; i += 10)
            {
                list.Add(highlights.GetRange(i, Math.Min(10, highlights.Count - i)));
            }
            return list;
        }
        //figure out what page to display
        //[HttpGet]
        public IActionResult RecentHighlights(int? page)
        {
            List<Highlight> highlights = FootballDAL.GetHighlights();
            List<List<Highlight>> list = SplitList(highlights).ToList();
            if (page == null)
            {
                page = 1;
            }
            ViewBag.pageCount = page;

            return View(list[(int)page - 1]);
            //switch (page)
            //{
            //    case 1:
            //        return View(list[0]);
            //    case 2:
            //        return View(list[1]);
            //    case 3:
            //        return View(list[2]);
            //    case 4:
            //        return View(list[3]);
            //    case 5:
            //        return View(list[4]);
            //}
            //return View();
        }

        [HttpPost]
        public IActionResult LeagueTeams(string league, string season)
        {
            List<Club> clubs = FootballDAL.GetTeams(league, season);
            return View(clubs);
        }

        public IActionResult Matches()
        {
            List<Match> clubs = FootballDAL.GetMatches();
            return View(clubs);
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
