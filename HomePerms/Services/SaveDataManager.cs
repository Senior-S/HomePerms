using Newtonsoft.Json;
using SeniorS.HomePerms.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Logger = Rocket.Core.Logging.Logger;

namespace SeniorS.HomePerms.Services
{
    public class SaveDataManager
    {
        private readonly string SavePath;

        public SaveDataManager()
        {
            SavePath = $"{Environment.CurrentDirectory}/Plugins/HomePerms/Homes.json";
        }

        public void Save(List<PlayerHome> homes)
        {
            Logger.Log($"Saving {homes.Count} homes...");

            using (var saveFile = File.Create(SavePath))
            {
                var json = JsonConvert.SerializeObject(homes, Formatting.None);
                using (var streamWriter = new StreamWriter(saveFile))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }
                saveFile.Close();
            }

            Logger.Log("Save done!");
        }

        public void Load()
        {
            List<PlayerHome> homes = new();

            if (File.Exists(SavePath))
            {
                using (var streamReader = new StreamReader(SavePath))
                {
                    var json = streamReader.ReadToEnd();
                    streamReader.BaseStream.Flush();
                    homes = JsonConvert.DeserializeObject<List<PlayerHome>>(json);
                }
            }

            HomePerms Instance = HomePerms.Instance;

            Instance.Homes = homes;

        }
    }
}
