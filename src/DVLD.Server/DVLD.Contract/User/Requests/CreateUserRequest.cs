namespace DVLD.Contract.User.Requests;

public sealed record CreateUserRequest(
    Guid PersonId,
    string Username,
    string Password,
    int RoleId
    );