using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;
using SahoBackend.Models;
using SahoBackend.Validators.RegularExpressions;

namespace SahoBackend.Validators;

public partial class UserAuthCredentialsValidator : AbstractValidator<UserAuthCredentials>
{
    public UserAuthCredentialsValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Matches(Regexes.Title).WithMessage("{PropertyName} must be a valid Title/Nickname")
            .Custom((name, context) =>
                {
                    var rg = new Regex(Regexes.HTML_Tag);
                    if (rg.Matches(name).Count > 0) {
                        context.AddFailure(new ValidationFailure("Title", "The parameter has invalid content"));
                    }
                });
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Matches(Regexes.Password).WithMessage("{PropertName} must be a valid password")
            .Custom((name, context) => {
                var rg = new Regex(Regexes.HTML_Tag);
                if (rg.Matches(name).Count > 0) {
                    context.AddFailure(new ValidationFailure("Password", "The parameter has invalid content"));
                }
            });
    }
}
