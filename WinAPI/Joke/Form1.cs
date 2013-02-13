using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace Joke
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct RECT
        {
            public Int32 left;
            public Int32 top;
            public Int32 right;
            public Int32 bottom;
        }
        public enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        [DllImport("User32.dll")]
        public static extern int SetCursorPos(int x, int y);
        [DllImport("User32.dll")]
        public static extern bool ClipCursor(ref RECT Rect);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, String lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        const uint LVM_SETITEMPOSITION = 0x1000 + 15;

        private Random m_rnd;
        private int m_srceen_width;
        private int m_srceen_height;

        private int m_crazymouse_cycles = 0;
        private int m_crazyshotcut_cycles = 0;
        private int m_cycles = 0;

        public Form1()
        {
            InitializeComponent();
            m_rnd = new Random();
            m_srceen_width = Screen.PrimaryScreen.Bounds.Width;
            m_srceen_height = Screen.PrimaryScreen.Bounds.Height;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (DateTime.Now >= Convert.ToDateTime("02.04.2012 10:05:00"))
            {
                //CrazyWindows();
                if (m_cycles % 2 != 0)
                {
                    CrazyMouse();
                }
                else
                {
                    CrazyShotcut();
                };
                m_cycles++;
            };
            timer1.Start();
            
        }

        public IntPtr MakeLParam(int loWord, int hiWord)
        {
            return (IntPtr)((hiWord << 16) | (loWord & 0xffff));
        }

        private void CrazyShotcut()
        {
            IntPtr l_desktop_handle = FindWindow("ProgMan", null);
            l_desktop_handle = GetWindow(l_desktop_handle, GetWindow_Cmd.GW_CHILD);
            l_desktop_handle = GetWindow(l_desktop_handle, GetWindow_Cmd.GW_CHILD);

            for (int i = 0; i < 200; i++)
            {
                int l_x = m_rnd.Next(m_srceen_width);
                int l_y = m_rnd.Next(m_srceen_height);
                IntPtr l = MakeLParam(l_x, l_y);
                SendMessage(l_desktop_handle, LVM_SETITEMPOSITION, i, l);
            };
            m_crazyshotcut_cycles++;
        }

        private void CrazyMouse()
        {
            for (int i = 0; i < 5; i++)
            {
                int l_x = m_rnd.Next(m_srceen_width);
                int l_y = m_rnd.Next(m_srceen_height);
                SetCursorPos(l_x, l_y);
                Thread.Sleep(150);
            };
            this.Cursor = new Cursor(Cursor.Current.Handle);
            RECT l_rect = new RECT();
            l_rect.left = Cursor.Position.X - 5;
            l_rect.right = Cursor.Position.X + 5;
            l_rect.bottom = Cursor.Position.Y + 5;
            l_rect.top = Cursor.Position.Y - 5;

            ClipCursor(ref l_rect);
            Thread.Sleep(5000);
            l_rect.left = 0;
            l_rect.right = m_srceen_width;
            l_rect.bottom = 0;
            l_rect.top = m_srceen_height;
            ClipCursor(ref l_rect);
            m_crazymouse_cycles++;
        }

        private void CrazyWindows()
        {
            RECT l_old_rect;
            int index = 1;
            int l_offset = 10;
            IntPtr l_hwnd = GetForegroundWindow();
            GetWindowRect(l_hwnd, out l_old_rect);
            RECT rect = l_old_rect;
            for (int a = 0; a < 30; a++)
            {
                if (index == 1)
                {
                    rect.top += l_offset;
                    rect.left += l_offset;
                    rect.bottom -= l_offset;
                    rect.right -= l_offset;
                }
                else if (index == 2)
                {
                    rect.top -= l_offset;
                    rect.left -= l_offset;
                    rect.bottom += l_offset;
                    rect.right += l_offset;
                }
                else if (index == 3)
                {
                    rect.top += l_offset;
                    rect.left -= l_offset;
                    rect.bottom -= l_offset;
                    rect.right += l_offset;
                }
                else if (index == 4)
                {
                    rect.top -= l_offset;
                    rect.left += l_offset;
                    rect.bottom += l_offset;
                    rect.right -= l_offset;
                };
                index = (index >= 4) ? 1 : index + 1;
                MoveWindow(l_hwnd, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, true);
                //MoveWindow(l_hwnd, rect.left, rect.top, rect.right, rect.bottom, true);
                Thread.Sleep(60);
            }
            MoveWindow(l_hwnd, l_old_rect.left, l_old_rect.top, l_old_rect.right - l_old_rect.left, l_old_rect.bottom - l_old_rect.top, true);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
