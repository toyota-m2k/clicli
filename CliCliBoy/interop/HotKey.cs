using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace CliCliBoy.interop
{
    public class HotKey : Form
    {
        //
        // HotKey登録関数
        //
        // 登録に失敗(他のアプリが使用中)の場合は、0が返却されます。
        //
        [DllImport("user32.dll")]
        extern static int RegisterHotKey(IntPtr hWnd, int id, int modKey, int key);

        //
        // HotKey解除関数
        //
        // 解除に失敗した場合は、0が返却されます。
        //
        [DllImport("user32.dll")]
        extern static int UnregisterHotKey(IntPtr HWnd, int ID);

        [DllImport("kernel32", EntryPoint = "GlobalAddAtomA")]  
        static extern short GlobalAddAtom(string lpString);
        [DllImport("kernel32")]
        static extern short GlobalDeleteAtom(short nAtom);

        public delegate void Proc();
        private Dictionary<int, Proc> mProcs;

        // テンキー
        public const int VK_NUMPAD0 = 0x60;         // 0
        public const int VK_ADD = 0x6B;             // +
        public const int VK_MULTIPLY = 0x6A;        // *
        public const int VK_SUBTRACT = 0x6D;        // -
        public const int VK_DIVIDE = 0x6F;          // /
        public const int VK_SEPARATOR = 0x6C;       // Enter
        public const int VK_DECIMAL = 0x6E;         // .
        
        public const int VK_PRIOR = 0x21;           // PGUP
        public const int VK_NEXT = 0x22;            // PGDN
        public const int VK_END = 0x23;
        public const int VK_HOME = 0x24;
        public const int VK_LEFT = 0x25;
        public const int VK_UP = 0x26;
        public const int VK_RIGHT = 0x27;
        public const int VK_DOWN = 0x28;

        public const int VK_F1 = 0x70;



        public enum MOD_KEY : int
        {
            ALT = 0x0001,
            CONTROL = 0x0002,
            SHIFT = 0x0004,
        }

        public HotKey()
        {
            mProcs = new Dictionary<int, Proc>();
        }

        private int makeKey(MOD_KEY modkey, int key)
        {
            return GlobalAddAtom("CCBKey." + modkey.ToString() + "." + key.ToString());
        }

        public bool Register(MOD_KEY modkey, int key, Proc proc)
        {
            int id = makeKey(modkey, key);
            if (RegisterHotKey(this.Handle, id, (int)modkey, key) == 0)
            {
                return false;
            }
            mProcs.Add(id, proc);
            return true;
        }

        public void Unregister(MOD_KEY modkey, int key)
        {
            int id = makeKey(modkey, key);
            if (UnregisterHotKey(this.Handle, id) != 0)
            {
                mProcs.Remove(id);
            }
        }

        protected override void Dispose(bool disposing) 
        {
            try
            {
                if (null != mProcs)
                {
                    foreach (int id in mProcs.Keys)
                    {
                        UnregisterHotKey(this.Handle, id);
                    }
                    mProcs.Clear();
                    mProcs = null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            base.Dispose(disposing);
        }

        const int WM_HOTKEY = 0x0312;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_HOTKEY)
            {
                Proc proc;
                if (mProcs.TryGetValue((int)m.WParam, out proc))
                {
                    proc();
                }
            }
        }
    }
}
