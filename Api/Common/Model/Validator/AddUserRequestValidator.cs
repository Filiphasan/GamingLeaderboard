using Api.Common.Model.Request;
using FluentValidation;

namespace Api.Common.Model.Validator;

public class AddUserRequestValidator : AbstractValidator<AddUserRequest>
{
    public AddUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(MessageConstant.UserMessage.UsernameIsRequired);

        RuleFor(x => x.Username)
            .Must(x => x is { Length: >= 3 and <= 30 } && x.All(char.IsLetterOrDigit))
            .WithMessage(MessageConstant.UserMessage.UsernameIsNotValid);

        RuleFor(x => x.Password)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(MessageConstant.UserMessage.PasswordIsRequired);

        RuleFor(x => x.Password)
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!@#$%^&*(),.?"":{}|<>]{8,20}$")
            .WithMessage(MessageConstant.UserMessage.PasswordIsNotValid);

        RuleFor(x => x.DeviceId)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(MessageConstant.UserMessage.DeviceIdIsRequired);

        RuleFor(x => x.DeviceId)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage(MessageConstant.UserMessage.DeviceIdIsNotValid);
    }
}