using Microsoft.AspNetCore.SignalR.Client;
using People.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Views
{
    public class CLI
    {
        private readonly CupidListener _listener;
        private string _username = "";

        public CLI(CupidListener listener)
        {
            this._listener = listener;
        }

        public async Task Start()
        {
            Console.WriteLine("--- CLI ---");
            var data = Register();

            await _listener.connection.InvokeAsync("InitSinglePerson", data.username, data.grad, data.godine, data.telefon);
            _username = data.username;

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
                Console.Write(">> ");
                string input = Console.ReadLine()?.Trim();

                if(input == "--help")
                {
                    this.PrintCommands();
                }
                else if (string.IsNullOrEmpty(input))
                {
                    await _listener.connection.InvokeAsync("ConfirmReceipt", _username);
                }
                else if (input.StartsWith("/block "))
                {
                    string userToBlock = input.Replace("/block ", "").Trim();
                    await _listener.connection.InvokeAsync("BlockUser", _username, userToBlock);
                }
            }
        }


        private void PrintCommands()
        {
            Console.WriteLine("/block <username> - used to block user");
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
                int age = int.Parse(input);

                if (age > 0)
                    return age;

                Console.WriteLine($"[Error]: Age must be bigger than 0");
            }
        }
    }
}
