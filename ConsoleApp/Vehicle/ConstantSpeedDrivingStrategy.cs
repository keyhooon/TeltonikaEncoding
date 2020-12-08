using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class ConstantSpeedDrivingStrategy : DrivingStrategy
    {
        private LineString Route;
        public ConstantSpeedDrivingStrategy(VehicleManager vehicle, double spead) : base(vehicle)
        {
            Speed = spead;
            var densifier = new NetTopologySuite.Densify.Densifier(vehicle.Route.Path);
            densifier.DistanceTolerance = 0.000001 * spead; ;
            Route = (LineString)densifier.GetResultGeometry();
        }
        protected override void CalculateLocation(VehicleManager vehicle)
        {
            var oldPos = vehicle.Position;
            var distanceOp = new NetTopologySuite.Operation.Distance.DistanceOp(oldPos.Location,Route);
            distanceOp.NearestPoints()[1]
            Route.near
            vehicle.Position = new Model.VehiclePosition(vehicle.Position.Location, Speed, vehicle.Position.Angle);
        }
        public double Speed { get; }
    }
}
