using Football.Application.Facades;
using Football.Application.Interfaces.Facades;
using Football.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace Football.Web.ViewComponents;

public class SidebarViewComponent : ViewComponent
{
    private readonly IFootballPredictionFacade _facade;

    public SidebarViewComponent(IFootballPredictionFacade facade)
    {
        _facade = facade;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Backend-dən data alırıq
        var todayMatches = await _facade.GetTodayMatchesAsync();
        var liveMatches = await _facade.GetLiveMatchesAsync();

        // Sadə league mapping (sonra API-dən də gələ bilər)
        var leagues = new List<SidebarLeagueVm>
        {
            new() { LeagueId = 39, LeagueName = "Premier League" },
            new() { LeagueId = 140, LeagueName = "La Liga" },
            new() { LeagueId = 135, LeagueName = "Serie A" },
            new() { LeagueId = 78, LeagueName = "Bundesliga" }
        };

        // Canlı oyunları liqaya görə say
        foreach (var league in leagues)
        {
            league.LiveMatchCount = liveMatches
                .Count(x => x.LeagueId == league.LeagueId);
        }

        var model = new SidebarVm
        {
            TodayMatchCount = todayMatches.Count,
            LiveMatchCount = liveMatches.Count,
            Leagues = leagues
        };

        return View("~/Views/Shared/_Sidebar.cshtml", model);

    }
}
