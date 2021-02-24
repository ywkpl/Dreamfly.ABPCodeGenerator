using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;

namespace Dreamfly.JavaEstateCodeGenerator.Helper
{
    public class SerilogHelper
    {
        private static Logger logger;

        public static Logger Instance =>
            logger ??= new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
    }
}
