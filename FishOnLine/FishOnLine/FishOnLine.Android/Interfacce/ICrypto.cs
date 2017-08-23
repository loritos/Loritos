using System;
using FishOnLine.Dependency;

using System.Security.Cryptography;
using FishOnLine.Droid;
using System.Text;

using System.IO;
using System.Linq;

[assembly: Xamarin.Forms.Dependency(typeof(Crypto))]

namespace FishOnLine.Droid
{
    public class Crypto : ICrypto
    {
       
        public string Encrypt(string textToBeEncrypted, string Password,string IVString)
        {

            byte[] Key = ASCIIEncoding.UTF8.GetBytes(Password);
            byte[] IV = ASCIIEncoding.UTF8.GetBytes(IVString);

            string encrypted = null;
            RijndaelManaged rj = new RijndaelManaged();
            rj.Key = Key;
            rj.IV = IV;
            rj.Mode = CipherMode.CBC;

            try
            {
                MemoryStream ms = new MemoryStream();

                using (CryptoStream cs = new CryptoStream(ms, rj.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(textToBeEncrypted);
                        sw.Close();
                    }
                    cs.Close();
                }
                byte[] encoded = ms.ToArray();
                encrypted = Convert.ToBase64String(encoded);

                ms.Close();
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("A file error occurred: {0}", e.Message);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: {0}", e.Message);
            }
            finally
            {
                rj.Clear();
            }

            return encrypted;

        }
        public string Decrypt(string encrypted, string Password, string IVString)
        {
            string decrypted="";
            byte[] Key = ASCIIEncoding.UTF8.GetBytes(Password);
            byte[] IV = ASCIIEncoding.UTF8.GetBytes(IVString);
            try
            {
               


                RijndaelManaged rj = new RijndaelManaged();
                rj.Key = Key;
                rj.IV = IV;
                rj.Mode = CipherMode.CBC;


                var buffer = Convert.FromBase64String(encrypted);
                var transform = rj.CreateDecryptor();
               
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                    {
                        cs.Write(buffer, 0, buffer.Length);
                        cs.FlushFinalBlock();
                        decrypted = Encoding.UTF8.GetString(ms.ToArray());
                        cs.Close();
                    }
                    ms.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: {0}", ex.Message);
            }
            return decrypted;

        }
    }
}