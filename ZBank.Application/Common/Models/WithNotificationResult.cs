using ZBank.Domain.Common.Models;

namespace ZBank.Application.Common.Models;

public class WithNotificationResult<T1, T2> where T1 : notnull where T2 : Notification
{
    public T1 Result { get; }

    public T2 Notification { get; }

    public WithNotificationResult(T1 result, T2 notification)
    {
        Result = result;
        Notification = notification;
    }
}