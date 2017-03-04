using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BLEApp.Models;
using Newtonsoft.Json;
using SQLite;

namespace BLEApp.Activities
{
    [Activity(Label = "MeterActivity")]
    public class MeterActivity : Activity
    {
        Meter obj;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.meterLayout);
            int text = Intent.GetIntExtra("meter",9);
            using (var db = new SQLiteConnection(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.DbName))
            {
               var meterResult= db.Query<Meter>("SELECT * FROM Meter WHERE meter_id=?",text);
                var beaconResult = db.Query<Beacon>("SELECT * FROM Beacon WHERE beacon_id=?", meterResult[0].beacon_id);
                var buildingResult = db.Query<Building>("SELECT * FROM Building WHERE building_id=?", meterResult[0].building_id);
                obj = meterResult[0];
                obj.beacon = beaconResult[0];
                obj.building = buildingResult[0];
            }            

            FindViewById<ImageView>(Resource.Id.meterImage).SetImageResource(Resources.GetIdentifier(obj.meter_photo.Split('.')[0],"drawable", PackageName));
            FindViewById<EditText>(Resource.Id.editText1).Text =obj.meter_name;
            FindViewById<EditText>(Resource.Id.editText2).Text = obj.meter_type;
            FindViewById<EditText>(Resource.Id.editText3).Text = obj.meter_level.ToString();
            FindViewById<EditText>(Resource.Id.editText4).Text = obj.meter_building_hall.ToString();
            FindViewById<EditText>(Resource.Id.editText5).Text = obj.meter_building_flat.ToString();
            FindViewById<EditText>(Resource.Id.editText6).Text = obj.beacon.uuid;
            FindViewById<EditText>(Resource.Id.editText7).Text = obj.beacon.major.ToString();
            FindViewById<EditText>(Resource.Id.editText8).Text = obj.beacon.minor.ToString();
            //FindViewById<EditText>(Resource.Id.editText9).Text = obj.building.building_address;
            //FindViewById<EditText>(Resource.Id.editText10).Text = obj.building.building_latitude.ToString();
            //FindViewById<EditText>(Resource.Id.editText11).Text = obj.building.building_longitude.ToString();
            

            Button SaveButton = FindViewById<Button>(Resource.Id.button1);
            SaveButton.Click += (sender, e) =>
            {
                SaveChangesToDb();
                Finish();
            };
            Button InputButton = FindViewById<Button>(Resource.Id.button2);
            InputButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(InputStatActivity));
                intent.PutExtra("meter", obj.meter_id);
                StartActivity(intent);
            };
            Button StatButton = FindViewById<Button>(Resource.Id.historyButton);
            StatButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(MeterHistoryActivity));
                intent.PutExtra("meter", obj.meter_id);
                StartActivity(intent);
            };
        }

        protected void SaveChangesToDb()
        {
            obj.meter_name = FindViewById<EditText>(Resource.Id.editText1).Text;
            obj.meter_type = FindViewById<EditText>(Resource.Id.editText2).Text ;
            obj.meter_level = int.Parse(FindViewById<EditText>(Resource.Id.editText3).Text);
            obj.meter_building_hall = int.Parse(FindViewById<EditText>(Resource.Id.editText4).Text);
            obj.meter_building_flat = int.Parse(FindViewById<EditText>(Resource.Id.editText5).Text);
            obj.beacon.uuid =FindViewById<EditText>(Resource.Id.editText6).Text ;
            obj.beacon.major = int.Parse(FindViewById<EditText>(Resource.Id.editText7).Text);
            obj.beacon.minor = int.Parse(FindViewById<EditText>(Resource.Id.editText8).Text);
            //obj.building.building_address = FindViewById<EditText>(Resource.Id.editText9).Text;
            //obj.building.building_latitude = double.Parse(FindViewById<EditText>(Resource.Id.editText10).Text);
            //obj.building.building_longitude = double.Parse(FindViewById<EditText>(Resource.Id.editText11).Text);

            using (var db = new SQLiteConnection(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/"+ StaticParams.DbName))
            {

                //                var ans=meter.Find(x => x.meter_id == obj.meter_id).FirstOrDefault();
                //                if (ans != null)
                //                {
                //   meter.Update(obj.meter_id,obj);
                db.Update(obj.beacon);
                db.Update(obj);

                    Console.WriteLine(obj.meter_description);
//                }               
            }

        }
    }
}