using Microsoft.AspNetCore.Mvc;

namespace BandR.Exceptions;

public interface IProblemConvertible
{
    void ToProblemDetails(in ProblemDetails details);
}