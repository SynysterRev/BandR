using BandR.DTOs;
using BandR.Entities;

namespace BandR.Extensions;

public static class InstrumentExtensions
{
    public static InstrumentDto ToDto(this Instrument instrument) => new InstrumentDto(instrument.Name);
}

public static class TagExtensions
{
    public static TagDto ToDto(this Tag tag) => new TagDto(tag.Name);
}

public static class StyleExtensions
{
    public static StyleDto ToDto(this Style style) => new StyleDto(style.Name);
}