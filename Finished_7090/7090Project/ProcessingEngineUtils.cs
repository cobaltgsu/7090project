using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Project7090.DataTypes;
using System.Collections.Concurrent;
using System.Threading;
using Project7090.Validation;

namespace Project7090
{
    public partial class ProcessingEngine
    {

        public static List<GISDataPoint>[] PartitionDataSet (List<GISDataPoint> list, int totalPartitions)
        {
            if (list == null)
                throw new ArgumentNullException("List cannot be null.");

            if (totalPartitions < 1)
                throw new ArgumentOutOfRangeException("TotalPartitions cannot be less than 1.");

            List<GISDataPoint>[] partitionedGISDataPoints = new List<GISDataPoint>[totalPartitions];
            GISDataPoint[] listAsArray = list.ToArray();

            int partitionSize = list.Count / totalPartitions;
            int overallPointCounter = 0;

            for (int indx = 0; indx < totalPartitions; indx++)
            {
                partitionedGISDataPoints[indx] = new List<GISDataPoint>();

                int pointCounter = 0;

                if (indx + 1 == totalPartitions)
                {
                    while (overallPointCounter < listAsArray.Length)
                    {
                        partitionedGISDataPoints[indx].Add(listAsArray[overallPointCounter]);
                        pointCounter++;
                        overallPointCounter++;
                    }
                }
                else
                {
                    while (pointCounter < partitionSize && overallPointCounter < listAsArray.Length)
                    {
                        partitionedGISDataPoints[indx].Add(listAsArray[overallPointCounter]);
                        pointCounter++;
                        overallPointCounter++;
                    }
                }
            }

            return partitionedGISDataPoints;
        }

        public static List<T>[] Partition<T>(List<T> list, int totalPartitions)
        {
            if (list == null)
                throw new ArgumentNullException("List cannot be null.");

            if (totalPartitions < 1)
                throw new ArgumentOutOfRangeException("TotalPartitions cannot be less than 1.");

            List<T>[] partitions = new List<T>[totalPartitions];

            int maxSize = (int)Math.Ceiling(list.Count / (double)totalPartitions);
            int k = 0;

            for (int i = 0; i < partitions.Length; i++)
            {
                partitions[i] = new List<T>();
                for (int j = k; j < k + maxSize; j++)
                {
                    if (j >= list.Count)
                        break;
                    partitions[i].Add(list[j]);
                }
                k += maxSize;
            }

            return partitions;
        }

        private void SetTimeRange(Common.TimeDomain DataSetTimeDomain)
        {
            //I'm unsure how to do the year...Need to research more
            if (DataSetTimeDomain == Common.TimeDomain.Year)
            {
                //we will need to get the min year and max year from the measurement file

                //probably can optimize later
                var minYear = from gsPoint in _gisDataSet.OrderBy(o => o.timeContainer.Year).Take(1)
                              select gsPoint.timeContainer.Year;


                var maxYear = from gsPoint in _gisDataSet.OrderByDescending(o => o.timeContainer.Year)
                              select gsPoint.timeContainer.Year;

                _dataYear = minYear.First();
                _dataYearEnd = maxYear.First();

                //check this!!!!!!!
                rangeOfTime = Common.GetEncodedTimesToInterpolate(this.DataSetTimeDomain, this.TimeEncodingFactor, minYear.First(), maxYear.First());

                rangeOfTimeDictionary = Common.GetEncodedTimesToInterpolateForYearDictionary(_dataYear, _dataYearEnd, this.DataSetTimeDomain, this.TimeEncodingFactor);
            }
            else
            {
                rangeOfTime = Common.GetEncodedTimesToInterpolate(this.DataSetTimeDomain, this.TimeEncodingFactor);
                rangeOfTimeDictionary = Common.GetEncodedTimesToInterpolateDictionary(_dataYear, this.DataSetTimeDomain, this.TimeEncodingFactor);
            }
        }

        //private int getTimeOutputs()
        //{
        //    if (this.DataSetTimeDomain == Common.TimeDomain.Year)
        //    {
        //        return 1;
        //    }
        //    else if (this.DataSetTimeDomain == Common.TimeDomain.YearMonth)
        //    {
        //        return 12;
        //    }
        //    else if (this.DataSetTimeDomain == Common.TimeDomain.YearQuarter)
        //    {
        //        return 4;
        //    }
        //    else if (this.DataSetTimeDomain == Common.TimeDomain.YearMonthDay)
        //    {
        //        return 365;
        //    }
        //    else
        //    {
        //        return -1;
        //    }
        //}

