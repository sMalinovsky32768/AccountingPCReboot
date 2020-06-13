using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using AccountingPC.Properties;

namespace AccountingPC
{
    internal static class Security
    {
        public static string Login
        {
            get
            {
                try
                {
                    var ByteConverter = new UnicodeEncoding();

                    byte[] keyForLogin;
                    byte[] ivForLogin;
                    string decLogin;

                    using (var sha = SHA256.Create())
                    {
                        keyForLogin = sha.ComputeHash(Convert.FromBase64String(SecuritySettings.Default.PASSWORD));
                    }

                    using (var md5 = MD5.Create())
                    {
                        ivForLogin = Convert.FromBase64String(SecuritySettings.Default.LOGIN_HASH);
                    }

                    using (var myAes = new AesCryptoServiceProvider())
                    {
                        myAes.Key = keyForLogin;
                        myAes.IV = ivForLogin;
                        decLogin = DecryptStringFromBytes_Aes(Convert.FromBase64String(SecuritySettings.Default.LOGIN),
                            myAes.Key, myAes.IV);
                    }

                    return decLogin;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static void SetUserCredentials(string login, string pass)
        {
            try
            {
                login = !string.IsNullOrWhiteSpace(login) ? login : Login;

                var ByteConverter = new UnicodeEncoding();

                string encLogin;
                byte[] loginByte;
                byte[] encryptedLogin;

                byte[] passByte;
                string encPass;
                byte[] encryptedPass;

                byte[] keyForPass;
                byte[] keyForLogin;

                byte[] ivForPass;
                byte[] ivForLogin;

                loginByte = ByteConverter.GetBytes(login);
                passByte = ByteConverter.GetBytes(pass);
                using (var sha = SHA256.Create())
                {
                    keyForPass = sha.ComputeHash(loginByte);
                }

                using (var md5 = MD5.Create())
                {
                    ivForPass = md5.ComputeHash(passByte);
                }

                using (var myAes = new AesCryptoServiceProvider())
                {
                    myAes.Key = keyForPass;
                    myAes.IV = ivForPass;
                    encryptedPass = EncryptStringToBytes_Aes(pass, myAes.Key, myAes.IV);
                    encPass = Convert.ToBase64String(encryptedPass);
                }

                using (var sha = SHA256.Create())
                {
                    keyForLogin = sha.ComputeHash(encryptedPass);
                }

                using (var md5 = MD5.Create())
                {
                    ivForLogin = md5.ComputeHash(loginByte);
                }

                using (var myAes = new AesCryptoServiceProvider())
                {
                    myAes.Key = keyForLogin;
                    myAes.IV = ivForLogin;
                    encryptedLogin = EncryptStringToBytes_Aes(login, myAes.Key, myAes.IV);
                    encLogin = Convert.ToBase64String(encryptedLogin);
                }

                SecuritySettings.Default.PASSWORD = encPass;
                SecuritySettings.Default.LOGIN = encLogin;
                SecuritySettings.Default.LOGIN_HASH = Convert.ToBase64String(ivForLogin);

                SecuritySettings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        public static void SetUserCredentials(string userCredentials)
        {
            SetUserCredentials(userCredentials, userCredentials);
        }

        public static void SetUserCredentials()
        {
            SetUserCredentials("admin");
        }

        public static bool VerifyCredentials(string inLogin, string inPass)
        {
            try
            {
                var ByteConverter = new UnicodeEncoding();

                byte[] encryptedLogin;
                byte[] encryptedPass;

                byte[] keyForPass;
                byte[] keyForLogin;

                byte[] ivForPass;
                byte[] ivForLogin;

                using (var sha = SHA256.Create())
                {
                    keyForPass = sha.ComputeHash(ByteConverter.GetBytes(inLogin));
                }

                using (var md5 = MD5.Create())
                {
                    ivForPass = md5.ComputeHash(ByteConverter.GetBytes(inPass));
                }

                using (var myAes = new AesCryptoServiceProvider())
                {
                    myAes.Key = keyForPass;
                    myAes.IV = ivForPass;

                    encryptedPass = EncryptStringToBytes_Aes(inPass, myAes.Key, myAes.IV);
                }

                using (var sha = SHA256.Create())
                {
                    keyForLogin = sha.ComputeHash(encryptedPass);
                }

                using (var md5 = MD5.Create())
                {
                    ivForLogin = md5.ComputeHash(ByteConverter.GetBytes(inLogin));
                }

                using (var myAes = new AesCryptoServiceProvider())
                {
                    myAes.Key = keyForLogin;
                    myAes.IV = ivForLogin;
                    encryptedLogin = EncryptStringToBytes_Aes(inLogin, myAes.Key, myAes.IV);
                }

                var validLogin = Convert.ToBase64String(encryptedLogin) == SecuritySettings.Default.LOGIN;
                var validPass = Convert.ToBase64String(encryptedPass) == SecuritySettings.Default.PASSWORD;
                return validLogin && validPass;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OKCancel, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool UpdatCredentials(string oldPass, string newPass, string login = null)
        {
            try
            {
                var ByteConverter = new UnicodeEncoding();

                byte[] keyForPass;
                byte[] keyForLogin;

                byte[] ivForPass;
                byte[] ivForLogin;

                string decLogin;
                string decPass;

                using (var sha = SHA256.Create())
                {
                    keyForLogin = sha.ComputeHash(Convert.FromBase64String(SecuritySettings.Default.PASSWORD));
                }

                using (var md5 = MD5.Create())
                {
                    ivForLogin = Convert.FromBase64String(SecuritySettings.Default.LOGIN_HASH);
                }

                using (var myAes = new AesCryptoServiceProvider())
                {
                    myAes.Key = keyForLogin;
                    myAes.IV = ivForLogin;
                    decLogin = DecryptStringFromBytes_Aes(Convert.FromBase64String(SecuritySettings.Default.LOGIN),
                        myAes.Key, myAes.IV);
                }


                using (var sha = SHA256.Create())
                {
                    keyForPass = sha.ComputeHash(ByteConverter.GetBytes(decLogin));
                }

                using (var md5 = MD5.Create())
                {
                    ivForPass = md5.ComputeHash(ByteConverter.GetBytes(oldPass));
                }

                using (var myAes = new AesCryptoServiceProvider())
                {
                    myAes.Key = keyForPass;
                    myAes.IV = ivForPass;
                    decPass = DecryptStringFromBytes_Aes(Convert.FromBase64String(SecuritySettings.Default.PASSWORD),
                        myAes.Key, myAes.IV);
                }

                if (oldPass == decPass)
                {
                    SetUserCredentials(login, newPass);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            using (var aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (var aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}