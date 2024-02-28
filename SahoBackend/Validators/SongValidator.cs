using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;
using SahoBackend.Models;
using SahoBackend.Validators.RegularExpressions;

namespace SahoBackend.Validators;

public partial class SongValidator : AbstractValidator<Song>
{
    public SongValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Matches(Regexes.Title).WithMessage("{PropertyName} must be a valid Title/Nickname")
            .Custom((name, context) =>
                {
                    var rg = new Regex(Regexes.HTML_Tag);
                    if (rg.Matches(name).Count > 0) {
                        context.AddFailure(new ValidationFailure("Title", "The parameter has invalid content"));
                    }
                });
    }
}