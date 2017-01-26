using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.InputMethodServices;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using PetriNetXam;


namespace PetriNetXamarin
{
    public class EditTextAdapter : BaseAdapter
    {
        private int _cols ;
        private int _rows ;
        private int _gridCols;
        private int _gridRows;
        private String etPrevText = "0";
        private Context context;

        public EditTextAdapter(Context c)
        {
            context = c;
            _cols = SharedObjects.MyPetriNet.NumberOfPlaces;
            _rows = SharedObjects.MyPetriNet.NumberOfTransitions;

        _gridCols = _cols + 1; //+1 bo dodatkowe wiersz i kolumna na etykiety
            _gridRows = _rows + 1;
        }

        public override int Count
        {
            //tzra 2darray zamienic na ciag i podac Count do metod abstarkcyjnych
            get { return _gridRows*_gridCols; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        // create a new ImageView for each item referenced by the Adapter
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //EditText editText;
            View view;
            EditText editText;

            if (convertView == null)
            {
                // if it's not recycled, initialize some attributes
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.GridItem, parent, false);
            }
            else
            {
                //editText = (EditText)convertView;
                view = convertView;
            }

            editText = view.FindViewById<EditText>(Resource.Id.EditTextTile);
            editText.SetPadding(5, 5, 5, 5);
            editText.Gravity = GravityFlags.Center;
            editText.SetLines(1);
            editText.SetMaxLines(1);
            editText.SetRawInputType(InputTypes.ClassNumber);
            editText.SetImeActionLabel("Donee", ImeAction.Done);

            //konwerjsa na macierzy 2D na liste do gridview
            //dodanie etykeit wierszy kolumn i pokolorawnie ich
            var lstGridView = new List<string>(); 
            lstGridView.Add("  ");//narozny tile, dwie spacje potem tam mam jednego ifa mniej 
            for (var i = 0; i < _cols; i++)
            {
                lstGridView.Add("Pl" + (i + 1)); //pierwszy wiersz=etykiety kolumn
            }

            //pozostale wiersze z danymi + pierwsa kolumna etykeita wiersza
            var j = 1;
            if (SharedObjects.MyPetriNet.CurrentMatrix=="Din")
            {
                foreach (var lst in SharedObjects.MyPetriNet.DinMatrix)
                {
                    lstGridView.Add("Tr" + j);//etykietea wiersza
                    var lst2 = lst.ConvertAll(s => s.ToString());
                    lstGridView.AddRange(lst2);//zawartosc macierzy
                    j++;
                }

            }
            if (SharedObjects.MyPetriNet.CurrentMatrix == "Dout")
            {
                foreach (var lst in SharedObjects.MyPetriNet.DoutMatrix)
                {
                    lstGridView.Add("Tr" + j); //etykietea wiersza
                    var lst2 = lst.ConvertAll(s => s.ToString());
                    lstGridView.AddRange(lst2); //zawartosc macierzy
                    j++;
                }
            }
            if (SharedObjects.MyPetriNet.CurrentMatrix == "Dincidence")
            {
                foreach (var lst in SharedObjects.MyPetriNet.DincidenceMatrix)
                {
                    lstGridView.Add("Tr" + j); //etykietea wiersza
                    var lst2 = lst.ConvertAll(s => s.ToString());
                    lstGridView.AddRange(lst2); //zawartosc macierzy
                    j++;
                }
            }

            //wypelnianie gridview
            var str = lstGridView[position];
            editText.Text = str;

            //kolorowanie etykiet

            if (position<_gridCols || position%_gridCols==0) //-1 bo position idzie od 0
            {
                editText.SetBackgroundColor(new Color(Color.Orange));
                editText.Enabled = false;
            }

            #region zdarzenia
            editText.Click += (sender, args) =>
            {
                // nic nie rob i nie otwieraj klawiatury
                //Toast.MakeText(context, "click"+k, ToastLength.Short).Show();
                //Toast.MakeText(context, position.ToString(), ToastLength.Short).Show();
                //Dismiss Keybaord
            };

            editText.EditorAction += (sender, e) =>
            {
                e.Handled = false;
                if (e.ActionId == ImeAction.Done)
                {
                    if (editText.Text == "") editText.Text = etPrevText;
                    e.Handled = true;
                    var row = position / _gridCols; //-1 bo pomijam etykiety w gridview
                    var col = position % _gridCols;
                    var pos = position;
                    //var str2 = "pos" + pos + " row" + row + " col" + col;
                    //Toast.MakeText(context, str2, ToastLength.Short).Show();
                    if (SharedObjects.MyPetriNet.CurrentMatrix == "Din")
                        SharedObjects.MyPetriNet.DinMatrix[row - 1][col - 1] = int.Parse(editText.Text);
                        SharedObjects.MyPetriNet.MakeIncidenceMatrix();
                    if (SharedObjects.MyPetriNet.CurrentMatrix == "Dout")
                        SharedObjects.MyPetriNet.DoutMatrix[row - 1][col - 1] = int.Parse(editText.Text);
                        SharedObjects.MyPetriNet.MakeIncidenceMatrix();
                    //jak Dincidence to nic nie rob, albo moze przelicz ja
                    if (SharedObjects.MyPetriNet.CurrentMatrix == "Dincidence")
                    {
                        SharedObjects.MyPetriNet.MakeIncidenceMatrix();
                    }
                    //Dismiss Keybaord
                }
            };

            editText.BeforeTextChanged += (sender, e) =>
            {
                etPrevText = editText.Text;
            };

            #endregion

            return view;
        }
    }
}