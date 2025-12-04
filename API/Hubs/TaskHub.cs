using Microsoft.AspNetCore.SignalR;
public class TaskHub : Hub
{
    // Called by server when a task is updated
    public async Task SendTaskUpdate(string message)
    {
        await Clients.All.SendAsync("ReceiveTaskUpdate", message);
    }
}
