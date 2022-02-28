using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Dtos;

namespace UdemyAuthServer.API.Validation
{
    public class CreateUserDtoValidator:AbstractValidator<CreateUserDto>

    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("E mail is wrong");

            RuleFor(x => x.Password).NotEmpty().WithMessage("password is required");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");

        }
    }
}
