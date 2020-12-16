using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Model
{
    public class Position
    {
        private Coordinate location;
        private double speed;
        private double angle;

        public Position(Coordinate location, double speed = 0.0d, double angle = 0.0d)
        {
            Location = location;
            Speed = speed;
            Angle = angle;
        }
        public event EventHandler PositionChanged;

        public Coordinate Location { get => location;
            set
            {
                location = value;
                PositionChanged?.Invoke(this, null);
            }
        }

        public double Speed
        {
            get => speed;
            set
            {
                speed = value;
                PositionChanged?.Invoke(this, null);
            }
        }

        public double Angle { get => angle;
            set
            {
                angle = value;
                PositionChanged?.Invoke(this, null);
            }
        }
    }
}
