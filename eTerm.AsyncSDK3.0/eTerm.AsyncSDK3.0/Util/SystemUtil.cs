using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace eTerm.AsyncSDK.Util {
    /// <summary>
    /// 系统工具类
    /// </summary>
    public static class SystemUtil {
        [DllImport( "Kernel32.dll" )]
        private static extern Boolean SetSystemTime([In, Out] SystemTime st);

      /// <summary>
      /// 设置系统时间
      /// </summary>
      /// <param name="newdatetime">新时间</param>
      /// <returns></returns>
      public static bool SetSysTime(DateTime newdatetime)
      {
       SystemTime st = new SystemTime();
       st.year    = Convert.ToUInt16(newdatetime.Year);
       st.month   = Convert.ToUInt16(newdatetime.Month);
       st.day    = Convert.ToUInt16(newdatetime.Day);
       st.dayofweek  = Convert.ToUInt16(newdatetime.DayOfWeek);
       st.hour    = Convert.ToUInt16(newdatetime.Hour - TimeZone.CurrentTimeZone.GetUtcOffset(new DateTime(2001,09,01)).Hours);
       st.minute   = Convert.ToUInt16(newdatetime.Minute);
       st.second   = Convert.ToUInt16(newdatetime.Second);
       st.milliseconds  = Convert.ToUInt16(newdatetime.Millisecond);
       return SetSystemTime(st);
      }
    }

    /// <summary>
    ///系统时间类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal class SystemTime {
        public ushort year;
        public ushort month;
        public ushort dayofweek;
        public ushort day;
        public ushort hour;
        public ushort minute;
        public ushort second;
        public ushort milliseconds;
    }

}
