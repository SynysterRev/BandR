namespace BandR.Entities;

// Entity
public class RefreshToken : BaseEntity
{
    public Guid AppUserId { get; set; }
    public ApplicationUser AppUser { get; set; } = null!;
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}