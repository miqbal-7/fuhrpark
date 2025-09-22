using Fuhrpark.Models;
using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuhrpark.Service
{
    public class DatabaseService
    {
        //ConnectionString für die Datenbank
        private readonly string _connectionString = "server=localhost;database=fuhrpark;user=root;password=;";

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        /// <summary>
        /// Ruft alle Fahrzeuge aus der Datenbank ab.
        /// </summary>
        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            var vehicles = new List<Vehicle>();

            try
            {
                using (var conn = GetConnection())
                {
                    await conn.OpenAsync();
                    var query = "SELECT Id, LicensePlate, Manufacturer, Model, VehicleClass FROM Vehicles";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            vehicles.Add(new Vehicle
                            {
                                Id = reader.GetInt32("Id"),
                                LicensePlate = reader.GetString("LicensePlate"),
                                Manufacturer = reader.GetString("Manufacturer"),
                                Model = reader.GetString("Model"),
                                VehicleClass = reader.GetString("VehicleClass")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Konnte keine Verbindung zur Datenbank hergestellt werden. Fehler: {ex.Message}");
            }
            return vehicles;
        }

        /// <summary>
        /// Fügt ein neues Fahrzeug zur Datenbank hinzu.
        /// </summary>
        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    await conn.OpenAsync();
                    var query = "INSERT INTO Vehicles (LicensePlate, Manufacturer, Model, VehicleClass) VALUES (@LicensePlate, @Manufacturer, @Model, @VehicleClass)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@LicensePlate", vehicle.LicensePlate);
                        cmd.Parameters.AddWithValue("@Manufacturer", vehicle.Manufacturer);
                        cmd.Parameters.AddWithValue("@Model", vehicle.Model);
                        cmd.Parameters.AddWithValue("@VehicleClass", vehicle.VehicleClass);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Konnte keine Verbindung zur Datenbank hergestellt werden. Fehler: {ex.Message}");
            }
        }

        /// <summary>
        /// Löscht ein Fahrzeug anhand seiner ID.
        /// </summary>
        public async Task DeleteVehicleAsync(int vehicleId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    await conn.OpenAsync();
                    var query = "DELETE FROM Vehicles WHERE Id = @Id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", vehicleId);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Konnte keine Verbindung zur Datenbank hergestellt werden. Fehler: {ex.Message}");
            }
        }

        /// <summary>
        /// Sucht Fahrzeuge anhand der Fahrzeugklasse.
        /// </summary>
        public async Task<List<Vehicle>> SearchByClassAsync(string vehicleClass)
        {
            var vehicles = new List<Vehicle>();
            try
            {
                using (var conn = GetConnection())
                {
                    await conn.OpenAsync();
                    var query = "SELECT Id, LicensePlate, Manufacturer, Model, VehicleClass FROM Vehicles WHERE VehicleClass LIKE @VehicleClass";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@VehicleClass", $"%{vehicleClass}%");
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                vehicles.Add(new Vehicle
                                {
                                    Id = reader.GetInt32("Id"),
                                    LicensePlate = reader.GetString("LicensePlate"),
                                    Manufacturer = reader.GetString("Manufacturer"),
                                    Model = reader.GetString("Model"),
                                    VehicleClass = reader.GetString("VehicleClass")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Konnte keine Verbindung zur Datenbank hergestellt werden. Fehler: {ex.Message}");
            }
            return vehicles;
        }

        /// <summary>
        /// Sucht Fahrzeuge anhand des Kennzeichens.
        /// </summary>
        public async Task<List<Vehicle>> SearchByLicensePlateAsync(string licensePlate)
        {
            var vehicles = new List<Vehicle>();
            try
            {
                using (var conn = GetConnection())
                {
                    await conn.OpenAsync();
                    var query = "SELECT Id, LicensePlate, Manufacturer, Model, VehicleClass FROM Vehicles WHERE LicensePlate LIKE @LicensePlate";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@LicensePlate", $"%{licensePlate}%");
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                vehicles.Add(new Vehicle
                                {
                                    Id = reader.GetInt32("Id"),
                                    LicensePlate = reader.GetString("LicensePlate"),
                                    Manufacturer = reader.GetString("Manufacturer"),
                                    Model = reader.GetString("Model"),
                                    VehicleClass = reader.GetString("VehicleClass")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Konnte keine Verbindung zur Datenbank hergestellt werden. Fehler: {ex.Message}");
            }
            return vehicles;
        }
    }

}