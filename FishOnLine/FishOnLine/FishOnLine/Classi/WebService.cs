
using System.Collections.Generic;
using System.Net.Http ;
using FishOnLine.Data;
using System.Threading.Tasks;
using Newtonsoft.Json;

using System.Text;
using FishOnLine.SettingVar;
using System;
using Xamarin.Forms;
using FishOnLine.Dependency;

namespace FishOnLine.WebService
{
  
    class WebService
    {
        public async Task <string> GetToken(string email, string password)
        {
            string token = "";
            try
            {
                
                var httpClient = new HttpClient();
        
                Dictionary<string, string> parameter = new Dictionary<string, string>();
                email=DependencyService.Get<ICrypto>().Encrypt(email, CostantiCrypaggioDati.key,CostantiCrypaggioDati.IV );
                password = DependencyService.Get<ICrypto>().Encrypt(password, CostantiCrypaggioDati.key, CostantiCrypaggioDati.IV);
                parameter.Add("email", email);
                parameter.Add("password", password);

                FormUrlEncodedContent formContent = new FormUrlEncodedContent(parameter);

                var httpResponse = await httpClient.PostAsync(CostantiApp.Sito + "/ws_rest/login.php", formContent);
                var responseContent = await httpResponse.Content.ReadAsStringAsync();

                var msg = JsonConvert.DeserializeObject<MsgToken>(responseContent);

                if (msg.token   != null)
                {
                
                   token = DependencyService.Get<ICrypto>().Decrypt(msg.token, CostantiCrypaggioDati.key, CostantiCrypaggioDati.IV);
                    if (msg.status!=0)
                    {
                        DependencyService.Get<IToast>().Show(msg.msg);
                    }
                }
                else
                {
                    token = "";
                }
            } catch(Exception ex){
                DependencyService.Get<IToast>().Show(ex.Message );
            }
            return token;

           
        }
        public async Task DeleteToken(string token)
        {
            var httpClient = new HttpClient();

            Dictionary<string, string> parameter = new Dictionary<string, string>();
            parameter.Add("token", token);

            FormUrlEncodedContent formContent = new FormUrlEncodedContent(parameter);


            var httpResponse = await httpClient.PostAsync(CostantiApp.Sito + "/ws_rest/logout.php", formContent);
            var responseContent = await httpResponse.Content.ReadAsStringAsync();



            var msg = JsonConvert.DeserializeObject<List<MsgWS>>(responseContent);
        }

        public async Task<List<MenuEl>> GetMenu(string url)
        {
            List<MsgWS> msg = null;
            try
            {
               

                var httpClient = new HttpClient();
               

                var httpResponse = await httpClient.GetStringAsync(url);

                msg = JsonConvert.DeserializeObject<List<MsgWS>>(httpResponse.ToString());
            }catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show(ex.Message);

            }
            finally
            {
                await DeleteToken(UserData.access_token);
              
            }
            return msg[0].dati;

        }
       // emailUtente.Text, passUtente.Text,nomeUtente.Text,numberTel.Text
        public async Task <List<MsgWS>> Registrazione(string email,string password,string nomeutente,string telefono )
        {

            var httpClient = new HttpClient();

            Dictionary<string, string> utente = new Dictionary<string, string>();
            utente.Add("telefono", telefono);
            utente.Add("password", password);
            utente.Add("nome", nomeutente);
            utente.Add("email", email );
           
            FormUrlEncodedContent formContent = new FormUrlEncodedContent(utente);

            var httpResponse =  await httpClient.PostAsync(CostantiApp.Sito  + "/ws_rest/registrazione.php", formContent);
            var responseContent = await httpResponse.Content.ReadAsStringAsync();



            return JsonConvert.DeserializeObject<List<MsgWS>>(responseContent);
        }
        public async Task<List<MsgWS>> AggiornamentoUtente( string nomeutente, string telefono,string token)
        {

            var httpClient = new HttpClient();

            Dictionary<string, string> utente = new Dictionary<string, string>();
            utente.Add("telefono", telefono);
            utente.Add("nome", nomeutente);
            utente.Add("token", token);

            FormUrlEncodedContent formContent = new FormUrlEncodedContent(utente);

            var httpResponse = await httpClient.PostAsync(CostantiApp.Sito + "/ws_rest/AggiornamentoUtente.php", formContent);
            var responseContent = await httpResponse.Content.ReadAsStringAsync();



            return JsonConvert.DeserializeObject<List<MsgWS>>(responseContent);
        }
        public async Task<MsgWS> Cancellazione(string url, string token)
        {

            var httpClient = new HttpClient();

            

            var httpResponse = await httpClient.GetAsync(url + "/ws_rest/cancellazione.php?token="+ System.Net.WebUtility.UrlEncode(token));
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MsgWS>(responseContent);
        }
        public async Task<List<MsgWS>> InserimentoOrdine(string url,List<rigaOrdine> ordini)
        {

            var httpClient = new HttpClient();
            // var parameters = new Dictionary<string, string> { { "imei", ordini[0].imei   }, { "data", ordini[0].data.ToString("yyyy-MM-dd HH:mm:ss") } };
            var json = JsonConvert.SerializeObject(ordini);
            // FormUrlEncodedContent formContent = new FormUrlEncodedContent(parameters);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await httpClient.PostAsync(url + "/ws_rest/InsertOrdine.php", content);
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<MsgWS>>(responseContent);
        }
    }
    

}