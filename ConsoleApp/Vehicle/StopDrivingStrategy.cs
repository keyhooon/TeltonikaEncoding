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

        protected override void CalculateLocation(VehicleManager vehicleManager)
        {
            vehicleManager.Position.Location = vehicleManager.Position.Location;
            vehicleManager.Position.Speed = 0;
        }
    }
}
