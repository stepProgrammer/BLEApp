using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using BLEApp.Models;
using Android.Content;
using BLEApp.Activities;
namespace BLEApp
{
    public static class StaticParams {
        public static string DbName = "bleapp_beacons_sqlite_v30.db";
        public static string ControllerNameFile = "ControllerName.txt";
        public static string ControllerName { get; set; }
    }
}