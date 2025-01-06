using Mapster;
using ZBank.Application.Wallets.Commands.AddBalance;
using ZBank.Application.Wallets.Commands.CreateWallet;
using ZBank.Contracts.Wallets.AddBalance;
using ZBank.Contracts.Wallets.CreateWallet;
using ZBank.Domain.CurrencyAggregate.ValueObjects;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate;
using ZBank.Domain.WalletAggregate.Entities;
using ZBank.Domain.WalletAggregate.ValueObjects;

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

        config.NewConfig<(AddBalanceRequest request, Guid? ownerId), AddBalanceCommand>()
            .Map(dest => dest.UserId, src => UserId.Create(src.ownerId!.Value))
            .Map(dest => dest.CurrencyId, src => CurrencyId.Create(src.request.CurrencyId))
            .Map(dest => dest.WalletId, src => WalletId.Create(src.request.WalletId))
            .Map(dest => dest.Amount, src => src.request.Amount);

        config.NewConfig<Balance, AddBalanceResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.CurrencyId, src => src.CurrencyId.Value);
    }
}