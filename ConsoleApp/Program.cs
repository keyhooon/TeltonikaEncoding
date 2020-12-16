using ConsoleApp.Devices;
using ConsoleApp.Model;
using ConsoleApp.Models;
using ConsoleApp.RouteService.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Teltonika.Avl;

namespace ConsoleApp
{
    internal class Program
    {
        private const string MqttClientId = "";
        private const string MqttUserName = "Simulator";
        private const string MqttPassword = "Simu@12345";
        private const string MqttBrokerAddress = "192.168.137.60";

        private static void Main(string[] args)
        {
            Route.RouteContext routeContext = new Route.RouteContext(new DirectionService("5b3ce3597851110001cf6248e88658f607ac495cad9088fe0cd49df3"));

            List<Models.IoPropertyType> ioPropertiesTypeList = new List<Models.IoPropertyType> (new Models.IoPropertyType[] {
                new Models.IoPropertyType() {
                    Id = 1,
                    Size = 2,
                    Description=""
                },
                new Models.IoPropertyType() {
                    Id = 2,
                    Size = 2,
                    Description=""
                },
                new Models.IoPropertyType() {
                    Id = 3,
                    Size = 2,
                    Description=""
                },
                new Models.IoPropertyType() {
                    Id = 4,
                    Size = 2,
                    Description=""
                },
                new Models.IoPropertyType() {
                    Id = 5,
                    Size = 2,
                    Description=""
                },
                new Models.IoPropertyType() {
                    Id = 6,
                    Size = 2,
                    Description=""
                },
                new Models.IoPropertyType() {
                    Id = 7,
                    Size = 2,
                    Description=""
                },
                new Models.IoPropertyType() {
                    Id = 8,
                    Size = 2,
                    Description=""
                },
                new Models.IoPropertyType() {
                    Id = 9,
                    Size = 2,
                    Description=""
                },

            });




            List<Scenario> scenariosList = new List<Scenario>(new Scenario[] { new Scenario() {
                    ObservedIoProperties = new List<Models.IoPropertyType>(new Models.IoPropertyType[] { ioPropertiesTypeList[0], ioPropertiesTypeList[1], ioPropertiesTypeList[2],}),
                    RecordConditions = new List<RecordCondition>(new RecordCondition[] {new FixedTimeRecordCondition() { Period = TimeSpan.FromSeconds(7)} })

                }, 
                new Scenario() {
                    ObservedIoProperties = new List<Models.IoPropertyType>(new Models.IoPropertyType[] {ioPropertiesTypeList[3], ioPropertiesTypeList[4], ioPropertiesTypeList[5],}),
                    RecordConditions = new List<RecordCondition>(new RecordCondition[] {new FixedTimeRecordCondition() { Period = TimeSpan.FromSeconds(3)} })

                }, new Scenario() {
                    ObservedIoProperties = new List<Models.IoPropertyType>(new Models.IoPropertyType[] {ioPropertiesTypeList[6], ioPropertiesTypeList[7], ioPropertiesTypeList[8],}),
                    RecordConditions = new List<RecordCondition>(new RecordCondition[] {new FixedTimeRecordCondition() { Period = TimeSpan.FromSeconds(5)} })
                } });
            List<Vehicle> VehiclesList = new List<Vehicle>();
            for (int i = 0; i < 100; i++)
                VehiclesList.Add(new Vehicle());
            foreach (var vehicle in VehiclesList)
            {
                foreach (var ioPropertyType in ioPropertiesTypeList)
                {
                    vehicle.IoPropertiesList.Add(IoProperty.Create(ioPropertyType.Id, 0));
                }
            }
            List<Device> DevicesList = new List<Device>();
            for (int i = 0; i < 100; i++)
                DevicesList.Add(new Device(
                    new Modem()
                    {
                        Imei = (ulong)i,
                        MqttBrokerAddress = MqttBrokerAddress,
                        MqttClientId = i.ToString(),
                        MqttUserName = "Simulator",
                        MqttPassword = "Simu@12345"
                    })
                    {
                        InstalledOnVehicle = VehiclesList[i],
                        Scenarios = new ObservableCollection<Scenario>(scenariosList),
                    }
                );

            var recordingTaskManager = new RecordingTaskManager();
            recordingTaskManager.RegisterRecordingPolicies((recordCondition) => new TimeFixedRecordingPolicies(((FixedTimeRecordCondition)recordCondition).Period), typeof(FixedTimeRecordCondition));

            List<DeviceManager> deviceManagerList = new List<DeviceManager>();
            deviceManagerList.AddRange(DevicesList.Select(o => new DeviceManager(o, recordingTaskManager)));
            List<VehicleManager> vehicleManagerList = new List<VehicleManager>();
            vehicleManagerList.AddRange(VehiclesList.Select(o => {
                var vehicleManager =  new VehicleManager(o);
                vehicleManager.Route = routeContext.GetRoute("");
                vehicleManager.IsIgnitiate = true;
                vehicleManager.Driving = new ConstantSpeedDrivingStrategy(vehicleManager, 100);
                return vehicleManager;
            }));
            while (true) { };
        }
    }
}
