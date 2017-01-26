using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace PetriNetXam
{
    [Activity(Label = "PetriNetXam", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private EditText etMbeginVector;
        private EditText etMCurrentVector;
        //private Keycode[] numbers ={Keycode.Num0, Keycode.Num1, Keycode.Num2, Keycode.Num3, Keycode.Num4, Keycode.Num5,Keycode.Num6, Keycode.Num7, Keycode.Num8, Keycode.Num9};
        //private String[] numbers = {"0","1","2","3","4","5","6","7","8","9"};
        private EditText etNumberOfPlaces;
        private EditText etNumberOfTransitions;

        protected override void OnCreate(Bundle bundle)
        {
            //NotificationManager.se
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            etNumberOfPlaces = FindViewById<EditText>(Resource.Id.etNumberOfPlaces);
            etNumberOfTransitions = FindViewById<EditText>(Resource.Id.etNumberOfTransitions);
            etMbeginVector = FindViewById<EditText>(Resource.Id.etMbeginVector);
            var btnSetDin = FindViewById<Button>(Resource.Id.btnSetDin);
            var btnSetDout = FindViewById<Button>(Resource.Id.btnSetDout);
            var btnShowNetProperties = FindViewById<Button>(Resource.Id.btnShowIncidenceMatrix);
            var btnSimulation = FindViewById<Button>(Resource.Id.btnSimulation);
            etMCurrentVector = FindViewById<EditText>(Resource.Id.etMCurrentVector);
            var btnHelp = FindViewById<Button>(Resource.Id.btnHelp);

            etNumberOfPlaces.SetImeActionLabel("Dalej", ImeAction.Done);
            etNumberOfTransitions.SetImeActionLabel("Dalej", ImeAction.Done);
            etMbeginVector.SetImeActionLabel("Dalej", ImeAction.Done);
            etMCurrentVector.SetImeActionLabel("Dalej", ImeAction.Done);


            SharedObjects.MyPetriNet = new PetriNet();
            SharedObjects.MyPetriNet.FIllWithExampleData();

            //#######################   przykladowe dane na poczatek  ########## 
            UpdateViewControls();
            //#############################################################


//            List<string> lst = etMbeginVector.Text.Split(' ').ToList();
            //          SharedObjects.MyPetriNet.Mbegin = lst.Select(s => int.Parse(s)).ToList();

            #region metody do zdarzen

            btnSetDin.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(DinMatrixActivity));
                Intent.PutExtra("Matrix", "Din");
                //intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
                SharedObjects.MyPetriNet.CurrentMatrix = "Din";
                StartActivity(intent);
            };

            btnSetDout.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(DinMatrixActivity));
                Intent.PutExtra("Matrix", "Dout");
                SharedObjects.MyPetriNet.CurrentMatrix = "Dout";
                StartActivity(intent);
            };

            btnShowNetProperties.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(DinMatrixActivity));
                Intent.PutExtra("Matrix", "Dincidence");
                SharedObjects.MyPetriNet.CurrentMatrix = "Dincidence";
                StartActivity(intent);
            };

            etMbeginVector.KeyPress += (sender, e) =>
            {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    Toast.MakeText(this, etMbeginVector.Text, ToastLength.Short).Show();
                    e.Handled = true;
                    //Dismiss Keybaord
                    var imm = (InputMethodManager) GetSystemService(InputMethodService);
                    imm.HideSoftInputFromWindow(etMbeginVector.WindowToken, 0);
                }
            };

            #endregion
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Toast.MakeText(this, "Hebebebe..", ToastLength.Short).Show();
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.mymenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.new_game:
                    //do something
                    var intent = new Intent(this, typeof(Splash));
                    StartActivity(intent);
                    return true;
                case Resource.Id.help:
                    //do something
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void UpdateViewControls()
        {
            etNumberOfPlaces.Text = SharedObjects.MyPetriNet.NumberOfPlaces.ToString();
            etNumberOfTransitions.Text = SharedObjects.MyPetriNet.NumberOfTransitions.ToString();
            etMbeginVector.Text = string.Join(" ", SharedObjects.MyPetriNet.Mbegin);
            etMCurrentVector.Text = string.Join(" ", SharedObjects.MyPetriNet.Mcurrent);
        }
    }
}