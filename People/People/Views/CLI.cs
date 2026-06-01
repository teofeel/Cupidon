using Microsoft.AspNetCore.SignalR.Client;
using People.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace People.Views
{
    public class CLI
    {
        private readonly CupidListener _listener;
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7204/api/") };
        private string _username = "";

        public CLI(CupidListener listener)
        {
            this._listener = listener;
        }

        public async Task Start()
        {
            Console.WriteLine("--- CLI ---");
            while (true)
            {
                var data = Register();

                await _listener.connection.InvokeAsync("InitSinglePerson", data.username, data.grad, data.godine, data.telefon);

                _listener.WaitForRegistration();

                if (_listener.RegistrationSucceeded)
                {
                    _username = data.username;
                    break;
                }

                Console.WriteLine("[System]: Username taken, try another one.");
                _listener.ResetRegistration();
            }

            await Run();
        }


        public (string username, string grad, int godine, string telefon) Register()
        {
            string username = this.ReadNonEmptyString("Username >> ");
            string grad = this.ReadNonEmptyString("City >> ");
            int godine = this.ReadValidAge("Age >> ");
            string telefon = this.ReadNonEmptyString("Phone number >> ");

            return (username, grad, godine, telefon);
        }


        private async Task Run()
        {
            Console.WriteLine($"You have entered Cupidon");

            while (true)
            {
                string input = Console.ReadLine()?.Trim();

                if(input == "--help")
                {
                    this.PrintCommands();
                }
                else if (string.IsNullOrEmpty(input))
                {
                    var content = new StringContent($"\"{_username}\"", Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("user/unbusy", content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"[System]: You can now recieve new letter");
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"[Error]: Server returned: {response.StatusCode} - {error}");
                    }

                    //if (_listener.HasPendingLetter)
                    //{
                    //    await _listener.connection.InvokeAsync("ConfirmReceipt", _username);
                    //}
                    //else
                    //{
                    //    Console.WriteLine("[System]: No new letters");
                    //}
                }
                else if (input.StartsWith("/block "))
                {
                    string userToBlock = input.Replace("/block ", "").Trim();

                    var content = JsonContent.Create(new { Current = _username, Target = userToBlock });
                    var response = await _httpClient.PostAsync("user/block", content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"[System]: Successfully blocked {userToBlock}");
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"[Error]: Could not block user. Server returned: {response.StatusCode} - {error}");
                    }
                }
                else if (input.StartsWith("/exit"))
                {
                    Console.WriteLine("Exiting.....");
                    await _listener.connection.StopAsync();
                    Environment.Exit(0);
                }
            }
        }


        private void PrintCommands()
        {
            Console.WriteLine("/block <username> - used to block user");
            Console.WriteLine("/exit - used to exit app");
            Console.WriteLine("<enter> - after letter recieved to activate your status");
        }
        private string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                    return input;

                Console.WriteLine($"[Error]: Field cannot be empty");
            }
        }

        private int ReadValidAge(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine($"[Error]: Age cannot be empty");
                    continue;

                }
                int age;
                try
                {
                    age = int.Parse(input);
                }catch(Exception ex)
                {
                    Console.WriteLine("[Error]: Age must be a number");
                    continue;
                }

                if (age > 0)
                    return age;

                Console.WriteLine($"[Error]: Age must be bigger than 0");
            }
        }
    }
}
