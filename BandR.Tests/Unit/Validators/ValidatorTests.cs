using BandR.DTOs.Musicians;
using BandR.Validators.Musicians;

namespace BandR.Tests.Unit.Validators;

public class CreateMusicianDtoValidatorTests
{
    private readonly CreateMusicianDtoValidator _validator = new();

    [Fact]
    public void ShouldPass_WhenValid()
    {
        var dto = new CreateMusicianDto("TestUser", "Montpellier", [], [], [], null);
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldFail_WhenUsernameEmpty()
    {
        var dto = new CreateMusicianDto("", "Montpellier", [], [], [], null);
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Username");
    }

    [Fact]
    public void ShouldFail_WhenUsernameTooLong()
    {
        var dto = new CreateMusicianDto(new string('a', 129), "Montpellier", [], [], [], null);
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Username");
    }

    [Fact]
    public void ShouldFail_WhenCityEmpty()
    {
        var dto = new CreateMusicianDto("TestUser", "", [], [], [], null);
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "City");
    }

    [Fact]
    public void ShouldFail_WhenCityTooLong()
    {
        var dto = new CreateMusicianDto("TestUser", new string('a', 201), [], [], [], null);
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "City");
    }

    [Fact]
    public void ShouldFail_WhenBioTooLong()
    {
        var dto = new CreateMusicianDto("TestUser", "Montpellier", [], [], [], new string('a', 1025));
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Bio");
    }

    [Fact]
    public void ShouldPass_WhenBioIsNull()
    {
        var dto = new CreateMusicianDto("TestUser", "Montpellier", [], [], [], null);
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldPass_WhenBioIsMaxLength()
    {
        var dto = new CreateMusicianDto("TestUser", "Montpellier", [], [], [], new string('a', 1024));
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }
}

public class UpdateMusicianDtoValidatorTests
{
    private readonly UpdateMusicianDtoValidator _validator = new();

    [Fact]
    public void ShouldPass_WhenAllNull()
    {
        var dto = new UpdateMusicianDto(null, null, null, null, null, null);
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldPass_WhenAllValid()
    {
        var dto = new UpdateMusicianDto("NewUsername", "Paris", "New bio", [], [], []);
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldFail_WhenUsernameTooLong()
    {
        var dto = new UpdateMusicianDto(new string('a', 129), null, null, null, null, null);
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Username");
    }

    [Fact]
    public void ShouldFail_WhenCityTooLong()
    {
        var dto = new UpdateMusicianDto(null, new string('a', 201), null, null, null, null);
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "City");
    }

    [Fact]
    public void ShouldFail_WhenBioTooLong()
    {
        var dto = new UpdateMusicianDto(null, null, new string('a', 1025), null, null, null);
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Bio");
    }

    [Fact]
    public void ShouldPass_WhenUsernameIsExactlyMaxLength()
    {
        var dto = new UpdateMusicianDto(new string('a', 128), null, null, null, null, null);
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldPass_WhenBioIsExactlyMaxLength()
    {
        var dto = new UpdateMusicianDto(null, null, new string('a', 1024), null, null, null);
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }
}