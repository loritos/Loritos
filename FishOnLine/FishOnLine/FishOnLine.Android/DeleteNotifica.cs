
using Android.App;
using Android.Content;
using Android.Widget;
using FishOnLine.Data;

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

[BroadcastReceiver(Enabled = true)]
[IntentFilter(new[] { "DeleteNotifica" })]
public class DeleteNotifica : BroadcastReceiver
{
   
    string url = "http://loritos.superweb.ws";
    public override async void OnReceive(Context context, Intent intent)
    {
        //Now that the user has opened the app, cancel the notification in the notification center if it is still there
        NotificationManager mNotificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);

       
      

        int userAnswer = intent.GetIntExtra("risp",0);
        if (userAnswer == 1)
        {
            mNotificationManager.Cancel(intent.GetIntExtra("id", 0) + 9999);
            List<MsgWS> risp = await CancellazioneMessaggio(url, intent.GetIntExtra("id", 0), intent.GetStringExtra("ordine"),intent.GetIntExtra("idriga",0));

           
            Toast.MakeText(context, "Messaggio cancellato", ToastLength.Long).Show();



        }       
        else if (userAnswer == 0)
        {
            mNotificationManager.Cancel(intent.GetIntExtra("id", 0)+ 9999);
        }
        Intent it = new Intent(Intent.ActionCloseSystemDialogs); context.SendBroadcast(it);


    }
    public async Task<List<MsgWS>> CancellazioneMessaggio(string url, int id,string rifordine,int idriga)
    {

        var httpClient = new HttpClient();



        var httpResponse = await httpClient.DeleteAsync(url + "/ws_rest/CancellazioneMessaggio.php?id=" + id.ToString()+"&ordine="+rifordine+ "&idriga="+ idriga);
        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<MsgWS>>(responseContent);
    
    }
    
}