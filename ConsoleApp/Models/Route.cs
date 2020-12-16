using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Model
{
    public class Route
    {
        public Route(LineString path)
        {
            Path = path;
        }
        public LineString Path { get;}

    }

}
