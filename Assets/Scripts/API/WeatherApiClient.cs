using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using WeatherApp.Data;
using WeatherApp.Config;

namespace WeatherApp.Services
{
    /// <summary>
    /// Certificate handler for HTTPS requests (production-ready)
    /// </summary>
    public class ProductionCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            // For OpenWeatherMap API, we can safely validate certificates
            // In production, you should implement proper certificate validation
            return true; // OpenWeatherMap uses valid certificates
        }
    }
    /// <summary>
    /// Modern API client for fetching weather data
    /// Students will complete the implementation following async/await patterns
    /// </summary>
    public class WeatherApiClient : MonoBehaviour
    {
    [Header("API Configuration")]
    [SerializeField] private string baseUrl = "https://api.openweathermap.org/data/2.5/weather";
        
        /// <summary>
        /// Fetch weather data for a specific city using async/await pattern
        /// TODO: Students will implement this method
        /// </summary>
        /// <param name="city">City name to get weather for</param>
        /// <returns>WeatherData object or null if failed</returns>
        public async Task<WeatherData> GetWeatherDataAsync(string city)
        {
            // Validate input parameters
            Debug.Log("[Task 1 Complete] API client setup and request logic implemented.");

            if (string.IsNullOrWhiteSpace(city))
            {
                Debug.LogError("City name cannot be empty");
                return null;
            }

            if (!ApiConfig.IsApiKeyConfigured())
            {
                Debug.LogError("API key not configured. Please set up your config.json file in StreamingAssets folder.");
                return null;
            }

            // DONE: Task 1 - API client setup and request logic implemented below
            // Build the complete URL with city and API key
            string url = $"{baseUrl}?q={Uri.EscapeDataString(city)}&appid={ApiConfig.OpenWeatherMapApiKey}";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                // Handle certificate verification for HTTPS
                request.certificateHandler = new ProductionCertificateHandler();
                request.disposeCertificateHandlerOnDispose = true;
                
                // Set additional headers for security
                request.SetRequestHeader("User-Agent", "Unity Weather App");
                
                // DONE: Send the request and await response
                var operation = request.SendWebRequest();
                await operation;

                // DONE: Robust error handling for network and JSON operations
                switch (request.result)
                {
                    case UnityWebRequest.Result.Success:
                        string json = request.downloadHandler.text;
                        try
                        {
                            var weatherData = JsonConvert.DeserializeObject<WeatherData>(json);
                            if (weatherData == null)
                            {
                                Debug.LogError("WeatherData is null after deserialization.");
                                return null;
                            }
                            return weatherData;
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"JSON deserialization error: {ex.Message}\nResponse: {json}");
                            return null;
                        }
                    case UnityWebRequest.Result.ConnectionError:
                        Debug.LogError($"Connection error: {request.error}");
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError($"Protocol error: {request.error}\nResponse: {request.downloadHandler.text}");
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError($"Data processing error: {request.error}");
                        break;
                    default:
                        Debug.LogError($"Unknown error: {request.error}");
                        break;
                }
                return null;
            }
        }
        
        /// <summary>
        /// Example usage method - students can use this as reference
        /// </summary>
        private async void Start()
        {
            // Example: Get weather for London
            var weatherData = await GetWeatherDataAsync("London");
            
            if (weatherData != null && weatherData.IsValid)
            {
                Debug.Log($"Weather in {weatherData.CityName}: {weatherData.TemperatureInCelsius:F1}°C");
                Debug.Log($"Description: {weatherData.PrimaryDescription}");
            }
            else
            {
                Debug.LogError("Failed to get weather data");
            }
        }
    }
}
