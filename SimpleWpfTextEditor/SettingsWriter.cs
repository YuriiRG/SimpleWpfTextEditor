using System;
using System.IO;
using System.Text.Json;

namespace SimpleWpfTextEditor
{
    public static class SettingsWriter
    {
        private static readonly string SettingsPath = Path.Combine(Environment.CurrentDirectory, "settings.json");
        public static AppSettings Read()
        {
            if (File.Exists(SettingsPath)) {
                string json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<AppSettings>(json)!;
            } else
            {
                return new AppSettings();
            }
        }
        public static void Save(AppSettings? data)
        {
            string json = JsonSerializer.Serialize(data);
            if (json != File.ReadAllText(SettingsPath))
            {
                File.WriteAllText(SettingsPath, json);
            }
        }
    }
}
