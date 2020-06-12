using AccountingPC.Properties;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace AccountingPC
{
    internal static class Security
    {
        public static void SetUserCredentials(string login, string pass)
        {
            try
            {
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

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
                using (SHA256 sha = SHA256.Create())
                {
                    keyForPass = sha.ComputeHash(loginByte);
                }
                using (MD5 md5 = MD5.Create())
                {
                    ivForPass = md5.ComputeHash(passByte);
                }
                using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
                {
                    myAes.Key = keyForPass;
                    myAes.IV = ivForPass;
                    encryptedPass = EncryptStringToBytes_Aes(pass, myAes.Key, myAes.IV);
                    encPass = Convert.ToBase64String(encryptedPass);
                }

                using (SHA256 sha = SHA256.Create())
                {
                    keyForLogin = sha.ComputeHash(encryptedPass);
                }
                using (MD5 md5 = MD5.Create())
                {
                    ivForLogin = md5.ComputeHash(loginByte);
                }

                using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
                {
                    myAes.Key = keyForLogin;
                    myAes.IV = ivForLogin;
                    encryptedLogin = EncryptStringToBytes_Aes(login, myAes.Key, myAes.IV);
                    encLogin = Convert.ToBase64String(encryptedLogin);
                }

                SecuritySettings.Default.PASSWORD = encPass;
                SecuritySettings.Default.PASSWORD_HASH = Convert.ToBase64String(ivForPass);
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
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                byte[] encryptedLogin;
                byte[] encryptedPass;

                byte[] keyForPass;
                byte[] keyForLogin;

                byte[] ivForPass;
                byte[] ivForLogin;

                using (SHA256 sha = SHA256.Create())
                {
                    keyForPass = sha.ComputeHash(ByteConverter.GetBytes(inLogin));
                }
                using (MD5 md5 = MD5.Create())
                {
                    ivForPass = md5.ComputeHash(ByteConverter.GetBytes(inPass));
                }

                using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
                {
                    myAes.Key = keyForPass;
                    myAes.IV = ivForPass;

                    encryptedPass = EncryptStringToBytes_Aes(inPass, myAes.Key, myAes.IV);
                }

                using (SHA256 sha = SHA256.Create())
                {
                    keyForLogin = sha.ComputeHash(encryptedPass);
                }
                using (MD5 md5 = MD5.Create())
                {
                    ivForLogin = md5.ComputeHash(ByteConverter.GetBytes(inLogin));
                }

                using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
                {
                    myAes.Key = keyForLogin;
                    myAes.IV = ivForLogin;
                    encryptedLogin = EncryptStringToBytes_Aes(inLogin, myAes.Key, myAes.IV);
                }
                bool validLogin = Convert.ToBase64String(encryptedLogin) == SecuritySettings.Default.LOGIN;
                bool validPass = Convert.ToBase64String(encryptedPass) == SecuritySettings.Default.PASSWORD;
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
                login = string.IsNullOrWhiteSpace(login) ? SecuritySettings.Default.LOGIN : login;
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                byte[] keyForPass;
                byte[] keyForLogin;

                byte[] ivForPass;
                byte[] ivForLogin;

                string decLogin;
                string decPass;

                using (SHA256 sha = SHA256.Create())
                {
                    keyForLogin = sha.ComputeHash(Convert.FromBase64String(SecuritySettings.Default.PASSWORD));
                }
                using (MD5 md5 = MD5.Create())
                {
                    ivForLogin = Convert.FromBase64String(SecuritySettings.Default.LOGIN_HASH);
                }

                using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
                {
                    myAes.Key = keyForLogin;
                    myAes.IV = ivForLogin;
                    decLogin = DecryptStringFromBytes_Aes(Convert.FromBase64String(SecuritySettings.Default.LOGIN), myAes.Key, myAes.IV);
                }


                using (SHA256 sha = SHA256.Create())
                {
                    keyForPass = sha.ComputeHash(ByteConverter.GetBytes(decLogin));
                }
                using (MD5 md5 = MD5.Create())
                {
                    ivForPass = Convert.FromBase64String(SecuritySettings.Default.PASSWORD_HASH);
                }
                using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
                {
                    myAes.Key = keyForPass;
                    myAes.IV = ivForPass;
                    decPass = DecryptStringFromBytes_Aes(Convert.FromBase64String(SecuritySettings.Default.PASSWORD), myAes.Key, myAes.IV);
                }

                if (oldPass == decPass)
                {
                    SetUserCredentials(login, newPass);
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OKCancel, MessageBoxImage.Error);
                return false;
            }
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
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
