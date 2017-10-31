using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;

namespace CliCliBoy.interop
{
    class MouseEmulator
    {
        // マウスイベント(mouse_eventの引数と同様のデータ)
        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        };

        // キーボードイベント(keybd_eventの引数と同様のデータ)
        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        };

        // ハードウェアイベント
        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        };

        // 各種イベント(SendInputの引数データ)
        [StructLayout(LayoutKind.Explicit)]
        private struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        };

        // キー操作、マウス操作をシミュレート(擬似的に操作する)
        [DllImport("user32.dll")]
        private extern static void SendInput(
            int nInputs, INPUT[] pInputs, int cbsize);

        private const int INPUT_MOUSE = 0;                  // マウスイベント
        private const int INPUT_KEYBOARD = 1;               // キーボードイベント
        private const int INPUT_HARDWARE = 2;               // ハードウェアイベント

        private const int MOUSEEVENTF_MOVE = 0x1;           // マウスを移動する
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;    // 絶対座標指定
        private const int MOUSEEVENTF_LEFTDOWN = 0x2;       // 左　ボタンを押す
        private const int MOUSEEVENTF_LEFTUP = 0x4;         // 左　ボタンを離す
        private const int MOUSEEVENTF_RIGHTDOWN = 0x8;      // 右　ボタンを押す
        private const int MOUSEEVENTF_RIGHTUP = 0x10;       // 右　ボタンを離す
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;    // 中央ボタンを押す
        private const int MOUSEEVENTF_MIDDLEUP = 0x40;      // 中央ボタンを離す
        private const int MOUSEEVENTF_WHEEL = 0x800;        // ホイールを回転する
        private const int WHEEL_DELTA = 120;                // ホイール回転値

        private const int KEYEVENTF_KEYDOWN = 0x0;          // キーを押す
        private const int KEYEVENTF_KEYUP = 0x2;            // キーを離す
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;      // 拡張コード
        private const int VK_SHIFT = 0x10;                  // SHIFTキー


        public delegate bool IsReady();

        private static INPUT[] prepare(Point click)
        {
            Rectangle screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            INPUT[] input = new INPUT[1];
            input[0].type = INPUT_MOUSE;
            input[0].mi.dx = (int)Math.Round((double)(click.X * 65536) / (double)screen.Width);
            input[0].mi.dy = (int)Math.Round((double)(click.Y * 65536) / (double)screen.Height);
            input[0].mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP | MOUSEEVENTF_ABSOLUTE;
            return input;
        }

        public static bool MoveMouse(Point pos, IsReady isReady)
        {
            if (null != isReady && !isReady())
            {
                return false;
            }
            System.Windows.Forms.Cursor.Position = pos;
            return true;
        }


        public static bool ClickAt(Point pos, IsReady isReady, bool dblclk)
        {
            Point curPos = System.Windows.Forms.Cursor.Position;
            INPUT[] input = prepare(pos);
            if (null != isReady && !isReady())
            {
                return false;
            }

            //System.Windows.Forms.Cursor.Position = curPos;
            SendInput(input.Length, input, Marshal.SizeOf(input[0]));
            if(dblclk){
                SendInput(input.Length, input, Marshal.SizeOf(input[0]));
            }
            System.Windows.Forms.Cursor.Position = curPos;
            return true;
        }

        public static bool _WheelAt(Point pos, int wheel, IsReady isReady)
        {
            if (wheel == 0)
            {
                return true;
            }
            Point curPos = System.Windows.Forms.Cursor.Position;
            Rectangle screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            INPUT[] input = new INPUT[2];
            input[0].type = INPUT_MOUSE;
            input[0].mi.dx = (int)Math.Round((double)(pos.X * 65536) / (double)screen.Width);
            input[0].mi.dy = (int)Math.Round((double)(pos.Y * 65536) / (double)screen.Height);
            input[0].mi.dwFlags = MOUSEEVENTF_MOVE| MOUSEEVENTF_ABSOLUTE;

            input[1].type = INPUT_MOUSE;
            input[1].mi.dx = 0;
            input[1].mi.dy = 0;
            input[1].mi.dwFlags = MOUSEEVENTF_WHEEL;
            input[1].mi.mouseData = -wheel * WHEEL_DELTA;

            if (null != isReady && !isReady())
            {
                return false;
            }
            SendInput(input.Length, input, Marshal.SizeOf(input[0]));
            System.Windows.Forms.Cursor.Position = curPos;

            return true;
        }

        public static bool WheelAt(Point pos, int wheel, IsReady isReady)
        {
            if (wheel == 0)
            {
                return true;
            }
            if (null != isReady && !isReady())
            {
                return false;
            }

            int w = Math.Abs(wheel);
            for(int i = 0; i<w; i++)
            {
                _WheelAt(pos, wheel / w, isReady);
            }
            return true;
        }

        public static void MoveTo(Point pos)
        {

            Rectangle screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            INPUT[] input = new INPUT[1];
            input[0].type = INPUT_MOUSE;
            input[0].mi.dx = (int)Math.Round((double)(pos.X * 65536) / (double)screen.Width);
            input[0].mi.dy = (int)Math.Round((double)(pos.Y * 65536) / (double)screen.Height);
            input[0].mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;

            SendInput(input.Length, input, Marshal.SizeOf(input[0]));
        }

        [DllImport("user32.dll")]
        static extern uint GetMessageExtraInfo();

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(
                                  uint nCode,     // 仮想キーコードまたはスキャンコード
                                  uint uMapType   // 実行したい変換の種類
                                );

        public const int KEY_DOWN   = 1;
        public const int KEY_UP     = 2;
        public const int KEY_DOWNUP = 3;

        public const int VK_ESCAPE  = 0x1b;
        public const int VK_RETURN  = 0x0d;
        public const int VK_PRIOR   = 0x21;   // PageUp
        public const int VK_NEXT    = 0x22; // PageDown
        public const int VK_END     = 0x23;
        public const int VK_HOME    = 0x24;
        public const int VK_UP      = 0x26;
        public const int VK_DOWN    = 0x28;
        public const int VK_LEFT    = 0x25;
        public const int VK_RIGHT   = 0x27;

        public static bool PressKey(int vkey, IsReady isReady, int keystate = KEY_DOWNUP)
        {
            if (vkey== 0)
            {
                return true;
            }
            if (null != isReady && !isReady())
            {
                return false;
            }

            INPUT[] input = new INPUT[2];
            int count = 0;

            if (0 != (keystate & KEY_DOWN)) {
                input[count].type = INPUT_KEYBOARD;
                input[count].ki.wVk = (short)vkey;
                input[count].ki.wScan = (short)MapVirtualKey((uint)vkey, 0);
                input[count].ki.dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN;
                input[count].ki.time = 0;
                input[count].ki.dwExtraInfo = (int)GetMessageExtraInfo();
                count++;
            }
            if (0 != (keystate & KEY_UP))
            {
                input[count].type = INPUT_KEYBOARD;
                input[count].ki.wVk = (short)vkey;
                input[count].ki.wScan = (short)MapVirtualKey((uint)vkey, 0);
                input[count].ki.dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP;
                input[count].ki.time = 0;
                input[count].ki.dwExtraInfo = (int)GetMessageExtraInfo();
                count++;
            }
            SendInput(count, input, Marshal.SizeOf(input[0]));
            return true;
        }
    }
}
