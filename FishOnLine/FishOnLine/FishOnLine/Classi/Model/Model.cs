




using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace FishOnLine.Data
{
    //risposta json dal sito degli utenti 
    public class MsgToken
    {
        public int status { get; set; }
        public string msg { get; set; }
        public string token { get; set; }
    }
    //record utenti 
    public class users
    {
        public int idcontatto { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string telefono { get; set; }
        public string access_token { get; set; }
    }
    //risposta json dal sito per i menu 
    public class MsgWS
    {
        public int status { get; set; }
        public string msg { get; set; }
        public List<MenuEl> dati { get; set; }
    }
    //risposta json dal sito per le notifiche 
    public class MsgNotifica
    {
        public int status { get; set; }
        public string msg { get; set; }
        public List<Notifica> dati { get; set; }
    }
    //record notifiche messaggi per gli utenti 
    public class Notifica
    {
        public int id { get; set; }
        public string  data { get; set; }
        public string rifordine { get; set; }
        public int idriga { get; set; }
        public string msg { get; set; }

    }
    //record menu 
    public class MenuEl
    {
        public int id { get; set; }

        public int idportata { get; set; }

        public string nome { get; set; }

        public string descrizione { get; set; }
        public string data { get; set; }
        public float disponibilita { get; set; }
        public float prezzo { get; set; }
        public float sconto { get; set; }
        public bool offerta { get; set; }
        public bool sempre_disponibile { get; set; }
        public bool disponibile { get; set; }
        public string categoria { get; set; }
        public string um { get; set; }
        public byte[] foto { get; set; }
        public ImageSource stream_foto { get; set; }
       
    }

}
//raggruppamento menu principale
public class Grouping<K, T> : ObservableCollection<T>
{
    public K Key { get; private set; }

    public Grouping(K key, IEnumerable<T> items)
    {
        Key = key;
        foreach (var item in items)
            this.Items.Add(item);
    }
}
//record ordine corrente
public class rigaOrdine
{
    public int riga { get; set; }
    public int idportata { get; set; }
    public string nome { get; set; }
    public string descrizione { get; set; }
    public string access_token { get; set; }
    public DateTime  data { get; set; }
    public string um { get; set; }
    public float prezzo { get; set; }
    public float qta { get; set; }
    public float totale { get; set; }
}
