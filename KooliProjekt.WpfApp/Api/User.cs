using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.WpfApp.Api
{
   public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
        //public List<UserBets> UserBets { get; set; } = new List<UserBets>();
        public ObservableCollection<UserBets> UserBets { get; set; } = new ObservableCollection<UserBets>();
    }
}
