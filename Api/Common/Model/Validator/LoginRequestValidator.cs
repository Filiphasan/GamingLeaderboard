using Api.Common.Model.Request;
using FluentValidation;

namespace Api.Common.Model.Validator;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(MessageConstant.UserMessage.UsernameIsRequired);

        RuleFor(x => x.Password)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(MessageConstant.UserMessage.PasswordIsRequired);
    }
}