using Figgle;
using Jane.Managers;
using Jane.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Jane
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Patrick Jane";
            while (true) 
            {
                Console.Clear();
                Console.WriteLine(FiggleFonts.Standard.Render("Welcome To Jane!"));
                Console.WriteLine("Please Enter Usernames (Seperate usernames with \",\" If you are wish to run multiple usernames)");
                string usernames = Console.ReadLine();
                Parallel.ForEach(usernames.Split(','), new ParallelOptions { MaxDegreeOfParallelism = 4 }, username =>
                {
                    List<ResponseStatus> results = new List<ResponseStatus>();
                    Parallel.ForEach(FileManager.GetConfigs(), new ParallelOptions { MaxDegreeOfParallelism = 4 }, config =>
                    {
                        var ConfigManager = new ConfigManager(config);
                        var account = ConfigManager.Send(username);
                        results.Add(account);

                    });
                    FileManager.SaveResults(results, username);
                });
                Console.WriteLine($"Results Saved Too: {Environment.CurrentDirectory}\\Results");
                Thread.Sleep(5000);
            }
        }
    }
}
