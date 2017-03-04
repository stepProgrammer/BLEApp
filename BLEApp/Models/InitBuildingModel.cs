using System;
using System.Collections.Generic;
using SQLite;
using System.IO;



namespace BLEApp.Models
{
    class InitBuildingModel
    {
        public InitBuildingModel()
        {
               
                Random r = new Random();
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.DbName))
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.DbName);
                    using (var db = new SQLiteConnection(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/"+ StaticParams.DbName))
                    {
                    // Get customer collection
                    db.CreateTable<Building>(CreateFlags.AutoIncPK);
                    db.CreateTable<Meter>(CreateFlags.AutoIncPK);
                    db.CreateTable<Data>(CreateFlags.AutoIncPK);
                    db.CreateTable<Beacon>(CreateFlags.AutoIncPK);
                    //  db.CreateTable<HistoryBuilding>(CreateFlags.AutoIncPK);

                    //HistoryBuilding history = new HistoryBuilding();
                    //    history.building_history = new List<Data>();

                           
                        for (int BuildingIndex = 1; BuildingIndex < 3; BuildingIndex++)
                        {
                        int meter_number = 0;
                        Building build = new Building();
                            build.building_address = "Москва, ул. Ленина д. " + BuildingIndex;
                            build.building_latitude = 53.63575100 + BuildingIndex;
                            build.building_longitude = 35.66265100;
                        build.building_levels = 5;

                               db.Insert(build);
        
                                for (int BuildingHall = 1; BuildingHall < 4; BuildingHall++)
                                {
                                    for (int BuildingLevel = 1; BuildingLevel < 6; BuildingLevel++)
                                    {
                                        for (int FlatNumber = BuildingLevel * 4 - 4+1; FlatNumber < BuildingLevel + 5; FlatNumber++)
                                        {
                                            Beacon beacon = new Beacon();
                                            beacon.uuid = "e2c56db5-dffb-48d2-b060-d0f5a71096e0";
                                    if (BuildingIndex == 1)
                                        beacon.major = 0;
                                    else
                                        beacon.major = 4;
                                    beacon.minor = meter_number;

                                            db.Insert(beacon);

                                    Meter meter = new Meter();
                                            meter.meter_building_hall = BuildingHall;
                                            meter.meter_level = BuildingLevel;
                                            meter.meter_description = "Электрический счетчик на " + build.building_address
                                                + " в подъезде " + BuildingHall + " на этаже " + BuildingLevel
                                                + " квартиры номер " + FlatNumber;

                                            meter.meter_name = "Счётчик №"+r.Next(0,1000);

                                            meter.meter_type = "Электрический счетчик";
                                            meter.meter_photo = "meter_"+(meter_number)+".jpg"; 
                                            meter.meter_building_flat = FlatNumber;
                                            meter.building_id = build.building_id;
                                            meter.beacon_id = beacon.beacon_id;

                                    meter_number = meter_number + 1;
                                   db.Insert(meter);

                                       //     for (int DayNumber = 1; DayNumber < 11; DayNumber++)
                                       //     {
                                       //         Data data = new Data();
                                       //         data.controller_name = "Тестова Контролерович";
                                       //         var EndDate = DateTime.Now.AddDays(10 - DayNumber);

                                       //         data.date_published = EndDate;
                                       //         data.meter_data = r.Next(0,1000);
                                       //         data.meter_id = meter.meter_id;
                                       //         db.Insert(data);

                                       //// history.building_history.Add(data);

                                                

                                       //     }
                                        }
                                    }
                            }                 
                        //db.Insert(history);
                        }
                    }
            
        }
    }
}