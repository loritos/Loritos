using System;
using Android.App;
using Android.Content;
using Android.OS;
using FishOnLine.Data;
using System.Collections.Generic;
using System.Net.Http;
using FishOnLine.SettingVar;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xamarin.Forms;
using FishOnLine.Dependency;
using Java.Lang;

[Service]
public class SeviceMessage : Service
{
    static async Task<List<Notifica>> RequestTimeAsync()
    {
        var httpClient = new HttpClient();
        Dictionary<string, string> parameter = new Dictionary<string, string>();
        var email = DependencyService.Get<ICrypto>().Encrypt(UserData.Email, CostantiCrypaggioDati.key, CostantiCrypaggioDati.IV);
        var password = DependencyService.Get<ICrypto>().Encrypt(UserData.password, CostantiCrypaggioDati.key, CostantiCrypaggioDati.IV);

        parameter.Add("email", email);
        parameter.Add("password", password);
        FormUrlEncodedContent formContent = new FormUrlEncodedContent(parameter);

        var httpResponse = await httpClient.PostAsync(CostantiApp.Sito + "/ws_rest/GetNotifiche.php", formContent);
        var responseContent = await httpResponse.Content.ReadAsStringAsync();

        var msg = JsonConvert.DeserializeObject<List<MsgNotifica>>(responseContent);

       

        return msg[0].dati;

    }

    public override IBinder OnBind(Intent intent)
    {
        throw new NotImplementedException();
    }

    public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
    {
      
        Device.StartTimer(TimeSpan.FromSeconds(30), () => {
            var t = new Thread(async() => {
            var msg = await RequestTimeAsync();

            // Switch back to the UI thread to update the UI
            Device.BeginInvokeOnMainThread(() =>
            {

                if (msg.Count > 0)
                {
                    foreach (Notifica el in msg)
                    {

                        DependencyService.Get<ILocalNotificationService>().CreateLocalNotification("FishOnLine", el.msg.ToString(), el.id, el.rifordine, el.idriga);
                    }
                }
                else
                {

                }
            });
          
            

        }
        );
        t.Start();
        return true; 
        });



        return StartCommandResult.Sticky;
    }


}