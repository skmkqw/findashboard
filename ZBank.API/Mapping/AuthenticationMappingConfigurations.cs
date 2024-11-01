using Mapster;
using ZBank.Application.Authentication.Commands.Register;
using ZBank.Application.Authentication.Common;
using ZBank.Application.Authentication.Queries.Login;
using ZBank.Contracts.Authentication;

namespace ZBank.API.Mapping;

public class AuthenticationMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<LoginRequest, LoginQuery>();
        
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest, src => src.User);
    }
}