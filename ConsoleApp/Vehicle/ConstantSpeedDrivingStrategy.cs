using NetTopologySuite.Densify;
using NetTopologySuite.Geometries;
using NetTopologySuite.LinearReferencing;
using NetTopologySuite.Operation.Distance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    public class ConstantSpeedDrivingStrategy : DrivingStrategy
    {
        private LineString Route;
        private LengthIndexedLine lengthIndexedLine;
        public ConstantSpeedDrivingStrategy(VehicleManager vehicle, double spead) : base(vehicle)
        {
            Speed = spead;
            Route = (LineString)Densifier.Densify(vehicle.Route.Path, 0.000001 * spead);
            lengthIndexedLine = new LengthIndexedLine(Route);
        }
        protected override void CalculateLocation(VehicleManager vehicleManager)
        {
            if (lengthIndexedLine == null)
                return;
            var oldCoordinate = vehicleManager.Position.Location;
            var newCoordinate = lengthIndexedLine.ExtractPoint(lengthIndexedLine.Project(oldCoordinate) + 0.000001 * Speed);
            vehicleManager.Position.Location = newCoordinate;
            vehicleManager.Position.Speed = Speed;
            vehicleManager.Position.Angle = vehicleManager.Position.Angle;
        }
        public double Speed { get; }
    }
}
