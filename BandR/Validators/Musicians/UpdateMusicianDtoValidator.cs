using BandR.DTOs.Musicians;
using FluentValidation;

namespace BandR.Validators.Musicians;

public class UpdateMusicianDtoValidator : AbstractValidator<UpdateMusicianDto>
{
    public UpdateMusicianDtoValidator()
    {
        RuleFor(x => x.Username).MaximumLength(128).When(x => x.Username is not null);
        RuleFor(x => x.City).MaximumLength(200).When(x => x.City is not null);
        RuleFor(x => x.Bio).MaximumLength(1024).When(x => x.Bio is not null);
    }
}