using BandR.Entities;
using BandR.Data;
using BandR.Exceptions;
using BandR.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BandR.Services;

public class AccountService(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService,
    ApplicationDbContext dbContext) : IAccountService
{
    public async Task DeactivateAccountAsync(Guid appUserId, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(appUserId.ToString())
                   ?? throw new AccountNotFoundException(appUserId);

        if (user.DeactivatedAt is not null)
            return;

        user.DeactivatedAt = DateTime.UtcNow;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException("Unable to deactivate the account.");

        var announcements = await dbContext.Announcements
            .Where(announcement => announcement.Musician.AppUserId == appUserId && announcement.IsActive)
            .ToListAsync(ct);
        foreach (var announcement in announcements)
            announcement.IsActive = false;

        var conversations = await dbContext.Conversations
            .Where(conversation => conversation.IsActive)
            .Where(conversation => conversation.MusicianConversations
                .Any(participant => participant.Musician.AppUserId == appUserId))
            .ToListAsync(ct);
        foreach (var conversation in conversations)
            conversation.IsActive = false;

        await dbContext.SaveChangesAsync(ct);

        await tokenService.RemoveTokensForUserAsync(appUserId, ct);
    }
}
