using System;
using BenchmarkDotNet.Attributes;
using ExceptionThrowing.Controllers;
using FluentValidation;

[MemoryDiagnoser]
public class ProgramBenchmarck
{
    private const string INVALID_EMAIL = "invalid_email#123";
    private IValidator<CreateUserRequest> validator;

    [GlobalSetup]
    public void GlobalSetup() => validator = new CreateUserRequestValidator();

    [Benchmark]
    public string ThrowingException()
    {
        try
        {
            var request = new CreateUserRequest(INVALID_EMAIL);
            validator.ValidateAndThrow(request);
            return "ThrowingException [Ignored return]";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [Benchmark]
    public string NotThrowingException()
    {
        var request = new CreateUserRequest(INVALID_EMAIL);
        var validationResult = validator.Validate(request);
        return "NotThrowingException";
    }
}