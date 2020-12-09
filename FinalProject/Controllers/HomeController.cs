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

        public IActionResult RecentHighlights()
        {
            List<Highlight> highlights = FootballDAL.GetHighlights();
            return View(highlights);
        }

        [HttpPost]
        public IActionResult LeagueStandings(string league)
        {
            FootballStandings standings = FootballDAL.GetStandings(league);
            return View(standings);
        }

        [HttpPost]
        public IActionResult LeagueTeams(string league)
        {
            FootballClubs clubs = FootballDAL.GetTeams(league);
            return View(clubs);
        }

        public IActionResult MatchResults(string league)
        {
            FootballMatches clubs = FootballDAL.GetMatches(league);
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
