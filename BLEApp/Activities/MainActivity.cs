using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using BLEApp.Models;
using Android.Content;
using BLEApp.Activities;
namespace BLEApp
{
    [Activity(Label = "BLEApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            InitBuildingModel model = new InitBuildingModel();
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            Button buildingsButton = FindViewById<Button>(Resource.Id.buildingsButton);
            buildingsButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(BuildingsActivity));
                StartActivity(intent);

            };

            Button buttonStatistics = FindViewById<Button>(Resource.Id.buttonStatistics);
            buttonStatistics.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(Activity1));
                StartActivity(intent);
            };

            Button BeaconsButton = FindViewById<Button>(Resource.Id.BeaconsButton);
            BeaconsButton.Click += (sender, e) =>
            {
                System.Console.WriteLine("Beacons!!!");
                var intent = new Intent(this, typeof(BeaconsActivity));
                StartActivity(intent);
            };
        }
    }
}

