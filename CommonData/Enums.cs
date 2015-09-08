using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using Nova.LCT.GigabitSystem.Common;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace Nova.SmartLCT.Interface
{
    public enum OperateState
    {
        Add,
        Edit,
        None
    }
    
    ///<summary>
    ///重复状态
    ///</summary>
    public enum RepetitionState
    {
        EveryDay,
        MonToFri,
        Custom
    }

    public enum Week
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    /// <summary>
    /// 缩放状态
    /// </summary>
    public enum IncreaseOrDecreaseState
    {
        Increase,
        Decrease,
        None
    }
    public enum OperatEnvironment
    {
        DesignScreen,
        AdjustScreenLocation,
        AdjustSenderLocation
    }
    public enum ArrangeType
    {
        LeftTop_Hor,
        RightTop_Hor,
        LeftBottom_Hor,
        RightBottom_Hor,
        LeftTop_Ver,
        RightTop_Ver,
        LeftBottom_Ver,
        RightBottom_Ver,

    }
    public enum ScannerSizeType
    {
        Custom,
        NoCustom
    }
    public enum AlignmentState
    {
        BottomAlignment,
        TopAlignment,
        LeftAlignment,
        RightAlignment,
        LevelMiddleAlignment,
        PlumbMiddleAlignment,
        CancelLevelSpace,
        CancelPlumbSpace,
        None
    }
    public enum OperateScreenType
    {
        CreateScreen,
        UpdateScreen,
        CreateScreenAndOpenConfigFile
    }
   
    public enum ConnectLineType
    {
        Auto,
        Manual,
        None
    }


    public enum ResizeType
    {
        TL,
        TC,
        TR,
        CL,
        CC,
        CR,
        BL,
        BC,
        BR
    }


    public enum SelectedState
    {
        None,
        Selected,
        SpecialSelected,
        FrameSelected,
        SelectedAll
    }
    public enum UnDoOrReDoState
    {
        UnDo,
        ReDo,
        None
    }
    public enum TagInfo
    {
        sender,
        port,
        receive,
        None,
        xsp
    }
    [Serializable]
    public enum ElementType
    {
        sender,
        port,
        receive,
        None,
        screen,
        baselayer,
        groupframe,
        baseScreen,
        newLayer,
        frameSelected
    }
    public enum CommMsgType
    {
        Error,
        Information,
        Alarm
    }
    public enum SizeChangedState
    {
        Fill,
        LeftToRight,
        TopToBottom
    }

    public enum OperactorState
    {
        Copy,
        Cut,
        None
    }
    public enum MouseState
    {
        MouseLeftDoubleDown,
        MouseUp,
        None
    }
}
