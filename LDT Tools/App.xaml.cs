using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Reflection;
using static LDT_Tools.Utility;
using System.Windows.Navigation;

namespace LDT_Tools
{
    [JsonObject]
    public class _GlobalSettings
    {
        public bool IsRoundCornerEnabled { get; set; } = true;
        public Wpf.Ui.Appearance.BackgroundType BackgroundType { get; set; } = Wpf.Ui.Appearance.BackgroundType.Auto;
        public Wpf.Ui.Appearance.ThemeType ThemeType { get; set; } = Wpf.Ui.Appearance.ThemeType.Dark;
    }
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static _GlobalSettings GlobalSettings;
        private JsonSerializerSettings jsonSerializerSettings;
        public App()
        {
            GlobalSettings = new _GlobalSettings();
            if (File.Exists("config.json"))
            {
                _GlobalSettings? settings = null;
                try
                {
                    settings = JsonConvert.DeserializeObject<_GlobalSettings>(File.ReadAllText("config.json"), jsonSerializerSettings);
                }
                catch(Newtonsoft.Json.JsonReaderException ex)
                {
                    MessageBox.Show("配置文件(config.json)错误，请尝试修改或删除config.json并重启应用\n具体错误：\n" + ex.Message, "配置文件错误");
                    Environment.Exit(1);
                }
                if (settings == null)
                {
                    MessageBox.Show("配置文件(config.json)错误，请尝试修改或删除config.json并重启应用", "配置文件错误");
                    Environment.Exit(1);
                }
                GlobalSettings = settings;
            }
            jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Formatting = Formatting.Indented;
            //settings.TypeNameHandling = TypeNameHandling.All;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            //MessageBox.Show(JsonConvert.SerializeObject(GlobalSettings, jsonSerializerSettings));
            File.WriteAllText("config.json", JsonConvert.SerializeObject(GlobalSettings, jsonSerializerSettings));
        }
    }
}
