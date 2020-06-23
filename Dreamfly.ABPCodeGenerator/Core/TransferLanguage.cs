using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dreamfly.ABPCodeGenerator.Core
{
    public static class TransferLanguage
    {
        const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;
        [DllImport("kernel32.dll", EntryPoint = "LCMapStringA")]
        public static extern int LCMapString(int Locale, int dwMapFlags, byte[] lpSrcStr, int cchSrc, byte[] lpDestStr,
            int cchDest);
        static string Transfer(string source, int type)
        {
            byte[] srcByte2 = Encoding.Default.GetBytes(source);
            byte[] desByte2 = new byte[srcByte2.Length];
            LCMapString(2052, type, srcByte2, -1, desByte2, srcByte2.Length);
            string des2 = Encoding.Default.GetString(desByte2);
            return des2;
        }

        /// <summary>
        /// 简体转繁体
        /// </summary>
        /// <param name="source">简体</param>
        /// <returns>繁体</returns>
        public static string ToTraditional(string source)
        {
           return Transfer(source, LCMAP_TRADITIONAL_CHINESE);
        }

        /// <summary>
        /// 繁体转简体
        /// </summary>
        /// <param name="source">繁体</param>
        /// <returns>简体</returns>
        public static string ToSimplified(string source)
        {
            return Transfer(source, LCMAP_SIMPLIFIED_CHINESE);
        }
    }
}
