using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace WeatherApp.UI
{
    /// <summary>
    /// Helper script to automatically set up the Weather UI and diagnose common issues
    /// </summary>
    public class WeatherUISetupHelper : MonoBehaviour
    {
        [Header("Auto-Setup Options")]
        [SerializeField] private bool autoCreateEventSystem = true;
        [SerializeField] private bool autoFixInputFieldSettings = true;
        [SerializeField] private bool showDetailedLogs = true;
        
        private void Start()
        {
            if (showDetailedLogs)
            {
                Debug.Log("=== Weather UI Setup Helper Started ===");
            }
            
            CheckAndFixEventSystem();
            CheckInputFieldSettings();
            CheckCanvasSettings();
            
            if (showDetailedLogs)
            {
                Debug.Log("=== Weather UI Setup Helper Completed ===");
            }
        }
        
        /// <summary>
        /// Check if EventSystem exists and create one if needed
        /// </summary>
        private void CheckAndFixEventSystem()
        {
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            
            if (eventSystem == null)
            {
                if (autoCreateEventSystem)
                {
                    // Create EventSystem GameObject
                    GameObject eventSystemGO = new GameObject("EventSystem");
                    eventSystemGO.AddComponent<EventSystem>();
                    eventSystemGO.AddComponent<StandaloneInputModule>();
                    
                    Debug.Log("✅ EventSystem created automatically!");
                }
                else
                {
                    Debug.LogError("❌ No EventSystem found! UI input will not work. Please add an EventSystem to your scene.");
                }
            }
            else
            {
                if (showDetailedLogs)
                {
                    Debug.Log("✅ EventSystem found and working.");
                }
            }
        }
        
        /// <summary>
        /// Check input field settings and fix common issues
        /// </summary>
        private void CheckInputFieldSettings()
        {
            TMP_InputField[] inputFields = FindObjectsOfType<TMP_InputField>();
            
            if (inputFields.Length == 0)
            {
                Debug.LogWarning("⚠️ No TMP_InputField components found in scene.");
                return;
            }
            
            foreach (TMP_InputField inputField in inputFields)
            {
                bool wasFixed = false;
                
                // Check if input field is interactable
                if (!inputField.interactable)
                {
                    if (autoFixInputFieldSettings)
                    {
                        inputField.interactable = true;
                        wasFixed = true;
                    }
                    else
                    {
                        Debug.LogError($"❌ Input field '{inputField.name}' is not interactable!");
                    }
                }
                
                // Check if input field is read-only
                if (inputField.readOnly)
                {
                    if (autoFixInputFieldSettings)
                    {
                        inputField.readOnly = false;
                        wasFixed = true;
                    }
                    else
                    {
                        Debug.LogError($"❌ Input field '{inputField.name}' is set to read-only!");
                    }
                }
                
                // Check if input field has a text component
                if (inputField.textComponent == null)
                {
                    Debug.LogError($"❌ Input field '{inputField.name}' has no text component assigned!");
                }
                
                if (wasFixed)
                {
                    Debug.Log($"✅ Fixed input field settings for '{inputField.name}'");
                }
                else if (showDetailedLogs)
                {
                    Debug.Log($"✅ Input field '{inputField.name}' settings are correct.");
                }
            }
        }
        
        /// <summary>
        /// Check Canvas settings for proper UI rendering
        /// </summary>
        private void CheckCanvasSettings()
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            
            if (canvases.Length == 0)
            {
                Debug.LogError("❌ No Canvas found in scene! UI will not be visible.");
                return;
            }
            
            foreach (Canvas canvas in canvases)
            {
                // Check if Canvas has a GraphicRaycaster
                GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
                if (raycaster == null)
                {
                    Debug.LogWarning($"⚠️ Canvas '{canvas.name}' has no GraphicRaycaster. UI interactions may not work.");
                }
                
                if (showDetailedLogs)
                {
                    Debug.Log($"✅ Canvas '{canvas.name}' - Render Mode: {canvas.renderMode}");
                }
            }
        }
        
        /// <summary>
        /// Manual method to run all checks (can be called from inspector)
        /// </summary>
        [ContextMenu("Run UI Diagnostics")]
        public void RunDiagnostics()
        {
            Debug.Log("=== Manual UI Diagnostics Started ===");
            CheckAndFixEventSystem();
            CheckInputFieldSettings();
            CheckCanvasSettings();
            Debug.Log("=== Manual UI Diagnostics Completed ===");
        }
    }
}
