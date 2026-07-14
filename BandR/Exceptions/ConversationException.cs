using Microsoft.AspNetCore.Mvc;

namespace BandR.Exceptions;

public abstract class ConversationException(string? message = null, Exception? innerException = null)
    : ApplicationException(message, innerException), IProblemConvertible
{
    public abstract void ToProblemDetails(in ProblemDetails inProblemDetails);
    
    public sealed class ConversationNotFoundException(Guid id, Exception? innerException = null)
        : ConversationException($"Conversation with id {id} not found", innerException)
    {
        public override void ToProblemDetails(in ProblemDetails inProblemDetails)
        {
            inProblemDetails.Type = "Conversation/Get/NotFound";
            inProblemDetails.Status = StatusCodes.Status404NotFound;
        }
    }

    public sealed class ConversationForbiddenException(Guid id, Exception? innerException = null)
        : ConversationException($"Access to Conversation {id} is forbidden", innerException)
    {
        public override void ToProblemDetails(in ProblemDetails inProblemDetails)
        {
            inProblemDetails.Type = "Conversation/Update/Forbidden";
            inProblemDetails.Status = StatusCodes.Status403Forbidden;
        }
    }
    
    public sealed class ConversationAlreadyExists(Guid announcementId, Exception? innerException = null)
        : ConversationException($"Conversation for announcement {announcementId} already exists", innerException)
    {
        public override void ToProblemDetails(in ProblemDetails inProblemDetails)
        {
            inProblemDetails.Type = "Conversation/Create/AlreadyExists";
            inProblemDetails.Status = StatusCodes.Status409Conflict;
        }
    }
}