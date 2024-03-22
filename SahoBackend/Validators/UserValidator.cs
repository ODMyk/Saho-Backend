using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;
using SahoBackend.Models;
using SahoBackend.Validators.RegularExpressions;

namespace SahoBackend.Validators;

public partial class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Nickname)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Matches(Regexes.Nickname).WithMessage("{PropertyName} must be a valid Nickname")
            .Custom((name, context) =>
                {
                    var rg = new Regex(Regexes.HTML_Tag);
                    if (rg.Matches(name).Count > 0) {
                        context.AddFailure(new ValidationFailure("Title", "The parameter has invalid content"));
                    }
                });
    }
}
