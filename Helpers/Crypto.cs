using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using Enroll.Managers;

namespace eNroll.Helpers
{
    public class Crypto
    {
        private static string _passphrase = string.Empty;

        public static string Encrypt(string message)
        {
            var key = WebConfigurationManager.AppSettings.GetValues("EncryptKey");
            if (key != null)
            {
                _passphrase = key.ToString();
            }
            var results = new byte[] {};
            var utf8 = new UTF8Encoding();
            var hashProvider = new MD5CryptoServiceProvider();
            var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(_passphrase));
            var tdesAlgorithm = new TripleDESCryptoServiceProvider
                                    {
                                        Key = tdesKey,
                                        Mode = CipherMode.ECB,
                                        Padding = PaddingMode.PKCS7
                                    };
            var dataToEncrypt = utf8.GetBytes(message);
            try
            {
                var encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            catch (Exception e)
            {
                ExceptionManager.ManageException(e);
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }
            return Convert.ToBase64String(results);
        }

        public static string Decrypt(string message)
        {
            var key = WebConfigurationManager.AppSettings.GetValues("EncryptKey");
            if (key != null)
            {
                _passphrase = key.ToString();
            }
            var results = new byte[] {};
            var utf8 = new UTF8Encoding();
            var hashProvider = new MD5CryptoServiceProvider();
            var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(_passphrase));
            var tdesAlgorithm = new TripleDESCryptoServiceProvider
                                    {
                                        Key = tdesKey,
                                        Mode = CipherMode.ECB,
                                        Padding = PaddingMode.PKCS7
                                    };
            var dataToDecrypt = Convert.FromBase64String(message);
            try
            {
                var decryptor = tdesAlgorithm.CreateDecryptor();
                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }
            return utf8.GetString(results);
        }
    }
}