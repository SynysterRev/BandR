using Microsoft.AspNetCore.Mvc;

namespace BandR.Exceptions;

public abstract class MusicianException(string? message = null, Exception? innerException = null)
    : ApplicationException(message, innerException), IProblemConvertible
{
    public abstract void ToProblemDetails(in ProblemDetails inProblemDetails);

    public sealed class MusicianNotFoundException(Guid id, Exception? innerException = null)
        : MusicianException($"Musician with id {id} not found", innerException)
    {
        public override void ToProblemDetails(in ProblemDetails inProblemDetails)
        {
            inProblemDetails.Type = "Musician/Get/NotFound";
            inProblemDetails.Status = StatusCodes.Status404NotFound;
        }
    }

    public sealed class MusicianForbiddenException(Guid id, Exception? innerException = null)
        : MusicianException($"Access to musician {id} is forbidden", innerException)
    {
        public override void ToProblemDetails(in ProblemDetails inProblemDetails)
        {
            inProblemDetails.Type = "Musician/Update/Forbidden";
            inProblemDetails.Status = StatusCodes.Status403Forbidden;
        }
    }
}