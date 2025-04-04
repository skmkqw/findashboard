using MapsterMapper;
using Microsoft.AspNetCore.SignalR;
using ZBank.API.Hubs;
using ZBank.Application.Common.Interfaces.Services.Currencies;
using ZBank.Contracts.Currencies;
using ZBank.Domain.CurrencyAggregate;

namespace ZBank.API.Services.Currencies;

public class CurrencyUpdateSignalRSender<T> : ICurrencyUpdateSender where T : Hub<IPriceClient>
{
    private readonly IMapper _mapper;
    
    private readonly IHubContext<T, IPriceClient> _currencyHubContext;

    public CurrencyUpdateSignalRSender(IHubContext<T, IPriceClient> hubContext, IMapper mapper)
    {
        _currencyHubContext = hubContext;
        _mapper = mapper;
    }

    public async Task SendUpdateAsync(IEnumerable<Currency> currencies, string groupName)
    {
        var result = _mapper.Map<GetCurrencyUpdatesResponse>(currencies);
        
        await _currencyHubContext.Clients.Group($"team-{groupName}")
            .ReceiveCurrencyUpdates(result);
    }
}