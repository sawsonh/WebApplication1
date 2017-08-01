using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Core.Services
{
    public interface ILoggingService
    {
        void Debug(string message);
        void Debug(string message, params object[] args);

        void Info(string message);
        void Info(string message, params object[] args);

        void Warning(string message);
        void Warning(string message, params object[] args);

        void Error(string message);
        void Error(string message, params object[] args);
        void Error(Exception exception, string message = null, bool isStackTraceIncluded = true);

        void Fatal(string message);
        void Fatal(string message, params object[] args);
        void Fatal(Exception exception, string message = null, bool isStackTraceIncluded = true);
    }
}
