using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CraftRiseChecker
{
    class Program
    {
        static string LoginAPI = "https://www.craftrise.com.tr/oyuncu/";
        static string paths = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string startup = Path.GetDirectoryName(paths);
        static string path = "";
        static string checkedpath = startup + "\\Valid.txt";

        static void Main(string[] args)
        {
            Console.Title = "CraftRise OG Checker";
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(@"
  _____   ______      _______ _     _ _______ _______ _     _ _______  ______
 |     | |  ____      |       |_____| |______ |       |____/  |______ |_____/
 |_____| |_____|      |_____  |     | |______ |_____  |    \_ |______ |    \_
                                                                             
");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Drag and drop your username file: ");
            path = Console.ReadLine();
            if (path.StartsWith("\""))
            {
                path = path.Substring(1, path.Length - 1);
            }

            if (path.EndsWith("\""))
            {
                path = path.Substring(0, path.Length - 1);
            }

            if (!File.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("This file does not exist!.");
                Console.ResetColor();
            }
            else
            {
                int acccount = File.ReadAllLines(path).Length;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("[" + acccount + "]" + " usernames loaded.");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Started.\r\n");
                Task.Run(Check);
                Console.ReadKey();
            }
        }

        public static async Task Check()
        {
            foreach (string username in File.ReadAllLines(path))
            {
                HttpClient client = new HttpClient();
                var pst = await client.GetAsync(LoginAPI + username);
                var stringContent = await pst.Content.ReadAsStringAsync();
                if (stringContent.Contains("get-head.php"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Taken: " + username);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Aviable: " + username);
                    File.AppendAllText(checkedpath, username + "\r\n");
                }
            }
        }
    }
}