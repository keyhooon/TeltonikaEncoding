using ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Teltonika.Avl;

namespace ConsoleApp.Model
{
    public class Vehicle
    {
        public Vehicle()
        {
            Position = new Position(new NetTopologySuite.Geometries.Coordinate());
            IoPropertiesList = new List<IoProperty>();
        }
        public int Id { get; set; }

        public Position Position { get; }

        public List<IoProperty> IoPropertiesList { get; } 

    }
}
