using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using People.DTOs;
using People.Listeners.Results;

namespace People.Listeners
{
    public class CupidListener
    {
        private readonly HubConnection _connection;
        public HubConnection connection => _connection;

        private readonly CupidListenerResults _listenerResults;

        public bool HasPendingLetter { get; private set; } = false;


        public CupidListener(string url)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            _listenerResults = new CupidListenerResults();

            _listenerResults.OnLetterReceived = () => HasPendingLetter = true;
            _listenerResults.OnConfirmationReceived = () => HasPendingLetter = false;

            this.RegisterListeners();
        }

        public async Task StartAsync()
        {
            await this._connection.StartAsync();
        }

        public void RegisterListeners()
        {
            this._connection.On<bool, string>("RegistrationResult", this._listenerResults.OnRegistrationResult);
            this._connection.On<string>("ConfirmationResult", this._listenerResults.OnConfirmationResult);
            this._connection.On<bool, string>("BlockedUser", this._listenerResults.OnBlockedUserResult);
            this._connection.On<LetterDTO>("ReceiveLetter", this._listenerResults.OnRecieveLetter);
        }
    }
}
