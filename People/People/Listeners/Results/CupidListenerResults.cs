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
        public void OnRegistrationResult(bool success, string message)
        {
            Console.WriteLine($"\n[Cupid]: {message}");

            if (!success) { 
                Console.WriteLine("Please try again later");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public void OnConfirmationResult(string message)
        {
            Console.WriteLine($"\n[Cupid]: {message}");
        }

        public void OnBlockedUserResult(bool success, string message)
        {
            Console.WriteLine($"\n[System]: {message}");
        }

        public void OnRecieveLetter(LetterDTO dto)
        {
            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine("STIGLO VAM JE PISMO");
            Console.WriteLine($"Od korisnika: {dto.FromUsername}");
            Console.WriteLine($"Grad: {dto.City}");
            Console.WriteLine($"Godine: {dto.Age}");
            Console.WriteLine($"Poruka: \"{dto.Message}\"");

            if (!string.IsNullOrEmpty(dto.PhoneNumber))
                Console.WriteLine($"Broj telefona: {dto.PhoneNumber}");
            else
                Console.WriteLine("Broj telefona je sakriven (Korisnik nije zainteresovan).");




            Console.WriteLine("\n--------------------------------------------------");
        }
    }
}
