﻿namespace TryMauiBlazor.Models;

internal class Note
{
    public string Filename { get; set; } = null!;
    public string? Title { get; set; }
    public string? Text { get; set; }
    public DateTime Date { get; set; }
}
