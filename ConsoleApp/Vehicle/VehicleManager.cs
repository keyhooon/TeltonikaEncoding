using ConsoleApp.Model;
using NetTopologySuite.Geometries;
using System;
using NetTopologySuite.Mathematics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class VehicleManager
    {
        private Model.Route route;
        private bool isIgnitiate;
        private DrivingStrategy driving;
        private VehiclePosition vehiclePosition;
        private VehiclePosition position;

        public event EventHandler RouteChanged;
        public event EventHandler IsIgnitiateChanged;
        public event EventHandler PositionChanged;

        public DeviceManager Device { get; set; }
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

        public VehiclePosition VehiclePosition
        {
            get => vehiclePosition;
            set
            {
                vehiclePosition = value;
                onPositionChanged();
            }
        }



        public VehiclePosition Position
        {
            get => position; 
            internal set
            {
                position = value;
                onPositionChanged();
            }
        }

        public Model.Route Route
        {
            get => route;
            set
            {
                if (route == value)
                    return;
                route = value;
                Position = new VehiclePosition(route.Path.StartPoint);
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
