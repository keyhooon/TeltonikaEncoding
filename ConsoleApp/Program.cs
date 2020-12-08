using ConsoleApp.RouteService.Dto;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Teltonika.Avl;
using Teltonika.Avl.Communication.FMPro3;
using Teltonika.Avl.Data.FMPro3.Commands.Device;
using Teltonika.Avl.Extensions;

namespace ConsoleApp
{
    class Program
    {
      
        static Route.RouteContext routeContext = new Route.RouteContext(new DirectionService("5b3ce3597851110001cf6248e88658f607ac495cad9088fe0cd49df3"));
        static void Main(string[] args)
        {
            var v = new VehicleManager();
            v.Route = routeContext.GetRoute("");
            v.Driving = new ConstantSpeedDrivingStrategy(v);
            v.IsIgnitiate = true;
            var route = routeContext.GetRoute("");
            Console.WriteLine(route.Path);
            route.Path.SRID = 2855;
            Console.WriteLine(route.Path);
            //var densifier = new NetTopologySuite.Densify.Densifier(route.Path.(2855));
            //densifier.DistanceTolerance = 0.0001;
            
            //Console.WriteLine(densifier.GetResultGeometry());
        }
    }
}
