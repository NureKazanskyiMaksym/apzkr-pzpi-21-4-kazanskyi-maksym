using EquipmentWatcherMAUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EquipmentWatcherMAUI.ViewModels
{
    public class GrandAccessViewModel : INotifyPropertyChanged
    {
        readonly IList<UserModel> source;
        int deviceID;
        UserModel selectedPerson;
        string searchText;

        public ObservableCollection<UserModel> Persons { get; private set; }

        public int DeviceID
        {
            get => deviceID;
            set
            {
                if (deviceID != value)
                {
                    deviceID = value;
                }
            }
        }

        public UserModel SelectedPerson
        {
            get => selectedPerson;
            set
            {
                if (selectedPerson != value)
                {
                    selectedPerson = value;
                }
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    OnPropertyChanged();
                    Search();
                }
            }
        }

        public GrandAccessViewModel(IList<UserModel> personModels)
        {
            source = personModels;

            Persons = new ObservableCollection<UserModel>(source);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Search()
        {
            StaticReference.ApiConnector.Get<UserModel[]>($"/AccessDevice/{deviceID}/grant/search?q={searchText}", (responseObject, error) =>
            {
                if (responseObject != null && responseObject.Success)
                {
                    Persons = new ObservableCollection<UserModel>(responseObject.Result);
                    OnPropertyChanged("Persons");
                }
            });
        }
    }
}
