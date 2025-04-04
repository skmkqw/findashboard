using Mapster;
using ZBank.Application.Wallets.Commands.AddBalance;
using ZBank.Application.Wallets.Commands.CreateWallet;
using ZBank.Contracts.Wallets;
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
            .Map(dest => dest.Symbol, src => CurrencyId.Create(src.request.Symbol.ToUpper()))
            .Map(dest => dest.WalletId, src => WalletId.Create(src.request.WalletId))
            .Map(dest => dest.Amount, src => src.request.Amount);

        config.NewConfig<Balance, AddBalanceResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.CurrencySymbol, src => src.CurrencyId.Value);

        config.NewConfig<Balance, GetBalanceResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.CurrencySymbol, src => src.CurrencyId.Value);

        config.NewConfig<Wallet, GetWalletUpdateResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Balances, src => src.Balances.Adapt<List<GetBalanceResponse>>());

        config.NewConfig<List<Wallet>, GetWalletUpdatesResponse>()
            .Map(dest => dest.WalletUpdates, src => src.Adapt<List<GetWalletUpdateResponse>>());
    }
}