using People.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.Listeners.Results
{
    public class CupidListenerResults
    {
        public Action OnLetterReceived { get; set; }
        public Action OnConfirmationReceived { get; set; }
        public Action<bool> OnRegistrationCompleted { get; set; }

        public void OnRegistrationResult(bool success, string message)
        {
            Console.WriteLine($"\n[Cupid]: {message}");
            OnRegistrationCompleted?.Invoke(success);
        }

        public void OnConfirmationResult(string message)
        {
            Console.WriteLine($"\n[Cupid]: {message}");
            OnConfirmationReceived?.Invoke();
        }

        public void OnBlockedUserResult(bool success, string message)
        {
            Console.WriteLine($"\n[System]: {message}");
        }

        public void OnRecieveLetter(LetterDTO dto)
        {
            OnLetterReceived?.Invoke();

            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine("YOU HAVE RECEIVED A LETTER");
            Console.WriteLine($"From user: {dto.FromUsername}");
            Console.WriteLine($"City: {dto.City}");
            Console.WriteLine($"Age: {dto.Age}");
            Console.WriteLine($"Message: \"{dto.Message}\"");

            if (!string.IsNullOrEmpty(dto.PhoneNumber))
                Console.WriteLine($"Phone Number: {dto.PhoneNumber}");


            Console.WriteLine("\n--------------------------------------------------");

            Console.Write("Press ENTER to confirm");
        }
    }
}
