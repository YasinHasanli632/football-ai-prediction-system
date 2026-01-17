using Football.Application.Interfaces.Facades;
using Microsoft.AspNetCore.Mvc;

namespace Football.Controllers
{
    public class FootballController : Controller
    {
        private readonly IFootballPredictionFacade _facade;

        public FootballController(IFootballPredictionFacade facade)
        {
            _facade = facade;
        }

        // =========================
        // BUGÜNKÜ OYUNLAR
        // =========================
        // /Football
        public async Task<IActionResult> Index()
        {
            var matches = await _facade.GetTodayMatchesAsync();
            return View(matches);
        }

        // =========================
        // CANLI OYUNLAR
        // =========================
        // /Football/Live
        public async Task<IActionResult> Live()
        {
            var matches = await _facade.GetLiveMatchesAsync();
            return View(matches);
        }

        // =========================
        // LİQAYA GÖRƏ OYUNLAR
        // =========================
        // /Football/League/39
        [HttpGet("Football/League/{leagueId}")]
        public async Task<IActionResult> League(int leagueId)
        {
            var matches = await _facade.GetMatchesByLeagueAsync(leagueId);
            return View("Index", matches);
        }

        // =========================
        // MATCH DETAILS (PREDICTION)
        // =========================
        // AJAX / modal üçün
        // /Football/Details/123
        public async Task<IActionResult> Details(int matchId)
        {
            try
            {
                var prediction = await _facade.PredictMatchAsync(matchId);
                return PartialView("_MatchDetailsModal", prediction);
            }
            catch
            {
                // Əgər prediction mümkün deyilsə
                return BadRequest("Prediction not available for this match.");
            }
        }
    }
}