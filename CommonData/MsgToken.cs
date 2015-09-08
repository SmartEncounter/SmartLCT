using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.SmartLCT.Interface
{
    public class MsgToken
    {
        public static string MSG_ZORDER_CHANGED = "ZOrderChanged";
        public static string MSG_DOUBLECLICKRELAY = "DoubleClickRelay";
        public static string MSG_MOUSEUP = "MouseUp";
        public static string MSG_X_CHANGED="XChanged";
        public static string MSG_Y_CHANGED = "YChanged";
        public static string MSG_RIGHTBUTTONDOWN_CHANGED = "RightButtonDownChanged";
        public static string MSG_Y_DECREASECHANGED = "MsgYDecreaseChanged";
        public static string MSG_X_DECREASECHANGED = "MsgXDecreaseChanged";
        public static string MSG_ADDRECEIVE = "MsgAddReceive";
        public static string MSG_COMPLETEADDRECEIVE = "MsgCompleteAddReceive";

        public static string MSG_SHOWCHANGEDSENDERTYPE = "MsgShowChangedSenderType";
        public static string MSG_SHOWCHANGEDSENDETTYPE_CANCEL = "MsgShowChangedSenderType_Cancel";
        public static string MSG_SHOWCHANGEDSENDETTYPE_OK = "MsgShowChangedSenderType_OK";
        public static string MSG_SHOWCHANGEDSENDETTYPE_CLOSE = "MsgShowChangedSenderType_Close";
        public static string MSG_ISMODIFYSENDERTYPE = "MsgIsModifySenderType";
        public static string MSG_INCREASEORDECREASEINDEX = "MsgIncreaseOrDecreaseIndex";
        public static string MSG_CHANGERECEIVESIZE = "MsgChangeReceiveSize";
        public static string MSG_SHOWGUIDETWO = "MsgShowGuideTwo";
        public static string MSG_CLOSE_GUIDETWOFORM = "MsgCloseGuideTwo";
        //public static string MSG_CREATEANDSHOWSCREEN = "MsgCreateAndShowScreen";
        public static string MSG_CLOSESTARTWINDOW = "MsgCloseStartWindow";
        public static string MSG_SHOWGUIDETWOINWINDOW = "MsgShowGuideTwoInWindow";
        #region 导出导出接收卡配置文件
        public static string MSG_EXPORTCONFIGFOLE = "ExportConfigFile";
        public static string MSG_IMPORTCONFIGFOLE = "ImportConfigFile";
        public static string MSG_EXPORTFILE_OK = "Button_ExportFile_Ok";
        public static string MSG_EXPORTFILE_CANCEL = "Button_ExportFile_Cancel";
        public static string MSG_EXPORTFILE_CLOSE = "Win_ExportFile_Close";
        public static string MSG_INPORTFILE_OK = "Button_ImportFile_Ok";
        public static string MSG_INPORTFILE_CANCEL = "Button_ImportFile_Cancel";
        public static string MSG_INPORTFILE_CLOSE = "Win_ImportFile_Close";
        public static string MSG_DELETEMSG_MSGBOX = "Win_IsDeleteFile";
        public static string MSG_EQUIPMENTTYPE_MESSAGEBOX = "Win_Errormsg";
        public static string Msg_PROGRESS_MSG = "Win_ProgressMsg";
        public static string MSG_PROGRESS_CLOSE = "Win_ProgressClose";

        public static string MSG_SHOWSETCUSTOMRECEIVESIZE = "MsgSetCustomReceiveSize";
        public static string MSG_SETCUSTOMRECEIVE_CLOSE = "MsgSetCustomReceiveClose";
        public static string MSG_SETCUSTOMRECEIVEFILE_CANCEL = "MsgSetCustomReceiveCancel";
        public static string MSG_SETCUSTOMRECEIVEFILE_OK = "MsgSetCustomReceiveOk";
        public static string MSG_SETCUSTOMRECEIVE_CANCEL = "MsgSetCustomReceiveCancel";
        #endregion

        public static string MSG_SELECTEDLAYERANDELEMENT_CHANGED = "SelectedLayerAndElementChanged";
        public static string MSG_EXIT = "Exit";
        public static string MSG_CLOSEADDPORTSTATE = "CloseAddPortState";
        public static string MSG_CLOSE_ADDPORTFORM = "CloseAddPortForm";
        public static string MSG_OPENADDPORTFORM = "OpenAddPortForm";
        public static string MSG_OPENADDSENDERFORM = "OpenAddSenderForm";
        public static string MSG_CLOSE_ADDSENDERFORM = "CloseAddSenderForm";
        public static string MSG_RECORD_ACTIONVALUE = "RecordActionValue";
        public static string MSG_STARTUP_COMPLETED = "MsgStartUpCompleted";
        public static string MSG_SHWOGLOBALMSGBOX = "ShowGlobalMessageBox";
        public static string MSG_SHOWGLOBALQUESTIONMSGBOX = "ShowGlobalQuestionMessageBox";
        public static string MSG_RESETGLOBALPROCESSMSG = "ResetGlobalProcessMsg";
        public static string MSG_SHOWGLOBALPROCESSEND = "ShowGlobalProcessEnd";
        public static string MSG_SHOWGLOBALPROCESSBEGIN = "ShowGlobalProcessBegin";
        public static string MSG_SHOWADDSMARTBRIGHT = "ShowAddSmartBright";
        public static string MSG_ADDBRIGHTTIMINGADJUSTDATA_CLOSE = "AddBrightTimingAdjustData_Close";
        public static string MSG_ADDBRIGHTTIMINGADJUSTDATA_CANCEL = "AddBrightTimingAdjustData_Cancel";
        public static string MSG_ADDBRIGHTTIMINGADJUSTDATA_OK = "AddBrightTimingAdjustData_Ok";
        public static string MSG_SHOWEditTIMINGADJUSTBRIGHT = "ShowEditTimingAdjustBright";
        public static string MSG_WINBRIGHTADJUST_CLOSE = "WinBrightAdjust_Close";
        public static string MSG_WINBRIGHTADJUST_CANCEL = "WinBrightAdjust_Cancel";
        public static string MSG_WINBRIGHTADJUST_OK = "WinBrightAdjust_OK";
        public static string MSG_SHOWFASTSEGMENTATION = "ShowFastSegmentation";
        public static string MSG_WINFASTSEGMENTATION_CLOSE = "WINFastSegmentation_Close";
        public static string MSG_WINFASTSEGMENTATION_OK = "WINFastSegmentation_OK";
        public static string MSG_WINFASTSEGMENTATION_CANCEL = "WINFastSegmentation_Cancel";
        public static string MSG_SHOWWINPERIPHERALSCONFIG = "ShowWinPeripheralsConfig";

        public static string MSG_SCREENINFO_CHANGED = "ScreenInfoChanged";

        #region 模块窗口打开
        public static string MSG_SHOWEQUIPMENTMANAGER = "ShowEquipmentManager";
        public static string MSG_SHOWSCANBOARDCONFIGMANAGER = "ShowScanBoardConfigManager";
        public static string MSG_SHOWEDIDMANAGER = "ShowEDIDManager";
        public static string MSG_SHOWBRIGHTADJUST = "ShowBrightAdjust";
        public static string MSG_SHOWLANGUAGE_SEL = "ShowLanguageSel";
        #endregion

        #region 模块关闭
        public static string MSG_CLOSELANGUAGE_SEL = "CloseLanguageSel";
        #endregion
    }
}
