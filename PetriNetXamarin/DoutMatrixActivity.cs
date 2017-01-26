using Android.App;
using Android.OS;
using Android.Widget;
using PetriNetXam;

namespace PetriNetXamarin
{
    [Activity(Label = "Macierz Dout")]
    public class DoutMatrixActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.DMatrix);

            var gridview = FindViewById<GridView>(Resource.Id.gridviewDmatrixes);
            gridview.Adapter = new EditTextAdapter(this);
            gridview.NumColumns = SharedObjects.MyPetriNet.NumberOfPlaces + 1; //+1 bo dodatkowa kolumna na etykiety
            Toast.MakeText(this, "Wpisz liczbe strzalek wychodzacych", ToastLength.Short).Show();
        }
        
        //protected override void OnDestroy()
        //{
        //    base.OnDestroy();
        //    Toast.MakeText(this, "Din exit", ToastLength.Short).Show();
        //}
    }
}