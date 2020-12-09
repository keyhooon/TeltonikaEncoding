using System;

namespace ConsoleApp.RouteService.Dto
{
    public class Engine
    {
        public string version { get; set; }
        public DateTime build_date { get; set; }
        public DateTime graph_date { get; set; }
    }
}
