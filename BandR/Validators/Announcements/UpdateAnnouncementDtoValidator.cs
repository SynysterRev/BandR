using BandR.DTOs.Announcements;
using FluentValidation;

namespace BandR.Validators.Announcements;

public class UpdateAnnouncementDtoValidator : AbstractValidator<UpdateAnnouncementDto>
{
    public UpdateAnnouncementDtoValidator()
    {
        RuleFor(x => x.Title).MaximumLength(100).NotEmpty().When(x => x.Title != null);
        RuleFor(x => x.Description).MaximumLength(500).NotEmpty().When(x => x.Description != null);
        RuleFor(x => x.City).MaximumLength(200).NotEmpty().When(x => x.City != null);
    }
}