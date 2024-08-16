using CommunityToolkit.Mvvm.Messaging;
using EquipmentWatcherMAUI.Models;
using EquipmentWatcherMAUI.ViewModels;
using System.Collections.ObjectModel;

namespace EquipmentWatcherMAUI.Pages;

public partial class DevicesPage : ContentPage
{
    private List<AccessDeviceModel> Devices = new();

    public DevicesPage()
	{
		InitializeComponent();

        LoadDevices();

        WeakReferenceMessenger.Default.Register<EventArgs>(this, (obj, ev) => LoadDevices() );

        if (StaticReference.ApiConnector == null) return;
    }

    private void LoadDevices()
    {
        Devices = new();
        StaticReference.ApiConnector.Get<AccessModel[]>("/Access/own",(responseObject, error) =>
        {
            if (responseObject != null && responseObject.Success)
            {
                foreach (var access in responseObject.Result)
                {
                    StaticReference.ApiConnector.Get<DeviceModel>("/AccessDevice/" + access.AccessDeviceID,
                        (responseObject, error) =>
                        {
                            if (responseObject != null && responseObject.Success)
                            {
                                var device = responseObject.Result;
                                Devices.Add(new AccessDeviceModel(access, device));
                            }
                        }
                    );
                }
                BindingContext = new DeviceViewModel(Devices);
            }
        });
    }

    private async void OnGrantButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var accessDeviceModel = (AccessDeviceModel)button.CommandParameter;
        IList<UserModel>? members = null;

        StaticReference.ApiConnector.Get<UserModel[]>("/AccessDevice/"
            + accessDeviceModel.DeviceID.ToString()
            +"/grant/search",
            (responseObject, error) =>
            {
                if (responseObject != null && responseObject.Success)
                {
                    members = new ObservableCollection<UserModel>(responseObject.Result);
                }
                else
                {
                    DisplayAlert("Error", responseObject.Error, "OK");
                }
            }
        );

        if (members == null)
        {
            return;
        }

        await Navigation.PushAsync(new GrandAccessPage()
        {
            BindingContext = new GrandAccessViewModel(members)
            {
                DeviceID = accessDeviceModel.DeviceID
            }
        }, true);
    }

    private async void OnMembersButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var accessDeviceModel = (AccessDeviceModel)button.CommandParameter;

        IList<UserModel>? members = null;
        StaticReference.ApiConnector.Get<UserModel[]>($"/AccessDevice/{accessDeviceModel.DeviceID}/members", (responseObject, error) =>
        {
            if (responseObject != null && responseObject.Success)
            {
                members = responseObject.Result.ToList();
            }
        });

        if (members == null)
        {
            return;
        }

        var membersPage = new DeviceMembersPage()
        {
            BindingContext = new DeviceMembersViewModel(members) { AccessDevice = accessDeviceModel }
        };

        await Navigation.PushAsync(membersPage, true);
    }
}