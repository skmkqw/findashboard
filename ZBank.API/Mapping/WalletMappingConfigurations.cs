using Mapster;
using ZBank.Application.Wallets.Commands.CreateWallet;
using ZBank.Contracts.Wallets.CreateWallet;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate;

namespace ZBank.API.Mapping;

public class WalletMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateWalletRequest request, Guid? ownerId), CreateWalletCommand>()
            .Map(dest => dest.OwnerId, src => UserId.Create(src.ownerId!.Value))
            .Map(dest => dest.ProfileId, src => ProfileId.Create(src.request.ProfileId))
            .Map(dest => dest, src => src.request);

        config.NewConfig<Wallet, CreateWalletResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.ProfileId, src => src.ProfileId.Value);
    }
}