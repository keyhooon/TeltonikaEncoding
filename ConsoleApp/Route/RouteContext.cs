using ConsoleApp.RouteService.Dto;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Route
{
    public class RouteContext
    {
        readonly DirectionService directionService;
        readonly Model.Route DefaultRoute;
        readonly Dictionary<string, Model.Route> RegisteredRoutes;


        public RouteContext(DirectionService directionService)
        {
            this.directionService = directionService;
            RegisteredRoutes = new Dictionary<string, Model.Route>();
            MultiPoint locations = new MultiPoint(new[] { new Point(8.681495, 49.41461), new Point(8.686507, 49.41943), new Point(8.687872, 49.420318) });

            DefaultRoute = directionService.GetRoute(locations);
            RegisteredRoutes.Add("", DefaultRoute);
        }

        public void RegisterRoute(string name, Model.Route route)
        {
            RegisteredRoutes.Add(name, route);
        }

        public Model.Route GetRoute(string name)
        {
            if (RegisteredRoutes.TryGetValue(name, out var ret))
                return ret;
            return DefaultRoute;

        }

    }
}
