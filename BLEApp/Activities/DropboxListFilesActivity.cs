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
using System.Net;
using System;
using System.IO;
using System.Threading.Tasks;

using Dropbox.CoreApi.Android;
using Java.Lang;
using Dropbox.CoreApi.Android.Session;
using BLEApp.Adapters;
using BLEApp.Models;

namespace BLEApp.Activities
{
    


    [Activity(Label = "Файлы на сервере")]
    public class DropboxListFilesActivity : Activity
    {

        string AppKey = "d430wur5k08lexr";
        string AppSecret = "y13hpl1s056mn0z";
        DropboxApi dropboxApi;
        List<string> FilesList;
        DropboxListAdapter lAdapter;
        Android.Widget.ListView listView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout1);
            listView = FindViewById<Android.Widget.ListView>(Resource.Id.List);

            FilesList = new List<string>();
            lAdapter = new DropboxListAdapter(this, FilesList);
            
            listView.Adapter = lAdapter;
            listView.ItemClick += OnListItemClick;

           
            // Create your application here
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var t = FilesList[e.Position];
             Android.Widget.Toast.MakeText(this, "Загружаю "+t, Android.Widget.ToastLength.Short).Show();
            getFile(t);


    }
        private void getFile(string filename)
        {
            Task.Run(() =>
            {
                using (var output = File.OpenWrite(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.DbName))
                {
                    try
                    {
                        // Gets the file from Dropbox and saves it to the local folder
                        dropboxApi.GetFile(filename, null, output, null);
                        RunOnUiThread(() => { Android.Widget.Toast.MakeText(this, "Загрузка завершена", Android.Widget.ToastLength.Short).Show(); });
                    }
                    catch (System.Exception e)
                    {

                        RunOnUiThread(() => { Android.Widget.Toast.MakeText(this, "Ошибка во время загрузки", Android.Widget.ToastLength.Short).Show(); });
                    }

                }
            });

        }

        protected override void OnStart()
        {
            base.OnStart();
            AppKeyPair appKeys = new AppKeyPair(AppKey, AppSecret);
            AndroidAuthSession session = new AndroidAuthSession(appKeys);
            if (File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + "dropboxKey.txt"))
                session.OAuth2AccessToken = File.ReadAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + "dropboxKey.txt");
            dropboxApi = new DropboxApi(session);

            if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + "dropboxKey.txt"))
            {

                (dropboxApi.Session as AndroidAuthSession).StartOAuth2Authentication(this);
            }

            getList();
        }

        protected override void OnResume()
        {
            base.OnResume();

            // After you allowed to link the app with Dropbox,
            // you need to finish the Authentication process
            var session = dropboxApi.Session as AndroidAuthSession;
            if (!session.AuthenticationSuccessful())
                return;

            try
            {
                // Call this method to finish the authentication process
                // Will bind the user's access token to the session.
                session.FinishAuthentication();

                // Save the Access Token somewhere
                var accessToken = session.OAuth2AccessToken;
                File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + "dropboxKey.txt", accessToken);
            }
            catch (IllegalStateException ex)
            {
                Toast.MakeText(this, ex.LocalizedMessage, ToastLength.Short).Show();
            }
        }

        private void getList()
        {
            Task.Run(() =>
            {
                try
                {
                    var metadata = dropboxApi.Metadata("/", 50, null, true, null);
                    var fileNames = new List<string>();
                    var filePaths = new List<string>();

                    // Gets all images in the specified folder
                    foreach (DropboxApi.Entry entry in metadata.Contents)
                    {
                        if (entry.IsDir ||
                            (!entry.FileName().EndsWith(".db", StringComparison.InvariantCultureIgnoreCase) &&
                            !entry.FileName().EndsWith(".db", StringComparison.InvariantCultureIgnoreCase)))
                            continue;

                        fileNames.Add(entry.FileName());
                        filePaths.Add(entry.Path);
                    }
                    foreach (var item in fileNames)
                    {

                        FilesList.Add(item);
                    }


                    RunOnUiThread(() =>
                    {

                        lAdapter.NotifyDataSetChanged();

                    });
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            });
        }
    }
}