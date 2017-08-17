using CliCliBoy.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CliCliBoy
{
    public class Globals
    {
        private static Globals mInstance;
        public static Globals Instance
        {
            get
            {
                if (null == mInstance)
                {
                    mInstance = new Globals();
                }
                return mInstance;
            }
        }

        public string DataFilePath { get; set; }
        public string SettingFilePath { get; set; }

        public Manager DataContext { get; set; }

        public Settings Settings { get; set; }

        /**
         * コンストラクタ（シングルトンなのでprivate）
         */
        private Globals()
        {
            DataFilePath = "default.clicli";    // 初期値
            SettingFilePath = "clicli.setting";
        }
    }

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void App_Exit(object sender, ExitEventArgs e)
        {
            Globals.Instance.DataContext.Serialize();
            Globals.Instance.Settings.Serialize();
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            //foreach (string arg in e.Args)
            //{
            //    if (arg[0] != '/')
            //    {
            //        Globals.Instance.DataFilePath = arg;
            //    }
            //}
            Globals.Instance.Settings = Settings.Deserialize();
            Globals.Instance.DataContext = Manager.Deserialize();
        }
    }
}
