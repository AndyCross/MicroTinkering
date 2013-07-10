using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
//using Microsoft.SPOT;
//using Microsoft.SPOT.Hardware;
//using SecretLabs.NETMF.Hardware;
//using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Elastacloud.AzureBlobDemo.NTP;
//using Microsoft.SPOT.Net.NetworkInformation;
using Elastacloud.AzureBlobDemo.Table;

namespace AzureMicroClients.NetduinoPlus
{
    public class Program
    {
        private const string AccountName = "bradygaster";
        private const string AccountKey = "bradybradygaster!";
        private static string _macAddress;
        private static TableClient _tableClient;

        public static void Main()
        {
            _macAddress = GetMAC();

            //var networkTime = NtpClient.GetNetworkTime();
            //Utility.SetLocalTime(networkTime);

            _tableClient = new TableClient(AccountName, AccountKey);
            _tableClient.CreateTable("netmfdata");

            while (true)
            {
                Thread.Sleep(2000);

                _tableClient.AddTableEntityForTemperature("netmfdata", 
                    _macAddress, 
                    (DateTime.Now.Ticks).ToString(), 
                    DateTime.Now, 
                    42,
                    "USA");
            }
        }

        private static string GetMAC()
        {
            return "Whatevz";
        }
    }
}
