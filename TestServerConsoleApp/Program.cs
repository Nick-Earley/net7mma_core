using System;
using System.Net;
using Media.Rtsp.Server;
using Media.Rtsp.Server.MediaTypes;

namespace TestServerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var ip = IPAddress.Any;//IPAddress.Parse("192.168.0.2");
            var port = 3001;

            using (Media.Rtsp.RtspServer server = new Media.Rtsp.RtspServer(ip, port)
            {
                Logger = new RtspServerConsoleLogger()
            })
            {
                string url = "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov";

                RtspSource source = new RtspSource("RtspSourceTest", url);
                server.TryAddMedia(source);
                server.Start();
                Console.WriteLine("Listening on: " + server.LocalEndPoint);

                var key = Console.ReadKey();
                while(key.Key != ConsoleKey.Q)
                {
                    key = Console.ReadKey();
                }
                server.Stop();
            }
        }
    }
}
