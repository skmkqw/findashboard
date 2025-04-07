namespace ZBank.API.Hubs.Common;

public interface IHubClient
{
    Task ReceiveMessage(string message);
}