
using Xamarin.Forms;

using FishOnLine.Droid;
using FishOnLine.Dependency;
using Android.Widget;


[assembly: Dependency(typeof(ToastAndroid))]

namespace FishOnLine.Droid
{
    public class ToastAndroid : IToast
    {
       
        public void Show(string message)
        {
            Toast toast = Toast.MakeText(Forms.Context, message, ToastLength.Long);
            toast.Show();
        }
    }
}