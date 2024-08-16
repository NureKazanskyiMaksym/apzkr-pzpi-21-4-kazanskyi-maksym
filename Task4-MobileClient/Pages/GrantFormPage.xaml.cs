using EquipmentWatcher.Requests;

namespace EquipmentWatcherMAUI.Pages;

public partial class GrantFormPage : ContentPage
{
	public AccessRequest AccessRequest { get; set; } = new();
	public Action SuccesAction { get; set; }

	public GrantFormPage()
	{
		InitializeComponent();
	}

	private void OnGrantButtonClicked(object sender, EventArgs e)
    {
        var expiresOn = DatePicker.Date + TimePicker.Time;
		var isProvider = IsProviderCheckBox.IsChecked;
		AccessRequest.ExpiresOn = expiresOn;
		AccessRequest.AllowProvide = isProvider;

		StaticReference.ApiConnector.Post("/Access", AccessRequest, (responseObject, error) =>
		{
			if (responseObject != null && responseObject.Success && error == null)
			{
				Navigation.PopModalAsync();
				SuccesAction();
			}
			else if (error != null)
			{
				DisplayAlert("Error", error, "OK");
			}
			else
			{
				DisplayAlert("Error", responseObject.Error, "OK");
			}
		});

    }
}