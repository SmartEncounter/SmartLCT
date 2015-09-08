using System.Windows;
using System.Windows.Controls;
using WinInterop = System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System;
using System.Windows.Shapes;
using DNBSoft.WPF;
using System.Windows.Threading;

namespace Nova.SmartLCT.Interface
{
    public class CustomWindow : Window
    {
        public CustomWindow()
        {
            InitializeStyle(); // 加载样式

            this.Loaded += delegate  // 加载事件委托
            {
                InitializeEvent();

            };



            // 解决最大化覆盖任务栏问题
            this.SourceInitialized += new EventHandler(win_SourceInitialized);
        }

        /// <summary>
        /// 加载自定义窗体样式
        /// </summary>
        private void InitializeStyle()
        {
            this.Style = (Style)Application.Current.Resources["CustomWindowStyle"];
        }

        /// <summary>
        /// 加载按钮事件委托
        /// </summary>
        private void InitializeEvent()
        {
            ControlTemplate baseWindowTemplate = (ControlTemplate)Application.Current.Resources["CustomWindowControlTemplate"];
  
            TextBlock titleText = (TextBlock)baseWindowTemplate.FindName("title", this);
            titleText.Text = this.Title;

            Button minBtn = (Button)baseWindowTemplate.FindName("btnMin", this);

            minBtn.Click += delegate
            {
                this.WindowState = WindowState.Minimized;
            };

            Button maxBtn = (Button)baseWindowTemplate.FindName("btnMax", this);

            maxBtn.Click += delegate
            {
                this.WindowState = (this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
            };
            Button closeBtn = (Button)baseWindowTemplate.FindName("btnClose", this);
            closeBtn.Click += delegate
            {
                this.Close();
            };

            Border tp = (Border)baseWindowTemplate.FindName("topborder", this);

            tp.MouseLeftButtonDown += delegate
            {
                this.DragMove();
            };
            tp.MouseDown += delegate
            {
                this.Mouse_DoubleClick();
            };
            if (this.ResizeMode == ResizeMode.NoResize)
            {
                minBtn.Visibility = Visibility.Collapsed;
                maxBtn.Visibility = Visibility.Collapsed;
            }
            else if (this.ResizeMode == ResizeMode.CanMinimize)
            {
                maxBtn.IsEnabled = false;
            }
            else
            {
                //鼠标拖拽改变窗口大小
                InitialzeWindowSize();
            }

        }
        private int _mouseDownNum = 0;
        private void Mouse_DoubleClick()
        {
            if (this.ResizeMode == ResizeMode.CanMinimize || this.ResizeMode == ResizeMode.NoResize)
            {
                return;
            }
            _mouseDownNum += 1;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);

            timer.Tick += (s, e1) => { timer.IsEnabled = false; _mouseDownNum = 0; };
            timer.IsEnabled = true;

            if (_mouseDownNum % 2 == 0)
            {
                timer.IsEnabled = false;
                _mouseDownNum = 0;

                this.WindowState = this.WindowState == WindowState.Maximized ?

                              WindowState.Normal : WindowState.Maximized;

            }

        }
        /// <summary>
        /// 鼠标拖拽改变窗口尺寸
        /// </summary>
        private void InitialzeWindowSize()
        {
            ControlTemplate baseWindowTemplate = (ControlTemplate)Application.Current.Resources["CustomWindowControlTemplate"];
            Rectangle left = (Rectangle)baseWindowTemplate.FindName("left", this);
            Rectangle right = (Rectangle)baseWindowTemplate.FindName("right", this);
            Rectangle down = (Rectangle)baseWindowTemplate.FindName("down", this);
            Rectangle leftDown = (Rectangle)baseWindowTemplate.FindName("leftDown", this);
            Rectangle rightDown = (Rectangle)baseWindowTemplate.FindName("rightDown", this);
            Rectangle up = (Rectangle)baseWindowTemplate.FindName("up", this);
            Rectangle leftUp = (Rectangle)baseWindowTemplate.FindName("leftUp", this);
            Rectangle rightUp = (Rectangle)baseWindowTemplate.FindName("rightUp", this);
            WindowResizer wr = new WindowResizer(this);
            wr.addResizerLeft(left);
            wr.addResizerRight(right);
            wr.addResizerDown(down);
            wr.addResizerLeftDown(leftDown);
            wr.addResizerRightDown(rightDown);
            wr.addResizerLeftUp(leftUp);
            wr.addResizerRightUp(rightUp);
            wr.addResizerUp(up);
        }
        /// <summary>
        /// 重绘窗体大小
        /// </summary>
        void win_SourceInitialized(object sender, EventArgs e)
        {
            System.IntPtr handle = (new WinInterop.WindowInteropHelper(this)).Handle;
            WinInterop.HwndSource.FromHwnd(handle).AddHook(new WinInterop.HwndSourceHook(WindowProc));
        }


        //////////////////////////////////////////////////////////////////////////
        // 使用Window API处理窗体大小  
        //////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// POINT aka POINTAPI
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            /// x coordinate of point.
            /// </summary>
            public int x;
            /// <summary>
            /// y coordinate of point.
            /// </summary>
            public int y;

            /// <summary>
            /// Construct a point of coordinates (x,y).
            /// </summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// 窗体大小信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        /// <summary> Win32 </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            /// <summary> Win32 </summary>
            public int left;
            /// <summary> Win32 </summary>
            public int top;
            /// <summary> Win32 </summary>
            public int right;
            /// <summary> Win32 </summary>
            public int bottom;

            /// <summary> Win32 </summary>
            public static readonly RECT Empty = new RECT();

            /// <summary> Win32 </summary>
            public int Width
            {
                get { return Math.Abs(right - left); }  // Abs needed for BIDI OS
            }
            /// <summary> Win32 </summary>
            public int Height
            {
                get { return bottom - top; }
            }

            /// <summary> Win32 </summary>
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }


            /// <summary> Win32 </summary>
            public RECT(RECT rcSrc)
            {
                this.left = rcSrc.left;
                this.top = rcSrc.top;
                this.right = rcSrc.right;
                this.bottom = rcSrc.bottom;
            }

            /// <summary> Win32 </summary>
            public bool IsEmpty
            {
                get
                {
                    // BUGBUG : On Bidi OS (hebrew arabic) left > right
                    return left >= right || top >= bottom;
                }
            }
            /// <summary> Return a user friendly representation of this struct </summary>
            public override string ToString()
            {
                if (this == RECT.Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }

            /// <summary> Determine if 2 RECT are equal (deep compare) </summary>
            public override bool Equals(object obj)
            {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }

            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode()
            {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }


            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
            }

            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }


        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            /// <summary>
            /// </summary>            
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

            /// <summary>
            /// </summary>            
            public RECT rcMonitor = new RECT();

            /// <summary>
            /// </summary>            
            public RECT rcWork = new RECT();

            /// <summary>
            /// </summary>            
            public int dwFlags = 0;
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        private static System.IntPtr WindowProc(System.IntPtr hwnd, int msg, System.IntPtr wParam, System.IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return (System.IntPtr)0;
        }

        /// <summary>
        /// 获得并设置窗体大小信息
        /// </summary>
        private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            System.IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != System.IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);

                // 这行如果不注释掉在多显示器情况下显示会有问题！ 
                // mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);

                // 可设置窗体的最小尺寸
                // mmi.ptMinTrackSize.x = 300;
                // mmi.ptMinTrackSize.y = 200;
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }


    }
}
