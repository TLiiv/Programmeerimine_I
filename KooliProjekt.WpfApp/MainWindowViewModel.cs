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
        public ICommand NewCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        //public Predicate<User> ConfirmDelete { get; set; }
        //public Action <string> OnError { get; set; }


        private readonly IApiClient _apiClient;
        private readonly IDialogProvider _dialogProvider;

        public MainWindowViewModel() : this(new ApiClient(), new DialogProvider() )
        {
        }

        public MainWindowViewModel(IApiClient apiClient, IDialogProvider dialogProvider)
        {
            _apiClient = apiClient;
            _dialogProvider = dialogProvider;

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
                    
                        var result = _dialogProvider.Confirm
                            (
                                "Are you sure you want to delete selected user?",
                                "Delete User?"
                            );
                        if (!result)
                        {
                            return;
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

            var result = await _apiClient.AllUsers();
            if (result.HasError)
            {
                _dialogProvider.Error(result.Error, "Error");
                return;
            }
            foreach (var user in result.Value)
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
