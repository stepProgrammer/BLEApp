using Android.App;
using Android.Widget;
using Android.OS;

using Android.Content;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




using System.Net;
using System.IO;

using BLEApp.Activities;


namespace BLEApp
{
    [Activity(Label = "BLEApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            if (System.IO.File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.ControllerNameFile))
            {
                StaticParams.ControllerName = File.ReadAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.ControllerNameFile);
            }
            else {
                File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.ControllerNameFile, "Гончарова Ольга");
                StaticParams.ControllerName = "Гончарова Ольга";
            }

                Button buildingsButton = FindViewById<Button>(Resource.Id.buildingsButton);
            buildingsButton.Click += (sender, e) =>
            {
                if (System.IO.File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.DbName))
                {
                    var intent = new Intent(this, typeof(BuildingsActivity));
                    StartActivity(intent);
                }
                else
                    Android.Widget.Toast.MakeText(this, "Сначала загрузите данные", Android.Widget.ToastLength.Short).Show();
            };

            Button buttonSettings = FindViewById<Button>(Resource.Id.buttonStatistics);
            buttonSettings.Click += (sender, e) =>
            {
               
                    var intent = new Intent(this, typeof(SettingsActivity));
                    StartActivity(intent);
                
            };

            Button BeaconsButton = FindViewById<Button>(Resource.Id.BeaconsButton);
            BeaconsButton.Click += (sender, e) =>
            { if (System.IO.File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.DbName))
                {
                    var intent = new Intent(this, typeof(BeaconsActivity));
                    StartActivity(intent);
                }
            else
                    Android.Widget.Toast.MakeText(this, "Сначала загрузите данные", Android.Widget.ToastLength.Short).Show();

            };
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}

