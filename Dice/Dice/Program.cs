using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dice
{
    class Program
    {
        private static string PlayerName;
        private static int Balance = 10;
        private static Random rnd = new Random();

        static void Main(string[] args)
        {
            Welcome();
            Game();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Any key to exit: ");
            Console.ReadKey();
        }

        private static void Welcome()
        {
            Console.WriteLine("Welcome to the Dice Game!\r\n");
            Console.WriteLine("What is your name?\r\n");
            PlayerName = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\r\nHello, " + PlayerName + "! Roll your dice!\r\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Win = x4 roll!\r\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Your balance is " + Balance);
        }

        private static int RollDice()
        {
            int result = rnd.Next(1, 7);
            return result;
        }

        private static void Game()
        {
            do
            {
                Console.Write("\r\nChoose you number 1-6 and press Enter to roll: ");
                int number = 0;
                int rndNumber;

                try
                {
                    number = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You are entered not a number!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                if (number >= 1 && number <= 6)
                {
                    rndNumber = RollDice();

                    if (number == rndNumber)
                    {
                        Win();
                    }
                    else
                    {
                        Loss();
                    }
                }
            } while (Balance > 0 && Balance < 20);

            if (Balance >= 20)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\r\nCongratulations, " + PlayerName + "! You are a winner!\r\n");
            }

            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\r\nSorry, " + PlayerName + "! You lose!\r\n");
            }
        }

        private static void Loss()
        {
            Balance--;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Loss!");
            Console.WriteLine("Your balance is " + Balance);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void Win()
        {
            Balance = Balance + 4;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Win!\r\n");
            Console.WriteLine("Your balance is " + Balance);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
