using VSP.Business.Entities;

using System;

namespace VSP.Business.Components
{
    public class ErrorHandler
    {
        public static void ProcessException(Exception exception)
        {
            LogException(exception);            
        }

        private static void LogException(Exception exception)
        {
            DateTime timestamp = DateTime.Now;
            ApplicationError err = new ApplicationError();
            err.TimeStamp = timestamp;
            err.Data = exception.Data.ToString();
            err.Message = exception.Message;
            err.Source = exception.Source;
            err.StackTrace = exception.StackTrace;

            err.SaveRecordToDatabase(Guid.Empty);
        }

    }
}
