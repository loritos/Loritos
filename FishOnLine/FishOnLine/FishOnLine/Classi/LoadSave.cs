using System;

using System.IO;

using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FishOnLine.Dependency;
using Xamarin.Forms;
using System.Xml.Linq;
using FishOnLine.SettingVar;

namespace FishOnLine.ReadWrite
{
    public class LoadSave
    {
        //----------------------
        async public void WriteXML()
        {


            StringWriter sw = new StringWriter();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = Encoding.UTF8;

            using (XmlWriter xmlwriter = XmlWriter.Create(sw, xmlWriterSettings))
            {



                xmlwriter.WriteStartElement("FishOnLine");
                xmlwriter.WriteStartElement("Setting");
                xmlwriter.WriteElementString("Utente", UserData.Utente  );
                xmlwriter.WriteElementString("Password", UserData.password );
                xmlwriter.WriteElementString("Telefono", UserData.Telefono);
                xmlwriter.WriteElementString("Email", UserData.Email);
               


                xmlwriter.WriteEndElement();
                xmlwriter.WriteEndDocument();


            }
            var fileService = DependencyService.Get<ISaveAndLoad>();
            await fileService.SaveTextAsync("setting.xml", sw.ToString());
        }
        //----------------------
        public async Task LoadXML()
        {
            try
            {

                var fileService = DependencyService.Get<ISaveAndLoad>();
                var fileXML = await fileService.LoadTextAsync("setting.xml");
               
                // Create an XmlReader
                XDocument doc = XDocument.Parse(fileXML);


                XElement element = doc.Element("FishOnLine");

                UserData.Utente = (string)element.Element("Setting").Element("Utente");
                UserData.password = (string)element.Element("Setting").Element("Password");

                UserData.Telefono = (string)element.Element("Setting").Element("Telefono");

                UserData.Email = (string)element.Element("Setting").Element("Email");

               
                UserData.access_token = "";




            }
            catch (Exception ex)
            {
                UserData.Utente = "";
                UserData.Telefono = "";
                UserData.Email = "";
                UserData.password="";
              
                UserData.access_token = "";
                //return 0;
            }
        }
    }
}
