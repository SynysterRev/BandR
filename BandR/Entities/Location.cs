namespace BandR.Entities;

public class Location : BaseEntity
{
    public string City { get; set; } =  string.Empty;
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
