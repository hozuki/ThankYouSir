using System;
using JetBrains.Annotations;

namespace OpenMLTD.ThankYouSir.Logging {
    /// <summary>
    /// Deep♂Dark♂Fantasy
    /// </summary>
    public static class Ddf {

        public static void Debug([NotNull] object message) {
            if (LoggingEnabled) {
                Logger.Debug(message);
            }
        }

        public static void Debug([NotNull] object message, [NotNull] Exception exception) {
            if (LoggingEnabled) {
                Logger.Debug(message, exception);
            }
        }

        public static void Debug([NotNull] string message) {
            if (LoggingEnabled) {
                Logger.Debug(message);
            }
        }

        [StringFormatMethod("format")]
        public static void DebugFormat([NotNull] IFormatProvider provider, [NotNull] string format, params object[] args) {
            if (LoggingEnabled) {
                Logger.DebugFormat(provider, format, args);
            }
        }

        [StringFormatMethod("format")]
        public static void DebugFormat([NotNull] string format, object arg0) {
            if (LoggingEnabled) {
                Logger.DebugFormat(format, arg0);
            }
        }

        [StringFormatMethod("format")]
        public static void DebugFormat([NotNull] string format, object arg0, object arg1) {
            if (LoggingEnabled) {
                Logger.DebugFormat(format, arg0, arg1);
            }
        }

        [StringFormatMethod("format")]
        public static void DebugFormat([NotNull] string format, object arg0, object arg1, object arg2) {
            if (LoggingEnabled) {
                Logger.DebugFormat(format, arg0, arg1, arg2);
            }
        }

        [StringFormatMethod("format")]
        public static void DebugFormat([NotNull] string format, params object[] args) {
            if (LoggingEnabled) {
                Logger.DebugFormat(format, args);
            }
        }

        public static void Info([NotNull] object message) {
            if (LoggingEnabled) {
                Logger.Info(message);
            }
        }

        public static void Info([NotNull] object message, [NotNull] Exception exception) {
            if (LoggingEnabled) {
                Logger.Info(message, exception);
            }
        }

        public static void Info([NotNull] string message) {
            if (LoggingEnabled) {
                Logger.Info(message);
            }
        }

        [StringFormatMethod("format")]
        public static void InfoFormat([NotNull] IFormatProvider provider, [NotNull] string format, params object[] args) {
            if (LoggingEnabled) {
                Logger.InfoFormat(provider, format, args);
            }
        }

        [StringFormatMethod("format")]
        public static void InfoFormat([NotNull] string format, object arg0) {
            if (LoggingEnabled) {
                Logger.InfoFormat(format, arg0);
            }
        }

        [StringFormatMethod("format")]
        public static void InfoFormat([NotNull] string format, object arg0, object arg1) {
            if (LoggingEnabled) {
                Logger.InfoFormat(format, arg0, arg1);
            }
        }

        [StringFormatMethod("format")]
        public static void InfoFormat([NotNull] string format, object arg0, object arg1, object arg2) {
            if (LoggingEnabled) {
                Logger.InfoFormat(format, arg0, arg1, arg2);
            }
        }

        [StringFormatMethod("format")]
        public static void InfoFormat([NotNull] string format, params object[] args) {
            if (LoggingEnabled) {
                Logger.InfoFormat(format, args);
            }
        }

        public static void Warn([NotNull] object message) {
            if (LoggingEnabled) {
                Logger.Warn(message);
            }
        }

        public static void Warn([NotNull] object message, [NotNull] Exception exception) {
            if (LoggingEnabled) {
                Logger.Warn(message, exception);
            }
        }

        public static void Warn([NotNull] string message) {
            if (LoggingEnabled) {
                Logger.Warn(message);
            }
        }

        [StringFormatMethod("format")]
        public static void WarnFormat([NotNull] IFormatProvider provider, [NotNull] string format, params object[] args) {
            if (LoggingEnabled) {
                Logger.WarnFormat(provider, format, args);
            }
        }

