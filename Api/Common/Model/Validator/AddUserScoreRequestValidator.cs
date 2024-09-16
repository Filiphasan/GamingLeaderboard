using Api.Common.Model.Request;
using FluentValidation;

namespace Api.Common.Model.Validator;

public class AddUserScoreRequestValidator : AbstractValidator<AddUserScoreRequest>
{
    public AddUserScoreRequestValidator()
    {
        RuleFor(x => x.Score)
            .GreaterThan(0)
            .WithMessage(MessageConstant.UserScoreMessage.ScoreGreaterThanZero);
    }
}