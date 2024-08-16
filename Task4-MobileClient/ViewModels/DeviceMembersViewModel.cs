using EquipmentWatcherMAUI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace EquipmentWatcherMAUI.ViewModels;

public class DeviceMembersViewModel : INotifyPropertyChanged
{
	readonly IList<UserModel> source;
    AccessDeviceModel accessDevice;

	public ObservableCollection<UserModel> DeviceMembers { get; private set; }

    public AccessDeviceModel AccessDevice
    {
        get => accessDevice;
        set
        {
            if (accessDevice != value)
            {
                accessDevice = value;
            }
        }
    }

    public ICommand RemovePersonCommand => new Command<UserModel>(RemovePerson);

	public DeviceMembersViewModel(IList<UserModel> personModels)
	{
		this.source = personModels;

        DeviceMembers = new ObservableCollection<UserModel>(source);
	}

    void RemovePerson(UserModel person)
    {
        StaticReference.ApiConnector.Delete<object>("/AccessDevice/"
            + AccessDevice.AccessDeviceID.ToString()
            + "/account/"
            + person.AccountID.ToString(),
            (responseObject) =>
            {
                if (responseObject != null && responseObject.Success)
                {
                    DeviceMembers.Remove(person);
                }
            });
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}