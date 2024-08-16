using EquipmentWatcherMAUI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EquipmentWatcherMAUI.ViewModels
{
    public class DeviceViewModel : INotifyPropertyChanged
    {
        readonly IList<AccessDeviceModel> source;

        public ObservableCollection<AccessDeviceModel> AccessDevices { get; private set; }

        public DeviceViewModel(IList<AccessDeviceModel> accessDeviceModels)
        {
            source = accessDeviceModels;

            AccessDevices = new ObservableCollection<AccessDeviceModel>(source);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
