using MapsterMapper;
using Microsoft.AspNetCore.SignalR;
using ZBank.API.Hubs;
using ZBank.Application.Common.Interfaces.Services.Wallets;
using ZBank.Contracts.Wallets;
using ZBank.Domain.WalletAggregate;

namespace ZBank.API.Services.Wallets;

public class WalletUpdateSignalRSender<T> : IWalletUpdateSender where T : Hub<IPriceClient>
{
    private readonly IMapper _mapper;

    private readonly IHubContext<T, IPriceClient> _priceHubContext;

    public WalletUpdateSignalRSender(IHubContext<T, IPriceClient> hubContext, IMapper mapper)
    {
        _priceHubContext = hubContext;
        _mapper = mapper;
    }

    public async Task SendUpdateAsync(List<Wallet> wallets, string groupName)
    {
        var result = _mapper.Map<GetWalletUpdatesResponse>(wallets);

        await _priceHubContext.Clients.Group($"team-{groupName}")
            .ReceiveWalletUpdates(result);
    }
}