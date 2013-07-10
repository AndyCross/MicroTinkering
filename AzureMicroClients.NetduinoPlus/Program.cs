using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Elastacloud.AzureBlobDemo.NTP;
using Microsoft.SPOT.Net.NetworkInformation;
using Elastacloud.AzureBlobDemo.Table;

namespace AzureMicroClients.NetduinoPlus
{
    public class Program
    {
        private const string AccountName = "";
        private const string AccountKey = "";
        private static string _macAddress;
        private static TableClient _tableClient;

        public static void Main()
        {
            _macAddress = GetMAC();

            var networkTime = NtpClient.GetNetworkTime();
            Utility.SetLocalTime(networkTime);

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
            NetworkInterface[] netIF = NetworkInterface.GetAllNetworkInterfaces();

            netIF[0].EnableStaticIP("192.168.1.110", "255.255.255.0", "192.168.1.1");
            var ip = netIF[0].IPAddress;

            string macAddress = "";

            // Create a character array for hexidecimal conversion.
            const string hexChars = "0123456789ABCDEF";

            // Loop through the bytes.
            for (int b = 0; b < 6; b++)
            {
                // Grab the top 4 bits and append the hex equivalent to the return string.
                macAddress += hexChars[netIF[0].PhysicalAddress[b] >> 4];

                // Mask off the upper 4 bits to get the rest of it.
                macAddress += hexChars[netIF[0].PhysicalAddress[b] & 0x0F];

                // Add the dash only if the MAC address is not finished.
                if (b < 5) macAddress += "-";
            }

            return macAddress;
        }
    }
}
