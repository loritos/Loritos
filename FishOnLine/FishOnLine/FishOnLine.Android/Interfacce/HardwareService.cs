
using FishOnLine.Dependency;
using FishOnLine.Droid;
using Android.Telephony;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ImeiService))]
namespace FishOnLine.Droid
{
    public class ImeiService : IImeiService
    {
        public string GetImei()
        {
            var imei = "";
            var telephonyManager = (TelephonyManager)Forms.Context.GetSystemService(Android.Content.Context.TelephonyService);
            imei=telephonyManager.DeviceId;
            return imei;
        }
    }
}