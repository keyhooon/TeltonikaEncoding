using System.Collections.Generic;

namespace ConsoleApp.RouteService.Dto
{
    public class Feature
    {
        public List<double> bbox { get; set; }
        public string type { get; set; }
        public Properties properties { get; set; }
        public Geometry geometry { get; set; }
    }
}