        [StringFormatMethod("format")]
        public static void WarnFormat([NotNull] string format, object arg0) {
            if (LoggingEnabled) {
                Logger.WarnFormat(format, arg0);
            }
        }

        [StringFormatMethod("format")]
        public static void WarnFormat([NotNull] string format, object arg0, object arg1) {
            if (LoggingEnabled) {
                Logger.WarnFormat(format, arg0, arg1);
            }
        }

        [StringFormatMethod("format")]
        public static void WarnFormat([NotNull] string format, object arg0, object arg1, object arg2) {
            if (LoggingEnabled) {
                Logger.WarnFormat(format, arg0, arg1, arg2);
            }
        }

        [StringFormatMethod("format")]
        public static void WarnFormat([NotNull] string format, params object[] args) {
            if (LoggingEnabled) {
                Logger.WarnFormat(format, args);
            }
        }

        public static void Error([NotNull] object message) {
            if (LoggingEnabled) {
                Logger.Error(message);
            }
        }

        public static void Error([NotNull] object message, [NotNull] Exception exception) {
            if (LoggingEnabled) {
                Logger.Error(message, exception);
            }
        }

        public static void Error([NotNull] string message) {
            if (LoggingEnabled) {
                Logger.Error(message);
            }
        }

        [StringFormatMethod("format")]
        public static void ErrorFormat([NotNull] IFormatProvider provider, [NotNull] string format, params object[] args) {
            if (LoggingEnabled) {
                Logger.ErrorFormat(provider, format, args);
            }
        }

        [StringFormatMethod("format")]
        public static void ErrorFormat([NotNull] string format, object arg0) {
            if (LoggingEnabled) {
                Logger.ErrorFormat(format, arg0);
            }
        }

        [StringFormatMethod("format")]
        public static void ErrorFormat([NotNull] string format, object arg0, object arg1) {
            if (LoggingEnabled) {
                Logger.ErrorFormat(format, arg0, arg1);
            }
        }

        [StringFormatMethod("format")]
        public static void ErrorFormat([NotNull] string format, object arg0, object arg1, object arg2) {
            if (LoggingEnabled) {
                Logger.ErrorFormat(format, arg0, arg1, arg2);
            }
        }

        [StringFormatMethod("format")]
        public static void ErrorFormat([NotNull] string format, params object[] args) {
            if (LoggingEnabled) {
                Logger.ErrorFormat(format, args);
            }
        }

        public static void Fatal([NotNull] object message) {
            if (LoggingEnabled) {
                Logger.Fatal(message);
            }
        }

        public static void Fatal([NotNull] object message, [NotNull] Exception exception) {
            if (LoggingEnabled) {
                Logger.Fatal(message, exception);
            }
        }

        public static void Fatal([NotNull] string message) {
            if (LoggingEnabled) {
                Logger.Fatal(message);
            }
        }

        [StringFormatMethod("format")]
        public static void FatalFormat([NotNull] IFormatProvider provider, [NotNull] string format, params object[] args) {
            if (LoggingEnabled) {
                Logger.FatalFormat(provider, format, args);
            }
        }

        [StringFormatMethod("format")]
        public static void FatalFormat([NotNull] string format, object arg0) {
            if (LoggingEnabled) {
                Logger.FatalFormat(format, arg0);
            }
        }

        [StringFormatMethod("format")]
        public static void FatalFormat([NotNull] string format, object arg0, object arg1) {
            if (LoggingEnabled) {
                Logger.FatalFormat(format, arg0, arg1);
            }
        }

        [StringFormatMethod("format")]
        public static void FatalFormat([NotNull] string format, object arg0, object arg1, object arg2) {
            if (LoggingEnabled) {
                Logger.FatalFormat(format, arg0, arg1, arg2);
            }
        }

        [StringFormatMethod("format")]
        public static void FatalFormat([NotNull] string format, params object[] args) {
            if (LoggingEnabled) {
                Logger.FatalFormat(format, args);
            }
        }

        public static bool LoggingEnabled { get; set; }

        [NotNull]
        private static ILogger Logger => _logger ?? (_logger = new ColoredConsoleLogger());

        private static ILogger _logger;

    }
}
