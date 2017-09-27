using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    public class SMSLog
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(SMSLog));
        
        public static void Log(string describe)
        {
            try
            {
                logger.Debug(describe);
                if (GlobalModel.WriteLogHandler != null && GlobalModel.IsShowLog)
                {
                    GlobalModel.WriteLogHandler(describe);
                }
            }
            catch (Exception)
            { 
            }
        }

        public static void Debug(string trace, string describe)
        {

            try
            {
                logger.Debug("[DEBUG](" + trace + ") " + describe);
                if (GlobalModel.WriteLogHandler != null && GlobalModel.IsShowLog)
                {
                    GlobalModel.WriteLogHandler("[DEBUG](" + trace + ") " + describe);
                }
            }
            catch (Exception)
            { 
            }
            
        }

        public static void Debug(string describe)
        {

            try
            {
                logger.Debug("[DEBUG] " + describe);
                if (GlobalModel.WriteLogHandler != null && GlobalModel.IsShowLog)
                {
                    GlobalModel.WriteLogHandler("[DEBUG] " + describe);
                }
            }
            catch (Exception)
            { 
            }

        }

        public static void Debug(string describe,bool isShow)
        {

            try
            {
                logger.Debug("[DEBUG] " + describe);
                if (GlobalModel.WriteLogHandler != null && isShow)
                {
                    GlobalModel.WriteLogHandler("[DEBUG] " + describe);
                }
            }
            catch (Exception)
            { 
            }

        }


        public static void Error(string describe)
        {

            try
            {
                logger.Error("[ERROR] " + describe);
                if (GlobalModel.WriteLogHandler != null && GlobalModel.IsShowLog)
                {
                    GlobalModel.WriteLogHandler("[ERROR] " + describe);
                }
            }
            catch (Exception)
            {  
            }
             
        }

        public static void Error(string trace, string describe)
        {

            try
            {
                logger.Error("[ERROR](" + trace + ") " + describe);
                if (GlobalModel.WriteLogHandler != null && GlobalModel.IsShowLog)
                {
                    GlobalModel.WriteLogHandler("[ERROR](" + trace + ") " + describe);
                }
            }
            catch (Exception)
            { 
            }
            
        }


    }
}
