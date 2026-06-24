using BandR.DTOs.Location;
using BandR.Entities;

namespace BandR.Extensions;

public static class LocationExtensions
{
    public static LocationDto ToDto(this Location location) => new LocationDto(location.City);
    public static LocationDto ToEn(this Location location) => new LocationDto(location.City);
}