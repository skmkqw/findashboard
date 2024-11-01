using ErrorOr;
using MediatR;
using ZBank.Application.Authentication.Common;

namespace ZBank.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;