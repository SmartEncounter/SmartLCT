using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading;
using System.Windows.Media.Effects;

namespace DNBSoft.WPF
{
    public class WindowResizer
    {
        private Window target = null;

        private bool resizeRight = false;
        private bool resizeLeft = false;
        private bool resizeUp = false;
        private bool resizeDown = false;

        private Dictionary<UIElement, short> leftElements = new Dictionary<UIElement, short>();
        private Dictionary<UIElement, short> rightElements = new Dictionary<UIElement, short>();
        private Dictionary<UIElement, short> upElements = new Dictionary<UIElement, short>();
        private Dictionary<UIElement, short> downElements = new Dictionary<UIElement, short>();

        private PointAPI startMousePoint = new PointAPI();          //鼠标左键点下时的坐标点
        private Size startWindowSize = new Size();                  //鼠标左键点下时,窗口的大小
        private Point startWindowLeftUpPoint = new Point();         //鼠标左键点下时,窗口左上角坐标点位置
        private static int workAreaMaxHeight = -1;                          //工作区域最大高度

        public int minWidth = 200;
        public int minHeight = 200;

        private HwndSource hs;

        private delegate void RefreshDelegate();

        public WindowResizer(Window target)
        {
            this.target = target;

            if (target == null)
            {
                throw new Exception("Invalid Window handle");
            }

            //窗口大小发生变化时，及时调整窗口最大高度和宽度，防止遮盖任务栏
            this.target.SourceInitialized += new EventHandler(MyMacClass_SourceInitialized);
        }

        #region 这一部分用于最大化时不遮蔽任务栏

        private void MyMacClass_SourceInitialized(object sender, EventArgs e)
        {
            hs = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            hs.AddHook(new HwndSourceHook(WndProc));
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:/* WM_GETMINMAXINFO */
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
                default: break;
            }
            return (System.IntPtr)0;
        }

        private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
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

                // 这行如果不注释掉在多显示器情况下显示会有问题！ 
                // mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
                workAreaMaxHeight = mmi.ptMaxSize.y;

                //当窗口的任务栏自动隐藏时，最大化的时候最下边要留几个像素的空白，否则此窗口将屏幕铺满，则鼠标移到最下边时，任务栏无法自动弹出
                if (rcWorkArea.Height == rcMonitorArea.Height)
                {
                    mmi.ptMaxSize.y -= 2;
                }
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

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

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

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

        #endregion

        #region add resize components
        private void connectMouseHandlers(UIElement element)
        {
            element.MouseLeftButtonDown += new MouseButtonEventHandler(element_MouseLeftButtonDown);
            element.MouseEnter += new MouseEventHandler(element_MouseEnter);
            element.MouseLeave += new MouseEventHandler(setArrowCursor);
        }

        public void addResizerRight(UIElement element)
        {
            connectMouseHandlers(element);
            rightElements.Add(element, 0);
        }

        public void addResizerLeft(UIElement element)
        {
            connectMouseHandlers(element);
            leftElements.Add(element, 0);
        }

        public void addResizerUp(UIElement element)
        {
            connectMouseHandlers(element);
            upElements.Add(element, 0);
        }

        public void addResizerDown(UIElement element)
        {
            connectMouseHandlers(element);
            downElements.Add(element, 0);
        }

        public void addResizerRightDown(UIElement element)
        {
            connectMouseHandlers(element);
            rightElements.Add(element, 0);
            downElements.Add(element, 0);
        }

        public void addResizerLeftDown(UIElement element)
        {
            connectMouseHandlers(element);
            leftElements.Add(element, 0);
            downElements.Add(element, 0);
        }

        public void addResizerRightUp(UIElement element)
        {
            connectMouseHandlers(element);
            rightElements.Add(element, 0);
            upElements.Add(element, 0);
        }

        public void addResizerLeftUp(UIElement element)
        {
            connectMouseHandlers(element);
            leftElements.Add(element, 0);
            upElements.Add(element, 0);
        }
        #endregion

        #region resize handlers
        private void element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GetCursorPos(out startMousePoint);
            startWindowSize = new Size(target.Width, target.Height);
            startWindowLeftUpPoint = new Point(target.Left, target.Top);

            #region updateResizeDirection
            UIElement sourceSender = (UIElement)sender;
            if (leftElements.ContainsKey(sourceSender))
            {
                resizeLeft = true;
            }
            if (rightElements.ContainsKey(sourceSender))
            {
                resizeRight = true;
            }
            if (upElements.ContainsKey(sourceSender))
            {
                resizeUp = true;
            }
            if (downElements.ContainsKey(sourceSender))
            {
                resizeDown = true;
            }
            #endregion

