using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KooliProjekt.WpfApp.Api;

namespace KooliProjekt.WpfApp
{
    public class MainWindowViewModel
    {

        public ObservableCollection<User> Lists { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public Predicate<User> ConfirmDelete { get; set; }

        private readonly IApiClient _apiClient;

        public MainWindowViewModel() : this(new ApiClient())
        {
        }

        public MainWindowViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;

            Lists = new ObservableCollection<User>();

           

            //SaveCommand = new RelayCommand<User>(
            //async user =>
            //{
            //   if (SelectedItem == null)
            //       return;
       
            //   if (SelectedItem.UserId == Guid.Empty)  
            //   {
            //        await _apiClient.Save(SelectedItem);
            //    }
            //   else  
            //   {
            //       await _apiClient.Save(SelectedItem);
            //    }
            //    },
            //    user => SelectedItem != null && !string.IsNullOrWhiteSpace(SelectedItem.UserName)
            //   );

            SaveCommand = new RelayCommand<User>(
             // Execute
             async user =>
             {
                 await _apiClient.Save(SelectedItem);
             },
             // CanExecute
             user =>
             {
                 return SelectedItem != null;
             }
         );

            DeleteCommand = new RelayCommand<User>(
                // Execute
                async list =>
                {
                    if (ConfirmDelete != null)
                    {
                        var result = ConfirmDelete(SelectedItem);
                        if (!result)
                        {
                            return;
                        }
                    }

                    await _apiClient.Delete(SelectedItem.UserId);
                    Lists.Remove(SelectedItem);
                    SelectedItem = null;
                },
                // CanExecute
                list =>
                {
                    return SelectedItem != null;
                }
            );
        }

        public async Task Load()
        {
            Lists.Clear();

            var users = await _apiClient.AllUsers();
            foreach (var user in users)
            {
                Lists.Add(user);
            }
        }

     

        public User SelectedItem
        {
            get;
            set;
        }
    }
}
