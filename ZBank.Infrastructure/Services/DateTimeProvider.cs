using ZBank.Application.Common.Interfaces.Services;

namespace ZBank.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}