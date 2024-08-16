using CommunityToolkit.Mvvm.Messaging;
using EquipmentWatcherMAUI.Models;
using System.Text;

namespace EquipmentWatcherMAUI.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var success = false;
        var username = Login.Text;
        var password = Password.Text;

        var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

        StaticReference.ApiConnector = new ApiConnector(authHeaderValue);

        StaticReference.ApiConnector.Get<UserModel>("/User/me", (responseObject, error) =>
        {
            if (responseObject != null && responseObject.Success && error == null)
            {
                StaticReference.CurrentUser = responseObject.Result;
                success = true;
            }
            else if (error != null)
            {
                DisplayAlert("Login failed", error, "OK");
            }
            else
            {
                DisplayAlert("Login failed", "Error message: " + responseObject?.Error, "OK");
            }
        });

        if (!success) return;

        StaticReference.ApiConnector.Get<PermissionModel[]>("/Permission/own", (responseObject, error) =>
        {
            if (responseObject != null && responseObject.Success && error == null)
            {
                StaticReference.CurrentPermissions = responseObject.Result;
            }
            else if (error != null)
            {
                DisplayAlert("Login failed", error, "OK");
            }
            else
            {
                DisplayAlert("Login failed", "Error message: " + responseObject?.Error, "OK");
            }
        });

        WeakReferenceMessenger.Default.Send<EventArgs>();

        await SecureStorage.SetAsync("userData", authHeaderValue);
        await Navigation.PopModalAsync();
    }
}
