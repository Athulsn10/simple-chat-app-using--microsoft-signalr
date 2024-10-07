using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Cors;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173")  // client side url
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors();

app.MapHub<ChatHub>("/chat");

app.Run();

public class ChatHub : Hub
{
    public async Task SendMessage(Message message)
    {
        if (message != null)
        {
            await Clients.All.SendAsync("message", message);
        }
    }
}


public class Message
{
    public string Username { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}