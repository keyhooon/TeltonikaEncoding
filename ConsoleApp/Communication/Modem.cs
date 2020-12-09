using MQTTnet;
using MQTTnet.Adapter;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using MQTTnet.Diagnostics;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Teltonika.Avl.Data.FMPro3.Commands.Device;
using Teltonika.Avl.Extensions;

namespace ConsoleApp.Communication
{
    public class Modem
    {

        MqttFactory factory;
        IMqttClient mqttClient;
        ILogger logger;
        private MqttNetLogger mqttLogger;

        public Modem(DeviceManager deviceManager)
        {
            DeviceManager = deviceManager;
            logger = LogManager.GetCurrentClassLogger();
            mqttLogger = new MqttNetLogger();
            mqttLogger.LogMessagePublished += MqttLogger_LogMessagePublished;

            
            factory = new MqttFactory(mqttLogger);
            // Create TCP based options using the builder.
            var options = new MqttClientOptionsBuilder()
                .WithClientId(deviceManager.Device.Imei.ToString())
                .WithCredentials("Simulator", "Simu@12345")
                .WithTcpServer("192.168.137.60")
                .WithCleanSession(false)
                .WithSessionExpiryInterval(60 * 60 * 24)
                .Build();

            mqttClient = factory.CreateMqttClient(mqttLogger);
            MqttClientAuthenticateResult conResult;
            Task<Task> connectionTask = Task.Factory.StartNew(async () =>
            {
                do
                {
                    conResult = await mqttClient.ConnectAsync(options);

                } while (conResult.ResultCode == MqttClientConnectResultCode.Success);
            });
            mqttClient.UseDisconnectedHandler((e) =>  connectionTask);
            mqttClient.UseConnectedHandler(async (e) => {
                while (mqttClient.IsConnected)
                {
                    var devicePacket = deviceManager.GetDevicePacket();
                    if (devicePacket != null)
                    {
                        await mqttClient.PublishAsync("/MachineData", devicePacket.Encode());
                        logger.Info($"Device Packet: {devicePacket}");
                        continue;
                    }
                    await Task.Delay(10000);
                }
            });
        }

        public DeviceManager DeviceManager { get; }

        private void MqttLogger_LogMessagePublished(object sender, MqttNetLogMessagePublishedEventArgs e)
        {
            switch (e.LogMessage.Level)
            {
                case MqttNetLogLevel.Verbose:
                    logger.Debug(e.LogMessage.Exception, e.LogMessage.Message);
                    break;
                case MqttNetLogLevel.Info:
                    logger.Info(e.LogMessage.Exception, e.LogMessage.Message);
                    break;
                case MqttNetLogLevel.Warning:
                    logger.Warn(e.LogMessage.Exception, e.LogMessage.Message);
                    break;
                case MqttNetLogLevel.Error:
                    logger.Error(e.LogMessage.Exception, e.LogMessage.Message);
                    break;
            }  
        }
    }
}
