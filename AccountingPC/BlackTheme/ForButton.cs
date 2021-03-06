﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using AccountingPC.AccountingReport;

namespace AccountingPC.BlackTheme
{
    public partial class ForButton
    {
        public static readonly RoutedCommand CloseCommand = new RoutedUICommand(
            "Close", "CloseCommand", typeof(ForButton),
            new InputGestureCollection(new InputGesture[] {new KeyGesture(Key.F4, ModifierKeys.Alt)}));

        public static readonly RoutedCommand MaximizedCommand = new RoutedUICommand(
            "Maximized", "MaximizedCommand", typeof(ForButton),
            new InputGestureCollection(new InputGesture[] {new KeyGesture(Key.F3, ModifierKeys.Alt)}));

        public static readonly RoutedCommand MinimizedCommand = new RoutedUICommand(
            "Minimized", "MinimizedCommand", typeof(ForButton),
            new InputGestureCollection(new InputGesture[] {new KeyGesture(Key.F2, ModifierKeys.Alt)}));

        public ForButton()
        {
            CommandManager.RegisterClassCommandBinding(typeof(AccountingPCWindow),
                new CommandBinding(CloseCommand, CloseWindow));
            CommandManager.RegisterClassCommandBinding(typeof(ParametersWindow),
                new CommandBinding(CloseCommand, CloseWindow));
            CommandManager.RegisterClassCommandBinding(typeof(ConfiguringReportWindow),
                new CommandBinding(CloseCommand, CloseWindow));
            CommandManager.RegisterClassCommandBinding(typeof(AccountingPCWindow),
                new CommandBinding(MaximizedCommand, MaximizeWindow));
            CommandManager.RegisterClassCommandBinding(typeof(AccountingPCWindow),
                new CommandBinding(MinimizedCommand, MinimizeWindow));
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            ((Window) sender).Close();
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (((Window) sender).WindowState == WindowState.Maximized)
            {
                ((Window) sender).WindowState = WindowState.Normal;
                if (sender is AccountingPCWindow pcWindow)
                {
                    ((Window) sender).Height = pcWindow.LastHeight;
                    ((Window) sender).Width = pcWindow.LastWidth;
                }

                ((Path) ((Button) e.OriginalSource).Template.FindName("Maximize", (Button) e.OriginalSource)).Visibility
                    = Visibility.Visible;
                ((Path) ((Button) e.OriginalSource).Template.FindName("Restore", (Button) e.OriginalSource)).Visibility
                    = Visibility.Collapsed;
            }
            else if (((Window) sender).WindowState == WindowState.Normal)
            {
                if (sender is AccountingPCWindow pcWindow)
                {
                    pcWindow.LastHeight = ((Window) sender).Height;
                    pcWindow.LastWidth = ((Window) sender).Width;
                }

                ((Window) sender).WindowState = WindowState.Maximized;
                ((Path) ((Button) e.OriginalSource).Template.FindName("Maximize", (Button) e.OriginalSource)).Visibility
                    = Visibility.Collapsed;
                ((Path) ((Button) e.OriginalSource).Template.FindName("Restore", (Button) e.OriginalSource)).Visibility
                    = Visibility.Visible;
            }
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            ((Window) sender).WindowState = WindowState.Minimized;
        }
    }
}