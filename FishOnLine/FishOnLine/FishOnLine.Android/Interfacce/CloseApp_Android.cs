
using FishOnLine.Dependency;
using FishOnLine.Droid;
using Android.App;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidMethods))]
namespace FishOnLine.Droid
    {
   public class AndroidMethods : IAndroidMethods
   {
       public void CloseApp(bool closeApp)
       {
            if (closeApp == false)
            {
                var app = Forms.Context as Activity; ;
                app.MoveTaskToBack(true);
            }
            else
            {
              

                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());

            }
        }
   }
}
