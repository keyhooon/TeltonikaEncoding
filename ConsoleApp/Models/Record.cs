using ConsoleApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Teltonika.Avl;

namespace ConsoleApp.Models
{
    public class Record
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public Position Position { get; set; }
        public List<IoProperty> IoProperties { get; set; }
    }
}
