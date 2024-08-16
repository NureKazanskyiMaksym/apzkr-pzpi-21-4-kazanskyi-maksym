using EquipmentWatcherMAUI.Models;
using EquipmentWatcherMAUI.ViewModels;

namespace EquipmentWatcherMAUI.Pages;

public partial class GrandAccessPage : ContentPage
{
    public GrandAccessPage()
	{
		InitializeComponent();
	}

    private async void OnGrantButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var user = (UserModel)button.CommandParameter;

        await Navigation.PushModalAsync(
            new GrantFormPage()
            {
                AccessRequest = new()
                {
                    AccessDeviceID = ((GrandAccessViewModel)BindingContext).DeviceID,
                    ReceiverAccountID = user.AccountID
                },
                SuccesAction = () => ((GrandAccessViewModel)BindingContext).Search()
            }, true);
    }
}