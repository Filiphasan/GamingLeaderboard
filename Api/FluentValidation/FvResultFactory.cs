﻿using Api.Common.Model;
using FluentValidation.Results;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Results;

namespace Api.FluentValidation;

public sealed class FvResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IResult CreateResult(EndpointFilterInvocationContext context, ValidationResult validationResult)
    {
        var messages = validationResult.Errors.Select(x => x.ErrorMessage).ToArray();
        var message = string.Join(", ", messages);
        var result = Result<NoDataResult>.BadRequest(message);
        return Results.Json(result, contentType: "application/json", statusCode: result.StatusCode);
    }
}

public sealed class NoDataResult
{
    
}