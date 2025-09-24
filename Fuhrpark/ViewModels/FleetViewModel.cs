using Fuhrpark.Models;
using Fuhrpark.Service;
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
    public class FleetViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<Vehicle> _vehicles;
        private string _searchText;
        private string _searchCriteria = "Kennzeichen";

        //Eigenschaften für das Hinzufügen eines neuen Fahrzeugs
        private string _newLicensePlate;
        private string _newManufacturer;
        private string _newModel;
        private string _newVehicleClass;

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


        public ICommand AddVehicleCommand { get; }
        public ICommand DeleteVehicleCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand LoadVehiclesCommand { get; }

        public FleetViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
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
                string.IsNullOrWhiteSpace(NewModel) || string.IsNullOrWhiteSpace(NewVehicleClass))
            {
                return;
            }

            var newVehicle = new Vehicle
            {
                LicensePlate = NewLicensePlate,
                Manufacturer = NewManufacturer,
                Model = NewModel,
                VehicleClass = NewVehicleClass
            };

            await _databaseService.AddVehicleAsync(newVehicle);
            await LoadVehiclesAsync(); // Liste neu laden

            // Felder leeren
            NewLicensePlate = string.Empty;
            NewManufacturer = string.Empty;
            NewModel = string.Empty;
            NewVehicleClass = string.Empty;
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
