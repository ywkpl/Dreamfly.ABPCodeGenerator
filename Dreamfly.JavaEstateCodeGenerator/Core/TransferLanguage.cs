using System;
using System.Runtime.InteropServices;

namespace Dreamfly.JavaEstateCodeGenerator.Core
{
    public static class TransferLanguage
    {
        const int LocaleSystemDefault = 0x0800;
        const int LcmapSimplifiedChinese = 0x02000000;
        const int LcmapTraditionalChinese = 0x04000000;

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LCMapString(int locale, int dwMapFlags, string lpSrcStr, int cchSrc,
            string lpDestStr, int cchDest);

        /// <summary>
        /// 繁体转简体
        /// </summary>
        /// <param name="argSource">繁体</param>
        /// <returns>简体</returns>
        public static string ToSimplified(string argSource)
        {
            var t = new String(' ', argSource.Length);
            LCMapString(LocaleSystemDefault, LcmapSimplifiedChinese, argSource, argSource.Length, t, argSource.Length);
            return t;
        }

        /// <summary>
        /// 简体转繁体
        /// </summary>
        /// <param name="argSource">简体</param>
        /// <returns>繁体</returns>
        public static string ToTraditional(string argSource)
        {
            var t = new String(' ', argSource.Length);
            LCMapString(LocaleSystemDefault, LcmapTraditionalChinese, argSource, argSource.Length, t, argSource.Length);
            return t;
        }
    }
}