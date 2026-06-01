// See https://aka.ms/new-console-template for more information
using People.Listeners;
using People.Views;

CupidListener listener = new CupidListener("https://localhost:7204/cupid");

try
{
    await listener.StartAsync();
    Console.WriteLine("[Status]: Server Started");
}
catch (Exception ex)
{
    // This will print the actual technical reason (e.g., 404, 401, Connection Refused)
    Console.WriteLine($"[FATAL ERROR]: {ex.Message}");
    if (ex.InnerException != null)
        Console.WriteLine($"[INNER EXCEPTION]: {ex.InnerException.Message}");

    Console.WriteLine("\nPress any key to close the window...");
    Console.ReadKey(); // Window will not close automatically
    return;
}

CLI cli = new CLI(listener);

await cli.Start();

