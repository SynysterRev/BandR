using BandR.DTOs.Announcements;
using BandR.Entities;
using BandR.Validators.Announcements;

namespace BandR.Tests.Unit.Validators;

public class CreateAnnouncementValidatorTests
{
    private readonly CreateAnnouncementDtoValidator _validator = new();

    [Fact]
    public void ShouldPass_WhenValid()
    {
        var announcement =
            new CreateAnnouncementDto(
                "Test",
                "Description",
                "City",
                AnnouncementType.LookingForBand,
                [],
                [],
                []
            );
        var result = _validator.Validate(announcement);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldFail_WhenTitleEmpty()
    {
        var announcement = new CreateAnnouncementDto(
            "",
            "Description",
            "City",
            AnnouncementType.LookingForBand,
            [],
            [],
            []
        );
        var result = _validator.Validate(announcement);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ShouldFail_WhenTitleTooLong()
    {
        var announcement = new CreateAnnouncementDto(
            new string('e', 130),
            "Description",
            "City",
            AnnouncementType.LookingForBand,
            [],
            [],
            []
        );
        var result = _validator.Validate(announcement);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ShouldFail_WhenDescriptionEmpty()
    {
        var announcement = new CreateAnnouncementDto(
            "Title",
            "",
            "City",
            AnnouncementType.LookingForBand,
            [],
            [],
            []
        );
        var result = _validator.Validate(announcement);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ShouldFail_WhenDescriptionTooLong()
    {
        var announcement = new CreateAnnouncementDto(
            "Title",
            new string('e', 501),
            "City",
            AnnouncementType.LookingForBand,
            [],
            [],
            []
        );
        var result = _validator.Validate(announcement);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ShouldFail_WhenCityEmpty()
    {
        var announcement = new CreateAnnouncementDto(
            "Title",
            "Description",
            "",
            AnnouncementType.LookingForBand,
            [],
            [],
            []
        );
        var result = _validator.Validate(announcement);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void ShouldFail_WhenCityTooLong()
    {
        var announcement = new CreateAnnouncementDto(
            "Title",
            "Description",
            new string('e', 201),
            AnnouncementType.LookingForBand,
            [],
            [],
            []
        );
        var result = _validator.Validate(announcement);
        Assert.False(result.IsValid);
    }
}

public class UpdateAnnouncementValidatorTests
{
    private readonly UpdateAnnouncementDtoValidator _validator = new();

    [Fact]
    public void ShouldPass_WhenAllFieldsNull()
    {
        var announcement = new UpdateAnnouncementDto(
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        var result = _validator.Validate(announcement);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldPass_WhenValid()
    {
        var announcement = new UpdateAnnouncementDto(
            "Title",
            "Description",
            "City",
            AnnouncementType.LookingForBand,
            [],
            [],
            [],
            null
        );

        var result = _validator.Validate(announcement);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldFail_WhenTitleEmpty()
    {
        var announcement = new UpdateAnnouncementDto(
            "",
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        Assert.False(_validator.Validate(announcement).IsValid);
    }

    [Fact]
    public void ShouldFail_WhenTitleTooLong()
    {
        var announcement = new UpdateAnnouncementDto(
            new string('e', 130),
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        Assert.False(_validator.Validate(announcement).IsValid);
    }

    [Fact]
    public void ShouldFail_WhenDescriptionEmpty()
    {
        var announcement = new UpdateAnnouncementDto(
            null,
            "",
            null,
            null,
            null,
            null,
            null,
            null
        );

        Assert.False(_validator.Validate(announcement).IsValid);
    }

    [Fact]
    public void ShouldFail_WhenDescriptionTooLong()
    {
        var announcement = new UpdateAnnouncementDto(
            null,
            new string('e', 501),
            null,
            null,
            null,
            null,
            null,
            null
        );

        Assert.False(_validator.Validate(announcement).IsValid);
    }

    [Fact]
    public void ShouldFail_WhenCityEmpty()
    {
        var announcement = new UpdateAnnouncementDto(
            null,
            null,
            "",
            null,
            null,
            null,
            null,
            null
        );

        Assert.False(_validator.Validate(announcement).IsValid);
    }

    [Fact]
    public void ShouldFail_WhenCityTooLong()
    {
        var announcement = new UpdateAnnouncementDto(
            null,
            null,
            new string('e', 201),
            null,
            null,
            null,
            null,
            null
        );

        Assert.False(_validator.Validate(announcement).IsValid);
    }
    
    [Fact]
    public void ShouldPass_WhenIsActiveIsNull()
    {
        var announcement = new UpdateAnnouncementDto(
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        var result = _validator.Validate(announcement);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldPass_WhenIsActiveHasValidValue(bool isActive)
    {
        var announcement = new UpdateAnnouncementDto(
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            isActive
        );

        var result = _validator.Validate(announcement);

        Assert.True(result.IsValid);
    }
}