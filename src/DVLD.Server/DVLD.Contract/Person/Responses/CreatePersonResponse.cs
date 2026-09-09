using Microsoft.AspNetCore.Mvc;

namespace DVLD.Contract.Person.Responses;

public sealed record CreatePersonResponse(Guid PersonId,string Message);