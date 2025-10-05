using CommunityToolkit.Maui.Views;
using Fuhrpark.Models;
using Fuhrpark.ViewModels;

namespace Fuhrpark.View;

public partial class AddVehicleView : Popup
{
	public AddVehicleView(FleetViewModel fleetViewModel)
	{
		InitializeComponent();
		BindingContext = fleetViewModel;
	}
}