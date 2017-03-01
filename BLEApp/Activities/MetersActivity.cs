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
using SQLite;
using BLEApp.Models;
using BLEApp.Adapters;
using Newtonsoft.Json;

namespace BLEApp.Activities
{
    [Activity(Label = "MetersActivity")]
    public class MetersActivity : Activity
    {
        ListView listView;
        List<Meter> meterList;
        List<List<Meter>> meterGroupedList;
        List<Beacon> beaconList;
        List<Building> buildingList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            int building_id = Intent.GetIntExtra("building_id",-1);

            using (var db = new SQLiteConnection(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/"+ StaticParams.DbName))
            {
                int building_levels = 0;
                //var res = db.Table<Meter>();
                //building_levels=res.Where(t => t.building_id == int.Parse(building_id)).Max(x => x.meter_level);
                //for (int i = 1; i < building_levels; i++)
                //{
                //    if (building_id == "")
                //        meterList = db.Query<Meter>("SELECT * FROM Meter WHERE  meter_level=?", i);
                //    else
                //        meterList = db.Query<Meter>("SELECT * FROM Meter WHERE building_id=? AND meter_level=?", building_id,i);
                //    meterGroupedList.Add(meterList);
                //}
                if (building_id == -1)
                    meterList = db.Query<Meter>("SELECT * FROM Meter ");
                else
                    meterList = db.Query<Meter>("SELECT * FROM Meter WHERE building_id=? ", building_id);

                SetContentView(Resource.Layout.layout1);
                listView = FindViewById<ListView>(Resource.Id.List); // get reference to the ListView in the layout
                                                                     // populate the listview with data
                                                                     
                listView.Adapter = new MetersAdapter(this, meterList);
                listView.ItemClick += OnListItemClick;  // to be defined
            }
            // Create your application here
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = meterList[e.Position];
            var intent = new Intent(this, typeof(MeterActivity));
            intent.PutExtra("meter",t.meter_id);
            StartActivity(intent);
        }
    }
}