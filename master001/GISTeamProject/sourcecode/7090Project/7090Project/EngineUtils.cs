using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Project7090.DataTypes;
using Project7090.Interpolation;

namespace Project7090
{
    public partial class ProcessingEngine
    {

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
                                                        
                            while (!sr.EndOfStream) // We can switch this loop to a for() and use ReadLines to get total measurements for Progress Bar.
                            {
                                string currentLine = sr.ReadLine();

                                string[] arrayValues = currentLine.Split('\t');

                                TimeDomainContainer domainContainer = null;
                                GISDataPoint gsPoint = null;

                                //TODO: need a way to get the layout to assign the values to the point better;
                                double encodedTime = 0;

                                switch (DataSetTimeDomain)
                                {
                                    case Common.TimeDomain.Year:
                                        domainContainer = new TimeDomainContainer(int.Parse(arrayValues[1]));
                                        encodedTime = Common.EncodeTimeAsDouble(domainContainer, TimeEncodingFactor);

                                        AddEncodedTimeToDictionary(domainContainer,encodedTime);

                                        gsPoint = new GISDataPoint(Int64.Parse(arrayValues[0]),
                                                        encodedTime,
                                                        Double.Parse(arrayValues[2]), Double.Parse(arrayValues[3]), Double.Parse(arrayValues[4]),domainContainer);
                                        break;
                                    case Common.TimeDomain.YearMonth:
                                        domainContainer = new TimeDomainContainer(int.Parse(arrayValues[1]), int.Parse(arrayValues[2]), true);
                                        encodedTime = Common.EncodeTimeAsDouble(domainContainer, TimeEncodingFactor);

                                        AddEncodedTimeToDictionary(domainContainer,encodedTime);
                                        
                                        gsPoint = new GISDataPoint(Int64.Parse(arrayValues[0]),
                                                        encodedTime,
                                                        Double.Parse(arrayValues[3]), Double.Parse(arrayValues[4]), Double.Parse(arrayValues[5]), domainContainer);

                                        break;
                                    case Common.TimeDomain.YearMonthDay:
                                        domainContainer = new TimeDomainContainer(int.Parse(arrayValues[1]), int.Parse(arrayValues[2]), int.Parse(arrayValues[3]));
                                        encodedTime = Common.EncodeTimeAsDouble(domainContainer, TimeEncodingFactor);

                                        AddEncodedTimeToDictionary(domainContainer,encodedTime);
                                        
                                        gsPoint = new GISDataPoint(Int64.Parse(arrayValues[0]),
                                                        encodedTime,
                                                        Double.Parse(arrayValues[4]), Double.Parse(arrayValues[5]), Double.Parse(arrayValues[6]), domainContainer);
                                        break;
                                    case Common.TimeDomain.YearQuarter:
                                        domainContainer = new TimeDomainContainer(int.Parse(arrayValues[1]), int.Parse(arrayValues[2]), true);
                                        encodedTime = Common.EncodeTimeAsDouble(domainContainer, TimeEncodingFactor);

                                        AddEncodedTimeToDictionary(domainContainer,encodedTime);

                                        gsPoint = new GISDataPoint(Int64.Parse(arrayValues[0]),
                                                        encodedTime,
                                                        Double.Parse(arrayValues[3]), Double.Parse(arrayValues[4]), Double.Parse(arrayValues[5]), domainContainer);

                                        break;
                                }
                                                                
                                _gisDataSet.Add(gsPoint);
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

            _locationDataSet = new GISDataSet();

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

                                _locationDataSet.Add(new GISDataPoint(Int64.Parse(arrayValues[COLUMN_ID]),
                                        Double.Parse(arrayValues[COLUMN_X]), Double.Parse(arrayValues[COLUMN_Y])));                               
                            }
                        }
                    }
                }
                else
                {
                    //should throw?
                    //probably just log and return false;
                }

                if (_locationDataSet.Count > 0)
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

        private string GetDecodedTimeFromDictionaryAsString(double encodedTime)
        {
            string result = null;

            if (_dictionaryTimeDecoder.ContainsKey(encodedTime))
            {
                result = _dictionaryTimeDecoder[encodedTime].ToString();
            }
            else
            {
                switch (DataSetTimeDomain)
                {
                    case Common.TimeDomain.Year:
                        result = new TimeDomainContainer(-999).ToString();
                        break;
                    case Common.TimeDomain.YearMonth:
                        result = new TimeDomainContainer(-999, -999, true).ToString();
                        break;
                    case Common.TimeDomain.YearMonthDay:
                        result = new TimeDomainContainer(-999, -999, -999).ToString();
                        break;
                    case Common.TimeDomain.YearQuarter:
                        result = new TimeDomainContainer(-999, -999).ToString();
                        break;
                }
            }

            return result;
        }

        private void SetTimeRange(Common.TimeDomain timeDomain)
        {
            //I'm unsure how to do the year...Need to research more
            if (timeDomain == Common.TimeDomain.Year)
            {
                //we will need to get the min year and max year from the measurement file
                //probably can optimize later
                var minYear = from gsPoint in _gisDataSet.OrderBy(o => o.timeContainer.Year).Take(1)
                              select gsPoint.timeContainer.Year;


                var maxYear = from gsPoint in _gisDataSet.OrderByDescending(o => o.timeContainer.Year)
                              select gsPoint.timeContainer.Year;

                //check this!!!!!!!
                rangeOfTime = Common.GetEncodedTimesToInterpolate(timeDomain, this.TimeEncodingFactor, minYear.First(), maxYear.First());
            }
            else
            {
                rangeOfTime = Common.GetEncodedTimesToInterpolate(timeDomain, this.TimeEncodingFactor);
            }
        }

        private bool InterpolateLocationFile()
        {
            return false;
        }

        private void OutputDataToFile(GISDataSet outputData)
		{
            int outputSize = outputData.Count;
            using (FileStream fs = new FileStream(InterpolationOutputFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
					if (this.DataSetTimeDomain == Common.TimeDomain.Year)
					{
						sw.Write("{0,-11}{1,-6}{2,-6}\n", "county_id", "year", "pm25");
						for (int i = 0; i < outputSize; i++)
						{
							// TO DO: Progress bar variable.
							sw.Write("{0,-11:D}{1,-6:D}{2,-6:F1}\n", outputData[i].id, outputData[i].timeContainer.Year, outputData[i].measurement);
						}
					}
                    else if (this.DataSetTimeDomain == Common.TimeDomain.YearMonth)
					{
						sw.Write("{0,-11}{1,-6}{2,-6}{3,-6}\n", "county_id", "year", "month", "pm25");
						for (int i = 0; i < outputSize; i++)
						{
							// TO DO: Progress bar variable.
                            sw.Write("{0,-11:D}{1,-6:D}{2,-6:D}{3,-6:F1}\n", outputData[i].id, outputData[i].timeContainer.Year, outputData[i].timeContainer.Month, outputData[i].measurement);
						}
					}
                    else if (this.DataSetTimeDomain == Common.TimeDomain.YearQuarter)
					{
						sw.Write("{0,-11}{1,-6}{2,-6}{3,-6}\n", "county_id", "year", "quarter", "pm25");
						for (int i = 0; i < outputSize; i++)
						{
							// TO DO: Progress bar variable.
                            sw.Write("{0,-11:D}{1,-6:D}{2,-6:D}{3,-6:F1}\n", outputData[i].id, outputData[i].timeContainer.Year, outputData[i].timeContainer.Quarter, outputData[i].measurement);
						}
					}
                    else if (this.DataSetTimeDomain == Common.TimeDomain.YearMonthDay)
					{
						sw.Write("{0,-11}{1,-6}{2,-6}{3,-6}{4,-6}\n", "county_id", "year", "month", "day", "pm25");
						for (int i = 0; i < outputSize; i++)
						{
							// TO DO: Progress bar variable.
                            sw.Write("{0,-11:D}{1,-6:D}{2,-6:D}{3,-6:D}{4,-6:F1}\n", outputData[i].id, outputData[i].timeContainer.Year, outputData[i].timeContainer.Month, outputData[i].timeContainer.Day, outputData[i].measurement);
						}
					}
					sw.Flush();
				}
			}
		}

        public GISDataPointDistance[] NearestNeighbors(GISDataPoint dp, int numNeighbors)
        {
            GISDataPointDistance[] results = new GISDataPointDistance[numNeighbors];
            foreach (GISDataPoint dpd in _gisDataSet)
            {
                double distance = IDW.Distance(dpd, dp);

                bool shift = false;
                GISDataPointDistance shifted = null;

                for (int i = 0; i < numNeighbors; i++)
                {
                    if (shift)
                    {
                        GISDataPointDistance temp = results[i];
                        results[i] = shifted;
                        shifted = temp;
                    }
                    else if (results[i] == null)
                    {
                        GISDataPointDistance newdpd = new GISDataPointDistance();
                        newdpd.GSPoint = dpd;
                        newdpd.Distance = distance;

                        results[i] = newdpd;
                    }
                    else if (distance < results[i].Distance)
                    {
                        GISDataPointDistance newdpd = new GISDataPointDistance();
                        newdpd.GSPoint = dpd;
                        newdpd.Distance = distance;

                        shifted = results[i];
                        results[i] = newdpd;

                        shift = true;
                    }
                }
            }
            return results;
        }

        //private class StateSaver
        //{
        //    // Used for saving program state at time of ThreadPool implementation.
        //}
    }
}
