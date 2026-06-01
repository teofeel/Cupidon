using Cupid.Backgrounds;
using Cupid.DataRepositories;
using Cupid.DataRepositories.Interfaces;
using Cupid.Hubs;
using Cupid.Services;
using Cupid.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ICupidService, CupidService>();
builder.Services.AddSingleton<Cupid.Utils.CupidConnectionTracker>();

builder.Services.AddHostedService<CupidBgService>();

builder.Services.AddSignalR();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.MapHub<CupidHub>("/cupid");
app.MapControllers();
app.Run();
