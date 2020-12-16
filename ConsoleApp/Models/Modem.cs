using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ConsoleApp.Model
{
    public class Modem
    {
        public int Id { get; set; }
        public ulong Imei { get; set; }
        public string MqttClientId { get; set; }
        public string MqttUserName { get; set; }
        public string MqttPassword { get; set; }
        public string MqttBrokerAddress { get; set; }


    }
}
