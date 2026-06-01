using Cupid.Services.Interfaces;
using Cupid.Utils;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace Cupid.Hubs
{
    public class CupidHub : Hub
    {
        private readonly IUserService _userService;
        private readonly CupidConnectionTracker _connectionTracker;

        public CupidHub(IUserService userService, CupidConnectionTracker tracker)
        {
            this._userService = userService;
            this._connectionTracker = tracker;
        }

        public async Task InitSinglePerson(string username, string city, int age, string phoneNumber)
        {
            try
            {
                bool isRegistered = this._userService.RegisterUser(username, city, age, phoneNumber);

                if (!isRegistered)
                {
                    await Clients.Caller.SendAsync("RegistrationResult", false, $"Couldnt register user");
                    return;
                }

                this._connectionTracker.Add(this.Context.ConnectionId, username);

                await Groups.AddToGroupAsync(Context.ConnectionId, username);
                await Clients.Caller.SendAsync("RegistrationResult", true, $"User registered");


            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("RegistrationResult", false, $"Couldnt validate: {ex.Message}");
            }

        }

        //public async Task ConfirmReceipt(string username)
        //{
        //    try
        //    {
        //        this._userService.SetUserBusy(username, false);
        //        await Clients.Caller.SendAsync("ConfirmationResult", $"You can now receive new letters");
        //    }
        //    catch (Exception ex)
        //    {
        //        await Clients.Caller.SendAsync("ConfirmationResult", $"Error: {ex.Message}");
        //    }
        //}

        //public async Task BlockUser(string current, string username)
        //{
        //    try
        //    {
        //        this._userService.BlockUser(current, username);
        //        await Clients.Caller.SendAsync("BlockedUser", true, $"BLocked user: {username}");
        //    }
        //    catch (Exception ex)
        //    {
        //        await Clients.Caller.SendAsync("BlockedUser", false, $"Couldnt block: {ex.Message}");
        //    }

        //}

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            if (this._connectionTracker.TryRemove(this.Context.ConnectionId, out string? username))
            {
                try
                {
                    this._userService.SetUserBusy(username, false);
                }
                catch (Exception busyEx)
                {
                    Debug.WriteLine($"Could not clear busy for {username}: {busyEx.Message}");
                }

                try
                {
                    this._userService.RemoveUser(username);
                }
                catch (Exception removeEx)
                {
                    Debug.WriteLine($"Error removing user {username}: {removeEx.Message}");
                }
            }

            await base.OnDisconnectedAsync(ex);

        }
    }
}