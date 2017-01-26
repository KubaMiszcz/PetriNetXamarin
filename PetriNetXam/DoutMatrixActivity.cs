using Android.App;
using Android.OS;
using Android.Widget;

namespace PetriNetXam
{
    [Activity(Label = "DoutMatrixActivity")]
    public class DoutMatrixActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DMatrix);

            var gridview = FindViewById<GridView>(Resource.Id.gridview);
            //var edittext = FindViewById<EditText>(Resource.Id);
            gridview.Adapter = new EditTextAdapter(this);
            // Create your application here
        }
    }
}