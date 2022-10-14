using System;
using log4net;
using log4net.Core;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log.config", Watch = true)]

namespace PPS.Logging
{
	public static class Log
	{
	    private const string StatsLogName = "Statistics";
		private static readonly ILog _iLog;
	    private static readonly ILog _statsLog;

		static Log()
		{
			_iLog = Create();

            if (!_iLog.Logger.Repository.Configured)
			{
				XmlConfigurator.Configure(_iLog.Logger.Repository);
			}

		    _statsLog = Create(StatsLogName);

		    if (!_statsLog.Logger.Repository.Configured)
		    {
		        XmlConfigurator.Configure(_statsLog.Logger.Repository);
		    }
        }

        public static ILog Default
		{
			get { return _iLog; }
		}

	    public static ILog StatsLog
	    {
	        get { return _statsLog; }
	    }

		private static ILog _data;

		public static ILog Data
		{
			get
			{
				if (_data == null)
					_data = Create("PPS.Core.Data");

				return _data;
			}
		}

		public static ILog Create()
		{
			return Create("PPS");
		}

		public static ILog Create(string name)
		{
			return LogManager.GetLogger(name);
		}
	}

	public static class LogMessage
	{
		public const string Start = "Start";
		public const string End = "End";
	}

	/// <summary>
	/// LoggingExtensions class
	/// </summary>
	public static class LoggingExtensions
	{
		/// <summary>
		/// Debugs the specified log.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="formattingCallback">The formatting callback.</param>
		public static void Debug(this ILog log, Func<string> formattingCallback)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug(formattingCallback());
			}
		}

		/// <summary>
		/// Infoes the specified log.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="formattingCallback">The formatting callback.</param>
		public static void Info(this ILog log, Func<string> formattingCallback)
		{
			if (log.IsInfoEnabled)
			{
				log.Info(formattingCallback());
			}
		}

		/// <summary>
		/// Warns the specified log.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="formattingCallback">The formatting callback.</param>
		public static void Warn(this ILog log, Func<string> formattingCallback)
		{
			if (log.IsWarnEnabled)
			{
				log.Warn(formattingCallback());
			}
		}

		/// <summary>
		/// Errors the specified log.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="formattingCallback">The formatting callback.</param>
		public static void Error(this ILog log, Func<string> formattingCallback)
		{
			if (log.IsErrorEnabled)
			{
				log.Error(formattingCallback());
			}
		}

		/// <summary>
		/// Fatals the specified log.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="formattingCallback">The formatting callback.</param>
		public static void Fatal(this ILog log, Func<string> formattingCallback)
		{
			if (log.IsFatalEnabled)
			{
				log.Fatal(formattingCallback());
			}
		}

		/// <summary>
		/// Initializes the logging.
		/// Must be called first
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		public static void InitializeLogging(string fileName)
		{
			if (!LogManager.GetRepository().Configured)
			{
				XmlConfigurator.Configure(new Uri(fileName));
			}
		}

		/// <summary>
		/// Gets the logger.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		public static ILog GetLogger(this Object o)
		{
			return LogManager.GetLogger(o.GetType());
		}

		/// <summary>
		/// Debugs the specified o.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="message">The message.</param>
		public static void Debug(this Object o, object message)
		{
			LogManager.GetLogger(o.GetType()).Debug(message);
		}

		/// <summary>
		/// Debugs the specified o.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public static void Debug(this Object o, object message, Exception exception)
		{
			LogManager.GetLogger(o.GetType()).Debug(message, exception);
		}

		/// <summary>
		/// Debugs the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public static void DebugFormat(this Object o, string format, params object[] args)
		{
			LogManager.GetLogger(o.GetType()).DebugFormat(format, args);
		}

		/// <summary>
		/// Debugs the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The arg0.</param>
		public static void DebugFormat(this Object o, string format, object arg0)
		{
			LogManager.GetLogger(o.GetType()).DebugFormat(format, arg0);
		}

		/// <summary>
		/// Debugs the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The arg0.</param>
		/// <param name="arg1">The arg1.</param>
		public static void DebugFormat(this Object o, string format, object arg0, object arg1)
		{
			LogManager.GetLogger(o.GetType()).DebugFormat(format, arg0, arg1);
		}

		/// <summary>
		/// Debugs the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The arg0.</param>
		/// <param name="arg1">The arg1.</param>
		/// <param name="arg2">The arg2.</param>
		public static void DebugFormat(this Object o, string format, object arg0, object arg1, object arg2)
		{
			LogManager.GetLogger(o.GetType()).DebugFormat(format, arg0, arg1, arg2);
		}

		/// <summary>
		/// Debugs the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="provider">The provider.</param>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public static void DebugFormat(this Object o, IFormatProvider provider, string format, params object[] args)
		{
			LogManager.GetLogger(o.GetType()).DebugFormat(provider, format, args);
		}

		/// <summary>
		/// Infoes the specified o.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="message">The message.</param>
		public static void Info(this Object o, object message)
		{
			LogManager.GetLogger(o.GetType()).Info(message);
		}

		/// <summary>
		/// Infoes the specified o.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public static void Info(this Object o, object message, Exception exception)
		{
			LogManager.GetLogger(o.GetType()).Info(message, exception);
		}

		/// <summary>
		/// Infoes the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public static void InfoFormat(this Object o, string format, params object[] args)
		{
			LogManager.GetLogger(o.GetType()).InfoFormat(format, args);
		}

		/// <summary>
		/// Infoes the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The arg0.</param>
		public static void InfoFormat(this Object o, string format, object arg0)
		{
			LogManager.GetLogger(o.GetType()).InfoFormat(format, arg0);
		}

		/// <summary>
		/// Infoes the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The arg0.</param>
		/// <param name="arg1">The arg1.</param>
		public static void InfoFormat(this Object o, string format, object arg0, object arg1)
		{
			LogManager.GetLogger(o.GetType()).InfoFormat(format, arg0, arg1);
		}

		/// <summary>
		/// Infoes the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The arg0.</param>
		/// <param name="arg1">The arg1.</param>
		/// <param name="arg2">The arg2.</param>
		public static void InfoFormat(this Object o, string format, object arg0, object arg1, object arg2)
		{
			LogManager.GetLogger(o.GetType()).InfoFormat(format, arg0, arg1, arg2);
		}

		/// <summary>
		/// Infoes the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="provider">The provider.</param>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public static void InfoFormat(this Object o, IFormatProvider provider, string format, params object[] args)
		{
			LogManager.GetLogger(o.GetType()).InfoFormat(provider, format, args);
		}

		/// <summary>
		/// Fatals the specified o.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="message">The message.</param>
		public static void Fatal(this Object o, object message)
		{
			LogManager.GetLogger(o.GetType()).Fatal(message);
		}

		/// <summary>
		/// Fatals the specified o.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public static void Fatal(this Object o, object message, Exception exception)
		{
			LogManager.GetLogger(o.GetType()).Fatal(message, exception);
		}

		/// <summary>
		/// Fatals the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public static void FatalFormat(this Object o, string format, params object[] args)
		{
			LogManager.GetLogger(o.GetType()).FatalFormat(format, args);
		}

		/// <summary>
		/// Fatals the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The arg0.</param>
		public static void FatalFormat(this Object o, string format, object arg0)
		{
			LogManager.GetLogger(o.GetType()).FatalFormat(format, arg0);
		}

		/// <summary>
		/// Fatals the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The arg0.</param>
		/// <param name="arg1">The arg1.</param>
		public static void FatalFormat(this Object o, string format, object arg0, object arg1)
		{
			LogManager.GetLogger(o.GetType()).FatalFormat(format, arg0, arg1);
		}

		/// <summary>
		/// Fatals the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The arg0.</param>
		/// <param name="arg1">The arg1.</param>
		/// <param name="arg2">The arg2.</param>
		public static void FatalFormat(this Object o, string format, object arg0, object arg1, object arg2)
		{
			LogManager.GetLogger(o.GetType()).FatalFormat(format, arg0, arg1, arg2);
		}

		/// <summary>
		/// Fatals the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="provider">The provider.</param>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public static void FatalFormat(this Object o, IFormatProvider provider, string format, params object[] args)
		{
			LogManager.GetLogger(o.GetType()).FatalFormat(provider, format, args);
		}

		/// <summary>
		/// Warns the specified o.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="message">The message.</param>
		public static void Warn(this Object o, object message)
		{
			LogManager.GetLogger(o.GetType()).Warn(message);
		}

		/// <summary>
		/// Warns the specified o.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public static void Warn(this Object o, object message, Exception exception)
		{
			LogManager.GetLogger(o.GetType()).Warn(message, exception);
		}

		/// <summary>
		/// Warns the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public static void WarnFormat(this Object o, string format, params object[] args)
		{
			LogManager.GetLogger(o.GetType()).WarnFormat(format, args);
		}

		/// <summary>
		/// Warns the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The arg0.</param>
		public static void WarnFormat(this Object o, string format, object arg0)
		{
			LogManager.GetLogger(o.GetType()).WarnFormat(format, arg0);
		}

		/// <summary>
		/// Warns the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The arg0.</param>
		/// <param name="arg1">The arg1.</param>
		public static void WarnFormat(this Object o, string format, object arg0, object arg1)
		{
			LogManager.GetLogger(o.GetType()).WarnFormat(format, arg0, arg1);
		}

		/// <summary>
		/// Warns the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The arg0.</param>
		/// <param name="arg1">The arg1.</param>
		/// <param name="arg2">The arg2.</param>
		public static void WarnFormat(this Object o, string format, object arg0, object arg1, object arg2)
		{
			LogManager.GetLogger(o.GetType()).WarnFormat(format, arg0, arg1, arg2);
		}

		/// <summary>
		/// Warns the format.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="provider">The provider.</param>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public static void WarnFormat(this Object o, IFormatProvider provider, string format, params object[] args)
		{
			LogManager.GetLogger(o.GetType()).WarnFormat(provider, format, args);
		}

		/// <summary>
		/// Applications the error.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="applicationId">The application id.</param>
		/// <param name="path">The path.</param>
		/// <param name="message">The message.</param>
		public static void ApplicationError(this Object o, int applicationId, string path, string message)
		{
			ILog log = LogManager.GetLogger(o.GetType());
			LoggingEvent loggingEvent = new LoggingEvent(o.GetType(), log.Logger.Repository, log.Logger.Name, Level.Error, message.Trim(), new Exception());
			loggingEvent.Properties["ApplicationId"] = applicationId;
			loggingEvent.Properties["EventPath"] = path;
			log.Logger.Log(loggingEvent);
		}

	}
}
