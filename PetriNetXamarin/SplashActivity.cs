using Android.App;
using Android.OS;
using System.Threading;
using Android.Content;
using Android.Media;
using Android.Widget;
using PetriNetXam;


/// <summary>
/// touch -> close i wlacz mainacitibity
/// albo timer i wlacz main activity
/// </summary>

namespace PetriNetXamarin
{
    [Activity(Label = "About", MainLauncher = false, Icon = "@drawable/icon")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.SplashScreen);
            var lay = FindViewById<LinearLayout>(Resource.Id.AboutRootLayout);
            var img = FindViewById<ImageView>(Resource.Id.splashImg);

			
            lay.Click += (sender, e) =>
            {
                //close activity
                Finish();
            };

            img.Click += (sender, e) =>
            {
                //close activity
                Finish();
            };
        }
    }
}