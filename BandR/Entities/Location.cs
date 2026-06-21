namespace BandR.Entities;

public class Location : BaseEntity
{
    public string City { get; set; } =  string.Empty;
    public string PostalCode { get; set; } = String.Empty;
    public string Country { get; set; } = String.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}