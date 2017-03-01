using System;
using System.Collections.Generic;
using SQLite;

namespace BLEApp.Models
{

    public class Building {
        [PrimaryKey, AutoIncrement]
        public int building_id { get; set; }
        public string building_address { get; set; }
        public double building_latitude { get; set; }
        public double building_longitude { get; set; }
        public int building_levels { get; set; }

    }
    public class Beacon
    {
        [PrimaryKey, AutoIncrement]
        public int beacon_id { get; set; }
        public string uuid { get; set; }
        public int major { get; set; }
        public int minor { get; set; }

    }
    public class Meter {
        [PrimaryKey, AutoIncrement]
        public int meter_id { get; set; }
        public string meter_name { get; set; }   
        public string meter_description { get; set; }
        public string meter_type { get; set; }
        public string meter_photo { get; set; }
        public int meter_level { get; set; }
        public double meter_building_hall { get; set; }
        public double meter_building_flat { get; set; }
        public int building_id { get; set; }
        public int beacon_id { get; set; }
        [Ignore]
        public Building building { get; set; }
        [Ignore]
        public Beacon beacon { get; set; }
    }
    public class Data {
        [PrimaryKey, AutoIncrement]
        public int data_id { get; set; }
        public DateTime date_published { get; set; }
        public int meter_data { get; set; }
        public int meter_id { get; set; }
        [Ignore]
        public Meter meter { get; set; }   
           
        public string controller_name { get; set; }
    }

    //public class HistoryBuilding {
    //    [PrimaryKey, AutoIncrement]
    //    public int history_id { get; set; }
    //    [Ignore]
    //    public Building building { get; set; }
    //    public int building_id { get; set; }
    //    [Ignore]
    //    public List<Data> building_history { get; set; }
    //}
}