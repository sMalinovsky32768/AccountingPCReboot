﻿using AccountingPC.Properties;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AccountingPC.ParametersPages
{
    public partial class ParametersSecurityPage : Page
    {
        public ParametersSecurityPage()
        {
            InitializeComponent();
            login.Text = Settings.Default.USER_NAME;
            switch (Settings.Default.USE_AUTH)
            {
                case true:
                    useAuth.SelectedIndex = 0;
                    break;
                case false:
                    useAuth.SelectedIndex = 1;
                    break;
            }
        }

        private void ChangeClick(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(oldPass.Password) 
                || string.IsNullOrWhiteSpace(newPass.Password) 
                || string.IsNullOrWhiteSpace(repeatPass.Password)))
            {
                KeyValuePair<bool, string> res = ChangePassword();
                if (res.Key)
                {
                    changeStatus.Content = res.Value;
                    Task task;
                    task = new Task(() =>
                    {
                        try
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                i++;
                                Thread.Sleep(1000);
                            }
                            Dispatcher.Invoke(() => changeStatus.Content = string.Empty);

                        }
                        catch { }
                    });
                    task.Start();
                }
                else
                {
                    MessageBox.Show(res.Value);
                }
            }
            Settings.Default.USER_NAME = login.Text;
        }

        /// <summary>
        /// Метод сравнивает старый пароль с сохраненным. 
        /// Если пароли равны, то сравнивается новый пароль с его повтором. 
        /// Если новый пароль совпадает, хешированный пароль сохраняется.
        /// </summary>
        /// <returns>Возвращает пару ключ-значение.
        /// Ключ представляет логическое значение. True - пароль изменен. False - Произошла ошибка.
        /// Значение представляет сообщение о статусе изменения пароля.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public KeyValuePair<bool, string> ChangePassword()
        {
            string enPass = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(oldPass.Password)));
            string setPass = Settings.Default.PASSWORD_HASH;
            if (enPass == setPass)
            {
                if (newPass.Password == repeatPass.Password)
                {
                    Settings.Default.PASSWORD_HASH = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(repeatPass.Password)));
                    Settings.Default.Save();
                    return new KeyValuePair<bool, string>(true, "Пароль успешно изменен");
                }
                else
                {
                    return new KeyValuePair<bool, string>(false, "Пароли не совпадают");
                }
            }
            else
            {
                return new KeyValuePair<bool, string>(false, "Неверный пароль");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UseAuth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (useAuth.SelectedIndex)
            {
                case 0:
                    Settings.Default.USE_AUTH = true;
                    break;
                case 1:
                    Settings.Default.USE_AUTH = false;
                    break;
            }
        }
    }
}
