namespace BandR.Services.Interfaces;

public interface IAccountService
{
    Task DeactivateAccountAsync(Guid appUserId, CancellationToken ct);
}
