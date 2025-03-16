using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KooliProjekt.WpfApp
{
    public class DialogProvider : IDialogProvider
    {
        public bool Confirm(string message, string title)
        {
            var result = MessageBox.Show(
                           message,
                           title,
                           MessageBoxButton.YesNo,
                           MessageBoxImage.Stop
                           );
            return (result == MessageBoxResult.Yes);
        }

        public void Message(string message, string title)
        {
            MessageBox.Show
              (
                  message,
                  title,
                  MessageBoxButton.OK,
                  MessageBoxImage.Information
              );
        }

        public void Error(string message, string title)
        {
            MessageBox.Show
              (
                  message,
                  title,
                  MessageBoxButton.OK,
                  MessageBoxImage.Error
              );
        }
    }
}
