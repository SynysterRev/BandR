using Microsoft.AspNetCore.Mvc;

namespace BandR.Exceptions;

public abstract class AnnouncementException(string? message = null, Exception? innerException = null)
    : ApplicationException(message, innerException), IProblemConvertible
{
public abstract void ToProblemDetails(in ProblemDetails inProblemDetails);

public sealed class AnnouncementNotFoundException(Guid id, Exception? innerException = null)
    : AnnouncementException($"Announcement with id {id} not found", innerException)
{
    public override void ToProblemDetails(in ProblemDetails inProblemDetails)
    {
        inProblemDetails.Type = "Announcement/Get/NotFound";
        inProblemDetails.Status = StatusCodes.Status404NotFound;
    }
}

public sealed class AnnouncementForbiddenException(Guid id, Exception? innerException = null)
    : AnnouncementException($"Access to Announcement {id} is forbidden", innerException)
{
    public override void ToProblemDetails(in ProblemDetails inProblemDetails)
    {
        inProblemDetails.Type = "Announcement/Update/Forbidden";
        inProblemDetails.Status = StatusCodes.Status403Forbidden;
    }
}
}
