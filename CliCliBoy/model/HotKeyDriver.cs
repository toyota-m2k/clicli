using CliCliBoy.interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliCliBoy.model
{
    public enum HotKeyIndex
    {
        HK_NONE,
        HK0, HK1, HK2, HK3, HK4, HK5, HK6, HK7, HK8, HK9,
        HK_DOT, HK_DIV, HK_MUL, HK_SUB, HK_PLS,
        HK_PGUP, HK_PGDN, HK_HOME, HK_END,
    }

    public class HotKeyDriver
    {
        private HotKey mHotKey;
        private HotKeyIndex mHotKeyIndex;
        private int mSuspending;

        public delegate void Toggle();
        public Toggle TogglePlay;

        private int currentKey
        {
            get
            {
                switch (mHotKeyIndex)
                {
                    case HotKeyIndex.HK0:
                        return HotKey.VK_NUMPAD0;
                    case HotKeyIndex.HK1:
                        return HotKey.VK_NUMPAD0 + 1;
                    case HotKeyIndex.HK2:
                        return HotKey.VK_NUMPAD0 + 2;
                    case HotKeyIndex.HK3:
                        return HotKey.VK_NUMPAD0 + 3;
                    case HotKeyIndex.HK4:
                        return HotKey.VK_NUMPAD0 + 4;
                    case HotKeyIndex.HK5:
                        return HotKey.VK_NUMPAD0 + 5;
                    case HotKeyIndex.HK6:
                        return HotKey.VK_NUMPAD0 + 6;
                    case HotKeyIndex.HK7:
                        return HotKey.VK_NUMPAD0 + 7;
                    case HotKeyIndex.HK8:
                        return HotKey.VK_NUMPAD0 + 8;
                    case HotKeyIndex.HK9:
                        return HotKey.VK_NUMPAD0 + 9;

                    case HotKeyIndex.HK_DOT:
                        return HotKey.VK_DECIMAL;
                    case HotKeyIndex.HK_DIV:
                        return HotKey.VK_DIVIDE;
                    case HotKeyIndex.HK_MUL:
                        return HotKey.VK_MULTIPLY;
                    case HotKeyIndex.HK_SUB:
                        return HotKey.VK_SUBTRACT;
                    case HotKeyIndex.HK_PLS:
                        return HotKey.VK_ADD;

                    case HotKeyIndex.HK_PGUP:
                        return HotKey.VK_PRIOR;
                    case HotKeyIndex.HK_PGDN:
                        return HotKey.VK_NEXT;
                    case HotKeyIndex.HK_HOME:
                        return HotKey.VK_HOME;
                    case HotKeyIndex.HK_END:
                        return HotKey.VK_END;

                    case HotKeyIndex.HK_NONE:
                    default:
                        return 0;

                }
            }
        }

        public string CurrentHotKeyName
        {
            get
            {
                switch (mHotKeyIndex)
                {
                    case HotKeyIndex.HK0:
                        return "0";
                    case HotKeyIndex.HK1:
                        return "1";
                    case HotKeyIndex.HK2:
                        return "2";
                    case HotKeyIndex.HK3:
                        return "3";
                    case HotKeyIndex.HK4:
                        return "4";
                    case HotKeyIndex.HK5:
                        return "5";
                    case HotKeyIndex.HK6:
                        return "6";
                    case HotKeyIndex.HK7:
                        return "7";
                    case HotKeyIndex.HK8:
                        return "8";
                    case HotKeyIndex.HK9:
                        return "9";

                    case HotKeyIndex.HK_DOT:
                        return ".";
                    case HotKeyIndex.HK_DIV:
                        return "/";
                    case HotKeyIndex.HK_MUL:
                        return "*";
                    case HotKeyIndex.HK_SUB:
                        return "-";
                    case HotKeyIndex.HK_PLS:
                        return "+";

                    case HotKeyIndex.HK_PGUP:
                        return "PgUp";
                    case HotKeyIndex.HK_PGDN:
                        return "PgDn";
                    case HotKeyIndex.HK_HOME:
                        return "Home";
                    case HotKeyIndex.HK_END:
                        return "End";

                    case HotKeyIndex.HK_NONE:
                    default:
                        return "none";
                }
            }
        }

        public bool Suspending
        {
            get { return mSuspending>0; }
            set
            {
                if (value)
                {
                    if(mSuspending ==0) {
                        unregisterHotKey();
                    }
                    mSuspending++;
                }
                else
                {
                    mSuspending--;
                    if (mSuspending == 0)
                    {
                        registerHotKey();
                    }
                }
            }
        }


        public HotKeyDriver()
        {
            mHotKey = null;
            mHotKeyIndex = HotKeyIndex.HK_NONE;
            mSuspending = 0;
            TogglePlay = null;
        }

        public void Dispose()
        {
            unregisterHotKey();
            mHotKeyIndex = HotKeyIndex.HK_NONE;
            mSuspending = 0;
            TogglePlay = null;
        }

        public HotKeyIndex CurrentHotKey
        {
            get { return mHotKeyIndex; }
            set
            {
                if (mHotKeyIndex != value)
                {
                    mHotKeyIndex = value;
                    registerHotKey();
                }
            }
        }

        private void unregisterHotKey()
        {
            if (null != mHotKey)
            {
                mHotKey.Dispose();
                mHotKey = null;
            }
        }

        private void registerHotKey()
        {
            unregisterHotKey();

            if (mHotKeyIndex != HotKeyIndex.HK_NONE && !Suspending)
            {
                int key = currentKey;
                if (key != 0)
                {
                    mHotKey = new HotKey();
                    mHotKey.Register(0, currentKey, () =>
                    {
                        if (null != TogglePlay)
                        {
                            TogglePlay();
                        }
                    });
                }
            }
        }

    }
}
