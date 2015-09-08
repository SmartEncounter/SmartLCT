using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Nova.SmartLCT.Interface
{
    public class CommonStaticMethod
    {
        public static GlobalParameters GetGlobalParamsFormResources()
        {
            return (GlobalParameters)Application.Current.FindResource(LCTConstData.GLOBAL_PARAMES_KEY);
        }

        public static bool GetLanguageString(string defaultStr, string key, out string msg)
        {
            msg = (string)Application.Current.TryFindResource(key);
            if (msg == null)
            {
                msg = defaultStr;
                return false;
            }
            else
            {
                return true;
            }
        }

        public static RepetitionState GetWeekRepetition(List<DayOfWeek> weekList)
        {
            if (weekList == null ||
                weekList.Count == 0)
            {
                return RepetitionState.Custom;
            }
            
            bool isAllDay = true;
            if (weekList.Count != 7)
            {
                isAllDay = false;
            }

            bool isOneToFine = true;
            if (weekList.Count != 5)
            {
                isOneToFine = false;
            }

            for (int i = 0; i < weekList.Count; i++)
            {
                if (weekList[i] == DayOfWeek.Saturday ||
                    weekList[i] == DayOfWeek.Sunday)
                {
                    isOneToFine = false;
                }
            }

            if (isAllDay)
            {
                return RepetitionState.EveryDay;
            }

            if(isOneToFine)
            {
                return RepetitionState.MonToFri;
            }

            return RepetitionState.Custom;
        }

        public static bool SaveApplicationConfig(Stream stream, AppConfiguration config)
        {
            XmlSerializer xmls = null;
            StreamWriter sw = null;
            string msg = string.Empty;
            try
            {
                xmls = new XmlSerializer(typeof(AppConfiguration));
                sw = new StreamWriter(stream);
                //XmlWriterSettings setting = new XmlWriterSettings();
                //setting.CloseOutput = true;

                XmlWriter xmlWriter = XmlWriter.Create(sw);
                xmls.Serialize(sw, config);
                sw.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        public static bool LoadApplicationConfig(Stream stream, out AppConfiguration config)
        {
            XmlSerializer xmls = null;
            StreamReader sr = null;
            XmlReader xmlReader = null;
            string msg = string.Empty;
            config = null;
            try
            {
                xmls = new XmlSerializer(typeof(AppConfiguration));
                sr = new StreamReader(stream);
                xmlReader = XmlReader.Create(sr);
                if (!xmls.CanDeserialize(xmlReader))
                {
                    return false;
                }
                else
                {
                    config = (AppConfiguration)xmls.Deserialize(xmlReader);
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
            finally
            {

                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }
        }

    }
}
