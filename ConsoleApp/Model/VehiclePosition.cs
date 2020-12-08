using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Model
{
    public class VehiclePosition
    {
        public VehiclePosition(Point location, double speed = 0.0d, double angle = 0.0d)
        {
            Location = location;
            Speed = speed;
            Angle = angle;
        }

        public Point Location { get; set; }

        public double Speed { get; set; }

        public double Angle { get; set; }
    }
}
