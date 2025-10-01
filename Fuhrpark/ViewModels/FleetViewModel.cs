using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fuhrpark.Models;
using Fuhrpark.Service;
using Fuhrpark.View;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fuhrpark.ViewModels
{
    public partial class FleetViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<Vehicle> _vehicles;
        private string _searchText;
        private string _vehicleType = "PKW";
        private string _searchCriteria = "Kennzeichen";
        private bool _isTruckVehicle;

        //Eigenschaften für das Hinzufügen eines neuen Fahrzeugs
        private string _newLicensePlate;
        private string _newManufacturer;
        private string _newModel;
        private int? _newMileage;
        private int? _newYearOfManufacture;
        private double? _newTon;
        private string _newVehicleClass;
        private string _newState = "available";
        private object Test;

        public ObservableCollection<Vehicle> Vehicles
        {
            get => _vehicles;
            set { _vehicles = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        public string VehicleType
        {
            get => _vehicleType;
            set { _vehicleType = value; OnPropertyChanged(); IsTruckVehicle = (value == "LKW"); }
        }

        public string VehicleState
        {
            get => _newState;
            set { _newState = value; OnPropertyChanged(); }
        }

        public string SearchCriteria
        {
            get => _searchCriteria;
            set { _searchCriteria = value; OnPropertyChanged(); }
        }

        //Properties für neues Fahrzeug
        public string NewLicensePlate { get => _newLicensePlate; set { _newLicensePlate = value; OnPropertyChanged(); } }
        public string NewManufacturer { get => _newManufacturer; set { _newManufacturer = value; OnPropertyChanged(); } }
        public string NewModel { get => _newModel; set { _newModel = value; OnPropertyChanged(); } }
        public string NewVehicleClass { get => _newVehicleClass; set { _newVehicleClass = value; OnPropertyChanged(); } }
        public int? NewMileage { get => _newMileage; set { _newMileage = value; OnPropertyChanged(); } }
        public int? NewYearOfManufacture { get => _newYearOfManufacture; set { _newYearOfManufacture = value; OnPropertyChanged(); } }
        public double? NewTon { get => _newTon; set { _newTon = value; OnPropertyChanged(); } }
        public string NewState { get => _newState; set {  _newState = value; OnPropertyChanged(); } }
        public bool IsTruckVehicle
        {
            get => _isTruckVehicle;
            set
            {
                _isTruckVehicle = value;
                OnPropertyChanged();
            }
        }


        public ICommand AddVehicleCommand { get; }
        public ICommand DeleteVehicleCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand LoadVehiclesCommand { get; }
        public ICommand UpdateStateCommand { get; }

        private readonly IPopupService _popupService;
        public FleetViewModel(DatabaseService databaseService, IPopupService popupService)
        {
            _databaseService = databaseService;
            _popupService = popupService;
            Vehicles = new ObservableCollection<Vehicle>();

            AddVehicleCommand = new Command(async () => await AddVehicleAsync());
            DeleteVehicleCommand = new Command<Vehicle>(async (vehicle) => await DeleteVehicleAsync(vehicle));
            SearchCommand = new Command(async () => await SearchVehiclesAsync());
            LoadVehiclesCommand = new Command(async () => await LoadVehiclesAsync());

            _ = LoadVehiclesAsync();
        }

        public async Task LoadVehiclesAsync()
        {
            var vehiclesFromDb = await _databaseService.GetVehiclesAsync();
            Vehicles.Clear();
            foreach (var v in vehiclesFromDb)
            {
                Vehicles.Add(v);
            }
        }

        private async Task AddVehicleAsync()
        {
            if (string.IsNullOrWhiteSpace(NewLicensePlate) || string.IsNullOrWhiteSpace(NewManufacturer) ||
                string.IsNullOrWhiteSpace(NewModel) || string.IsNullOrWhiteSpace(VehicleType))
            {
                return;
            }

            if (VehicleType == "PKW")
            {
                NewVehicleClass = "PKW";
            }
            else if (VehicleType == "LKW")
            {
                NewVehicleClass = "LKW";
            }

            var newVehicle = new Vehicle
            {
                LicensePlate = NewLicensePlate,
                Manufacturer = NewManufacturer,
                Model = NewModel,
                Mileage = NewMileage,
                YearOfManufacture = NewYearOfManufacture,
                Ton = NewTon,
                VehicleClass = NewVehicleClass,
                State = NewState
            };

            await _databaseService.AddVehicleAsync(newVehicle);

            // Felder leeren
            NewLicensePlate = string.Empty;
            NewManufacturer = string.Empty;
            NewModel = string.Empty;
            NewMileage = null;
            NewYearOfManufacture = null;
            NewTon = null;
            NewVehicleClass = string.Empty;
            NewState = string.Empty;

            await _popupService.ClosePopupAsync();
            await LoadVehiclesAsync(); // Liste neu laden
        }

        public async Task UpdateVehicleState(Vehicle vehicle)
        {
            if(vehicle == null) { return; }
            VehicleState = vehicle.State; 
            await _databaseService.UpdateVehicleStateAsync(vehicle.Id, VehicleState);
            //await LoadVehiclesAsync(); // Liste neu laden
        }

        private async Task DeleteVehicleAsync(Vehicle vehicle)
        {
            if (vehicle == null) return;
            await _databaseService.DeleteVehicleAsync(vehicle.Id);
            await LoadVehiclesAsync(); // Liste neu laden
        }

        private async Task SearchVehiclesAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadVehiclesAsync(); //Wenn die Suche leer ist alle laden
                return;
            }

            List<Vehicle> searchResult;
            if (SearchCriteria == "Klasse")
            {
                searchResult = await _databaseService.SearchByClassAsync(SearchText);
            }
            else
            {
                searchResult = await _databaseService.SearchByLicensePlateAsync(SearchText);
            }

            Vehicles.Clear();
            foreach (var v in searchResult)
            {
                Vehicles.Add(v);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
