using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
namespace MobileApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        static readonly uint keyHash32 = 0xe033994c; //cihansol.com
        static uint[] polynomialTbl = new uint[256];

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            InitCrc32();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //Initializing button and input from layout
            AppCompatButton checkBtn = FindViewById<AndroidX.AppCompat.Widget.AppCompatButton>(Resource.Id.check);
            AppCompatEditText ppInput = FindViewById<AndroidX.AppCompat.Widget.AppCompatEditText>(Resource.Id.input_passphrase);

            checkBtn.Click += (object sender, EventArgs e) => {
                CheckPassphrase(ppInput.Text);
            };



        }

        private void InitCrc32()
        {
            //Build the polynomial Table required by this demo
            for (int i = 0; i < polynomialTbl.Length; i++)
            {
                var tableEntry = (uint)i;
                for (var j = 0; j < 8; ++j)
                {
                    tableEntry = ((tableEntry & 1) != 0)
                        ? (0xEDB88320 ^ (tableEntry >> 1))
                        : (tableEntry >> 1);
                }
                polynomialTbl[i] = tableEntry;
            }
        }

        private uint HashStr(string str)
        {
            byte[] strData = System.Text.Encoding.ASCII.GetBytes(str);
            uint result = uint.MaxValue;
            for (int i = 0; i < strData.Length; i++)
            {
                result = (result >> 8) ^ polynomialTbl[(result ^ strData[i]) & 0xFF];
            }
            return ~result;
        }

        private bool CheckInputPP(string ppA)
        {
            return HashStr(ppA) == keyHash32;
        }

        private void CheckPassphrase(string inputDataStr)
        {
            if (CheckInputPP(inputDataStr))
                Android.Widget.Toast.MakeText(this, "Correct Passphrase Entered!", Android.Widget.ToastLength.Short).Show();
            else
                Android.Widget.Toast.MakeText(this, "Incorrect Passphrase Entered!", Android.Widget.ToastLength.Short).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
