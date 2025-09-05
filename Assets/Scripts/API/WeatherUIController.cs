using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WeatherApp.Services;
using WeatherApp.Data;

namespace WeatherApp.UI
{
    /// <summary>
    /// UI Controller for the Weather Application
    /// Students will connect this to the API client and handle user interactions
    /// </summary>
    public class WeatherUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_InputField cityInputField;
        [SerializeField] private Button getWeatherButton;
        [SerializeField] private TextMeshProUGUI weatherDisplayText;
        [SerializeField] private TextMeshProUGUI statusText;
        
        [Header("API Client")]
        [SerializeField] private WeatherApiClient apiClient;
        
        private void Start()
        {
            // Validate UI components are assigned
            ValidateUIComponents();
            
            // Set up button click listener
            if (getWeatherButton != null)
            {
                getWeatherButton.onClick.AddListener(OnGetWeatherClicked);
            }

            // Initialize UI state
            SetStatusText("Enter a city name and click Get Weather");
            
            // Enable input field interaction
            if (cityInputField != null)
            {
                cityInputField.interactable = true;
                cityInputField.readOnly = false;
            }
        }
        
        /// <summary>
        /// Validate that all required UI components are assigned
        /// </summary>
        private void ValidateUIComponents()
        {
            bool hasErrors = false;
            
            if (cityInputField == null)
            {
                Debug.LogError("City Input Field is not assigned in WeatherUIController!");
                hasErrors = true;
            }
            
            if (getWeatherButton == null)
            {
                Debug.LogError("Get Weather Button is not assigned in WeatherUIController!");
                hasErrors = true;
            }
            
            if (weatherDisplayText == null)
            {
                Debug.LogError("Weather Display Text is not assigned in WeatherUIController!");
                hasErrors = true;
            }
            
            if (statusText == null)
            {
                Debug.LogError("Status Text is not assigned in WeatherUIController!");
                hasErrors = true;
            }
            
            if (apiClient == null)
            {
                Debug.LogError("API Client is not assigned in WeatherUIController!");
                hasErrors = true;
            }
            
            // Check for EventSystem
            if (UnityEngine.EventSystems.EventSystem.current == null)
            {
                Debug.LogError("No EventSystem found in scene! UI input will not work. Please add an EventSystem to your scene.");
                hasErrors = true;
            }
            
            if (!hasErrors)
            {
                Debug.Log("All UI components are properly assigned and EventSystem is present.");
            }
        }
        
        /// DONE: API integration and response handling implemented
        private async void OnGetWeatherClicked()
        {
            // Get city name from input field
            string cityName = cityInputField.text;
            
            // Validate input
            if (string.IsNullOrWhiteSpace(cityName))
            {
                SetStatusText("Please enter a city name");
                return;
            }
            
            // Disable button and show loading state
            getWeatherButton.interactable = false;
            SetStatusText("Loading weather data...");
            weatherDisplayText.text = "";
            
            try
            {
                // Call API client to get weather data
                WeatherData weatherData = await apiClient.GetWeatherDataAsync(cityName);
                
                // Handle the response
                if (weatherData != null && weatherData.IsValid)
                {
                    DisplayWeatherData(weatherData);
                    SetStatusText("Weather data loaded successfully");
                }
                else
                {
                    SetStatusText("Failed to get weather data. Please check the city name and try again.");
                    weatherDisplayText.text = "";
                }
            }
            catch (System.Exception ex)
            {
                // Handle exceptions
                Debug.LogError($"Error getting weather data: {ex.Message}");
                SetStatusText("An error occurred. Please try again.");
            }
            finally
            {
                // Re-enable button
                getWeatherButton.interactable = true;
            }
        }
        
        /// Display weather data in the UI
        private void DisplayWeatherData(WeatherData weatherData)
        {
            // Format and display weather information
            string displayText = $"City: {weatherData.CityName}\n";
            
            if (weatherData.Main != null)
            {
                displayText += $"Temperature: {weatherData.TemperatureInCelsius:F1}°C (Feels like: {weatherData.Main.FeelsLike - 273.15f:F1}°C)\n";
                displayText += $"Humidity: {weatherData.Main.Humidity}%\n";
                displayText += $"Pressure: {weatherData.Main.Pressure} hPa\n";
            }
            
            if (weatherData.Weather != null && weatherData.Weather.Length > 0)
            {
                displayText += $"Description: {weatherData.PrimaryDescription}\n";
            }
            
            weatherDisplayText.text = displayText;
        }
        
        private void SetStatusText(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }
        
        public void ClearDisplay()
        {
            weatherDisplayText.text = "";
            cityInputField.text = "";
            SetStatusText("Enter a city name and click Get Weather");
        }
    }
}
