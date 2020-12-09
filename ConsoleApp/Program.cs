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
            var v = new VehicleManager[] { new VehicleManager(), new VehicleManager() };
            v[0].Route = routeContext.GetRoute("");
            v[0].Driving = new ConstantSpeedDrivingStrategy(v[0], 100);
            v[0].DeviceManager = new DeviceManager(v[0], new Model.Device() { Imei = 0x1122334455667788 });
            v[0].DeviceManager.RecordingPolicies.AddRecordingPolicies(new TimerRecordingPolicies(TimeSpan.FromSeconds(5)));
            v[0].IsIgnitiate = true;
            //v[1].Route = routeContext.GetRoute("");
            //v[1].Driving = new ConstantSpeedDrivingStrategy(v[1], 100);
            //v[1].DeviceManager = new DeviceManager(v[1], new Model.Device() { Imei = 0x1122334455667711 });
            //v[1].DeviceManager.RecordingPolicies.AddRecordingPolicies(new TimerRecordingPolicies(TimeSpan.FromSeconds(5)));
            //v[1].IsIgnitiate = true;
            while (true) { };
        }
    }
}
