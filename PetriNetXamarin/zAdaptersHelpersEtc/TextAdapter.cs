using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace PetriNetXam
{
    public class TextAdapter : BaseAdapter
    {
        private readonly Context context;


        // references to our images
        private readonly string[] thumbIds =
        {
            " A ", " B ", " C ", " D ", " E ", " F ", " G ", " H ", " I ", " J ", " K ", " L ", " M ", " N ", " O ",
            " P ", " Q ",
            " R ", " S ", " T ", " U ", " V ", " W ", " X ", " Y ", " Z "
        };

        public TextAdapter(Context c)
        {
            context = c;
        }

        public override int Count
        {
            get { return thumbIds.Length; }
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
            TextView textView;

            if (convertView == null)
            {
                // if it's not recycled, initialize some attributes
                textView = new TextView(context);
                textView.LayoutParameters = new AbsListView.LayoutParams(85, 85);
                //imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                textView.SetPadding(5, 5, 5, 5);
            }
            else
            {
                textView = (TextView) convertView;
            }

            //imgIcon.Click += ImgIcon_Click
            //imageView.SetImageResource(thumbIds[position]);
            var str = thumbIds[position];
            textView.Text = str;
            return textView;
        }
    }
}