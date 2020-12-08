using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class ConstantSpeedDrivingStrategy : DrivingStrategy
    {
        public ConstantSpeedDrivingStrategy(VehicleManager vehicle) : base(vehicle)
        {
            Speed = 100;
        }
        protected override void CalculateLocation(VehicleManager vehicle)
        {
            var oldPos = vehicle.Position;

            vehicle.Position = new Model.VehiclePosition(vehicle.Position.Location, Speed, vehicle.Position.Angle);
        }
        public double Speed { get; set; }
    }
}
