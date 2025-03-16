using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.WpfApp
{
    public interface IDialogProvider
    {
        bool Confirm(string message,string title);
        void Message(string message,string title);
        void Error(string message,string title);
    }
}
