namespace Football.ViewModels
{
    public class SidebarVm
    {
        // Bugünkü matçların sayı
        public int TodayMatchCount { get; set; }

        // Canlı matçların ümumi sayı
        public int LiveMatchCount { get; set; }

        // Liqalar
        public List<SidebarLeagueVm> Leagues { get; set; } = new();
    }
   
}
