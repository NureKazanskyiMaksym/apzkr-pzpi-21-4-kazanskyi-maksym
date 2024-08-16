using EquipmentWatcherMAUI.Models;
using System.ComponentModel;

namespace EquipmentWatcherMAUI.Pages;

public partial class UserPage : ContentPage
{
	public UserPage()
	{
		InitializeComponent();
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        StaticReference.CurrentUser = null;
        StaticReference.CurrentPermissions = null;
        StaticReference.CurrentAccessToken = null;
        SecureStorage.Remove("userData");
        Navigation.PushModalAsync(new LoginPage());
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        FirstNameLabel.Text = StaticReference.CurrentUser?.FirstName;
        MiddleNameLabel.Text = StaticReference.CurrentUser?.MiddleName;
        LastNameLabel.Text = StaticReference.CurrentUser?.LastName;
        UserEmailLabel.Text = StaticReference.CurrentUser?.Email;
        base.OnNavigatedTo(args);
    }
}