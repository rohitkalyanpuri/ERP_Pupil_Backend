using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Services
{
    public static class GeneralHelper
    {
        public static int GetIdAfterSpilt(this string self, string delim, int skip)
        {
            try
            {
                return Convert.ToInt32(self.Split('-').Skip(skip).FirstOrDefault());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static DateTime? GetValidDate(string ReqStringDate, string DateFormat = "MM/dd/yyyy", bool NeedTodayDateDefaultOnEmpty = false)
        {
            string MonthDayYearFormat = string.Empty;
            try
            {
                DateTime d;
                bool chValidity = DateTime.TryParse(ReqStringDate, out d);
                if (chValidity)
                {
                    MonthDayYearFormat = Convert.ToDateTime(ReqStringDate).ToString(DateFormat);
                }
                if (NeedTodayDateDefaultOnEmpty && !chValidity)
                    MonthDayYearFormat = DateTime.Now.ToString(DateFormat);

                if (!string.IsNullOrEmpty(MonthDayYearFormat))
                    return Convert.ToDateTime(MonthDayYearFormat);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
                //MonthDayYearFormat = string.Empty;
            }

        }
    }
}
