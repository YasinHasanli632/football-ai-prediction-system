using Football.Application.Interfaces.Facades;
using Football.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Football.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFootballPredictionFacade _facade;

        public HomeController(IFootballPredictionFacade facade)
        {
            _facade = facade;
        }

        public async Task<IActionResult> Index()
        {
            var todayMatches = await _facade.GetTodayMatchesAsync();
            var liveMatches = await _facade.GetLiveMatchesAsync();

            var model = new HomeVm
            {
                TodayMatchCount = todayMatches.Count,
                LiveMatchCount = liveMatches.Count,

                // hələlik sabit, sonra dinamik edəcəyik
                LeagueCount = 4,
                PredictionCount = todayMatches.Count
            };

            return View(model);
        }
    }
}
