using BandR.DTOs.Announcements;
using FluentValidation;

namespace BandR.Validators.Announcements;

public class UpdateAnnouncementDtoValidator : AbstractValidator<UpdateAnnouncementDto>
{
    public UpdateAnnouncementDtoValidator()
    {
        RuleFor(x => x.Title).MaximumLength(100).When(x => x.Title != null);
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description != null);
        RuleFor(x => x.City).MaximumLength(200).When(x => x.City != null);
    }
}