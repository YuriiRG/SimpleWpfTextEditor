using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleWpfTextEditor
{
    public static class SettingsWriter
    {
        private static readonly string SettingsPath = Path.Combine(Environment.CurrentDirectory, "settings.json");
        public static ApplicationData Read()
        {
            if (File.Exists(SettingsPath)) {
                string json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<ApplicationData>(json)!;
            } else
            {
                return new ApplicationData();
            }
        }
        public static void Save(ApplicationData? data)
        {
            string json = JsonSerializer.Serialize(data);
            if (json != File.ReadAllText(SettingsPath))
            {
                File.WriteAllText(SettingsPath, json);
            }
        }
    }
}
