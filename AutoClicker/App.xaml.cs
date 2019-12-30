using AutoClicker.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace AutoClicker
{
    //public interface Logger
    //{
    //    void Output(string msg);
    //}


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


        public static readonly String EXT_C = ".clc";
        public static readonly String EXT_C_PREV = ".clicli";
        public static readonly String EXT_P = ".clp";
        public static readonly String EXT_P_PREV = ".cli";

        public static readonly String DEF_FILENAME = "default.clc";
        public static readonly String DEF_SETTINGFILE = "clicli.setting";

        //private class NoopLogger : Logger
        //{
        //    public void Output(string msg)
        //    {
        //        Debug.WriteLine(msg);
        //    }
        //}
        //private static Logger mDefaultLogger = new NoopLogger();
        //private static Logger mLogger = null;
        //public static Logger Logger
        //{
        //    get { return (null == mLogger) ? mDefaultLogger : mLogger; }
        //    set { mLogger = value; }
        //}
        /**
         * コンストラクタ（シングルトンなのでprivate）
         */
        private Globals()
        {
            DataFilePath = DEF_FILENAME;    // 初期値
            SettingFilePath = DEF_SETTINGFILE;
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

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalErrorHandler);
        }

        static void GlobalErrorHandler(object sender, UnhandledExceptionEventArgs args)
        {
            StreamWriter sw = null;
            try
            {
                // エラーファイルのパス
                bool append = true;
                var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var efile = Path.Combine(dir, "cli-err.txt");
                var info = new FileInfo(efile);
                if(info.Exists && info.Length>1*1024*1024)  // 1MBでリサイクル
                {
                    var bfile = Path.Combine(dir, "cli-err-old.txt");
                    File.Copy(efile, bfile, true);
                    append = false;
                }


                //書き込むファイルを開く（UTF-8 BOM無し）
                Exception e = (Exception)args.ExceptionObject;
                using (sw = new StreamWriter(efile, append, new System.Text.UTF8Encoding(false)))
                {
                    sw.WriteLine("");
                    sw.WriteLine("Date: {0}", DateTime.Now.ToString());
                    sw.WriteLine(e.ToString());
                    sw.WriteLine("-----------------------------------------------");
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
