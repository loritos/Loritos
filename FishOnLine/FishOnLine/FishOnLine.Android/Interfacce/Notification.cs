
using Android;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using FishOnLine.Dependency;
using LocalNotification.Droid;
using Xamarin.Forms;
using Android.Graphics;
using Android.OS;

[assembly: Dependency(typeof(LocalNotificationService))]
namespace LocalNotification.Droid
{
    public class LocalNotificationService : Java.Lang.Object, ILocalNotificationService 
    {
        public void CreateLocalNotification(string titolo, string descrizione,int id,string rifordine,int idriga)
        {
            Context ctx = Forms.Context;
            NotificationManager nm = (NotificationManager)ctx.GetSystemService(Context.NotificationService);
            
         
            int notId = id + 9999;
            //esecuzione dell'intent
            //Yes intent
            Intent yesReceive = new Intent( );
            yesReceive.SetAction("YES_ACTION");
            Bundle yesBundle = new Bundle();
            yesBundle.PutInt("risp", 1);//This is the value I want to pass
            yesBundle.PutInt("id", id);
            yesBundle.PutString("ordine", rifordine);
            yesBundle.PutInt("idriga", idriga);
            yesReceive.PutExtras(yesBundle);
            PendingIntent pendingIntentYes = PendingIntent.GetBroadcast(ctx, notId, yesReceive, PendingIntentFlags.CancelCurrent);
            //no intent
            Intent noReceive = new Intent();
            noReceive.SetAction("NO_ACTION");
            Bundle noBundle = new Bundle();
            noBundle.PutInt("risp", 0);//This is the value I want to pass
            noBundle.PutInt("id",id);
            noReceive.PutExtras(noBundle);
            PendingIntent pendingIntentNo = PendingIntent.GetBroadcast(ctx, notId, noReceive, PendingIntentFlags.CancelCurrent);



            Android.Content.Res.Resources res = ctx.Resources;
            Bitmap bm = BitmapFactory.DecodeResource(res, FishOnLine.Droid.Resource.Drawable.icon);
            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            System.Text.Encoding iso = System.Text.Encoding.GetEncoding("ISO-8859-1");
            string msg = iso.GetString(utf8.GetBytes(descrizione));
            NotificationCompat.Builder builder = new NotificationCompat.Builder(ctx)
                       .SetPriority(NotificationCompat.PriorityMax)
                       .SetAutoCancel(true)
                       .SetLargeIcon(Bitmap.CreateScaledBitmap(bm, 80, 80, false))
                       .SetSmallIcon(Resource.Drawable.IcDialogInfo)
                       .SetContentTitle(titolo)
                       .SetContentText(msg)
                       .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                       .SetOngoing(true)
                       .AddAction(Resource.Drawable.IcDelete, "Cancella", pendingIntentYes)
                       .AddAction(Resource.Drawable.IcMenuCloseClearCancel, "Chiudi", pendingIntentNo);
            NotificationCompat.BigTextStyle bigTextStyle = new NotificationCompat.BigTextStyle();
            bigTextStyle.SetBigContentTitle(titolo);
            bigTextStyle.BigText(descrizione);

            builder.SetStyle(bigTextStyle);

            
            nm.Notify(notId, builder.Build());
           

        }

       
    }
}

