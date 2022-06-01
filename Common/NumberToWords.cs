using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WebPayroll.Common
{
    public static class NumberToWords
    {
        public static string SpellNumber(string MyNumber)
        {
            string Result = "";
            try
            {
                MyNumber = "0000000000" + MyNumber;
                if (MyNumber.Contains(".") == true)
                {
                    int decimalPlace = MyNumber.IndexOf(".");
                    string afterDecimal = MyNumber.Substring(decimalPlace + 1, MyNumber.Length - (decimalPlace + 1));
                    string beforeDecimal = MyNumber.Substring(0, decimalPlace);
                    MyNumber = beforeDecimal.ToString();
                    Result = Result = GetMillion(MyNumber.ToString()) + GetThoudand(MyNumber.ToString()) + GetHundreds(MyNumber.Substring(MyNumber.Length - 3, 3));
                    MyNumber = "000" + afterDecimal.ToString();
                    if (Convert.ToDouble(MyNumber) != 0)
                    {
                        Result = Result + " And " + GetHundreds(MyNumber.Substring(MyNumber.Length - 3, 3));
                    }
                }
                else
                {
                    if (Convert.ToInt32(MyNumber) != 0)
                    {
                        Result = GetMillion(MyNumber.ToString()) + GetThoudand(MyNumber.ToString()) + GetHundreds(MyNumber.Substring(MyNumber.Length - 3, 3));
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return Result;
        }

        private static string GetMillion(string MyNumber)
        {
            string Result = "";
            if (Convert.ToInt32(MyNumber.Substring(MyNumber.Length - 9, 3)) != 0)
            {
                Result = GetHundreds(MyNumber.Substring(MyNumber.Length - 9, 3)) + " Million ";
            }
            return Result;
        }

        private static string GetThoudand(string MyNumber)
        {
            string Result = "";
            if (Convert.ToInt32(MyNumber.Substring(MyNumber.Length - 6, 3)) != 0)
            {
                Result = GetHundreds(MyNumber.Substring(MyNumber.Length - 6, 3)) + " Thousand";
            }
            return Result;
        }

        private static string GetHundreds(string MyNumber)
        {
            string Result = "";
            if (Convert.ToInt32(MyNumber) == 0)
            {
                Result = "";
            }
            if (MyNumber.Substring(MyNumber.Length - 3, 1) != "0")
            {
                Result = GetDigit(MyNumber.Substring(0, 1)) + " Hundred";
            }

            if (MyNumber.Substring(MyNumber.Length - 2, 1) != "0")
            {
                Result = Result + GetTens(MyNumber.Substring(MyNumber.Length - 2, 2));
            }
            else
            {
                Result = Result + GetDigit(MyNumber.Substring(MyNumber.Length - 1, 1));
            }

            return Result;
        }

        private static string GetDigit(string MyNumber)
        {
            switch (Convert.ToInt32(MyNumber))
            {
                case 1: return " One";
                case 2: return " Two";
                case 3: return " Three";
                case 4: return " Four";
                case 5: return " Five";
                case 6: return " Six";
                case 7: return " Seven";
                case 8: return " Eight";
                case 9: return " Nine";
                default: return "";
            }
        }

        private static string GetTens(string MyNumber)
        {
            string Result = "";
            if (MyNumber.Substring(0, 1) == "1")
            {
                switch (Convert.ToInt32(MyNumber))
                {
                    case 10:
                        Result = " Ten";
                        break;
                    case 11:
                        Result = " Eleven";
                        break;
                    case 12:
                        Result = " Twelve";
                        break;
                    case 13:
                        Result = " Thirteen";
                        break;
                    case 14:
                        Result = " Fourteen";
                        break;
                    case 15:
                        Result = " Fifteen";
                        break;
                    case 16:
                        Result = " Sixteen";
                        break;
                    case 17:
                        Result = " Seventeen";
                        break;
                    case 18:
                        Result = " Eighteen";
                        break;
                    case 19:
                        Result = " Nineteen";
                        break;
                    default:
                        Result = "";
                        break;
                }
            }
            else
            {
                switch (Convert.ToInt32(MyNumber.Substring(0, 1)))
                {
                    case 2:
                        Result = " Twenty";
                        break;
                    case 3:
                        Result = " Thirty";
                        break;
                    case 4:
                        Result = " Forty";
                        break;
                    case 5:
                        Result = " Fifty";
                        break;
                    case 6:
                        Result = " Sixty";
                        break;
                    case 7:
                        Result = " Seventy";
                        break;
                    case 8:
                        Result = " Eighty";
                        break;
                    case 9:
                        Result = " Ninety";
                        break;
                    default:
                        Result = "";
                        break;
                }
                Result = Result + GetDigit(MyNumber.Substring(1, 1));
            }
            return Result;
        }
    }

    public static class MyExtension
    {
        public static string ToFinancialYear(this DateTime dateTime)
        {
            return (dateTime.Month >= 4 ? dateTime.ToString("yyyy") + "-" + dateTime.AddYears(1).ToString("yy") : dateTime.ToString("yyyy") + "-" + dateTime.AddYears(1).ToString("yy"));
        }

        public static string FyMonth(this DateTime dateTime)
        {
            string month = dateTime.Month < 10 ? '0' + dateTime.Month.ToString() : dateTime.Month.ToString();

            return month + "/" + dateTime.Year;
        }

        public static string MonthYear(this DateTime dt)
        {
            string month = dt.Month >= 10 ? dt.Month.ToString() : "0" + dt.Month.ToString();
            string year = dt.Year.ToString();
            return month + year;
        }

        public static int GetWeekNumberOfMonth(this DateTime date)
        {
            return GetWeekNumberOfMonth(date, CultureInfo.CurrentCulture);
        }

        public static int GetWeekNumberOfMonth(this DateTime date, CultureInfo culture)
        {
            return date.GetWeekNumber(culture)
                 - new DateTime(date.Year, date.Month, 1).GetWeekNumber(culture)
                 + 1; // Or skip +1 if you want the first week to be 0.
        }

        public static int GetWeekNumber(this DateTime date, CultureInfo culture)
        {
            return culture.Calendar.GetWeekOfYear(date,
                culture.DateTimeFormat.CalendarWeekRule,
                culture.DateTimeFormat.FirstDayOfWeek);
        }

        public static int NoOfDaysInMonth(this string month)
        {
            int year = Convert.ToInt32(month.Substring(3, 4));
            int m = Convert.ToInt32(month.Substring(0, 2));
            int days = DateTime.DaysInMonth(year, m);
            return days;
        }
       
    }

    public class DateDifference
    {
        /// <summary>
        /// defining Number of days in month; index 0=> january and 11=> December
        /// february contain either 28 or 29 days, that's why here value is -1
        /// which wil be calculate later.
        /// </summary>
        private int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        /// <summary>
        /// contain from date
        /// </summary>
        private DateTime fromDate;

        /// <summary>
        /// contain To Date
        /// </summary>
        private DateTime toDate;

        /// <summary>
        /// this three variable for output representation..
        /// </summary>
        private int year;
        private int month;
        private int day;

        public DateDifference(DateTime d1, DateTime d2)
        {
            int increment;

            if (d1 > d2)
            {
                this.fromDate = d2;
                this.toDate = d1;
            }
            else
            {
                this.fromDate = d1;
                this.toDate = d2;
            }

            /// 
            /// Day Calculation
            /// 
            increment = 0;

            if (this.fromDate.Day > this.toDate.Day)
            {
                increment = this.monthDay[this.fromDate.Month - 1];

            }
            /// if it is february month
            /// if it's to day is less then from day
            if (increment == -1)
            {
                if (DateTime.IsLeapYear(this.fromDate.Year))
                {
                    // leap year february contain 29 days
                    increment = 29;
                }
                else
                {
                    increment = 28;
                }
            }
            if (increment != 0)
            {
                day = (this.toDate.Day + increment) - this.fromDate.Day;
                increment = 1;
            }
            else
            {
                day = this.toDate.Day - this.fromDate.Day;
            }

            ///
            ///month calculation
            ///
            if ((this.fromDate.Month + increment) > this.toDate.Month)
            {
                this.month = (this.toDate.Month + 12) - (this.fromDate.Month + increment);
                increment = 1;
            }
            else
            {
                this.month = (this.toDate.Month) - (this.fromDate.Month + increment);
                increment = 0;
            }

            ///
            /// year calculation
            ///
            this.year = this.toDate.Year - (this.fromDate.Year + increment);

        }

        public override string ToString()
        {
            //return base.ToString();
            return this.year + "." + this.month;
        }

        public int Years
        {
            get
            {
                return this.year;
            }
        }

        public int Months
        {
            get
            {
                return this.month;
            }
        }

        public int Days
        {
            get
            {
                return this.day;
            }
        }

    }

}