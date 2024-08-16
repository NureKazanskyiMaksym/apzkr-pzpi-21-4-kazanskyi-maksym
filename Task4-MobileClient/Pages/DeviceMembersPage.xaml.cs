using EquipmentWatcherMAUI.Models;

namespace EquipmentWatcherMAUI.Pages;

public partial class DeviceMembersPage : ContentPage
{
	private List<PersonAccountModel> Members = new();

	public DeviceMembersPage()
	{
		InitializeComponent();
	}
}