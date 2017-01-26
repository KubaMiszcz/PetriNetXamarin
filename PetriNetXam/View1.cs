using Android.Content;
using Android.Util;
using Android.Widget;

namespace PetriNetXam
{
    public class View1 : GridView
    {
        public View1(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public View1(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
        }
    }
}