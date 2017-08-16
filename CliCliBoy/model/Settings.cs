using CliCliBoy.interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliCliBoy.model
{
    public class Settings
    {
        public WinPlacement Placement { get; set; }

        public string FilePath
        {
            get
            {
                return Globals.Instance.DataFilePath;
            }
            set
            {
                Globals.Instance.DataFilePath = value;
            }
        }

        public List<string> MRU { get; set; }

        public void AddMru(string path)
        {
            if(null==path)
            {
                return;
            }
            MRU.Remove(path);
            MRU.Insert(0, path);
            while(MRU.Count>10)
            {
                MRU.RemoveAt(10);
            }
        }

        public bool HasMru
        {
            get
            {
                foreach (var s in MRU)
                {
                    if (s == Globals.Instance.DataFilePath)
                    {
                        continue;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public Settings()
        {
            Placement = new WinPlacement();
            //ProjectSelection = new List<int>();
        }

        //public List<int> ProjectSelection { get; set; }

        public void Serialize()
        {
            System.IO.StreamWriter sw = null;
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Settings));
                //書き込むファイルを開く（UTF-8 BOM無し）
                sw = new System.IO.StreamWriter(Globals.Instance.SettingFilePath, false, new System.Text.UTF8Encoding(false));
                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(sw, this);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                //ファイルを閉じる
                if (null != sw)
                {
                    sw.Close();
                }
            }
        }
        public static Settings Deserialize()
        {
            System.IO.StreamReader sr = null;
            Object obj = null;

            try
            {
                //XmlSerializerオブジェクトを作成
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Settings));

                //読み込むファイルを開く
                sr = new System.IO.StreamReader(Globals.Instance.SettingFilePath, new System.Text.UTF8Encoding(false));

                //XMLファイルから読み込み、逆シリアル化する
                obj = serializer.Deserialize(sr);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                obj = new Settings();
            }
            finally
            {
                if (null != sr)
                {
                    //ファイルを閉じる
                    sr.Close();
                }
            }
            return (Settings)obj;
        }
    }
}
