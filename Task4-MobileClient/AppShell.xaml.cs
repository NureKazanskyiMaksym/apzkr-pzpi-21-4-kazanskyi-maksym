using CommunityToolkit.Mvvm.Messaging;
using EquipmentWatcherMAUI.Models;
using EquipmentWatcherMAUI.Pages;

namespace EquipmentWatcherMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Init();
            UpdateAccessToken();
        }

        private async void Init()
        {
            var userData = await SecureStorage.GetAsync("userData");

            if (userData != null)
            {
                StaticReference.ApiConnector = new ApiConnector(userData);

                StaticReference.ApiConnector.Get<UserModel>("/User/me", (responseObject, error) =>
                {
                    if (responseObject != null && responseObject.Success && error == null)
                    {
                        StaticReference.CurrentUser = responseObject.Result;
                    }
                    else if (error != null)
                    {
                        SecureStorage.Remove("userData");
                        Init();
                    }
                });

                StaticReference.ApiConnector.Get<PermissionModel[]>("/Permission/own", (responseObject, error) =>
                {
                    if (responseObject != null && responseObject.Success)
                    {
                        StaticReference.CurrentPermissions = responseObject.Result;
                    }
                });
            }
            else
            {
                await Navigation.PushModalAsync(new LoginPage(), true);
            }
        }

        private async void UpdateAccessToken()
        {
            WeakReferenceMessenger.Default.Register<EventArgs>(this, (obj, ev) =>
            {
                StaticReference.UpdateAccessToken();
            });

            while (true)
            {
                StaticReference.UpdateAccessToken();
                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }
    }
}
