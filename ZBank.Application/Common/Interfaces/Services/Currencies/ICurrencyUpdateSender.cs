using ZBank.Domain.CurrencyAggregate;

namespace ZBank.Application.Common.Interfaces.Services.Currencies;

public interface ICurrencyUpdateSender
{
    Task SendUpdateAsync(IEnumerable<Currency> currencies);
}