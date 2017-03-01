using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLEApp.Models;
using Android.App;
using SQLite;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RadiusNetworks.IBeaconAndroid;

namespace BLEApp.Activities
{
    [Activity(Label = "@string/Buildings")]
    public class BuildingsActivity : ListActivity
    {
        List<Building> buildingsList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            using (var db = new SQLiteConnection(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/"+ StaticParams.DbName))
            {
                buildingsList = db.Query<Building>("SELECT * FROM Building");
                List<string> result = new List<string>();
                foreach (var item in buildingsList)
                    result.Add(item.building_address);

                this.ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, result);
            }
      

            // Create your application here


        }
        protected override void OnListItemClick(ListView l, View v, int position, long id) {
            var t = buildingsList[position];
            var intent = new Intent(this, typeof(MetersActivity));
            intent.PutExtra("building_id", t.building_id);
            StartActivity(intent);
           // Android.Widget.Toast.MakeText(this, t, Android.Widget.ToastLength.Short).Show();
        }
    }
}