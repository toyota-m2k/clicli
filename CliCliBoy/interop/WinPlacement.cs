using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace CliCliBoy.interop
{
    public class WinPlacement
    {
        #region Win32データ型

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }

            public bool Equals(RECT s)
            {
                return s.Left == Left && s.Right == Right && s.Top == Top && s.Bottom == Bottom;
            }

            public static bool operator==(RECT r0, RECT r1)
            {
                return r0.Equals(r1);
            }
            public static bool operator !=(RECT r0, RECT r1)
            {
                return !r0.Equals(r1);
            }
        }

        // POINT structure required by WINDOWPLACEMENT structure
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
            public bool Equals(POINT s)
            {
                return s.X == X && s.Y == Y;
            }

            public static bool operator ==(POINT r0, POINT r1)
            {
                return r0.Equals(r1);
            }
            public static bool operator !=(POINT r0, POINT r1)
            {
                return !r0.Equals(r1);
            }

        }

        // WINDOWPLACEMENT stores the position, size, and state of a window
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public POINT minPosition;
            public POINT maxPosition;
            public RECT normalPosition;

            public bool Equals(WINDOWPLACEMENT s)
            {
                return
                    s.flags == flags &&
                    s.showCmd == showCmd &&
                    s.minPosition == minPosition &&
                    s.maxPosition == maxPosition &&
                    s.normalPosition == normalPosition;
            }
            public static bool operator ==(WINDOWPLACEMENT r0, WINDOWPLACEMENT r1)
            {
                return r0.Equals(r1);
            }
            public static bool operator !=(WINDOWPLACEMENT r0, WINDOWPLACEMENT r1)
            {
                return !r0.Equals(r1);
            }
        }
        #endregion

        #region Win32 API declarations to set and get window placement
        [DllImport("user32.dll")]
        static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        const int SW_SHOWNORMAL = 1;
        const int SW_SHOWMINIMIZED = 2;
        #endregion


        private static WINDOWPLACEMENT GetWindowPlacement(Window w)
        {
            WINDOWPLACEMENT wp = new WINDOWPLACEMENT();
            IntPtr hwnd = new WindowInteropHelper(w).Handle;
            GetWindowPlacement(hwnd, out wp);
            return wp;
        }

        private static void SetWindowPlacement(Window w, WINDOWPLACEMENT wp)
        {
            wp.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
            wp.flags = 0;
            wp.showCmd = (wp.showCmd == SW_SHOWMINIMIZED ? SW_SHOWNORMAL : wp.showCmd);
            IntPtr hwnd = new WindowInteropHelper(w).Handle;
            SetWindowPlacement(hwnd, ref wp);
        }


        public bool HasValue
        {
            get; set;
        }

        public WINDOWPLACEMENT Placement
        {
            get; set;
        }


        public WinPlacement()
        {
            Placement = new WINDOWPLACEMENT();
            HasValue = false;
        }

        public void GetPlacementFrom(Window w)
        {
            var wp = GetWindowPlacement(w);
            if (Placement != wp)
            {
                HasValue = true;
                Placement = wp;
            }
        }

        public void ApplyPlacementTo(Window w)
        {
            if (HasValue)
            {
                SetWindowPlacement(w, Placement);
            }
        }



        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT point);

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        //[DllImport("user32.dll")]
        //public static extern bool GetCursorPos(ref POINT lpPoint);

        public static Window GetWindowFromPoint(Point point)
        {
            var hwnd = WindowFromPoint(new POINT((int)point.X, (int)point.Y));
            if (hwnd == IntPtr.Zero) return null;
            var p = GetParent(hwnd);
            while (p != IntPtr.Zero)
            {
                hwnd = p;
                p = GetParent(hwnd);
            }
            foreach (Window w in Application.Current.Windows)
            {
                if (w.IsVisible)
                {
                    var helper = new WindowInteropHelper(w);
                    if (helper.Handle == hwnd) return w;
                }
            }
            return null;
        }
    }
}
