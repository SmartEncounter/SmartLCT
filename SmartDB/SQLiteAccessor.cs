using Nova.Database.DataBaseManager;
using Nova.LCT.GigabitSystem.Common;
using Nova.SmartLCT.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Nova.SmartLCT.Database
{
    public class SQLiteAccessor : IDisposable
    {
        #region 属性
        public static SQLiteAccessor Instance
        {
            get
            {
                if (_sqliteAcc == null)
                {
                    _sqliteAcc = new SQLiteAccessor();
                    Initialize();
                }
                return _sqliteAcc;
            }
        }
        private static SQLiteAccessor _sqliteAcc = null;
        #endregion

        #region 字段
        private static DbSqLiteHelper _helper = null;
        #endregion

        #region 构造函数

        #endregion
        
        #region 公有函数
        private static bool Initialize()
        {
            bool res = CheckFileExistAndMoveTo();
            if (!res)
            {
                return false;
            }
            _helper = new DbSqLiteHelper(ConstValue.SMART_CONFIG_DB_NAME, "");
            _helper.ConnectionString = "DataSource=" + ConstValue.SMART_CONFIG_DB_NAME;
            res = _helper.ConnectionInit();
            if (!res)
            {
                return false;
            }

            return true;
        }

        public SmartBrightSeleCondition LoadDisplayEasyConfig(string displayUDID)
        {
            string sql = "SELECT rowid, DisplayUDID, LastModifyTime, ConfigurationFile, ConfigFileVersion, BrightAdjustMode FROM DisplaySSmartBrightEasyConfig";
            
            {
                sql += " WHERE DisplayUDID='" + displayUDID + "'";
            }

            DataTable dt = _helper.ExecuteDataTable(sql);
            
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            DataRow row = dt.Rows[0];

            byte[] fileData = (byte[])row["ConfigurationFile"];

            MemoryStream stream = new MemoryStream(fileData);
            DisplaySmartBrightEasyConfig config;
            bool res = CustomTransform.LoadSmartBrightEasyConfig(stream, out config);
           
            if (!res)
            {
                return null;
            }

            SmartBrightSeleCondition condition = null;
            try
            {
                condition = new SmartBrightSeleCondition()
                {
                    BrightAdjMode = (BrightAdjustMode)((int)(short)row["BrightAdjustMode"]),
                    DataVersion = (int)(short)row["ConfigFileVersion"],
                    EasyConfig = config
                };
            }
            catch (Exception e)
            {

            }

            return condition;
        }

        public bool SaveDisplayEasyConfig(string displayUDID, SmartBrightSeleCondition condition)
        {
            MemoryStream stream = new MemoryStream();
            bool res = CustomTransform.SaveSmartBrightEasyConfig(stream, condition.EasyConfig);
            if (!res)
            {
                return false;
            }

            string sql = "SELECT rowid, DisplayUDID, LastModifyTime, ConfigurationFile, ConfigFileVersion, BrightAdjustMode FROM DisplaySSmartBrightEasyConfig WHERE DisplayUDID='" + displayUDID + "'";
            DataTableUpdate dtUpdate = _helper.ExecuteDataTableUpdate(sql);
            
            DataRow row = null;
            bool isAdd = false;
            if (dtUpdate.DtResult.Rows.Count == 0)
            {
                row = dtUpdate.DtResult.NewRow();
                isAdd = true;
            }
            else
            {
                row = dtUpdate.DtResult.Rows[0];
            }

            row["DisplayUDID"] = condition.EasyConfig.DisplayUDID;
            row["LastModifyTime"] = DateTime.Now;
            row["ConfigurationFile"] = stream.ToArray();
            row["ConfigFileVersion"] = condition.DataVersion;
            row["BrightAdjustMode"] = (int)condition.BrightAdjMode;

            if (isAdd)
            {
                dtUpdate.DtResult.Rows.Add(row);
            }
            dtUpdate.DtResult.TableName = "DisplaySSmartBrightEasyConfig";
            int isOK = _helper.DataTableUpdateToDB(dtUpdate);
            if (isOK == -5)
            {
                return false;
            }
            return true;
        }

        public List<DisplaySmartBrightEasyConfig> GetAllNeedEasyConfig(SmartBrightSeleCondition condition)
        {
            List<DisplaySmartBrightEasyConfig> dataList = new List<DisplaySmartBrightEasyConfig>();

            string sql = "SELECT rowid, DisplayUDID, LastModifyTime, ConfigurationFile, ConfigFileVersion, BrightAdjustMode FROM DisplaySSmartBrightEasyConfig ";
            sql += " WHERE BrightAdjustMode=" + (int)condition.BrightAdjMode;

            DataTable dt = _helper.ExecuteDataTable(sql);

            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                byte[] fileData = (byte[])dt.Rows[i]["ConfigurationFile"];
                
                MemoryStream stream = new MemoryStream(fileData);
                DisplaySmartBrightEasyConfig config;
                bool res = CustomTransform.LoadSmartBrightEasyConfig(stream, out config);
           
                if (res)
                {
                    dataList.Add(config);
                }
            }

            return dataList;
            
        }

        public AppConfiguration LoadAppConfig()
        {
            string sql = "SELECT rowid, AppConfig FROM AppConfiguration";

            {
                //sql += " WHERE rowid=1";
            }

            DataTable dt = _helper.ExecuteDataTable(sql);

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            DataRow row = dt.Rows[0];

            byte[] fileData = (byte[])row["AppConfig"];

            MemoryStream stream = new MemoryStream(fileData);
            AppConfiguration config;
            bool res = CommonStaticMethod.LoadApplicationConfig(stream, out config);

            if (!res)
            {
                return null;
            }

            return config;
        }

        public bool SaveAppConfig(AppConfiguration config)
        {
            MemoryStream stream = new MemoryStream();
            bool res = CommonStaticMethod.SaveApplicationConfig(stream, config);
            if (!res)
            {
                return false;
            }

            string sql = "SELECT rowid, AppConfig FROM AppConfiguration";
            DataTableUpdate dtUpdate = _helper.ExecuteDataTableUpdate(sql);

            DataRow row = null;
            bool isAdd = false;
            if (dtUpdate.DtResult.Rows.Count == 0)
            {
                row = dtUpdate.DtResult.NewRow();
                isAdd = true;
            }
            else
            {
                row = dtUpdate.DtResult.Rows[0];
            }

            row["AppConfig"] = (byte[])stream.ToArray();

            if (isAdd)
            {
                dtUpdate.DtResult.Rows.Add(row);
            }
            dtUpdate.DtResult.TableName = "AppConfiguration";
            int isOK = _helper.DataTableUpdateToDB(dtUpdate);
            if (isOK == -5)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region 私有函数
        private static bool CheckFileExistAndMoveTo()
        {
            if (File.Exists(ConstValue.SMART_CONFIG_DB_NAME))
            {
                return true;
            }
            else
            {
                if (!File.Exists(ConstValue.SMART_CONFIG_DB_NAME_Original))
                {
                    return false;
                }

                string path = Path.GetDirectoryName(ConstValue.SMART_CONFIG_DB_NAME);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                File.Copy(ConstValue.SMART_CONFIG_DB_NAME_Original, ConstValue.SMART_CONFIG_DB_NAME);

                return true;
            }
        }
        #endregion

        public void Dispose()
        {
            if (_helper != null)
            {
                _helper.Dispose();
            }
            _helper = null;
            _sqliteAcc = null;
        }
    }
}
