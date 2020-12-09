using System.Collections.Generic;

namespace ConsoleApp.RouteService.Dto
{
    public class Properties
    {
        public List<Segment> segments { get; set; }
        public Summary summary { get; set; }
        public List<int> way_points { get; set; }
    }
}
