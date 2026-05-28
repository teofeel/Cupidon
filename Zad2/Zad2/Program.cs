using Zad2.Backgrounds;
using Zad2.DataRepositories;
using Zad2.DataRepositories.Interfaces;
using Zad2.Hubs;
using Zad2.Services;
using Zad2.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ICupidService, CupidService>();

builder.Services.AddHostedService<CupidBgService>();

builder.Services.AddSignalR();

var app = builder.Build();

app.UseRouting();

app.MapHub<CupidHub>("/cupid");

app.Run();
