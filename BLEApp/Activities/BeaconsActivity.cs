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
using RadiusNetworks.IBeaconAndroid;
using Color = Android.Graphics.Color;
using BLEApp.Models;
using BLEApp.Adapters;
using Newtonsoft.Json;

using SQLite;
//using Android.Support.V4.App;

namespace BLEApp.Activities
{
    [Activity(Label = "@string/Beacons")]
    public class BeaconsActivity : Activity, IBeaconConsumer
    {
        private const string UUID = "e2c56db5-dffb-48d2-b060-d0f5a71096e0";
        private const string monkeyId = "Monkey";

        bool _paused=true;
        ListView listView;
        IBeaconManager _iBeaconManager;
        MonitorNotifier _monitorNotifier;
        RangeNotifier _rangeNotifier;
        Region _monitoringRegion;
        Region _rangingRegion;

        List<Meter> meterList;
        List<IBeacon> BeaconsNearbyList;
        List<bool> BeaconsNearbyFlagList;
        MetersAdapter itemsAdapter;

        int _previousProximity;

        public BeaconsActivity() {
            _iBeaconManager = IBeaconManager.GetInstanceForApplication(this);

            _monitorNotifier = new MonitorNotifier();
            _rangeNotifier = new RangeNotifier();
            meterList = new List<Meter>();
            BeaconsNearbyList = new List<IBeacon>();
            BeaconsNearbyFlagList = new List<bool>();
            _monitoringRegion = new Region(monkeyId, UUID, null, null);
            _rangingRegion = new Region(monkeyId, UUID, null, null);
            

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            // SetContentView(Resource.Layout.Main);

            SetContentView(Resource.Layout.layout1);
            listView = FindViewById<ListView>(Resource.Id.List); // get reference to the ListView in the layout
                                                                 // populate the listview with data

            itemsAdapter = new MetersAdapter(this, meterList);
            listView.Adapter = itemsAdapter;
            listView.ItemClick += OnListItemClick;  // to be defined

            _iBeaconManager.Bind(this);

            _monitorNotifier.EnterRegionComplete += EnteredRegion;
            _monitorNotifier.ExitRegionComplete += ExitedRegion;

            _rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;
            ShowNotification();
        }
        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = meterList[e.Position];
            var intent = new Intent(this, typeof(MeterActivity));
            intent.PutExtra("meter", t.meter_id);
            StartActivity(intent);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            _monitorNotifier.EnterRegionComplete -= EnteredRegion;
            _monitorNotifier.ExitRegionComplete -= ExitedRegion;

            _rangeNotifier.DidRangeBeaconsInRegionComplete -= RangingBeaconsInRegion;

            _iBeaconManager.StopMonitoringBeaconsInRegion(_monitoringRegion);
            _iBeaconManager.StopRangingBeaconsInRegion(_rangingRegion);
            _iBeaconManager.UnBind(this);
        }
        #region IBeaconConsumer impl
        public void OnIBeaconServiceConnect()
        {
            _iBeaconManager.SetMonitorNotifier(_monitorNotifier);
            _iBeaconManager.SetRangeNotifier(_rangeNotifier);

            _iBeaconManager.StartMonitoringBeaconsInRegion(_monitoringRegion);
            _iBeaconManager.StartRangingBeaconsInRegion(_rangingRegion);
        }
        #endregion

        protected override void OnResume()
        {
            base.OnResume();
            _paused = false;
        }

        protected override void OnPause()
        {
            base.OnPause();
            _paused = true;
        }
        void EnteredRegion(object sender, MonitorEventArgs e)
        {
            if (_paused)
            {
                ShowNotification();
            }
        }

        void ExitedRegion(object sender, MonitorEventArgs e)
        {
        }
        void RangingBeaconsInRegion(object sender, RangeEventArgs e)
        {
            
            Console.WriteLine("Founded beacon:" + e.Beacons.Count);

            if (e.Beacons.Count > 0)
            {
                foreach (var item in e.Beacons)
                {
                    if (!checkBeacon(item))
                    {
                        BeaconsNearbyList.Add(item);
                        BeaconsNearbyFlagList.Add(true);
                        var beacon = item;
                        List<Meter> result = new List<Meter>();
                        List<Beacon> Beacons;
                        Console.WriteLine("Founded beacon:" + beacon.ProximityUuid + " " + +beacon.Major + " " + beacon.Minor);
                        using (var db = new SQLiteConnection(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.DbName))
                        {
                            Beacons = db.Query<Beacon>("SELECT * FROM Beacon WHERE uuid=? AND major =? AND minor=?", beacon.ProximityUuid, beacon.Major, beacon.Minor);
                            if (Beacons.Count > 0) { 
                            var tempResult = db.Query<Meter>("SELECT * FROM Meter  WHERE Meter.beacon_id=?", Beacons[0].beacon_id);
                            result.AddRange(tempResult.ToList());
                                    }
                        }

                        if (result.Count > 0)
                        {
                            for (int i = 0; i < result.Count; i++)
                                result[i].beacon = Beacons[0];
                            ShowNotification("—четчик неподалеку");
                            AddMeters(result);
                        }
                    }
                }
            }
        }

 

        private void AddMeters(List<Meter> items)
        {
            meterList.AddRange(items);
            RunOnUiThread(() =>
            {               
                itemsAdapter.NotifyDataSetChanged();

            });
        }
        private void UpdateDisplay(string message, Color color)
        {
            RunOnUiThread(() =>
            {
            //    items.Add(message);
            //itemsAdapter.Clear();
            //foreach (var item in items)
            //{
            //    itemsAdapter.Insert(item, itemsAdapter.Count);
            //}
            //itemsAdapter.NotifyDataSetChanged();
                   
            });
           
        }
        private void ShowNotification(string Text="Empty")
        {
            var notificationId = 0;

            var builder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.ble)
                .SetContentTitle(this.GetText(Resource.String.ApplicationName))
                .SetContentText(Text);

            var notification = builder.Build();

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(notificationId, notification);
        }
        private bool checkBeacon(IBeacon input)
        {
            bool result = false;
            foreach (var item in BeaconsNearbyList)
            {
                if (input.ProximityUuid == item.ProximityUuid && input.Major == item.Major && input.Minor == item.Minor)
                    result= true;
               
            }
            return result;
        }


    }
}