using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project7090.DataTypes;

namespace Project7090
{
    public class Common
    {
        public enum TimeDomain
        {
            Year,
            YearMonth,
            YearQuarter,
            YearMonthDay
        }

        public const int YEAR_TIME_DOMAIN_SPECIAL_CASE = -999;

        public static int EncodeTimeAsInteger(TimeDomainContainer domainContainer)
        {
            int result = 0;

            switch (domainContainer.CurrentTimeDomain)
            {
                case TimeDomain.Year:
                    result = domainContainer.Year;
                    break;
                case TimeDomain.YearMonth:
                    result = domainContainer.Month;
                    break;
                case TimeDomain.YearMonthDay:
                    result = domainContainer.ToDate().DayOfYear;
                    break;
                case TimeDomain.YearQuarter:
                    result = domainContainer.Quarter;
                    break;
            }

            return result;
        }

        public static TimeDomainContainer DecodeTime(TimeDomain timeDomain, double encodedTime, double timeEncodingFactor)
        {
            TimeDomainContainer container = null;

            switch (timeDomain)
            {
                case TimeDomain.Year:
                    container = new TimeDomainContainer(Convert.ToInt32(encodedTime * timeEncodingFactor));                    
                    break;
                case TimeDomain.YearMonth:
                    container = new TimeDomainContainer(Convert.ToInt32(encodedTime * timeEncodingFactor));                    
                    break;
                case TimeDomain.YearMonthDay:
                    int decodedTime = Convert.ToInt32((encodedTime * timeEncodingFactor));

                    break;
                case TimeDomain.YearQuarter:
                    container = new TimeDomainContainer(Convert.ToInt32(encodedTime * timeEncodingFactor));                    
                    break;
            }

            return container;
        }

        public static double EncodeTimeAsDouble(TimeDomainContainer domainContainer, double timeEncodingFactor)
        {          
            return ((EncodeTimeAsInteger(domainContainer) / timeEncodingFactor));
        }

        public static List<double> GetEncodedTimesToInterpolate(TimeDomain timeDomain, double timeEncodingFactor, int yearStartRange, int yearEndRange)
        {

            List<double> times = new List<double>();

            List<int> unEncodedTimes = GetTimesToInterpolate(timeDomain,yearStartRange, yearEndRange);

            foreach(int unEncodedTime in unEncodedTimes)
            {
                times.Add(unEncodedTime / timeEncodingFactor);
            }

            return times;
        }

        public static List<double> GetEncodedTimesToInterpolate(TimeDomain timeDomain, double timeEncodingFactor)
        {
            List<double> times = new List<double>();

            List<int> unEncodedTimes = GetTimesToInterpolate(timeDomain);

            foreach (int unEncodedTime in unEncodedTimes)
            {
                times.Add(unEncodedTime / timeEncodingFactor);
            }

            return times;
        }

        /// <summary>
        /// Handles the special case of the year domain being selected
        /// </summary>
        /// <param name="year"></param>
        /// <param name="timeEncodingFactor"></param>
        /// <returns></returns>
        public static Queue<double> GetEncodedYearToInterpolate(int year, double timeEncodingFactor)
        {
            Queue<double> times = new Queue<double>();            

            times.Enqueue(year / timeEncodingFactor);           

            return times;
        }

        /// <summary>
        /// The is a very simplistic function. It was only created to maintain
        /// the same queue results structure as GetTimesToInterplate().  It allows
        /// for a smoother logic flow.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static Queue<int> GetYearToInterpolate(int year)
        {
            Queue<int> timeQueue = new Queue<int>();

            timeQueue.Enqueue(year);

            return timeQueue;
        }

        //TODO:  TEST this one.
        public static List<int> GetTimesToInterpolate(TimeDomain timeDomain, int yearRangeStart, int yearRangeEnd)
        {
            List<int> times = new List<int>();

            for (int indx = yearRangeStart; indx <= yearRangeEnd; indx++)
            {
                times.Add(indx);
            }                
            

            return times;
        }

        public static List<int> GetTimesToInterpolate(TimeDomain timeDomain)
        {
            List<int> times = new List<int>();

            int endValue = 0;

            switch (timeDomain)
            {
                case TimeDomain.Year:
                    endValue = YEAR_TIME_DOMAIN_SPECIAL_CASE;                    
                    break;
                case TimeDomain.YearMonth:
                    endValue = 12;
                    break;
                case TimeDomain.YearMonthDay:
                    endValue = 365;
                    break;
                case TimeDomain.YearQuarter:
                    endValue = 4;
                    break;
            }

            for (int indx = 1; indx <= endValue; indx++)
            {
                times.Add(indx);
            }

            return times;
        }
    }
}
