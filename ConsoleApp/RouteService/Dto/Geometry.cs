using System.Collections.Generic;

namespace ConsoleApp.RouteService.Dto
{
    public class Geometry
    {
        public List<List<double>> coordinates { get; set; }
        public string type { get; set; }
    }
}
