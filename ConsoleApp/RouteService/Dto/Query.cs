using System.Collections.Generic;

namespace ConsoleApp.RouteService.Dto
{
    public class Query
    {
        public List<List<double>> coordinates { get; set; }
        public string profile { get; set; }
        public string format { get; set; }
    }
}
