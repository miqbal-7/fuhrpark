using Fuhrpark.ViewModels;

namespace Fuhrpark;

public partial class HomePage : ContentPage
{
	public HomePage(FleetViewModel fleetViewModel)
	{
		InitializeComponent();
		BindingContext = fleetViewModel;
	}
}