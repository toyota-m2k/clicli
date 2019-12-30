using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.model
{
    public interface IArrayData
    {
        object[] Items { get; set; }
        Type[] IncludeTypes { get; }

        Type ThisType { get; }
    }

    public class ArrayData<T>
    {
        public object[] Items { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public static Type[] IncludeTypes { get { return new Type[] { typeof(T) }; } }

        public ArrayData(IList items)
        {
            this.Items = new ArrayList(items).ToArray();
        }
        public ArrayData()
        {
            this.Items = null;
        }
    }

    public class ArraySerializer
    {
        public static string Serialize<T>(IList items)
        {
            if (null == items || items.Count == 0)
            {
                return null;
            }

            try
            {
                var obj = new ArrayData<T>(items);
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ArrayData<T>), ArrayData<T>.IncludeTypes);
                //書き込むファイルを開く（UTF-8 BOM無し）
                using (var sw = new StringWriter())
                {
                    //シリアル化し、XMLファイルに保存する
                    serializer.Serialize(sw, obj);
                    sw.Close();
                    return sw.ToString();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        public static object[] Deserialize<T>(string src)
        {
            if (null == src)
            {
                return null;
            }

            try
            {
                //XmlSerializerオブジェクトを作成
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ArrayData<T>), ArrayData<T>.IncludeTypes);

                //読み込むファイルを開く
                using (var sr = new StringReader(src))
                {
                    //XMLファイルから読み込み、逆シリアル化する
                    var obj = serializer.Deserialize(sr) as ArrayData<T>;
                    return obj.Items;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
    }

    public abstract class ArrayClipboardAccessor<T> {


        public abstract string CB_FORMAT { get; }


        public bool PutToClipboard(IList items)
        {
            var cd = ArraySerializer.Serialize<T>(items);
            if (null != cd)
            {
                System.Windows.Clipboard.Clear();
                System.Windows.Clipboard.SetData(CB_FORMAT, cd);
                return true;
            }
            return false;
        }

        public object[] GetFromClipboard()
        {
            if (!HasDataOnClipboard)
            {
                return null;
            }
            string cd = System.Windows.Clipboard.GetData(CB_FORMAT) as string;
            return ArraySerializer.Deserialize<T>(cd);
        }

        public bool HasDataOnClipboard
        {
            get
            {
                return System.Windows.Clipboard.ContainsData(CB_FORMAT);
            }
        }
    }

    public class TargetClipboardAccessor : ArrayClipboardAccessor<TargetItem>
    {
        public override string CB_FORMAT { get; } = "CliCliTarget";

        private TargetClipboardAccessor() {
        }
        private static TargetClipboardAccessor mInstance = null;
        public static TargetClipboardAccessor Instance
        {
            get
            {
                if (null == mInstance)
                {
                    mInstance = new TargetClipboardAccessor();
                }
                return mInstance;
            }
        }


    }

    public class ProjectClipboardAccessor : ArrayClipboardAccessor<Project>
    {
        public override string CB_FORMAT { get { return "CliCliProject"; } }
        private ProjectClipboardAccessor()
        {
        }
        private static ProjectClipboardAccessor mInstance = null;
        public static ProjectClipboardAccessor Instance
        {
            get
            {
                if (null == mInstance)
                {
                    mInstance = new ProjectClipboardAccessor();
                }
                return mInstance;
            }
        }
    }

}
