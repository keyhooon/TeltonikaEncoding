using System.Collections.Generic;

namespace ConsoleApp.RouteService.Dto
{
    public class RequestGeometry
    {
        public RequestGeometry(List<List<double>> coordinate)
        {
            this.coordinates = coordinate;
        }
        public List<List<double>> coordinates { get; set; }

    }
}
