using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HighlightService _highlightService;
        private readonly FootballDBContext _db;

        public HomeController(ILogger<HomeController> logger, FootballDBContext db)
        {
            _logger = logger;
            _highlightService = new HighlightService();
            _db = db;
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
        public IActionResult LeagueTeams(string league)
        {
            List<Club> clubs = FootballDAL.GetTeams(league);
            return View(clubs);
        }

        public IActionResult MatchResults(string league, string season)
        {
            List<Match> clubs = FootballDAL.GetMatches(league, season);
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
        public IActionResult FavoriteTeamHighlights(int? page)
        {
            var favoriteTeams = (from t in _db.UserFavoriteTeams
                                 where _db.UserFavoriteTeams.Any(x => x.UserId == FindUser())
                                 select t.Team).ToList();
            List<List<Highlight>> highlights = _highlightService.GetFavoriteHighlights(favoriteTeams);
            if (page == null)
            {
                page = 1;
            }
            ViewBag.pageCount = page;
            ViewBag.listCount = highlights.Count;
            return View(highlights[(int)page - 1]);
        }

        public IActionResult FavoriteTeams()
        {
            var favoriteTeamsIDs = from t in _db.UserFavoriteTeams
                                   where _db.UserFavoriteTeams.Any(x => x.UserId == FindUser())
                                   select t.TeamId;

            var favoriteTeams = (from t in _db.UserFavoriteTeams
                                   where _db.UserFavoriteTeams.Any(x => x.UserId == FindUser())
                                   select t.Team).ToList();

            var LeagueID = from all in _db.Teams
                           where favoriteTeamsIDs.Any(x => x.Value == all.Id)
                           select all.LeagueId;

            var favoriteTeamLeagues = from l in _db.Leagues
                                     where LeagueID.Any(x => x.Value == l.Id)
                                     select l.LeagueName;
            List<Match> matches = new List<Match>();

            foreach (var league in favoriteTeamLeagues)
            {
                string leagueOut = "";
                switch (league)
                {
                    case "England":
                        leagueOut = "en.1";
                        break;
                    case "Spain":
                        leagueOut = "es.1";
                        break;
                    case "Germany":
                        leagueOut = "de.1";
                        break;
                    case "Italy":
                        leagueOut = "it.1";
                        break;
                    case "France":
                        leagueOut = "fr.1";
                        break;
                    default:
                        break;
                }
                foreach (var team in favoriteTeams)
                {
                    matches.AddRange(FootballDAL.GetMatches(leagueOut, "2020-21").Where(x => x.team1 == team.TeamName || x.team2 == team.TeamName));
                }
            }            
            return View(matches);
        }

        public string FindUser()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var userId = claim.Value;
            return userId;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
