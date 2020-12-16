using ConsoleApp.Model;
using NetTopologySuite.Geometries;
using System;
using NetTopologySuite.Mathematics;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace ConsoleApp
{
    public class VehicleManager
    {
        private Model.Route route;
        private bool isIgnitiate;
        private DrivingStrategy driving;
        private readonly Vehicle Vehicle;
        ILogger logger;

        public event EventHandler RouteChanged;
        public event EventHandler IsIgnitiateChanged;
        public event EventHandler PositionChanged;
        public VehicleManager(Vehicle vehicle)
        {
            logger = LogManager.GetCurrentClassLogger();
            Vehicle = vehicle;
            Driving = new StopDrivingStrategy(this);
            Vehicle.Position.PositionChanged += (s, e) => onPositionChanged();
        }
        public bool IsIgnitiate
        {
            get => isIgnitiate;
            set
            {
                if (isIgnitiate == value)
                    return;
                if (value && Driving == null)
                    throw new Exception();
                isIgnitiate = value;
                onIsIgnitiatedChanged();
            }
        }

        public Position Position
        {
            get => Vehicle.Position; 
        }

        public Model.Route Route
        {
            get => route;
            set
            {
                if (route == value)
                    return;
                route = value;
                onRouteChanged();
            }
        }

        public DrivingStrategy Driving
        {
            get => driving;
            set
            {
                if (driving == value)
                    return;
                driving = value;
            }
        }

        private void onPositionChanged()
        {
            PositionChanged?.Invoke(this, null);

        }
        private void onIsIgnitiatedChanged()
        {
            IsIgnitiateChanged?.Invoke(this, null);
        }
        private void onRouteChanged()
        {
            RouteChanged?.Invoke(this, null);
        }


    }
}
