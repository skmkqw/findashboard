using MapsterMapper;
using Microsoft.AspNetCore.SignalR;
using ZBank.API.Hubs;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Application.Common.Interfaces.Services.Currencies;
using ZBank.Contracts.Currencies;
using ZBank.Domain.CurrencyAggregate;

namespace ZBank.API.Services.Currencies;

public class CurrencyUpdateSignalRSender<T> : ICurrencyUpdateSender where T : Hub<ICurrencyClient>
{
    private readonly IMapper _mapper;
    
    private readonly IUserConnectionManager _connectionManager;

    private readonly IHubContext<T, ICurrencyClient> _currencyHubContext;

    public CurrencyUpdateSignalRSender(IHubContext<T, ICurrencyClient> hubContext,
        IUserConnectionManager connectionManager,
        IMapper mapper)
    {
        _currencyHubContext = hubContext;
        _connectionManager = connectionManager;
        _mapper = mapper;
    }

    public async Task SendUpdateAsync(IEnumerable<Currency> currencies)
    {
        // var connections = _connectionManager.GetConnections(receiverId);
        //
        // if (connections != null)
        // {
        //     foreach (var connectionId in connections)
        //     {
        //
        //     }
        // }

        var result = _mapper.Map<GetCurrencyUpdatesResponse>(currencies);
        
        await _currencyHubContext.Clients.All
            .ReceiveCurrencyUpdates(result);
    }
}