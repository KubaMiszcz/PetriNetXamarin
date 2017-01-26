using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using PetriNetXam;

namespace PetriNetXamarin
{
    [Activity(Label = "Petri Net Xamarin", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        //odwolania sa do sharedobjectmy[etrinet, a nie robie zmiennej do tego na poczatku , bo to moze byc w ronzych watkach 
        //i metody pracowalyby na kopiach a nei instancji
        //latwo to sprawdzic ale nie zdazylem a nie chce tego rozgrzebac bo nie zdaze i nie bedzie dzialac:]
        private EditText etMbeginVector;
        private EditText etMCurrentVector;
        private EditText etMVectorNminus1;
        private EditText etMVectorNminus2;

        //private Keycode[] numbers ={Keycode.Num0, Keycode.Num1, Keycode.Num2, Keycode.Num3, Keycode.Num4, Keycode.Num5,Keycode.Num6, Keycode.Num7, Keycode.Num8, Keycode.Num9};
        //private String[] numbers = {"0","1","2","3","4","5","6","7","8","9"};
        private EditText etNumberOfPlaces;
        private EditText etNumberOfTransitions;
        private Button btnNextStep;

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
            var btnShowDincidence = FindViewById<Button>(Resource.Id.btnShowIncidenceMatrix);
            btnNextStep = FindViewById<Button>(Resource.Id.btnNextStep);
            etMCurrentVector = FindViewById<EditText>(Resource.Id.etMCurrentVector);
            etMVectorNminus1 = FindViewById<EditText>(Resource.Id.etMVectorNminus1);
            etMVectorNminus2 = FindViewById<EditText>(Resource.Id.etMVectorNminus2);
            var btnHelp = FindViewById<Button>(Resource.Id.btnHelp);

            etNumberOfPlaces.SetImeActionLabel("Dalej", ImeAction.Done);
            etNumberOfTransitions.SetImeActionLabel("Dalej", ImeAction.Done);
            etMbeginVector.SetImeActionLabel("Dalej", ImeAction.Done);
            etMCurrentVector.SetImeActionLabel("Dalej", ImeAction.Done);

            SharedObjects.MyPetriNet = new PetriNet();
            SharedObjects.MyPetriNet.FIllWithExampleData();

            //#######################   aktualizacja kontrolek  ########## 
            UpdateViewControls();
            etMVectorNminus1.Text = etMCurrentVector.Text;
            etMVectorNminus2.Text = etMCurrentVector.Text;
            //#############################################################


            #region metody do zdarzen
            etNumberOfPlaces.EditorAction += (sender, e) =>
            {
                e.Handled = false;
                if (e.ActionId == ImeAction.Done)
                {
                    if (etNumberOfPlaces.Text == "") etNumberOfPlaces.Text = SharedObjects.MyPetriNet.NumberOfPlaces.ToString();
                    if (etNumberOfPlaces.Text == "0") etNumberOfPlaces.Text = "1";
                    e.Handled = true;
                    SharedObjects.MyPetriNet.NumberOfPlaces=int.Parse(etNumberOfPlaces.Text);
                    SharedObjects.MyPetriNet.RestartPetriNet();
                    UpdateViewControls();
                    etMVectorNminus1.Text = etMCurrentVector.Text;
                    etMVectorNminus2.Text = etMCurrentVector.Text;
                    //Dismiss Keybaord
                    var imm = (InputMethodManager)GetSystemService(InputMethodService);
                    imm.HideSoftInputFromWindow(etNumberOfPlaces.WindowToken, 0);
                }
            };

            etNumberOfTransitions.EditorAction += (sender, e) =>
            {
                e.Handled = false;
                if (e.ActionId == ImeAction.Done)
                {
                    if (etNumberOfTransitions.Text == "") etNumberOfTransitions.Text = SharedObjects.MyPetriNet.NumberOfTransitions.ToString();
                    if (etNumberOfTransitions.Text == "0") etNumberOfTransitions.Text = "1";
                    e.Handled = true;
                    SharedObjects.MyPetriNet.NumberOfTransitions = int.Parse(etNumberOfTransitions.Text);
                    SharedObjects.MyPetriNet.RestartPetriNet();
                    UpdateViewControls();
                    etMVectorNminus1.Text = etMCurrentVector.Text;
                    etMVectorNminus2.Text = etMCurrentVector.Text;
                    //Dismiss Keybaord
                    var imm = (InputMethodManager)GetSystemService(InputMethodService);
                    imm.HideSoftInputFromWindow(etNumberOfTransitions.WindowToken, 0);
                }
            };


            btnSetDin.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(DinMatrixActivity));
                Intent.PutExtra("Matrix", "Din");
                SharedObjects.MyPetriNet.CurrentMatrix = "Din";
                StartActivity(intent);
            };

            btnSetDout.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(DoutMatrixActivity));
                Intent.PutExtra("Matrix", "Dout");
                SharedObjects.MyPetriNet.CurrentMatrix = "Dout";
                StartActivity(intent);
            };

            btnShowDincidence.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(DincidendceMatrixActivity));
                Intent.PutExtra("Matrix", "Dincidence");
                SharedObjects.MyPetriNet.CurrentMatrix = "Dincidence";
                StartActivity(intent);
            };

            btnNextStep.Click += (sender, e) =>
            {
                Button btn = sender as Button;
                SharedObjects.MyPetriNet.NextStep();
                UpdateViewControls();
            };

            btnHelp.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(SplashActivity));
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

        private void UpdateViewControls()
        {
            etNumberOfPlaces.Text = SharedObjects.MyPetriNet.NumberOfPlaces.ToString();
            etNumberOfTransitions.Text = SharedObjects.MyPetriNet.NumberOfTransitions.ToString();
            etMbeginVector.Text = string.Join(" ", SharedObjects.MyPetriNet.Mbegin);
            etMVectorNminus2.Text = etMVectorNminus1.Text;
            etMVectorNminus1.Text = etMCurrentVector.Text;
            btnNextStep.Text = "Nastepny  krok " + SharedObjects.MyPetriNet.CurrentStep + " >>";
            etMCurrentVector.Text = string.Join(" ", SharedObjects.MyPetriNet.Mcurrent);
        }

        #region MENU TOOLBAR W ACTIONBAR
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.mymenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.help:
                    //do something
                    var intent1 = new Intent(this, typeof(HelpActivity));
                    StartActivity(intent1);
                    return true;
                case Resource.Id.about:
                    //do something
                    var intent2 = new Intent(this, typeof(SplashActivity));
                    StartActivity(intent2);
                    return true;
                case Resource.Id.zeroAll:
                    //do something
                    SharedObjects.MyPetriNet.FIllAllWithZeros();
                    SharedObjects.MyPetriNet.RestartPetriNet();
                    UpdateViewControls();
                    etMVectorNminus1.Text = etMCurrentVector.Text;
                    etMVectorNminus2.Text = etMCurrentVector.Text;
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        #endregion     
    }
}