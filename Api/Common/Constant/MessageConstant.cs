﻿namespace Api.Common.Constant;

public static class MessageConstant
{
    public static class ResultMessage
    {
        public const string Success = "Success";
        public const string Error = "Error";
        public const string NotFound = "Not Found";
        public const string BadRequest = "BadRequest";
    }
    
    public static class UserMessage
    {
        public const string UsernameIsRequired = "Username is required";
        public const string UsernameIsNotValid = "Username is not valid, minimum length is 3 and maximum length is 30 and can only contain letters and numbers";
        public const string PasswordIsRequired = "Password is required";
        public const string PasswordIsNotValid = "Password is not valid, minimum length is 6 and maximum length is 20 and contain letters and numbers";
        public const string DeviceIdIsRequired = "DeviceId is required";
        public const string DeviceIdIsNotValid = "DeviceId is not valid";
        public const string UsernameOrPasswordIsNotCorrect = "Username or password is not correct";
        public const string UsernameAlreadyExists = "Username already exists";
    }

    public static class UserScoreMessage
    {
        public const string ScoreGreaterThanZero = "Score must be greater than 0";
    }
}