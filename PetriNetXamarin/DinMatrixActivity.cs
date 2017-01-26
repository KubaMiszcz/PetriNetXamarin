using Android.App;
using Android.OS;
using Android.Widget;

namespace PetriNetXam
{
    [Activity(Label = "Din Matrix")]
    public class DinMatrixActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.DMatrix);

            var gridview = FindViewById<GridView>(Resource.Id.gridview);
            gridview.Adapter = new EditTextAdapter(this);
            gridview.NumColumns = SharedObjects.MyPetriNet.NumberOfPlaces + 1; //+1 bo dodatkowa kolumna na etykiety


            ////zdarzenia
            //gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            //    {
            //        Toast.MakeText(this, args.Position.ToString(), ToastLength.Short).Show();
            //        //Toast.MakeText(this, "asdasdsad", ToastLength.Short).Show();
            //        //EditText et = sender as EditText;
            //        //et.SetImeActionLabel("Donee", ImeAction.Done);
            //    };
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            Toast.MakeText(this, "Din exit", ToastLength.Short).Show();
        }
    }
}