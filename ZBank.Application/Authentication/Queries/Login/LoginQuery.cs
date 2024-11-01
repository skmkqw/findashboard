using ErrorOr;
using MediatR;
using ZBank.Application.Authentication.Common;

namespace ZBank.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<ErrorOr<AuthenticationResult>>;