        private bool LoadDataFromFile(string filePath)
        {
            bool loaded = false;

            _gisDataSet = new GISDataSet();
            _dictionaryTimeDecoder = new Dictionary<double, TimeDomainContainer>();

            try
            {
                if (File.Exists(filePath))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            sr.ReadLine(); //just the header.  There's no need to do anything with it

                            while (!sr.EndOfStream)
                            {
                                string currentLine = sr.ReadLine();

                                string[] arrayValues = currentLine.Split('\t');


                                GISDataPoint gsPoint = null;

                                //TODO: need a way to get the layout to assign the values to the point better;
                                double encodedTime = 0;

                                _dataYear = int.Parse(arrayValues[1]);

                                switch (DataSetTimeDomain)
                                {
                                    case Common.TimeDomain.Year:
                                        domainContainer = new TimeDomainContainer(int.Parse(arrayValues[1]));
                                        encodedTime = Common.EncodeTimeAsDouble(domainContainer, TimeEncodingFactor);

                                        AddEncodedTimeToDictionary(domainContainer, encodedTime);

                                        gsPoint = new GISDataPoint(Int64.Parse(arrayValues[0]),
                                                        encodedTime,
                                                        Double.Parse(arrayValues[2]), Double.Parse(arrayValues[3]), Double.Parse(arrayValues[4]), domainContainer);
                                        break;
                                    case Common.TimeDomain.YearMonth:
                                        domainContainer = new TimeDomainContainer(int.Parse(arrayValues[1]), int.Parse(arrayValues[2]), true);
                                        encodedTime = Common.EncodeTimeAsDouble(domainContainer, TimeEncodingFactor);

                                        AddEncodedTimeToDictionary(domainContainer, encodedTime);

                                        gsPoint = new GISDataPoint(Int64.Parse(arrayValues[0]),
                                                        encodedTime,
                                                        Double.Parse(arrayValues[3]), Double.Parse(arrayValues[4]), Double.Parse(arrayValues[5]), domainContainer);

                                        break;
                                    case Common.TimeDomain.YearMonthDay:
                                        domainContainer = new TimeDomainContainer(int.Parse(arrayValues[1]), int.Parse(arrayValues[2]), int.Parse(arrayValues[3]));
                                        encodedTime = Common.EncodeTimeAsDouble(domainContainer, TimeEncodingFactor);

                                        AddEncodedTimeToDictionary(domainContainer, encodedTime);

                                        gsPoint = new GISDataPoint(Int64.Parse(arrayValues[0]),
                                                        encodedTime,
                                                        Double.Parse(arrayValues[4]), Double.Parse(arrayValues[5]), Double.Parse(arrayValues[6]), domainContainer);
                                        break;
                                    case Common.TimeDomain.YearQuarter:
                                        domainContainer = new TimeDomainContainer(int.Parse(arrayValues[1]), int.Parse(arrayValues[2]), true);
                                        encodedTime = Common.EncodeTimeAsDouble(domainContainer, TimeEncodingFactor);

                                        AddEncodedTimeToDictionary(domainContainer, encodedTime);

                                        gsPoint = new GISDataPoint(Int64.Parse(arrayValues[0]),
                                                        encodedTime,
                                                        Double.Parse(arrayValues[3]), Double.Parse(arrayValues[4]), Double.Parse(arrayValues[5]), domainContainer);

                                        break;
                                }

                                _listGISDataPoints.Add(gsPoint);
                            }

                        }
                    }
                }
                else
                {
                    //should throw?
                    //probably just log and return false;
                }

                if (_gisDataSet.Count > 0)
                {
                    loaded = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return loaded;
        }

        private bool LoadLocationDataFile(string filePath)
        {
            bool loaded = false;

            _locationDataSet = new DataPointSet();

            try
            {
                if (File.Exists(filePath))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            sr.ReadLine(); //just the header.  There's no need to do anything with it

                            GISDataPoint gsPoint = null;

                            while (!sr.EndOfStream)
                            {
                                string currentLine = sr.ReadLine();

                                string[] arrayValues = currentLine.Split('\t');

                                foreach (double rangeKey in rangeOfTimeDictionary.Keys)
                                {
                                    switch (DataSetTimeDomain)
                                    {
                                        case Common.TimeDomain.Year:

                                            gsPoint = new GISDataPoint(Int64.Parse(arrayValues[COLUMN_ID]),
                                                            rangeKey,
                                                            Double.Parse(arrayValues[COLUMN_X]), Double.Parse(arrayValues[COLUMN_Y]), 0, rangeOfTimeDictionary[rangeKey]);
                                            break;
                                        case Common.TimeDomain.YearMonth:

                                            gsPoint = new GISDataPoint(Int64.Parse(arrayValues[COLUMN_ID]),
                                                            rangeKey,
                                                            Double.Parse(arrayValues[COLUMN_X]), Double.Parse(arrayValues[COLUMN_Y]), 0, rangeOfTimeDictionary[rangeKey]);

                                            break;
                                        case Common.TimeDomain.YearMonthDay:

                                            gsPoint = new GISDataPoint(Int64.Parse(arrayValues[COLUMN_ID]),
                                                            rangeKey,
                                                            Double.Parse(arrayValues[COLUMN_X]), Double.Parse(arrayValues[COLUMN_Y]), 0, rangeOfTimeDictionary[rangeKey]);
                                            break;
                                        case Common.TimeDomain.YearQuarter:

                                            gsPoint = new GISDataPoint(Int64.Parse(arrayValues[COLUMN_ID]),
                                                            rangeKey,
                                                            Double.Parse(arrayValues[COLUMN_X]), Double.Parse(arrayValues[COLUMN_Y]), 0, rangeOfTimeDictionary[rangeKey]);
                                            break;
                                    }

                                    _listLocationDataPoints.Add(gsPoint);
                                }

                            }
                        }
                    }
                }
                else
                {
                    //should throw?
                    //probably just log and return false;
                }

                if (_listLocationDataPoints.Count > 0)
                {
                    loaded = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return loaded;
        }

        private void AddEncodedTimeToDictionary(TimeDomainContainer timeContainer, double encodedTime)
        {
            if (!_dictionaryTimeDecoder.ContainsKey(encodedTime))
            {
                _dictionaryTimeDecoder.Add(encodedTime, timeContainer);
            }
        }


        private bool InterpolateLocationFile()
        {
            return false;
        }
    }
}
