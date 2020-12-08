using System.Collections.Generic;

namespace ConsoleApp.RouteService.Dto
{
    public class Segment
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public List<Step> steps { get; set; }
    }
}
