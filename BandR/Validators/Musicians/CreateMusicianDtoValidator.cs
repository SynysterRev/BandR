using BandR.DTOs.Musicians;
using FluentValidation;

namespace BandR.Validators.Musicians;

public class CreateMusicianDtoValidator : AbstractValidator<CreateMusicianDto>
{
    public CreateMusicianDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(128);
        RuleFor(x => x.City).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Bio).MaximumLength(1024);
    }
}