            Thread t = new Thread(new ThreadStart(updateSizeLoop));
            t.Name = "Mouse Position Poll Thread";
            t.Start();
        }

        private void updateSizeLoop()
        {
            try
            {
                while (resizeDown || resizeLeft || resizeRight || resizeUp)
                {
                    target.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new RefreshDelegate(updateSize));
                    target.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new RefreshDelegate(updateMouseDown));
                    Thread.Sleep(25);
                }

                target.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new RefreshDelegate(setArrowCursor));
            }
            catch (Exception)
            {
            }
        }

        #region updates
        private void updateSize()
        {
            PointAPI currentMousePoint = new PointAPI();
            GetCursorPos(out currentMousePoint);

            try
            {
                if (resizeRight)
                {
                    //如果当鼠标左键点下时,窗口的宽度已经达到最小值,
                    if (target.Width == minWidth)
                    {
                        //鼠标向右移动,表示当前是将窗口变大,此时窗口只能变大,因为已经达到最小宽度了
                        if ((target.Left+target.Width - currentMousePoint.X) < 0)
                        {
                            target.Width = this.target.Width - (target.Left + target.Width - currentMousePoint.X);
                        }
                    }
                    else
                    {
                        //此时,窗口可大可小
                        if ((this.target.Width - (target.Left + target.Width - currentMousePoint.X)) >= minWidth)
                        {
                            target.Width = this.target.Width - (target.Left + target.Width - currentMousePoint.X);
                        }
                        else
                        {
                            //此时,只能限制窗口为最小值,不能继续变小
                            target.Width = minWidth;
                        }
                    }
                }

                if (resizeDown)
                {
                    if (target.Height == minHeight)
                    {
                        if ((target.Top+target.Height - currentMousePoint.Y) < 0)
                        {
                            //限制窗口的最底端，不能低于任务栏，即不能被任务栏遮挡
                            if (workAreaMaxHeight > 0)
                            {
                                target.Height = (((target.Height - (target.Top + target.Height - currentMousePoint.Y)) + target.Top) <= workAreaMaxHeight) ? (target.Height - (target.Top + target.Height - currentMousePoint.Y)) : (workAreaMaxHeight - target.Top);
                            }
                            else
                            {
                                target.Height = target.Height - (target.Top + target.Height - currentMousePoint.Y);
                            }
                        }
                    }
                    else
                    {
                        if ((startWindowSize.Height - (startMousePoint.Y - currentMousePoint.Y)) >= minHeight)
                        {
                            if (workAreaMaxHeight > 0)
                            {
                                target.Height = (((startWindowSize.Height - (startMousePoint.Y - currentMousePoint.Y)) + target.Top) <= workAreaMaxHeight) ? (startWindowSize.Height - (startMousePoint.Y - currentMousePoint.Y)) : (workAreaMaxHeight - target.Top);
                            }
                            else
                            {
                                target.Height = startWindowSize.Height - (startMousePoint.Y - currentMousePoint.Y);
                            }
                        }
                        else
                        {
                            target.Height = minHeight;
                        }
                    }
                }

                if (resizeLeft)
                {
                    if (target.Width == minWidth)
                    {
                        if ((target.Left - currentMousePoint.X) > 0)
                        {
                            target.Width = target.Width + (target.Left - currentMousePoint.X);
                            target.Left = target.Left - (target.Left - currentMousePoint.X);
                        }
                    }
                    else
                    {
                        if ((this.target.Width + (target.Left - currentMousePoint.X)) >= minWidth)
                        {
                            target.Width = target.Width + (target.Left - currentMousePoint.X);
                            target.Left = target.Left - (target.Left - currentMousePoint.X);
                        }
                        else
                        {
                            target.Width = minWidth;
                        }
                    }
                }

                if (resizeUp)
                {
                    if (target.Height == minHeight)
                    {
                        if ((target.Top - currentMousePoint.Y) > 0)
                        {
                            target.Height = target.Height + (target.Top - currentMousePoint.Y);
                            target.Top = target.Top - (target.Top - currentMousePoint.Y);
                        }
                        else
                        {
                            target.Height = minHeight;
                        }
                    }
                    else
                    {
                        if ((target.Height + (target.Top - currentMousePoint.Y)) >= minHeight)
                        {
                            target.Height = target.Height + (target.Top - currentMousePoint.Y);
                            target.Top = target.Top - (target.Top - currentMousePoint.Y);
                        }
                        else
                        {
                            target.Height = minHeight;
                        }
                    }
                }
            }
            catch
            {
            }
        }
        private void updateMouseDown()
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
            {
                resizeRight = false;
                resizeLeft = false;
                resizeUp = false;
                resizeDown = false;
            }
        }
        #endregion
        #endregion

        #region cursor updates
        private void element_MouseEnter(object sender, MouseEventArgs e)
        {
            bool resizeRight = false;
            bool resizeLeft = false;
            bool resizeUp = false;
            bool resizeDown = false;

            UIElement sourceSender = (UIElement)sender;
            if (leftElements.ContainsKey(sourceSender))
            {
                resizeLeft = true;
            }
            if (rightElements.ContainsKey(sourceSender))
            {
                resizeRight = true;
            }
            if (upElements.ContainsKey(sourceSender))
            {
                resizeUp = true;
            }
            if (downElements.ContainsKey(sourceSender))
            {
                resizeDown = true;
            }

            if ((resizeLeft && resizeDown) || (resizeRight && resizeUp))
            {
                setNESWCursor(sender, e);
            }
            else if ((resizeRight && resizeDown) || (resizeLeft && resizeUp))
            {
                setNWSECursor(sender, e);
            }
            else if (resizeLeft || resizeRight)
            {
                setWECursor(sender, e);
            }
            else if (resizeUp || resizeDown)
            {
                setNSCursor(sender, e);
            }
        }

        private void setWECursor(object sender, MouseEventArgs e)
        {
            target.Cursor = Cursors.SizeWE;
        }

        private void setNSCursor(object sender, MouseEventArgs e)
        {
            target.Cursor = Cursors.SizeNS;
        }

        private void setNESWCursor(object sender, MouseEventArgs e)
        {
            target.Cursor = Cursors.SizeNESW;
        }

        private void setNWSECursor(object sender, MouseEventArgs e)
        {
            target.Cursor = Cursors.SizeNWSE;
        }

        private void setArrowCursor(object sender, MouseEventArgs e)
        {
            if (!resizeDown && !resizeLeft && !resizeRight && !resizeUp)
            {
                target.Cursor = Cursors.Arrow;
            }
        }

        private void setArrowCursor()
        {
            target.Cursor = Cursors.Arrow;
        }
        #endregion

        #region external call
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out PointAPI lpPoint);

        private struct PointAPI
        {
            public int X;
            public int Y;
        }
        #endregion
    }
}
