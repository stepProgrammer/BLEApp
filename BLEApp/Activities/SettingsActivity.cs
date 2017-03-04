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
using BLEApp.Models;

using Dropbox.CoreApi.Android.Session;

namespace BLEApp.Activities
{
    [Activity(Label = "Настройки")]
    public class SettingsActivity : Activity
    {
        string AppKey = "d430wur5k08lexr";
        string AppSecret = "y13hpl1s056mn0z";
        string controllerName = "";
        DropboxApi dropboxApi;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SettingsLayout);


            // Create your application here
            Android.Widget.Button SendDb = FindViewById<Android.Widget.Button>(Resource.Id.ButtonPush);

            SendDb.Click += SendDb_Click;
            Android.Widget.Button GetDb = FindViewById<Android.Widget.Button>(Resource.Id.ButtonGet);

            GetDb.Click += GetDb_Click;

            var InitDb = FindViewById<Button>(Resource.Id.ButtonCreate);
            InitDb.Click += InitDb_Click;

            var EditText = FindViewById<EditText>(Resource.Id.editControllerText);
            EditText.Text = StaticParams.ControllerName;
            controllerName = EditText.Text;
            EditText.TextChanged += EditText_TextChanged;
        }

        protected override void OnDestroy()
        {
            try
            {
                File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.ControllerNameFile, StaticParams.ControllerName);
            }
            catch (Java.Lang.Exception ex)
            {
                Console.WriteLine(    ex.ToString()); }
            base.OnDestroy();
        }
        private void EditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            StaticParams.ControllerName = Convert.ToString(e.Text);
            
        }

        private void InitDb_Click(object sender, EventArgs e)
        {
            InitBuildingModel model = new InitBuildingModel();
        }

        private void GetDb_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(DropboxListFilesActivity));
            StartActivity(intent);
        }

        private void SendDb_Click(object sender, EventArgs e)
        {

          
          Task.Run(() => {

            FileStream f = new FileStream(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/" + StaticParams.DbName, FileMode.Open, FileAccess.Read);
              try
              {
                 dropboxApi.PutFile(DateTime.Now.Date.ToShortDateString() + ".db", f, f.Length, null, null);
                  RunOnUiThread(() => { Android.Widget.Toast.MakeText(this, "Данные отправлены успешно", Android.Widget.ToastLength.Short).Show(); }); 
              }
              catch (Java.Lang.Exception ex)
              {
                  RunOnUiThread(() => { Android.Widget.Toast.MakeText(this, "Ошибка при отправке данных", Android.Widget.ToastLength.Short).Show(); });
              }
            f.Close();
    });
            
           
        }

        private void ShowNotification(string v)
        {
            var notificationId = 1;

            var builder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.ble)
                .SetContentTitle(this.GetText(Resource.String.ApplicationName))
                .SetContentText(v);

            var notification = builder.Build();

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(notificationId, notification);
        }

        protected  override void OnStart()
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
        }

        protected  override void OnResume()
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
                File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/"+"dropboxKey.txt",accessToken);
            }
            catch (IllegalStateException ex)
            {
                Toast.MakeText(this, ex.LocalizedMessage, ToastLength.Short).Show();
            }
        }


    }
}