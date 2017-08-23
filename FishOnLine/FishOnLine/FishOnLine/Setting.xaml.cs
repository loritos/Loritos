using System;
using FishOnLine.ReadWrite;
using Xamarin.Forms;

using FishOnLine.Dependency;
using FishOnLine.Data;


using FishOnLine.SettingVar;
using System.Collections.Generic;

namespace FishOnLine
{   
    
    public partial class Setting : ContentPage
    {

        private bool CheckRegistrazione()
        {
            if (passUtente.Text == passConf.Text && passUtente.Text != "" && nomeUtente.Text != "" && emailUtente.Text != "")
            {
                return true;

            }
            else if (passUtente.Text != passConf.Text && passUtente.Text != "" && nomeUtente.Text != "" && emailUtente.Text != "")
            {
                DependencyService.Get<IToast>().Show("La password di conferma non corrisponde");
                return false;

            }
            else {
                DependencyService.Get<IToast>().Show("Mancano i campi obbligatori");
                return false;
                    
                    };
        }
            
    
        public Setting()
        {
            try
            {
                InitializeComponent();
       
                nomeUtente.Text = UserData.Utente;
                numberTel.Text = UserData.Telefono;
                emailUtente.Text = UserData.Email;
                passUtente.Text = UserData.password;
                passConf.Text = UserData.password;

                if (UserData.Utente!="")
                {
                  
                    btRegistra.IsEnabled = false;
                    btsalva.IsEnabled = true;
                }

                else
                {
                    btsalva.IsEnabled = false;
                    btRegistra.IsEnabled = true;
                }
            }
              catch(Exception ex)
            {
                DisplayAlert("FishOnLine - Ws", ex.Message.ToString(), "OK");
            }

           
        }

        //definizione eventi oggetti
        void numberTel_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((Entry)sender).Text; //cast sender to access the properties of the Entry
          
        }
        //-----------------------
        void nomeUtente_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((Entry)sender).Text; //cast sender to access the properties of the Entry
         
        }
        //-----------------------
        void emailUtente_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((Entry)sender).Text; //cast sender to access the properties of the Entry
            if (text == "") {
                passUtente.Text = "";
                passConf.Text = "";
            }
        
        }
        //-----------------------
       
        private async void registra_clicked(object sender, EventArgs e)
        {
            if (CheckRegistrazione())
            {

                var ws = new WebService.WebService();
                var email = DependencyService.Get<ICrypto>().Encrypt(emailUtente.Text, CostantiCrypaggioDati.key, CostantiCrypaggioDati.IV);
                var password = DependencyService.Get<ICrypto>().Encrypt(passUtente.Text, CostantiCrypaggioDati.key, CostantiCrypaggioDati.IV);
                var tel = DependencyService.Get<ICrypto>().Encrypt(numberTel.Text, CostantiCrypaggioDati.key, CostantiCrypaggioDati.IV);
                var nome = DependencyService.Get<ICrypto>().Encrypt(nomeUtente.Text, CostantiCrypaggioDati.key, CostantiCrypaggioDati.IV);

                var ris = await ws.Registrazione(email,password, nome,tel);
                if (ris[0].msg != "")
                {

                    DependencyService.Get<IToast>().Show(ris[0].msg);
                    //-------------------------
                    UserData.Utente = nomeUtente.Text;
                    UserData.Telefono = numberTel.Text;
                    UserData.Email = emailUtente.Text;
                    UserData.password = passUtente.Text;
                    //------------------------------------
                    var save = new LoadSave();

                    save.WriteXML();
                    //await DisplayAlert("FishOnLine - App", "Salvato", "OK");
                }
                else
                {

                    DependencyService.Get<IToast>().Show("Errore di registrazione");
                }
            }
        }
     /*   private async Task registrazione()
        {
            try
            {
                var answer = await DisplayAlert("FishOnLine - App", "Confermi la tua registazione nel sistema ?", "Yes", "No");
                if (answer)
                {
                    var ws = new FishOnLine.WebService.WebService();
                    List<MsgWS> ris = await ws.Registrazione(UserData.Sito, emailUtente.Text, passUtente.Text,nomeUtente.Text,numberTel.Text);

                    await DisplayAlert("FishOnLine - Ws", ris[0].msg, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("FishOnLine - Ws", ex.Message.ToString(), "OK");
            }
          
        }*/

        private async void cancella_clicked(object sender, EventArgs e)
        {
            try
            {
                var ws = new FishOnLine.WebService.WebService();
                var answer = await DisplayAlert("FishOnLine - App", "Confermi la tua cancellazione dal sistema ?", "Yes", "No");
                if (answer)
                {
                    //righiesta accesso alla risorsa
                    UserData.access_token = await ws.GetToken(UserData.Email, UserData.password);
                    //-------------------------------------------
                    if ((UserData.access_token != null) && (UserData.access_token != ""))
                    {
                        MsgWS ris = await ws.Cancellazione(CostantiApp.Sito, UserData.access_token);
                        //var fileService = DependencyService.Get<ISaveAndLoad>();
                        //fileService.DeleteFile("setting.xml");
                        DependencyService.Get<IToast>().Show("Utente cancellato con successo");
                    }
                }
            }
                
            catch (Exception ex)
            {
                await DisplayAlert("FishOnLine - Ws", ex.Message.ToString(), "OK");
            }
            
        }

       


      

        //private async void login_clicked(object sender, EventArgs e)
        //{
        //   var ws = new WebService.WebService();
        //   await ws.Login(emailUtente.Text, passUtente.Text);
        //    if (UserData.access_token != "")
        //    {
               
        //        UserData.Email = emailUtente.Text;
        //        UserData.password = passUtente.Text;
        //        var save = new LoadSave();

        //        save.WriteXML();
        //        DependencyService.Get<IToast>().Show("Utente loggato con successo");
        //        await Navigation.PushAsync(new MainPage());

        //    }
        //    else
        //    {
        //        DependencyService.Get<IToast>().Show("Errore nella login");
              
        //    }
        //}

        private async  void salva_clicked(object sender, EventArgs e)
        {
            var save = new LoadSave();
            var ws = new WebService.WebService();
            try
            {
                if (CheckRegistrazione())
                {
                   
                    //righiesta accesso alla risorsa
                    UserData.access_token = await ws.GetToken(UserData.Email, UserData.password);
                    //---------------------------------
                    if ((UserData.access_token != null) && (UserData.access_token != ""))
                    {
                       
                        var utente= DependencyService.Get<ICrypto>().Encrypt(nomeUtente.Text, CostantiCrypaggioDati.key, CostantiCrypaggioDati.IV);
                        var tel = DependencyService.Get<ICrypto>().Encrypt(numberTel.Text, CostantiCrypaggioDati.key, CostantiCrypaggioDati.IV);
                        List<MsgWS> ris= await ws.AggiornamentoUtente(utente ,tel, UserData.access_token);
                        UserData.Email = emailUtente.Text;
                        UserData.password = passUtente.Text;
                        UserData.Telefono = numberTel.Text;
                        UserData.Utente = nomeUtente.Text;
                        save.WriteXML();

                        DependencyService.Get<IToast>().Show(ris[0].msg);
                 
                    }   else DependencyService.Get<IToast>().Show("Nome utente e password errata");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("FishOnLine - Ws", ex.Message.ToString(), "OK");
            }
            await ws.DeleteToken(UserData.access_token);
        }
    }
    


}
