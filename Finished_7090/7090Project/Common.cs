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

        public static string GetOutputFileHeaderFromTimeDomain(TimeDomain timeDomain)
        {
            string header = string.Empty;

            switch (timeDomain)
            {
                case TimeDomain.Year:
                    header = "id\tyear\tpm25"; 
                    break;
                case TimeDomain.YearMonth:
                    header = "id\tyear\tmonth\tpm25"; 
                    break;
                case TimeDomain.YearMonthDay:
                    header = "id\tyear\tmonth\tday\tpm25"; 
                    break;
                case TimeDomain.YearQuarter:
                    header = "id\tyear\tquarter\tpm25"; 
                    break;
            }

            return header;
        }

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

            List<int> unEncodedTimes = GetTimesToInterpolate(timeDomain, yearStartRange, yearEndRange);

            foreach (int unEncodedTime in unEncodedTimes)
            {
                times.Add(unEncodedTime / timeEncodingFactor);
            }

            return times;
        }

        public static Dictionary<double, TimeDomain> GetEncodedTimesToInterpolateDictionary(TimeDomain timeDomain, double timeEncodingFactor, int yearStartRange, int yearEndRange)
        {
            Dictionary<double, TimeDomain> dictionary = new Dictionary<double, TimeDomain>();

            foreach (double key in GetTimesToInterpolateAsDictionary(timeDomain, yearStartRange, yearEndRange).Keys)
            {
                dictionary.Add(key / timeEncodingFactor, timeDomain);
            }

            return dictionary;
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

        public static Dictionary<double, TimeDomain> GetTimesToInterpolateAsDictionary(TimeDomain timeDomain, int yearRangeStart, int yearRangeEnd)
        {
            Dictionary<double, TimeDomain> dictionary = new Dictionary<double, TimeDomain>();

            for (int indx = yearRangeStart; indx <= yearRangeEnd; indx++)
            {
                dictionary.Add(indx, timeDomain);
            }

            return dictionary;
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

        /***************************************************************************/
        /*  New Development below to handle the time decoding for the output
        /**************************************************************************/

        public static Dictionary<double, TimeDomainContainer> GetEncodedTimesToInterpolateDictionary(int dataYear, TimeDomain timeDomain, double timeEncodingFactor)
        {
            Dictionary<double, TimeDomainContainer> lookupDictionary = new Dictionary<double, TimeDomainContainer>();
            Dictionary<int, TimeDomainContainer> dictionary = GetTimesToInterpolateAsDictionary(dataYear, timeDomain);

            foreach (int key in dictionary.Keys)
            {
                lookupDictionary.Add(key / timeEncodingFactor, dictionary[key]);
            }

            return lookupDictionary;
        }

        public static Dictionary<double, TimeDomainContainer> GetEncodedTimesToInterpolateForYearDictionary(int dataStartYear, int dataEndYear, TimeDomain timeDomain, double timeEncodingFactor)
        {
            Dictionary<double, TimeDomainContainer> lookupDictionary = new Dictionary<double, TimeDomainContainer>();
            Dictionary<int, TimeDomainContainer> dictionary = GetTimesToInterpolateForYearAsDictionary(dataStartYear, dataEndYear);

            foreach (int key in dictionary.Keys)
            {
                lookupDictionary.Add(key / timeEncodingFactor, dictionary[key]);
            }

            return lookupDictionary;
        }

        public static Dictionary<int, TimeDomainContainer> GetTimesToInterpolateForYearAsDictionary(int dataYearStart, int dataYearEnd)
        {
            Dictionary<int, TimeDomainContainer> dictionary = new Dictionary<int, TimeDomainContainer>();

            for (int indx = dataYearStart; indx <= dataYearEnd; indx++)
            {
                dictionary.Add(indx, new TimeDomainContainer(indx));
            }

            return dictionary;
        }

        public static Dictionary<int, TimeDomainContainer> GetTimesToInterpolateAsDictionary(int dataYear, TimeDomain timeDomain)
        {
            Dictionary<int, TimeDomainContainer> dictionary = new Dictionary<int, TimeDomainContainer>();

            int endValue = 0;

            switch (timeDomain)
            {
                case TimeDomain.Year:
                    endValue = YEAR_TIME_DOMAIN_SPECIAL_CASE;

                    break;
                case TimeDomain.YearMonth:
                    endValue = 12;
                    dictionary = GetTimesToInterporlateDictionaryForMonthOrQuarter(dataYear, endValue, true);
                    break;
                case TimeDomain.YearMonthDay:
                    endValue = 365;
                    dictionary = GetTimesToInterporlateDictionaryForDay(dataYear);
                    break;
                case TimeDomain.YearQuarter:
                    endValue = 4;
                    dictionary = GetTimesToInterporlateDictionaryForMonthOrQuarter(dataYear, endValue, false);
                    break;
            }


            return dictionary;
        }

        private static Dictionary<int, TimeDomainContainer> GetTimesToInterporlateDictionaryForYear(int startYear, int endYear)
        {
            Dictionary<int, TimeDomainContainer> timesDictionary = new Dictionary<int, TimeDomainContainer>();

            for (int indx = startYear; indx <= endYear; indx++)
            {
                timesDictionary.Add(indx, new TimeDomainContainer(indx));
            }

            return timesDictionary;
        }

        private static Dictionary<int, TimeDomainContainer> GetTimesToInterporlateDictionaryForMonthOrQuarter(int dataYear, int endValue, bool isMonth)
        {
            Dictionary<int, TimeDomainContainer> timesDictionary = new Dictionary<int, TimeDomainContainer>();

            for (int indx = 1; indx <= endValue; indx++)
            {
                timesDictionary.Add(indx, new TimeDomainContainer(dataYear, indx, isMonth));
            }

            return timesDictionary;
        }

        private static Dictionary<int, TimeDomainContainer> GetTimesToInterporlateDictionaryForDay(int dataYear)
        {
            Dictionary<int, TimeDomainContainer> timesDictionary = new Dictionary<int, TimeDomainContainer>();

            int dayCounter = 0;

            for (int month = 1; month <= 12; month++)
            {
                for (int day = 1; day <= DateTime.DaysInMonth(dataYear, month); day++)
                {
                    dayCounter++;
                    timesDictionary.Add(dayCounter, new TimeDomainContainer(dataYear, month, day));
                }
            }

            return timesDictionary;
        }

    }
}
