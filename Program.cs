using System;
using System.Text;
using System.Security.Cryptography;


namespace ConsoleApp1
{
    class Program
    {
        public static void PrintByteArray(byte[] array, string arg)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write($"{array[i]:X2}");
                if ((i % 4) == 3 && arg == "key") Console.Write(" ");
            }
            Console.WriteLine();
        }

        public static byte[] CreateHMACKey()
        {
            var _rng = RandomNumberGenerator.Create();
            byte[] byteArray = new byte[16];
            _rng.GetBytes(byteArray);
            return byteArray;
        }

        public static byte[] CreateHMAC(string arg, byte[] array)
        {
            Encoding enc = Encoding.GetEncoding("ASCII");
            HMACSHA256 hm = new HMACSHA256(array);
            byte[] result = hm.ComputeHash(enc.GetBytes(arg));
            return result;
        }

        public static bool CheckArrWinRange(string[] args, int pc_choice)
        {
            if (args.Length / 2 + pc_choice <= args.Length - 1) return true;
            else return false;
        }

        public static void CheckingResults(string[] args, int pc_choice, int players_choice, bool array_sides)
        {
            int numplayerschoice = 0;
            for (int i = 0; i <= args.Length - 1; i++)
                if (players_choice == i) { numplayerschoice = i; break; }

            if (pc_choice == numplayerschoice) { Console.WriteLine("Draw" , numplayerschoice); return; }

            if (array_sides == true)
            {
            if (numplayerschoice > pc_choice && numplayerschoice <= pc_choice + args.Length / 2) { Console.WriteLine("PC win!"); return; }
            else { Console.WriteLine("Plaer win!"); return; }
            } else
            {
            if (numplayerschoice < pc_choice && numplayerschoice >= pc_choice - args.Length / 2) { Console.WriteLine("Plaer win!"); return; }
            else { Console.WriteLine("PC win!"); return; }
            }
        }
        static void Main(string[] args)
        { 
            if (args.Length < 3)
            {
                Console.WriteLine($"You have not entered enough parameters: {args.Length}.");
                return;
            }
            
            foreach (string arg in args)
            {
                for (int i = Array.IndexOf(args, arg) + 1; i <= args.Length - 1; i++)
                {
                    if (arg == args[i])
                    {
                        Console.WriteLine($"You have entered duplicate arguments. Index of duplicate argument {arg}. Try again...");
                        return;
                    }

                }

            }

            if (args.Length % 2 == 0)
            {
                Console.WriteLine("You have entered an even number of arguments.");
                return;   
            }

            bool steel_play = true;

            while (steel_play)
            {
                Random rnd = new Random();
                int pc_choice = rnd.Next(0, args.Length);
                byte[] key = CreateHMACKey();
                PrintByteArray(CreateHMAC(args[pc_choice], key), "");

                Console.WriteLine("Available moves:");
                for (int i = 0; i <= args.Length - 1; i++)
                {
                    Console.WriteLine($"{i + 1} - {args[i]}");
                }

                Console.WriteLine("To exit type \"EXIT\".");

                bool incorrect_input = true;
                int players_choice = 0;
                while (incorrect_input)
                {
                    Console.Write("Enter your move: ");
                    string input_str  = Console.ReadLine();
                    if (input_str == "EXIT") return;
                    else
                    {
                        try
                        {
                            players_choice = int.Parse(input_str);
                        }
                        catch(Exception)
                        {
                            Console.WriteLine("!!!Invalid input. Try again!!!");
                        }
                    }
                    if (players_choice > 0 && players_choice <= args.Length) { incorrect_input = false; Console.WriteLine($"Your move: {args[--players_choice]}"); }


                }

                Console.WriteLine($"Computer move: {args[pc_choice]}");

                bool array_sides = CheckArrWinRange(args, pc_choice);

                CheckingResults(args, pc_choice, players_choice, array_sides);

                PrintByteArray(key, "key");
                Console.WriteLine("Type any button");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
