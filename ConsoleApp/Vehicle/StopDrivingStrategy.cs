using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    class StopDrivingStrategy : DrivingStrategy
    {
        public StopDrivingStrategy(VehicleManager vehicle) : base(vehicle)
        {

        }

        protected override void CalculateLocation(VehicleManager vehicle)
        {
            vehicle.Position = new Model.VehiclePosition(vehicle.Position.Location, 0, vehicle.Position.Angle);
        }
    }
}
