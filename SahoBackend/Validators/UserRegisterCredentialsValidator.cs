using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;
using SahoBackend.Models;
using SahoBackend.Validators.RegularExpressions;

namespace SahoBackend.Validators;

public partial class UserRegisterCredentialsValidator : AbstractValidator<UserRegisterCredentials>
{
    public UserRegisterCredentialsValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Matches(Regexes.Login).WithMessage("{PropertyName} must be a valid login")
            .Custom((name, context) => {
                var rg = new Regex(Regexes.HTML_Tag);
                    if (rg.Matches(name).Count > 0) {
                        context.AddFailure(new ValidationFailure("Login", "The parameter has invalid content"));
                    }
            });
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Matches(Regexes.Email).WithMessage("{PropertyName} must be a valid email")
            .Custom((name, context) => {
                var rg = new Regex(Regexes.HTML_Tag);
                if (rg.Matches(name).Count > 0) {
                    context.AddFailure(new ValidationFailure("Email", "The parameter has invalid content"));
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
