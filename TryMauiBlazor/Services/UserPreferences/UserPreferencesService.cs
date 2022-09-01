// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;

namespace TryMauiBlazor.Services.UserPreferences;

public class UserPreferencesService
{
    private const string Key = "userPreferences";

    public UserPreferences LoadUserPreferences()
    {
        var result = Preferences.Get(Key, null);
        return result != null
            ? JsonSerializer.Deserialize<UserPreferences>(result) ?? new()
            : new();
    }

    public void SaveUserPreferences(UserPreferences userPreferences)
    {
        var json = JsonSerializer.Serialize(userPreferences);
        Preferences.Set(Key, json);
    }
}