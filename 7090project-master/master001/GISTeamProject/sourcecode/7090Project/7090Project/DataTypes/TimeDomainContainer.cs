using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project7090;

namespace Project7090.DataTypes
{
    public class TimeDomainContainer
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Quarter { get; set; }
        public int Day { get; set; }
        public Common.TimeDomain CurrentTimeDomain { get; set; }

        public TimeDomainContainer(int year)
        {
            Year = year;
            CurrentTimeDomain = Common.TimeDomain.Year;
        }

        public TimeDomainContainer(int year, int monthOrQuarter, bool isMonth = false)
        {
            Year = year;
            
            if (isMonth)
            {
                Month = monthOrQuarter;
                CurrentTimeDomain = Common.TimeDomain.YearMonth;
            }
            else
            {
                Quarter = monthOrQuarter;
                CurrentTimeDomain = Common.TimeDomain.YearQuarter;
            }            
        }

        public TimeDomainContainer(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;

            CurrentTimeDomain = Common.TimeDomain.YearMonthDay;
        }

        public DateTime ToDate()
        {
            DateTime? date = null;

            if (CurrentTimeDomain == Common.TimeDomain.YearMonthDay)
            {
                date = new DateTime(Year,Month,Day);
            }

            return date.Value;
        }

        public override string ToString()
        {
            string output = string.Empty;

            switch (this.CurrentTimeDomain)
            {
                case Common.TimeDomain.Year:
                    output = this.Year.ToString();
                    break;
                case Common.TimeDomain.YearMonth:
                    output = string.Concat(this.Year, "\t", this.Month);
                    break;
                case Common.TimeDomain.YearMonthDay:
                    output = string.Concat(this.Year, "\t", this.Month, "\t", this.Day);
                    break;
                case Common.TimeDomain.YearQuarter:
                    output = string.Concat(this.Year, "\t", this.Quarter);
                    break;
            }

            return output;
        }
    }
}
