using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Net;
using Media.Rtsp.Server;
using Media.Rtsp.Server.MediaTypes;
using System.Threading;
using Android.Content;
using System.Threading.Tasks;

namespace TestAndroidServer
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Media.Rtsp.RtspServer server;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            FloatingActionButton fab2 = FindViewById<FloatingActionButton>(Resource.Id.fab2);
            fab2.Click += Fab2OnClick;

            var ip = IPAddress.Any;//IPAddress.Parse("192.168.0.2");
            var port = 3001;

            server = new Media.Rtsp.RtspServer(ip, port)
            {
                Logger = new RtspServerConsoleLogger(),
                ClientSessionLogger = new RtspServerConsoleLogger()
            };

            //Another one to test over http
            //string url = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4";
            string url = "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov";

            RtspSource source = new RtspSource("RtspSourceTest", url);
            server.TryAddMedia(source);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            Task.Run(async () =>
            {
                using (server)
                {
                    server.Start();
                    Console.WriteLine("Listening on: " + server.LocalEndPoint);
                }
            });
        }

        private void Fab2OnClick(object sender, EventArgs eventArgs)
        {
            if (server != null)
            {
                server.Stop();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
