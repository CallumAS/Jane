using Jane.Models;
using Jane.Models.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jane.Managers
{
    public class FileManager
    {
        public static void SaveResults(List<ResponseStatus> results, string username)
        {
            if (!System.IO.Directory.Exists("Results"))
                System.IO.Directory.CreateDirectory("Results");
            foreach (Status status in (Status[])Enum.GetValues(typeof(Status)))
                System.IO.File.WriteAllText($"{Environment.CurrentDirectory}\\Results\\{username}_{Enum.GetName(typeof(Status), status)}.txt", string.Join(Environment.NewLine, results.Where(x=> x.Status == status).Select(z=> $"{username} | {z.Url} | {string.Join(" | ", z.Variables.Select(x => x.Key + "=" + x.Value).ToArray())}")));
        }
        public static List<Config> GetConfigs()
        {
            List<Config> configs = new List<Config>();
            if (System.IO.Directory.Exists("Configs"))
            {
                foreach (var config in System.IO.Directory.GetFiles($"{Environment.CurrentDirectory}\\Configs", "*.json"))
                {
                    try {
                        configs.Add(JsonConvert.DeserializeObject<Config>(System.IO.File.ReadAllText(config)));
                    } catch { }
                }
            }
            return configs;
        }
    }
}
