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

        [HttpGet]
        public IActionResult Quiz(string league, string season)
        {
            List<Match> matches = FootballDAL.GetMatches(league, season);

            var random = new Random();
            var question = random.Next(matches.Count);

            // Do the random math in here. Send only a single match to the view
            return View(matches[question]);
        }

        [HttpPost]
        public IActionResult Quiz(Match matchResult, string answer)
        {
            //var winner = "";

            //if(team1score > team2score)
            //{
            //    winner = team1;
            //}
            //else
            //{
            //    winner = team2;
            //}

            //if(answer.Contains(winner))
            //{
                ViewBag.Result = "Winner winner chicken dinner!";
            //}
            //else
            //{
            //    ViewBag.Result = "Loser loser, pick your snoozer";
            //}

            return View(matchResult);
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
