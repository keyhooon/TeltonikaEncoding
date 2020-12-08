using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public abstract class DrivingStrategy : IDisposable
    {
        private bool isRun;
        Timer timer;
        public DrivingStrategy(VehicleManager vehicle)
        {
            vehicle.IsIgnitiateChanged += (sender, e) => IsRun = vehicle.IsIgnitiate;
            timer = new Timer((o) => {
                if (((VehicleManager)o).Driving != this)
                    Dispose();
                CalculateLocation((VehicleManager)o); 
            
            }, vehicle, 0, Timeout.Infinite);      
        }

        public bool IsRun {
            get => isRun;
            private set 
            {
                isRun = value;

                timer.Change(0, isRun ? 100 : Timeout.Infinite) ;
            } 
        }

        public void Dispose()
        {
            timer.Dispose();
        }

        protected abstract void CalculateLocation(VehicleManager vehicle);

    }
}
