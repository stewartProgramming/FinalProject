using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IActionResult SearchHighlights(string searchFor, int? page)
        {
            List<Highlight> highlights = FootballDAL.GetHighlights();
            List<Highlight> searchResults = new List<Highlight> { };

            foreach (var video in highlights)
            {
                if (video.competition.name.ToLower().Contains(searchFor.ToLower()))
                {
                    searchResults.Add(video);
                }
                if (video.title.ToLower().Contains(searchFor.ToLower()))
                {
                    searchResults.Add(video);
                }
            }
            List<List<Highlight>> list = _highlightService.SplitList(searchResults);
            if (page == null)
            {
                page = 1;
            }
            ViewBag.pageCount = page;
            ViewBag.listCount = list.Count;
            var beautifiedSearch = string.Join(" ", searchFor.ToLower().Split(" ").Select(word => $"{char.ToUpper(word[0])}{word.Substring(1)}"));
            ViewBag.Search = beautifiedSearch;
            return View(list[(int)page - 1]);
        }
        [HttpPost]
        public IActionResult LeagueStandings(string league, string season)
        {
            FootballStandings standings = FootballDAL.GetStandings(league, season);
            return View(standings);
        }

        [HttpPost]
        public IActionResult LeagueTeams(string league, string season)
        {
            FootballClubs clubs = FootballDAL.GetTeams(league, season);
            return View(clubs);
        }

        public IActionResult MatchResults(string league, string season)
        {
            FootballMatches clubs = FootballDAL.GetMatches(league, season);
            return View(clubs);
        }

        // Grabbing scores based on rank difference (in standings) and current form (last five games results)
        public IActionResult Probability(string league, string team1, string team2)
        {
            // Translates between different APIs (standings api and league/results api)
            switch (league)
            {
                case "Premier League 2020/21":
                    league = "39";
                    team1 = ConvertTeamEngland(team1);
                    team2 = ConvertTeamEngland(team2);
                    break;
                case "Primera División 2020/21":
                    league = "140";
                    team1 = ConvertTeamSpain(team1);
                    team2 = ConvertTeamSpain(team2);
                    break;
                case "Bundesliga 2020/21":
                    league = "78";
                    team1 = ConvertTeamGermany(team1);
                    team2 = ConvertTeamGermany(team2);
                    break;
                case "Serie A 2020/21":
                    league = "135";
                    team1 = ConvertTeamItaly(team1);
                    team2 = ConvertTeamItaly(team2);
                    break;
                case "Ligue 1 2020/21":
                    league = "61";
                    team1 = ConvertTeamFrance(team1);
                    team2 = ConvertTeamFrance(team2);
                    break;
                default:
                    break;
            }


            // Calls the standings API based on league of teams playing
            var results = FootballDAL.GetStandings(league, "2020");
            var standings = results.response[0].league.standings[0];
            List<Standing> standingslist = new List<Standing>();

            // Filtering through standings and grabbing selected teams (from match results view)
            foreach(var item in standings)
            {
                if (item.team.name == team1)
                {
                    standingslist.Add(item);
                }
            }

            foreach (var item in standings)
            {
                if (item.team.name == team2)
                {
                    standingslist.Add(item);
                }
            }

            //Checks current form scores and adds according to results (W, D, L)
            string team1Form = standingslist[0].form;
            string team2Form = standingslist[1].form;
            double team1FormScore = 0;
            double team2FormScore = 0;
            foreach (char f in team1Form)
            {
                if (f == 'W')
                {
                    team1FormScore += 2;
                }
                else if (f == 'D')
                {
                    team1FormScore += 1;
                }
            }
            foreach (char f in team2Form)
            {
                if (f == 'W')
                {
                    team2FormScore += 2;
                }
                else if (f == 'D')
                {
                    team2FormScore += 1;
                }
            }

            // Checks current rank difference between two teams
            int team1Rank = standingslist[0].rank;
            int team2Rank = standingslist[1].rank;
            double team1RankScore = 0;
            double team2RankScore = 0;
            double difference = 0;

            // Statements to prevent negative number result
            if (team1Rank < team2Rank)
            {
                difference = team2Rank - team1Rank;
                team1RankScore = difference / standings.Length * 10;
                team2RankScore = difference / standings.Length * 5;
            }
            else
            {
                difference = team1Rank - team2Rank;
                team1RankScore = difference / standings.Length * 5;
                team2RankScore = difference / standings.Length * 10;
            }

            int team1GD = standingslist[0].goalsDiff;
            int team2GD = standingslist[1].goalsDiff;
            
            double team1GDScore = 0;
            double team2GDScore = 0;

            if (team1GD != team2GD)
            {
                if (team1GD > team2GD)
                {
                    int teamsGD = team1GD - team2GD;
                    if (teamsGD < 5 && teamsGD > 0)
                    {
                        team1GDScore = 1;
                    }
                    else if (teamsGD < 10)
                    {
                        team1GDScore = 2;
                    }
                    else if (teamsGD < 15)
                    {
                        team1GDScore = 3;
                    }
                    else
                    {
                        team1GDScore = 4;
                    }
                }
                else
                {
                    int teamsGD = team2GD - team1GD;
                    if (teamsGD < 5 && teamsGD > 0)
                    {
                        team2GDScore = 1;
                    }
                    else if (teamsGD < 10)
                    {
                        team2GDScore = 2;
                    }
                    else if (teamsGD < 15)
                    {
                        team2GDScore = 3;
                    }
                    else
                    {
                        team2GDScore = 4;
                    }
                } 
            }

            // Adding all the values grabbed and sending them to the VM
            MatchProbabilityViewModel vm = new MatchProbabilityViewModel();
            vm.Team1 = team1;
            vm.Team2 = team2;
            vm.Team1Score = team1FormScore + team1RankScore + team1GDScore;
            vm.Team2Score = team2FormScore + team2RankScore + team2GDScore;
            return View(vm);
        }

        // Translates the team names between the two separate APIs
        public string ConvertTeamEngland(string team)
        {
            switch (team)
            {
                case "Tottenham Hotspur FC":
                    team = "Tottenham";
                    break;
                case "Chelsea FC":
                    team = "Chelsea";
                    break;
                case "Manchester United FC":
                    team = "Manchester United";
                    break;
                case "Leicester City FC":
                    team = "Leicester";
                    break;
                case "Newcastle United FC":
                    team = "Newcastle";
                    break;
                case "Fulham FC":
                    team = "Fulham";
                    break;
                case "Crystal Palace FC":
                    team = "Crystal Palace";
                    break;
                case "Brighton & Hove Albion FC":
                    team = "Brighton";
                    break;
                case "Wolverhampton Wanderers FC":
                    team = "Wolves";
                    break;
                case "Everton FC":
                    team = "Everton";
                    break;
                case "Liverpool FC":
                    team = "Liverpool";
                    break;
                case "West Ham United FC":
                    team = "West Ham";
                    break;
                case "Southampton FC":
                    team = "Southampton";
                    break;
                case "Burnley FC":
                    team = "Burnley";
                    break;
                case "Arsenal FC":
                    team = "Arsenal";
                    break;
                case "Manchester City FC":
                    team = "Manchester City";
                    break;
                case "West Bromwich Albion FC":
                    team = "West Brom";
                    break;
                case "Aston Villa FC":
                    team = "Aston Villa";
                    break;
                case "Leeds United FC":
                    team = "Leeds";
                    break;
                case "Sheffield United FC":
                    team = "Sheffield";
                    break;
                default:
                    break;
            }
            return team;
        }

        public string ConvertTeamSpain(string team)
        {
            switch (team)
            {
                case "Cádiz CF":
                    team = "Cadiz";
                    break;
                case "Elche CF":
                    team = "Elche";
                    break;
                case "CA Osasuna":
                    team = "Osasuna";
                    break;
                case "Real Valladolid CF":
                    team = "Valladolid";
                    break;
                case "Levante UD":
                    team = "Levante";
                    break;
                case "RC Celta Vigo":
                    team = "Celta Vigo";
                    break;
                case "Villarreal CF":
                    team = "Villarreal";
                    break;
                case "FC Barcelona":
                    team = "Barcelona";
                    break;
                case "Deportivo Alavés":
                    team = "Alaves";
                    break;
                case "SD Eibar":
                    team = "Eibar";
                    break;
                case "SD Huesca":
                    team = "Huesca";
                    break;
                case "Sevilla FC":
                    team = "Sevilla";
                    break;
                case "Getafe CF":
                    team = "Getafe";
                    break;
                case "Valencia CF":
                    team = "Valencia";
                    break;
                case "Atlético Madrid":
                    team = "Atletico Madrid";
                    break;
                case "Athletic Club Bilbao":
                    team = "Athletic Club";
                    break;
                default:
                    break;
            }
            return team;
        }public string ConvertTeamGermany(string team)
        {
            switch (team)
            {
                case "1. FC Köln":
                    team = "FC Koln";
                    break;
                case "1. FC Union Berlin":
                    team = "Union Berlin";
                    break;
                case "Bayern München":
                    team = "Bayern Munich";
                    break;
                case "TSG 1899 Hoffenheim":
                    team = "1899 Hoffenheim";
                    break;
                case "Hertha BSC":
                    team = "Hertha Berlin";
                    break;
                case "Bor. Mönchengladbach":
                    team = "Borussia Monchengladbach";
                    break;
                case "Bayer 04 Leverkusen":
                    team = "Bayer Leverkusen";
                    break;
                case "1. FSV Mainz 05":
                    team = "FSV Mainz 05";
                    break;
                default:
                    break;
            }
            return team;
        }public string ConvertTeamItaly(string team)
        {
            switch (team)
            {
                case "Juventus":
                    team = "Juventus";
                    break;
                case "SS Lazio":
                    team = "Lazio";
                    break;
                case "SSC Napoli":
                    team = "Napoli";
                    break;
                case "Torino FC":
                    team = "Torino";
                    break;
                case "Cagliari Calcio":
                    team = "Cagliari";
                    break;
                case "US Sassuolo Calcio":
                    team = "Sassuolo";
                    break;
                case "FC Internazionale Milano":
                    team = "Inter";
                    break;
                case "Udinese Calcio":
                    team = "Udinese";
                    break;
                case "Bologna FC":
                    team = "Bologna";
                    break;
                case "Atalanta Bergamo":
                    team = "Atalanta";
                    break;
                case "UC Sampdoria":
                    team = "Sampdoria";
                    break;
                case "ACF Fiorentina":
                    team = "Fiorentina";
                    break;
                case "Genoa CFC":
                    team = "Genoa";
                    break;
                case "Hellas Verona":
                    team = "Verona";
                    break;
                case "FC Crotone":
                    team = "Crotone";
                    break;
                default:
                    break;
            }
            return team;
        }public string ConvertTeamFrance(string team)
        {
            switch (team)
            {
                case "Olympique de Marseille":
                    team = "Marseille";
                    break;
                case "FC Nantes":
                    team = "Nantes";
                    break;
                case "AS Monaco":
                    team = "Monaco";
                    break;
                case "Montpellier HSC":
                    team = "Montpellier";
                    break;
                case "Dijon FCO":
                    team = "Dijon";
                    break;
                case "Lille OSC":
                    team = "Lille";
                    break;
                case "Stade Rennais FC":
                    team = "Rennes";
                    break;
                case "As Saint-Étienne":
                    team = "Saint Etienne";
                    break;
                case "OGC Nice":
                    team = "Nice";
                    break;
                case "Stade de Reims":
                    team = "Reims";
                    break;
                case "Angers SCO":
                    team = "Angers";
                    break;
                case "Nîmes Olympique":
                    team = "Nimes";
                    break;
                case "Olympique Lyonnais":
                    team = "Lyon";
                    break;
                case "Paris Saint-Germain":
                    team = "Paris Saint Germain";
                    break;
                case "Girondins de Bordeaux":
                    team = "Bordeaux";
                    break;
                case "RC Strasbourg":
                    team = "Strasbourg";
                    break;
                case "RC Lens":
                    team = "Lens";
                    break;
                case "FC Lorient":
                    team = "Lorient";
                    break;
                case "FC Metz":
                    team = "Metz";
                    break;
                default:
                    break;
            }
            return team;
        }

        [HttpGet]
        public IActionResult Quiz(string league, string season)
        {
            FootballMatches matches = FootballDAL.GetMatches(league, season);

            Random r = new Random();
            int index = r.Next(matches.matches.Count);

            Match match = matches.matches[index];

            while (match.score == null)
            {
                index = r.Next(matches.matches.Count);
                match = matches.matches[index];
            }

            TempData["League"] = league;
            TempData["Season"] = season;
            TempData["MatchIndex"] = index;
            return View(match);
        }

        [HttpPost]
        public IActionResult QuizResult(int index, string league, string season, string answer)
        {
            FootballMatches matches = FootballDAL.GetMatches(league, season);
            Match match = matches.matches[index];

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

        public IActionResult AddFavoriteTeam()
        {
            CascadingModel cm = new CascadingModel();
            var leagues = _db.Leagues.ToList();
            foreach (var league in leagues)
            {
                cm.Leagues.Add(new SelectListItem
                {
                    Text = league.LeagueName,
                    Value = league.Id.ToString()
                });
            }
            return View(cm);
        }        

        [HttpPost]
        public IActionResult AddFavoriteTeam(int? LeagueID, int? TeamID)
        {
            CascadingModel cm = new CascadingModel();
            var leagues = _db.Leagues.ToList();
            foreach (var league in leagues)
            {
                cm.Leagues.Add(new SelectListItem
                {
                    Text = league.LeagueName,
                    Value = league.Id.ToString()
                });
            }
            if (LeagueID.HasValue)
            {
                var teams = (from team in _db.Teams
                             where team.LeagueId == LeagueID.Value
                             select team).ToList();
                foreach (var team in teams)
                {
                    cm.Teams.Add(new SelectListItem
                    {
                        Text = team.TeamName,
                        Value = team.Id.ToString()
                    });
                }
            }
            if (TeamID.HasValue)
            {
                UserFavoriteTeams uf = new UserFavoriteTeams();
                uf.UserId = FindUser();
                uf.TeamId = (int)TeamID;
                _db.UserFavoriteTeams.Add(uf);
                try
                {
                _db.SaveChanges();
                return RedirectToAction("FavoriteTeams");
                }
                catch (Exception)
                {
                    return RedirectToAction("FavoriteTeams");
                }
            }
            return View(cm);
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
                           where favoriteTeamsIDs.Any(x => x == all.Id)
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
