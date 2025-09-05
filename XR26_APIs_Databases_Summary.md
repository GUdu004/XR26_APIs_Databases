# XR26_APIs_Databases Codebase Summary

## Project Type
Unity project (multiple `.csproj` files, Unity shaders, and scripts).

## Main Functional Areas

### Weather API Integration
- Scripts for fetching and displaying weather data using the OpenWeatherMap API.
- **Key classes:**
  - `WeatherApiClient` (async/await, UnityWebRequest, JSON parsing, error handling)
  - `WeatherUIController` (UI logic for weather display)
  - `WeatherData`, `MainWeatherInfo`, `WeatherDescription` (data models, JSON attributes to be completed by students)

### Database Functionality
- SQLite integration via `SQLiteConnection` and related classes.
- UI for testing database operations (`DatabaseTestUI`), including adding, showing, and clearing high scores.
- Placeholder logic for students to implement database queries and UI updates.

### TextMesh Pro & Shaders
- Custom shaders for advanced text rendering (SDF, mobile, overlay, etc.).
- Shader files configure outline, softness, bevel, and other visual effects for text.

## Educational Focus
- Many scripts contain `TODO` comments for students to implement key logic (API calls, JSON parsing, database queries, UI event handling).
- Designed for hands-on learning in Unity, C#, async programming, and database integration.

## Other Details
- Uses Unity 6000.0.48f1 (see `ProjectVersion.txt`).
- Organized with clear namespaces (`WeatherApp.Services`, `WeatherApp.Data`, `Databases.UI`).
- Codebase is modular, separating API, database, and UI logic.

## Summary Table

| Area                | Main Files/Classes                | Purpose                                      |
|---------------------|-----------------------------------|----------------------------------------------|
| Weather API         | WeatherApiClient, WeatherUIController, WeatherData | Fetch/display weather data, student tasks    |
| Database            | SQLiteConnection, DatabaseTestUI  | SQLite integration, high score management    |
| UI                  | WeatherUIController, DatabaseTestUI | User interaction, display logic              |
| Shaders/TextMeshPro | TMP_SDF*, TMPro_Surface.cginc     | Advanced text rendering in Unity             |

## Who is this for?
- Students learning Unity, C#, async/await, API integration, and database management.

## What’s missing/for students?
- Implementation of API calls, JSON parsing, error handling, and database queries.

---
Let me know if you want a deeper dive into any specific area or file!
