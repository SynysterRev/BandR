using Microsoft.AspNetCore.Mvc;

namespace BandR.Exceptions;

public sealed class AccountNotFoundException(Guid id)
    : ApplicationException($"Account with id {id} not found"), IProblemConvertible
{
    public void ToProblemDetails(in ProblemDetails inProblemDetails)
    {
        inProblemDetails.Type = "Account/Get/NotFound";
        inProblemDetails.Status = StatusCodes.Status404NotFound;
    }
}
