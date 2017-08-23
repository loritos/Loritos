

using Android.App;
using Android.Content;
using Android.Content.PM;

using Android.OS;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;


namespace FishOnLine.Droid
{
    [Activity(Label = "FishOnLine", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false , ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {

            try
            {

            
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);
            
           
            Forms.Init(this, bundle);
          
            LoadApplication(new App());

            StartService(new Intent(this, typeof(SeviceMessage)));
            } catch (Exception ex)
            {
                Toast toast = Toast.MakeText(Forms.Context, ex.Message, ToastLength.Long);
                toast.Show();
            }
        }
      



    }
}

