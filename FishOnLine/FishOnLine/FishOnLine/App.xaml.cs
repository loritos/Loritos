




using Xamarin.Forms;



namespace FishOnLine
{
  
   
    public partial class App : Application 
    {
      



       

        public App()
        {

            InitializeComponent();


            MainPage = new NavigationPage(new FishOnLine.MainPage());

           





        }

       

       
        protected override void OnStart()
        {
           
            // Handle when your app starts
            //cariacamento settaggio iniziale
            




        }
       
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected   override  void OnResume()
        {
            // Handle when your app resumes
          

           
        
        }
    }
}
