using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Nova.SmartLCT.Interface
{
    public class Function
    {

        /// <summary>
        /// 获取前一次打开的工程文件的路径
        /// </summary>
        /// <param name="projectFileCollection">最近打开的工程文件路径集合</param>
        /// <returns></returns>
        public static string GetDefaultCurrentProjectPath(ObservableCollection<ProjectInfo> projectFileCollection)
        {
            string projectPath = "";
            if (projectFileCollection.Count != 0)
            {
                //默认路径为前一次打开的文件夹
                string defaultProjectPath = projectFileCollection[0].ProjectPath;
                int lastIndex = defaultProjectPath.LastIndexOf('\\');
                projectPath = defaultProjectPath.Substring(0, lastIndex);
                if (!Directory.Exists(projectPath))
                {
                    projectPath = SmartLCTViewModeBase.ReleasePath + "SmartLCTProjectFile";
                }
            }
            else
            {
                projectPath = SmartLCTViewModeBase.ReleasePath + "SmartLCTProjectFile";
            }
            if (!Directory.Exists(projectPath))
            {
                projectPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            return projectPath;
        }

        /// <summary>
        /// 通过浏览文件的方式，获取文件
        /// </summary>
        /// <param name="InitialDirectory">初始路径</param>
        /// <returns></returns>
        public static string GetConfigFileName(string InitialDirectory)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string configFile = string.Empty;
            string msg = "";
            CommonStaticMethod.GetLanguageString("配置文件", "Lang_SmartLCT_VM_ConfileFile", out msg);
            configFile = msg;
            string allFile = string.Empty;
            CommonStaticMethod.GetLanguageString("所有文件", "Lang_SmartLCT_VM_AllFile", out msg);
            allFile = msg;
            if (InitialDirectory != "")
            {
                ofd.InitialDirectory = InitialDirectory;
            }
            ofd.Filter = configFile + "(*.xml)|*.xml|" + allFile + "(*.*)|*.*";
            if (ofd.ShowDialog() != true)
            {
                return "";
            }
            else
            {               
                return ofd.FileName;
            }
        }

        /// <summary>
        ///获取新建时默认的项目名
        /// </summary>
        /// <param name="projectMainName">项目名的主要文字</param>
        /// <param name="projectFileExtension">项目文件的后缀</param>
        /// <param name="directoryPath">项目文件存放的文件夹</param>
        /// <returns></returns>
        public static string GetDefaultProjectName(string projectMainName, string projectFileExtension, string directoryPath)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            FileInfo[] files = dir.GetFiles();
            ObservableCollection<string> projectnameList = new ObservableCollection<string>();
            foreach (FileInfo info in files)
            {
                if (info.Extension.ToLower() == projectFileExtension && info.Name.Length>projectMainName.Length+projectFileExtension.Length)
                {
                    //保存以SmartLCTProjectFile开头，数字结尾的文件名，数字部分
                    string n = info.Name.Substring(0, projectMainName.Length);
                    if (n == projectMainName)
                    {
                        string str = info.Name.Substring(projectMainName.Length, info.Name.Length - projectMainName.Length - projectFileExtension.Length);
                        bool check = Regex.IsMatch(str, @"^\d+$");
                        if (check && (!projectnameList.Contains(str)))
                        {
                            projectnameList.Add(str);
                        }
                    }
                }
            }
            bool isNoFind = true;
            int i = 1;
            while (isNoFind)
            {
                if (!projectnameList.Contains(i.ToString()))
                {
                    isNoFind = false;
                }
                else
                {
                    i += 1;
                }
            }
            return projectMainName + i.ToString();
        }

        public static void SetElementCollectionX(ObservableCollection<IRectElement> collection, double value)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].X += value;
            }
        }
        public static void SetElementCollectionY(ObservableCollection<IRectElement> collection, double value)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].Y += value;
            }
        }

        public static Size CalculatePortLoadSize(int frameRate, int colorBit)
        {
            Size size = new Size();
            size.Width = 1920;
            size.Height = 1000 * 1000 * 1000 * 0.95 / frameRate / colorBit / 1920;
            return size;
        }
        public static Size CalculateSenderLoadSize(int portcount, int frameRate, int colorBit)
        {
            Size size = new Size();
            //size.Width = 400;
            //size.Height = 200 * portcount;
            size.Width = 1920;
            size.Height = 1000 * 1000 * 1000 * 0.95 / frameRate / colorBit / 1920 * portcount;
            return size;
        }

        public static void SetElementCollectionState(ObservableCollection<IRectElement> collection, SelectedState value)
        {
            if(collection==null || collection.Count==0)
            {
                return;
            }
            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].ElementSelectedState != value)
                {
                    collection[i].ElementSelectedState = value;
                }
            }
        }
        public static void SetElementCollectionState(ObservableCollection<IElement> collection, SelectedState value)
        {
            if (collection == null || collection.Count == 0)
            {
                return;
            }
            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].ElementSelectedState != value)
                {
                    collection[i].ElementSelectedState = value;
                }
            }
        }
        public static void SetElementSelectedState(RectLayer layer, SelectedState state)
        {
            for (int i = 0; i < layer.ElementCollection.Count; i++)
            {
                if (layer.ElementCollection[i] is RectElement)
                {
                    layer.ElementCollection[i].ElementSelectedState = state;
                }
                else if (layer.ElementCollection[i] is RectLayer)
                {
                    SetElementSelectedState((RectLayer)layer.ElementCollection[i], state);
                }
            }
        }

        public static void UpdateSenderConnectInfo(ObservableCollection<SenderConnectInfo> senderConnectInfoList,IElement element)
        {
            //更新发送卡带载
            for (int i = 0; i < senderConnectInfoList.Count; i++)
            {
                for (int j = 0; j < senderConnectInfoList[i].PortConnectInfoList.Count; j++)
                {
                    senderConnectInfoList[i].PortConnectInfoList[j].LoadSize = Function.UnionRectCollection(senderConnectInfoList[i].PortConnectInfoList[j].ConnectLineElementList);
                }
                Rect senderRect= Function.UnionRectCollection(senderConnectInfoList[i].PortConnectInfoList);
                if (senderRect.Width == 0 && senderRect.Height == 0)
                {
                    senderConnectInfoList[i].MapLocation = new Point();
                }
                senderConnectInfoList[i].LoadSize = senderRect;
            }
            //更新整个屏的带载
            IElement screenElement = element;
            while (screenElement!=null && screenElement.EleType != ElementType.baseScreen)
            {
                screenElement = screenElement.ParentElement;
            }
            
            if (screenElement == null)
            {
                return;
            }

            RectLayer myscreen = (RectLayer)screenElement;
            for (int i = 0; i < myscreen.SenderConnectInfoList.Count; i++)
            {
                for (int j = 0; j < myscreen.SenderConnectInfoList[i].PortConnectInfoList.Count; j++)
                {
                    //统计发送卡i网口j的带载(不允许多卡的情况下，多个显示屏共用一个网口)
                    Rect portSize = new Rect();
                    for (int m = 0; m < myscreen.ElementCollection.Count; m++)
                    {
                        if (((RectLayer)myscreen.ElementCollection[m]).EleType == ElementType.newLayer)
                        {
                            continue; 
                        }

                        RectLayer portLayer = (RectLayer)((RectLayer)myscreen.ElementCollection[m]).ElementCollection[0];
                        Rect senderSize = portLayer.SenderConnectInfoList[i].LoadSize;
                        PortConnectInfo portConnectInfo = portLayer.SenderConnectInfoList[i].PortConnectInfoList[j];
                        Rect psize = portConnectInfo.LoadSize;
                        if (psize.Width == 0 || psize.Height == 0)
                        {
                            continue;
                        }

                        Rect realSize = new Rect(psize.X - senderSize.X + portConnectInfo.MapLocation.X, psize.Y - senderSize.Y + portConnectInfo.MapLocation.Y, psize.Width, psize.Height);

                        if (portSize == new Rect())
                        {
                            portSize = realSize;                           
                        }
                        else
                        {
                            portSize = Rect.Union(realSize, portSize);
                        }
                    }
                    myscreen.SenderConnectInfoList[i].PortConnectInfoList[j].LoadSize = portSize;
                    if (portSize.Width == 0 && portSize.Height == 0)
                    {
                        myscreen.SenderConnectInfoList[i].PortConnectInfoList[j].MapLocation = new Point();
                    }
                    if (myscreen.SenderConnectInfoList[i].PortConnectInfoList[j].IsOverLoad)
                    {
                        myscreen.SenderConnectInfoList[i].IsExpand = true;
                    }
                }
            }

            for (int i = 0; i < myscreen.SenderConnectInfoList.Count; i++)
            {
                Rect senderLoadSize = new Rect();
                for (int j = 0; j < myscreen.SenderConnectInfoList[i].PortConnectInfoList.Count; j++)
                {
                    Rect portSize = myscreen.SenderConnectInfoList[i].PortConnectInfoList[j].LoadSize;
                    if (portSize.Width == 0 && portSize.Height == 0)
                    {
                        continue; ;
                    }
                    if (senderLoadSize == new Rect())
                    {
                        senderLoadSize = portSize;
                    }
                    else
                    {
                        senderLoadSize = Rect.Union(senderLoadSize, portSize);
                    }
                }
                myscreen.SenderConnectInfoList[i].LoadSize = senderLoadSize;
            }

        }
        public static void InitMapLocation(RectLayer screen)
        {
            for (int i = 0; i < screen.SenderConnectInfoList.Count; i++)
            {
                for (int j = 0; j < screen.SenderConnectInfoList[i].PortConnectInfoList.Count; j++)
                {
                    PortConnectInfo portInfo = screen.SenderConnectInfoList[i].PortConnectInfoList[j];
                    portInfo.MapLocation = new Point();
                }
            }
        }
       
        public static Rect UnionRectCollection(ObservableCollection<PortConnectInfo> portConnectInfoCollection)
        {
            Rect unionRect = new Rect();
            int index = 0;
            for (int i = 0; i < portConnectInfoCollection.Count; i++)
            {
                if(portConnectInfoCollection[i].ConnectLineElementList!=null
                    && portConnectInfoCollection[i].ConnectLineElementList.Count > 0)
                {
                    index += 1;
                    if (index == 1)
                    {
                        unionRect = portConnectInfoCollection[i].LoadSize;
                    }
                    else
                    {
                        unionRect = Rect.Union(unionRect, portConnectInfoCollection[i].LoadSize);
                    }
                }
            }
            return unionRect;
        }
        public static Rect UnionRectCollection(ObservableCollection<IRectElement> rectCollection)
        {
            Rect unionRect = new Rect();
            
            if (rectCollection!=null &&　rectCollection.Count > 0)
            {
                for (int i = 0; i < rectCollection.Count; i++)
                {
                    if (rectCollection[i].Width == 0 && rectCollection[i].Height == 0)
                    {
                        continue;
                    }
                    if (unionRect == new Rect())
                    {
                        unionRect = new Rect(rectCollection[i].X, rectCollection[i].Y, rectCollection[i].Width, rectCollection[i].Height);
                    }
                    else
                    {
                        Rect rect = new Rect(rectCollection[i].X, rectCollection[i].Y, rectCollection[i].Width, rectCollection[i].Height);
                        unionRect = Rect.Union(unionRect, rect);
                    }
                }
            }
            return unionRect;
        }

        public static bool IsRectIntersect(IRectElement rect1, IRectElement rect2)
        {
            if (rect1 == null || rect2 == null)
            {
                return false;
            }
            Rect rc1 = new Rect(rect1.X, rect1.Y, rect1.Width, rect1.Height);
            Rect rc2 = new Rect(rect2.X, rect2.Y, rect2.Width, rect2.Height);
            return rc1.IntersectsWith(rc2);
        }

        public static int FindSenderCount(ObservableCollection<SenderConnectInfo> senderConnectInfoList, out int senderIndex)
        {
            int senderCount = 0;
            senderIndex = -1;
            for (int i = 0; i < senderConnectInfoList.Count; i++)
            {
                if (senderConnectInfoList[i].LoadSize.Width != 0 && senderConnectInfoList[i].LoadSize.Height != 0)
                {
                    senderIndex = senderConnectInfoList[i].SenderIndex;
                    senderCount += 1;
                }
            }
            return senderCount;
        }
        public static void FindConnectedIndex(RectLayer rectLayer)
        {
            if (rectLayer == null)
            {
                return;
            }
            for (int i = 0; i < rectLayer.ElementCollection.Count; i++)
            {
                if (rectLayer.ElementCollection[i] is RectElement)
                {
                    ((RectElement)rectLayer.ElementCollection[i]).ConnectedIndex = ((RectElement)rectLayer.ElementCollection[i]).ConnectedIndex;
                }
                if (rectLayer.ElementCollection[i] is RectLayer)
                {
                    FindConnectedIndex((RectLayer)rectLayer.ElementCollection[i]);
                }
            }

        }

        public static Color ToColor(string colorName)
        {
            if (colorName.StartsWith("#"))
                colorName = colorName.Replace("#", string.Empty);
            int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
            return new Color()
            {
                A = Convert.ToByte((v >> 24) & 255),
                R = Convert.ToByte((v >> 16) & 255),
                G = Convert.ToByte((v >> 8) & 255),
                B = Convert.ToByte((v >> 0) & 255)
            };
        }

        public static Brush StrToBrush(string str)
        {
            Brush brush;
            brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(str));
            return brush;
        }
    }
}
