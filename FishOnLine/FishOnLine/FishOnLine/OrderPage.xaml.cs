

using Xamarin.Forms;
using FishOnLine.Data;
using System.Collections.Generic;

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FishOnLine.SettingVar;
using FishOnLine.Dependency;

namespace FishOnLine
{

    public partial class OrderPage : ContentPage
    {
        private bool isRowEven;
        private int IDPortataSel;

        static List<rigaOrdine> Orders;

        public OrderPage(MenuEl selElement, List<rigaOrdine> MyOrders)
        {
            InitializeComponent();
            Orders = MyOrders;
            if (selElement != null)
            {
                lblPortata.Text = selElement.nome;
                lblDescrizione.Text = selElement.descrizione;
                lblUm.Text = selElement.um;
                IDPortataSel = selElement.idportata;
                lblPrezzo.Text = lblPrezzo.Text = String.Format(CultureInfo.InvariantCulture,
                                 "{0:N4}", selElement.prezzo);

            }
            else
            {
                stcSelMenu.IsVisible = false;
            }
            if (Orders.Count == 0) lstOrders.IsVisible = false; else lstOrders.IsVisible = true;
            lstOrders.ItemsSource = Orders;
            Calcolatotale();
        }

        private void btOk_Clicked(object sender, System.EventArgs e)
        {
            if (lstOrders.SelectedItem == null)
            {
                rigaOrdine el = new rigaOrdine();
                el.riga = Orders.Count + 1;
                el.access_token = "";
                el.nome = lblPortata.Text;
                el.descrizione = lblDescrizione.Text;
                el.um = lblUm.Text;
                el.idportata = IDPortataSel;
                el.data = DateTime.Now;
                el.prezzo = float.Parse(lblPrezzo.Text, CultureInfo.InvariantCulture);
                el.qta = float.Parse(txtQta.Text, CultureInfo.InvariantCulture);
                el.totale = el.qta * el.prezzo;
                Orders.Add(el);
                btOk.IsEnabled = false;
                txtQta.Text = "";
            }
            else
            {
                var i = (lstOrders.ItemsSource as List<rigaOrdine>).IndexOf(lstOrders.SelectedItem as rigaOrdine);
                if (i >= 0)
                {
                    var selElement = Orders[i];
                    selElement.qta = float.Parse(txtQta.Text, CultureInfo.InvariantCulture);
                    selElement.totale = selElement.qta * selElement.prezzo;
                    btOk.IsEnabled = false;
                    txtQta.Text = "";
                }
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                lstOrders.ItemsSource = null;
                lstOrders.ItemsSource = Orders;
                Calcolatotale();
                //mi posiziono all'ultimo record inserito
                var v = lstOrders.ItemsSource.Cast<rigaOrdine>().LastOrDefault();
                if (v != null)
                {
                    lstOrders.ScrollTo(v, ScrollToPosition.End, true);
                    lstOrders.SelectedItem = v;
                    if (Orders.Count == 0)
                    {
                        lstOrders.IsVisible = false;
                        btConfOrd.IsEnabled = false;
                    }
                  
                    else
                    {
                        lstOrders.IsVisible = true;
                        btConfOrd.IsEnabled = true;
                    }
                }
                else lstOrders.SelectedItem = null;
            });


        }
        private void Calcolatotale()
        {
            float tot = 0;

            foreach (rigaOrdine el in Orders)
            {
                tot = el.totale + tot;
            }
            if (tot > 0) btConfOrd.IsEnabled = true; else btConfOrd.IsEnabled = false;
            lblTotGen.Text = String.Format(CultureInfo.InvariantCulture,
                                 "{0:N2}", tot);
        }
        private void Cell_OnAppearing(object sender, System.EventArgs e)
        {
            if (this.isRowEven)
            {
                var viewCell = (ViewCell)sender;
                if (viewCell.View != null)
                {
                    viewCell.View.BackgroundColor = Color.Azure;

                }
                else
                {
                    viewCell.View.BackgroundColor = Color.LightBlue;
                }
            }

            this.isRowEven = !this.isRowEven;
        }

        private void txtQta_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                if (txtQta.Text != "")
                {
                    float val = 0;
                    val = float.Parse(txtQta.Text, CultureInfo.InvariantCulture);
                    if (val > 0) btOk.IsEnabled = true; else btOk.IsEnabled = false;
                }
                else btOk.IsEnabled = false;
            }
            catch (Exception ex)
            {
                DisplayAlert("FishOnLine - App", ex.Message.ToString(), "OK");
                btOk.IsEnabled = false;
            }

        }



        private void Elimina_riga_Clicked(object sender, EventArgs e)
        {

            var mi = ((MenuItem)sender);
            if (mi.CommandParameter != null)
            {
                Orders.Remove((rigaOrdine)mi.CommandParameter);
                lstOrders.ItemsSource = null;
                lstOrders.ItemsSource = Orders;
                Calcolatotale();
            }
            // var selel = Mylist.SingleOrDefault(el => (el.id == (int)item.CommandParameter));
        }

        private void lstOrders_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var i = (lstOrders.ItemsSource as List<rigaOrdine>).IndexOf(e.SelectedItem as rigaOrdine);
            if (i >= 0)
            {
                var selElement = Orders[i];
                lblPortata.Text = selElement.nome;
                lblDescrizione.Text = selElement.descrizione;
                lblUm.Text = selElement.um;
                IDPortataSel = selElement.idportata;
                lblPrezzo.Text = String.Format(CultureInfo.InvariantCulture,
                                 "{0:N4}", selElement.prezzo);
                txtQta.Text = String.Format(CultureInfo.InvariantCulture,
                                 "{0:N2}", selElement.qta);
                stcSelMenu.IsVisible = true;
            }
        }

        private async void btConfOrd_Clicked(object sender, EventArgs e)
        {
            try
            {
                await  InvioOrdine();
                await Navigation.PushAsync(new MainPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("FishOnLine - App", ex.Message.ToString(), "OK");
                btConfOrd.IsEnabled = false;
            }
        }
        private async Task InvioOrdine()
        {
            try
            {   var ws = new FishOnLine.WebService.WebService();

                var answer = await DisplayAlert("FishOnLine - App", "Confermi l'invio dell'ordine ?", "Yes", "No");

                if (answer)
                {
                    //righiesta accesso alla risorsa
                    UserData.access_token = await ws.GetToken(UserData.Email, UserData.password);
                    //---------------------------------
                    if ((UserData.access_token != null) && (UserData.access_token != ""))
                    {
                        //aggiorno il token agli ordini
                        foreach (rigaOrdine o in Orders)
                        {
                            o.access_token = UserData.access_token;
                        }
                        List<MsgWS> ris = await ws.InserimentoOrdine(CostantiApp.Sito, Orders);
                      
                        DependencyService.Get<IToast>().Show(ris[0].msg);
                        await ws.DeleteToken(UserData.access_token);
                    }
                    
                   
                     
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("FishOnLine - App", ex.Message.ToString(), "OK");
            }
            finally
            {


            }
        }
    }
}