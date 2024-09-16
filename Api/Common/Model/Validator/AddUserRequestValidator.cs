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

        RuleFor(x => x.Password)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(MessageConstant.UserMessage.PasswordIsRequired);

        RuleFor(x => x.DeviceId)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(MessageConstant.UserMessage.DeviceIdIsRequired);

        RuleFor(x => x.DeviceId)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage(MessageConstant.UserMessage.DeviceIdIsNotValid);
    }
}