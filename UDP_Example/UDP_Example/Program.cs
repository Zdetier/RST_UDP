using PcarsUDP;
using System;
using System.Net;
using System.Net.Sockets;


namespace UDP_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient listener = new UdpClient(5606);                       //Create a UDPClient object
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 5606);       //Start recieving data from any IP listening on port 5606 (port for PCARS2)

            
            PCars2_UDP uDP = new PCars2_UDP(listener, groupEP);             //Create an UDP object that will retrieve telemetry values from in game.

            while (true)
            {
                uDP.readPackets();                      //Read Packets ever loop iteration
                Console.WriteLine(uDP.Speed);           //Write to console what our current speed is.

                //For Wheel Arrays 0 = Front Left, 1 = Front Right, 2 = Rear Left, 3 = Rear Right.
            }

            
        }
    }
}
