// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using TryMauiBlazor.Services.UserPreferences;

namespace TryMauiBlazor.Services;

internal class LayoutService
{

    private readonly UserPreferencesService _userPreferencesService;
    private UserPreferences.UserPreferences _userPreferences = null!;

    public LayoutService(UserPreferencesService userPreferencesService)
    {
        _userPreferencesService = userPreferencesService;
    }

    public bool IsDarkMode { get; private set; } = false;

    public void SetDarkMode(bool value)
    {
        IsDarkMode = value;
    }


    public void ApplyUserPreferences(bool isDarkModeDefaultTheme)
    {
        _userPreferences = _userPreferencesService.LoadUserPreferences();
        if (_userPreferences != null)
        {
            IsDarkMode = _userPreferences.DarkTheme;
        }
        else
        {
            IsDarkMode = isDarkModeDefaultTheme;
            _userPreferences = new UserPreferences.UserPreferences { DarkTheme = IsDarkMode };
            _userPreferencesService.SaveUserPreferences(_userPreferences);
        }
    }

    public event EventHandler? MajorUpdateOccurred;

    private void OnMajorUpdateOccurred() => MajorUpdateOccurred?.Invoke(this, EventArgs.Empty);

    public void ToggleDarkMode()
    {
        IsDarkMode = !IsDarkMode;
        _userPreferences.DarkTheme = IsDarkMode;
        _userPreferencesService.SaveUserPreferences(_userPreferences);
        OnMajorUpdateOccurred();
    }
}
