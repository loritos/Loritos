


using System.Threading.Tasks;


namespace FishOnLine.Dependency
{
    public interface ISaveAndLoad
    {
        Task SaveTextAsync(string filename, string text);
        Task<string> LoadTextAsync(string filename);
        bool FileExists(string filename);
        bool DeleteFile(string filename);
    }

   

    public interface IAndroidMethods
    {
        void CloseApp(bool closeApp);
    }

    public interface IImeiService
    {
        string GetImei();
    }
    public interface ILocalNotificationService
    {
        void CreateLocalNotification(string titolo , string descrizione,int id ,string rifordine,int idriga);
    }
    public interface IToast
    {
 
        void Show(string message);
    }
    public interface ICrypto
    {
        string Encrypt( string cipherText, string key, string IV);
        string Decrypt(string encrypted, string Password, string IVString);


    }
}
