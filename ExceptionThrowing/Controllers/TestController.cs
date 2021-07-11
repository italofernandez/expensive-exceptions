using System;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;

namespace ExceptionThrowing.Controllers
{
    [ApiController]
    [MemoryDiagnoser]
    public class TestController : ControllerBase
    {
        private static string INVALID_EMAIL = "invalid_email#123";

        private readonly IValidator<CreateUserRequest> validator;

        public TestController(IValidator<CreateUserRequest> validator)
        {
            this.validator = validator;
        }
        
        [HttpGet("test/with")]
        [Benchmark]
        public IActionResult ThrowingException()
        {
            try
            {
                var request = new CreateUserRequest(email: INVALID_EMAIL);
                validator.ValidateAndThrow(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("test/without")]
        [Benchmark]
        public IActionResult NotThrowingException()
        {
            var request = new CreateUserRequest(INVALID_EMAIL);
            var validationResult = validator.Validate(request);

            return Ok(validationResult);
        }
    }

    public class CreateUserRequest 
    {
        public string Email { get; private set; }
        public CreateUserRequest(string email)
        {
            Email = email;
        }
    }

    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator() => RuleFor(x => x.Email).EmailAddress();
    }
}