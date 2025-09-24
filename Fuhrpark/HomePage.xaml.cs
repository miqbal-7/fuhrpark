using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using Fuhrpark.View;
using Fuhrpark.ViewModels;

namespace Fuhrpark;

public partial class HomePage : ContentPage
{
    private FleetViewModel _fleetViewModel;
    private IPopupService _popupService;
    public HomePage(FleetViewModel fleetViewModel, IPopupService popupService)
	{
		InitializeComponent();
        _fleetViewModel = fleetViewModel;
        _popupService = popupService;
		BindingContext = _fleetViewModel;
	}

    [RelayCommand]
    public async Task ShowPopupButtonAsync()
    {
        var popupView = new AddVehicleView(_fleetViewModel);
        await _popupService.ShowPopupAsync<FleetViewModel>();
    }
}