using project2.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace NoteSpace.Tests;

public class DocumentValidationTests
{
    [Fact]
    public void EmptyDocument_ReturnsValidationErrors()
    {
        var document = new DocumentInputViewModel { Title = "", Content = "" };

        var results = Validate(document);

        Assert.Contains(results, result => result.MemberNames.Contains(nameof(DocumentInputViewModel.Title)));
        Assert.Contains(results, result => result.MemberNames.Contains(nameof(DocumentInputViewModel.Content)));
    }

    [Fact]
    public void ValidDocument_PassesValidation()
    {
        var document = new DocumentInputViewModel
        {
            Title = "Architecture notes",
            Content = "A clear description of the system design."
        };

        Assert.Empty(Validate(document));
    }

    private static List<ValidationResult> Validate(object value)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(value, new ValidationContext(value), results, validateAllProperties: true);
        return results;
    }
}
