namespace Football.ViewModels
{
    public class SidebarLeagueVm
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; } = string.Empty;

        // Bu liqada canlı neçə oyun var
        public int LiveMatchCount { get; set; }
    }
}
