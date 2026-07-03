using BandR.DTOs.Announcements;
using FluentValidation;

namespace BandR.Validators.Announcements;

public class CreateAnnouncementDtoValidator : AbstractValidator<CreateAnnouncementDto>
{
    public CreateAnnouncementDtoValidator()
    {
        RuleFor(x => x.Title).MaximumLength(100).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(500).NotEmpty();
        RuleFor(x => x.City).MaximumLength(200).NotEmpty();
    }
}