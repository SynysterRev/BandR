using BandR.DTOs.Messages;
using FluentValidation;

namespace BandR.Validators.Conversations;

public class CreateMessageDtoValidator : AbstractValidator<CreateMessageDto>
{
    public CreateMessageDtoValidator()
    {
        RuleFor(m => m.Content).NotEmpty().MaximumLength(1024);
    }
}