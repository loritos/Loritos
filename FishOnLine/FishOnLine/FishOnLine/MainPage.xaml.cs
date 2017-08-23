using System;
using FishOnLine.Dependency;
using Xamarin.Forms;
using FishOnLine.ReadWrite;
using System.Threading.Tasks;

using System.IO;
using System.Collections.Generic;
using FishOnLine.Data;
using System.Diagnostics;

using System.Linq;
using System.Collections.ObjectModel;
using FishOnline.AndroidOption;

using FishOnLine.SettingVar;


namespace FishOnLine
{
    
    public partial class MainPage : ContentPage
    {
        private List<MenuEl> Mylist;
        private List<rigaOrdine> MyOrders = new List<rigaOrdine>();
      
        public  MainPage()
        {
            CostantiApp.isstarting = false;
            InitializeComponent();

        }
        private   async Task Caricamento( )
        {

            try
            {

                var ws = new WebService.WebService();
                //righiesta accesso alla risorsa
                UserData.access_token= await ws.GetToken(UserData.Email, UserData.password);
                //---------------------------------
                if ((UserData.access_token != null) && (UserData.access_token != ""))
                {


                    lstViewMenu.IsRefreshing = true;


                    Mylist = await ws.GetMenu(CostantiApp.Sito + "/ws_rest/GetMenu.php?token=" + System.Net.WebUtility.UrlEncode(UserData.access_token));
                   
                   
                    //---------------------------------------
                 
                    if (Mylist != null)
                    {
                        if (Mylist.Count > 0)
                        {
                            //----------------------------------------------------
                            CaricamentoFoto(Mylist);
                            var sorted = from el in Mylist
                                         orderby el.categoria
                                         group el by el.categoria into elGroup
                                         select new Grouping<string, MenuEl>(elGroup.Key, elGroup);

                            //create a new collection of groups
                            var catGrouped = new ObservableCollection<Grouping<string, MenuEl>>(sorted);
                            lstViewMenu.ItemsSource = catGrouped;
                            lstViewMenu.IsGroupingEnabled = true;
                            lstViewMenu.GroupDisplayBinding = new Binding("Key");
                            lstViewMenu.HasUnevenRows = true;
                            lstViewMenu.GroupHeaderTemplate = new DataTemplate(typeof(HeaderCell));
                            lstViewMenu.IsRefreshing = false;

                            //-------------------------------------------------------
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DependencyService.Get<IToast>().Show("Nessun menu disponibile");
                                lstViewMenu.IsRefreshing = false;
                            });
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DependencyService.Get<IToast>().Show("Utente non registrato nel sistema");
                            lstViewMenu.IsRefreshing = false;
                        });

                    }
                   
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("FishOnLine - App", ex.Message.ToString(), "OK");
            }
        
          
        }

        

        private void CaricamentoFoto(List<MenuEl> Mylist)
        {
            try
            {
                foreach (MenuEl el in Mylist)
                {


                    if (el.foto == null)
                    {
                        el.stream_foto = null;
                    }
                    else
                    {
                        byte[] bytes = el.foto as byte[];

                        var memoryStream = new MemoryStream();

                       

                        el.stream_foto = ImageSource.FromStream(() => new MemoryStream(bytes));
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }
        private async  void Esci_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("FishOnLine - App", "Confermi l'uscita dall'app ?", "Yes", "No");
           
                if (Device.OS == TargetPlatform.Android)
                {
                   
                DependencyService.Get<IAndroidMethods>().CloseApp(answer);
                  
                    
               }

        }

        private   void cell_tapped(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
               
                 viewCell.View.BackgroundColor = Color.Transparent;
              

            }
        }

        private async  void impostazioni_Clicked(object sender, EventArgs e)
        {
    
            await Navigation.PushAsync(new Setting());
             
        }

        private async  void appearing(object sender, EventArgs e)
        {

           if (CostantiApp.isstarting == false)
           {
               var load = new LoadSave();
               await load.LoadXML();
               CostantiApp.isstarting = true;
               
            }
            await Caricamento();
           
        }

        private void selected_item(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

      

        private async void btOrdina_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = (Button)sender;

                var selel = Mylist.SingleOrDefault(el => (el.id == (int)item.CommandParameter));
                await Navigation.PushAsync(new OrderPage(selel, MyOrders));
            }
            catch(Exception  ex)
            {
              await   DisplayAlert("FishOnLine - App", ex.Message.ToString(), "OK");
            }





        }

        private async  void toolordine_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new OrderPage(null, MyOrders));
            }
            catch (Exception ex)
            {
                await DisplayAlert("FishOnLine - App", ex.Message.ToString(), "OK");
            }
        }
    }
    



    }

public class HeaderCell : ViewCell
{
    public HeaderCell()
    {
        this.Height = 35;
        var title = new Label
        {
            Font = Font.SystemFontOfSize(14, FontAttributes.Bold),
            TextColor = Color.White,
            VerticalOptions = LayoutOptions.Center
        };

        title.SetBinding(Label.TextProperty, "Key");

        View = new GradientColorStack
        {
            StartColor = Color.FromHex("#00008b"),
            EndColor = Color.FromHex("#ffffff"),
            HorizontalOptions = LayoutOptions.FillAndExpand,
            HeightRequest = 25,
          //  BackgroundColor = Color.FromHex("#008060"),
            Padding = 5,
            Orientation = StackOrientation.Horizontal,
            Children = { title }
        };
        
       
      
    }
}