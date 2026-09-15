namespace DVLD.Contract.Authentication;

public sealed record LoginRequest(
    string Username,
    string Password
    );