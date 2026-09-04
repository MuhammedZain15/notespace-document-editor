using System.ComponentModel.DataAnnotations;

namespace project2.ViewModels;

public class DocumentInputViewModel
{
    public int Id { get; set; }

    [Required, StringLength(150, MinimumLength = 2)]
    public string Title { get; set; } = "Untitled document";

    [Required, StringLength(100_000, MinimumLength = 1)]
    public string Content { get; set; } = string.Empty;
}
