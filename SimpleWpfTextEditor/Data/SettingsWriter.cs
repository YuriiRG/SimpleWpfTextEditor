using System;
using System.IO;
using System.Text.Json;

namespace SimpleWpfTextEditor.Data
{
    public static class SettingsWriter
    {
        public static readonly string SettingsPath = Path.Combine(Environment.CurrentDirectory, "settings.json");
        public static AppSettings Read()
        {
            if (File.Exists(SettingsPath))
            {
                string json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<AppSettings>(json)!;
            }
            else
            {
                return new AppSettings();
            }
        }
        public static void Save(AppSettings? data)
        {
            if (File.Exists(SettingsPath))
            {
                string json = JsonSerializer.Serialize(data);
                if (json != File.ReadAllText(SettingsPath))
                {
                    File.WriteAllText(SettingsPath, json);
                }
            }
            else
            {
                string json = JsonSerializer.Serialize(data);
                File.WriteAllText(SettingsPath, json);
            }
        }
    }
}
