using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;

namespace PetriNetXam
{
    public class EditTextAdapter : BaseAdapter
    {
        private readonly int _cols = SharedObjects.MyPetriNet.NumberOfPlaces;
        private readonly int _gridCols;
        private readonly int _gridRows;
        private readonly int _rows = SharedObjects.MyPetriNet.NumberOfTransitions;
        private readonly Context context;


        private int[] exampleDin =
        {
            11, 12, 13, 14,
            21, 22, 23, 24,
            31, 32, 33, 34
        };

        private int k;

        public EditTextAdapter(Context c)
        {
            context = c;
            _gridCols = _cols + 1; //+1 bo dodatkowe wiersz i kolumna na etykiety
            _gridRows = _rows + 1;
        }

        public override int Count
        {
            //tzra 2darray zamienic na ciag i podac Count do metod abstarkcyjnych
            get { return _gridRows*_gridCols; }
        }

        public override Object GetItem(int position)
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
            if (position == 2) editText.SetBackgroundColor(new Color(Resource.Color.gridLabels));

            var lstGridView = new List<string>();
            lstGridView.Add("");
            for (var i = 0; i < _cols; i++)
            {
                lstGridView.Add("Pl" + (i + 1)); //pierwszy wiersz=etykiety kolumn
            }


            //teraz trza pododowadac te wtykeity
            var j = 1;
            foreach (var lst in SharedObjects.MyPetriNet.DinMatrix)
            {
                lstGridView.Add("Tr" + j);
                var lst2 = lst.ConvertAll(s => s.ToString());
                lstGridView.AddRange(lst2);
                j++;
            }

            var str = lstGridView[position];
            editText.Text = str;
            //Toast.MakeText(context, "updated"+k, ToastLength.Short).Show();
            k++;

            //editText.AfterTextChanged += (sender, args) =>
            //{
            //    Toast.MakeText(context, "AfterTextChanged"+k, ToastLength.Short).Show();
            //    k++;
            //};

            editText.Click += (sender, args) =>
            {
                //Toast.MakeText(context, "click"+k, ToastLength.Short).Show();
                Toast.MakeText(context, position.ToString(), ToastLength.Short).Show();
                k++;
            };

            editText.KeyPress += (sender, e) =>
            {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    Toast.MakeText(context, editText.Text, ToastLength.Short).Show();
                    e.Handled = true;
                    //Dismiss Keybaord
                    SharedObjects.MyPetriNet.DinMatrix[0][0] = 10;
                }
            };

            editText.EditorAction += (sender, e) =>
            {
                e.Handled = false;
                if (e.ActionId == ImeAction.Done)
                {
                    e.Handled = true;
                    var row = position/_gridCols; //-1 bo pomijam etykiety w gridview
                    var col = position%_gridCols;
                    var pos = position;
                    var str2 = "pos" + pos + " row" + row + " col" + col;
                    Toast.MakeText(context, str2, ToastLength.Short).Show();
                    SharedObjects.MyPetriNet.DinMatrix[row - 1][col - 1] = int.Parse(editText.Text);
                }
            };

            return view;

            //String str = thumbIds[position];
            //editText.Text = str;
            //return editText;
        }
    }
}