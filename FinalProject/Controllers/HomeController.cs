using FinalProject.Models;
using FinalProject.Services;
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
        private readonly HighlightService _highlightService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _highlightService = new HighlightService();
        }

        public IActionResult Index()
        {
            return View();
        }

        //figure out what list to display
        public IActionResult RecentHighlights(int? page)
        {
            List<List<Highlight>> list = _highlightService.GetHighlights();
            if (page == null)
            {
                page = 1;
            }
            ViewBag.pageCount = page;
            ViewBag.listCount = list.Count;
            return View(list[(int)page - 1]);
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

        [HttpGet]
        public IActionResult Quiz(string league, string season)
        {
            List<Match> matches = FootballDAL.GetMatches(league, season);

            Random r = new Random();
            int index = r.Next(matches.Count);

            Match match = matches[index];

            // Still needs to successfully check if score is null
            while (match.score == null)
            {
                index = r.Next(matches.Count);
                match = matches[index];
            }

            TempData["League"] = league;
            TempData["Season"] = season;
            TempData["MatchIndex"] = index;
            return View(match);
        }

        [HttpPost]
        public IActionResult QuizResult(int index, string league, string season, string answer)
        {
            List<Match> matches = FootballDAL.GetMatches(league, season);
            Match match = matches[index];

            var winner = "";

            if (match.score.ft[0] > match.score.ft[1])
            {
                winner = "team1";
            }
            else if (match.score.ft[0] < match.score.ft[1])
            {
                winner = "team2";
            }
            else if (match.score.ft[0] == match.score.ft[1])
            {
                winner = "tie";
            }

            if (answer == winner)
            {
                ViewBag.Result = "Congratulations! You really know your football trivia.";
            }
            else
            {
                ViewBag.Result = "Sorry, you were incorrect. Better luck next time.";
            }
            return View(match);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
