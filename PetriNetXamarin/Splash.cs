using Android.App;
using Android.OS;
using System.Threading;


/// <summary>
/// touch -> close i wlacz mainacitibity
/// albo timer i wlacz main activity
/// </summary>

namespace PetriNetXam
{
    [Activity(Label = "PetriNetXam", MainLauncher = false, Icon = "@drawable/icon")]
    public class Splash : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.SplashScreen);
        }
    }
}