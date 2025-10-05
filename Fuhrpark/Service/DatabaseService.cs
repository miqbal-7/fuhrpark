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
        private readonly string _connectionString = "server=127.0.0.1;port=3306;database=fuhrpark;user=root;password=;";

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
                    var query = "SELECT id, kennzeichen, hersteller, modell, baujahr, kilometer, tonnen, typ, status FROM wagen";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            vehicles.Add(new Vehicle
                            {
                                Id = reader.GetInt32("id"),
                                LicensePlate = reader.GetString("kennzeichen"),
                                Manufacturer = reader.GetString("hersteller"),
                                Model = reader.GetString("modell"),
                                YearOfManufacture = await reader.IsDBNullAsync("baujahr") ? (int?)null : reader.GetInt32("baujahr"),
                                Mileage = await reader.IsDBNullAsync("kilometer") ? (int?)null : reader.GetInt32("kilometer"),
                                Ton = await reader.IsDBNullAsync("tonnen") ? (double?)null : reader.GetDouble("tonnen"),
                                VehicleClass = reader.GetString("typ"),
                                State = reader.GetString("status"),
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
                    var query = "INSERT INTO wagen (kennzeichen, hersteller, modell, baujahr, kilometer, tonnen, typ, status) VALUES (@kennzeichen, @hersteller, @modell, @baujahr, @kilometer, @tonnen, @typ, @status)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kennzeichen", vehicle.LicensePlate);
                        cmd.Parameters.AddWithValue("@hersteller", vehicle.Manufacturer);
                        cmd.Parameters.AddWithValue("@modell", vehicle.Model);
                        cmd.Parameters.AddWithValue("@baujahr", vehicle.YearOfManufacture ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@kilometer", vehicle.Mileage ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@tonnen", vehicle.Ton ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@typ", vehicle.VehicleClass);
                        cmd.Parameters.AddWithValue("@status", vehicle.State);
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
                    var query = "DELETE FROM wagen WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", vehicleId);
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
                    var query = "SELECT id, kennzeichen, hersteller, modell, baujahr, kilometer, tonnen, typ, status FROM wagen WHERE typ LIKE @typ";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@typ", $"%{vehicleClass}%");
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                vehicles.Add(new Vehicle
                                {
                                    Id = reader.GetInt32("id"),
                                    LicensePlate = reader.GetString("kennzeichen"),
                                    Manufacturer = reader.GetString("hersteller"),
                                    Model = reader.GetString("modell"),
                                    YearOfManufacture = await reader.IsDBNullAsync("baujahr") ? (int?)null : reader.GetInt32("baujahr"),
                                    Mileage = await reader.IsDBNullAsync("kilometer") ? (int?)null : reader.GetInt32("kilometer"),
                                    Ton = await reader.IsDBNullAsync("tonnen") ? (double?)null : reader.GetDouble("tonnen"),
                                    VehicleClass = reader.GetString("typ"),
                                    State = reader.GetString("status"),
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
                    var query = "SELECT id, kennzeichen, hersteller, modell, baujahr, kilometer, tonnen, typ, status FROM wagen WHERE kennzeichen LIKE @kennzeichen";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kennzeichen", $"%{licensePlate}%");
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                vehicles.Add(new Vehicle
                                {
                                    Id = reader.GetInt32("id"),
                                    LicensePlate = reader.GetString("kennzeichen"),
                                    Manufacturer = reader.GetString("hersteller"),
                                    Model = reader.GetString("modell"),
                                    YearOfManufacture = await reader.IsDBNullAsync("baujahr") ? (int?)null : reader.GetInt32("baujahr"),
                                    Mileage = await reader.IsDBNullAsync("kilometer") ? (int?)null : reader.GetInt32("kilometer"),
                                    Ton = await reader.IsDBNullAsync("tonnen") ? (double?)null : reader.GetDouble("tonnen"),
                                    VehicleClass = reader.GetString("typ"),
                                    State = reader.GetString("status"),
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

        public async Task UpdateVehicleStateAsync(int vehicleId, string newState)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    await conn.OpenAsync();

                    // Die SQL-Abfrage aktualisiert nur die 'state'-Spalte
                    var query = "UPDATE wagen SET status = @status WHERE id = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        // Nur die zwei benötigten Parameter hinzufügen
                        cmd.Parameters.AddWithValue("@status", newState);
                        cmd.Parameters.AddWithValue("@id", vehicleId);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fehler beim Aktualisieren des Status: {ex.Message}");
            }
        }

        public async Task<string> GetCurrentVehicleStateAsync(int vehicleId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    await conn.OpenAsync();
                    var query = "SELECT status FROM wagen WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", vehicleId);

                        object result = await cmd.ExecuteScalarAsync();
                        return result?.ToString() ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fehler beim Abrufen des Status: {ex.Message}");
                return string.Empty;
            }
        }
    }
}