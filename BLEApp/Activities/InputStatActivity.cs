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
using Android.Views.InputMethods;
using BLEApp.Models;
using SQLite;
namespace BLEApp.Activities
{
    [Activity(Label = "¬вод показаний")]
    public class InputStatActivity : Activity
    {
        EditText editText;
        Meter obj;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.meterStats);
            // Create your application here 
            editText = FindViewById<EditText> (Resource.Id.datePick);
            editText.Text = DateTime.Now.ToLongDateString();

            var controllerText = FindViewById<EditText>(Resource.Id.textView8);
            controllerText.Text = StaticParams.ControllerName;

            int meter_id = Intent.GetIntExtra("meter", -1);
            if (meter_id > 0)
            {
                using (var db = new SQLiteConnection(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.DbName))
                {
                    var meterResult = db.Query<Meter>("SELECT * FROM Meter WHERE meter_id=?", meter_id);
                    var beaconResult = db.Query<Beacon>("SELECT * FROM Beacon WHERE beacon_id=?", meterResult[0].beacon_id);
                    var buildingResult = db.Query<Building>("SELECT * FROM Building WHERE building_id=?", meterResult[0].building_id);
                    obj = meterResult[0];
                    obj.beacon = beaconResult[0];
                    obj.building = buildingResult[0];
                };
                FindViewById<TextView>(Resource.Id.Text1).Text = obj.meter_name;
                FindViewById<TextView>(Resource.Id.Text2).Text = obj.meter_description;
                FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resources.GetIdentifier(obj.meter_photo.Split('.')[0], "drawable", "BLEApp.BLEApp"));
            }
            //    
            editText.Click += (sender, e) => {
                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(this, OnDateSet, today.Year, today.Month - 1, today.Day);
                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();
            };

            var buttonSave = FindViewById<Button>(Resource.Id.button1);

            buttonSave.Click += (sender, e) =>
            {
                if (meter_id > 0)
                {
                    using (var db = new SQLiteConnection(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.DbName))
                    {
                        Data data = new Data();
                        data.controller_name = FindViewById<TextView>(Resource.Id.textView7).Text;
                        var EndDate = DateTime.Parse(editText.Text);

                        data.date_published = EndDate;
                        data.meter_data = int.Parse(FindViewById<TextView>(Resource.Id.textView6).Text);
                        data.meter_id = meter_id;
                        db.Insert(data);
                    }
                }
            
                Finish();
            };

        }
        void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            editText.Text = e.Date.ToLongDateString();
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            var currentFocus = this.CurrentFocus;
            if (currentFocus != null)
            {
                inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
            }
        }
    }
}