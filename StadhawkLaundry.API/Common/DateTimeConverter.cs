﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StadhawkLaundry.API.Common
{
    public static class DateTimeConverter
    {
        public static string DatetimeConverterfromString(string date, DateFormat format)
        {

            try
            {
                char[] splitformat = new char[] { '.', '/', '-', ' ' };
                char spliter = new char();
                string[] arr = new string[] { };
                foreach (char s in splitformat)
                {
                    if (date.Contains(s))
                    {
                        spliter = s;
                        break;
                    }

                }

                switch (format)
                {
                    case DateFormat.DDMMYY:
                        arr = date.Split(spliter);
                        date = arr[1] + spliter + arr[0] + spliter + arr[2];
                        break;
                    case DateFormat.MMDDYY:
                        arr = date.Split(spliter);
                        date = arr[0] + spliter + arr[1] + spliter + arr[2];
                        break;
                    case DateFormat.YYMMDD:
                        arr = date.Split(spliter);
                        date = arr[2] + spliter + arr[0] + spliter + arr[1];
                        break;
                }
            }
            catch
            {
                return "";
            }

            return date;
        }

        public enum DateFormat
        {
            DDMMYY,
            MMDDYY,
            YYMMDD
        }
    }
}
