using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.WpfApp.Api
{
    public class Result
    {   
        public string Error { get; set; }
        public bool HasError 
        { 
            get 
            { 
                return !string.IsNullOrEmpty(Error);
            } 
        }
    }
}
