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
using SQLite;
using BLEApp.Adapters;

namespace BLEApp.Activities
{
    [Activity(Label = "Статистика счетчика")]
    public class MeterHistoryActivity : Activity
    {
        ListView listView;
        List<Data> dataList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            int meter_id = Intent.GetIntExtra("meter", 1);
            using (var db = new SQLiteConnection(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.DbName))
            {
                dataList = db.Query<Data>("SELECT * FROM Data WHERE meter_id=? ORDER BY date_published DESC", meter_id);
                SetContentView(Resource.Layout.layout1);
                listView = FindViewById<ListView>(Resource.Id.List);

                listView.Adapter = new MeterHistoryAdapter(this, dataList);
    
            }
            // Create your application here
        }
    }
}