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
    [Activity(Label = "Help", MainLauncher = false, Icon = "@drawable/icon")]
    public class HelpActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.HelpScreen);

            var lay = FindViewById<LinearLayout>(Resource.Id.helpMainLayout);

            lay.Click += (sender, e) =>
            {
                //close activity
                Finish();
            };
        }
    }
}