using Mapster;
using ZBank.Contracts.Currencies;
using ZBank.Domain.CurrencyAggregate;

namespace ZBank.API.Mapping;

public class CurrencyMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Currency, GetCurrencyResponse>()
            .Map(dest => dest.Symbol, src => src.Id.Value);
        
        config.NewConfig<List<Currency>, GetCurrencyUpdatesResponse>()
            .Map(dest => dest.Currencies, src => src.Adapt<List<GetCurrencyResponse>>());
    }
}