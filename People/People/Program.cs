// See https://aka.ms/new-console-template for more information
using People.Listeners;
using People.Views;

CupidListener listener = new CupidListener("http://localhost:5175/cupid");

try
{
    await listener.StartAsync();
    Console.WriteLine("[Status]: Server Started");
}catch(Exception ex)
{
    Console.WriteLine($"[Error]: {ex.Message}");
    return;
}

CLI cli = new CLI(listener);

await cli.Start();

