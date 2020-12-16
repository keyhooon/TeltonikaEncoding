using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;

namespace ConsoleApp.Model
{
    public class Device
    {
        public event EventHandler InstalledOnVehicleChanged;
        private Vehicle installedOnVehicle;

        public Device(Modem modem)
        {
            Modem = modem;
            Scenarios = new ObservableCollection<Scenario>();
        }

        public int Id { get; set; }
        public Modem Modem { get;}
        public Vehicle InstalledOnVehicle
        {
            get => installedOnVehicle;
            set
            {
                installedOnVehicle = value;
                OnInstalledOnVehicleChanged();
            }
        }
        public ObservableCollection<Scenario> Scenarios { get; set; }

        private void OnInstalledOnVehicleChanged()
        {
            InstalledOnVehicleChanged?.Invoke(this, null);
        }
    }
}
