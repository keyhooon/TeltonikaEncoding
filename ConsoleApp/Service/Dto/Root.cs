using System.Collections.Generic;

namespace ConsoleApp.RouteService.Dto
{
    public class Root
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
        public List<double> bbox { get; set; }
        public Metadata metadata { get; set; }
    }
}
