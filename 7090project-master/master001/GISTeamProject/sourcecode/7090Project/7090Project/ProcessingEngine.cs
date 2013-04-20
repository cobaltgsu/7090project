using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Project7090.DataTypes;
using Project7090;


namespace Project7090
{
    public class ProcessingEngine
    {
        private  GISDataSet _gisDataSet = null; //holds GIS information from file
        private DataPointSet _locationDataSet = null; // holds the locations to be interpolated from file
        private Dictionary<double, TimeDomainContainer> _dictionaryTimeDecoder = null;
        
        #region Public Properties
        public Common.TimeDomain DataSetTimeDomain { get; set; }
        public double TimeEncodingFactor {get;set;}
        public string InterpolationOutputFile { get; set; }
        public string GISInputFilePath { get; set; }
        public string LocationInputFilePath { get; set; }
        public double InverseDistanceWeightedExponent { get; set; }
        public int NumberOfNeighbors { get; set; }
        #endregion

        public delegate void GISLocationProcessedHandler();
        public event GISLocationProcessedHandler GISLocationProcessed;        
        

        private const int COLUMN_ID = 0;
        private const int COLUMN_X = 1;
        private const int COLUMN_Y = 2;

 
        public ProcessingEngine()
        {

        }

        public void Process()
        {
            LoadDataFromFile(this.GISInputFilePath);
            LoadLocationDataFile(this.LocationInputFilePath);

            //Prepare the IDW functionality
            Interpolation.InverseDistanceWeightedExtension idwFunction = new Interpolation.InverseDistanceWeightedExtension();

            idwFunction.GISDataToProcess = _gisDataSet;
            idwFunction.NumberOfNeighbors = this.NumberOfNeighbors;
            idwFunction.Exponent  = this.InverseDistanceWeightedExponent;
            
            List<double> rangeOfTime = null;

            //I'm unsure how to do the year...Need to research more
            if (DataSetTimeDomain == Common.TimeDomain.Year)
            {
                //we will need to get the min year and max year from the measurement file

                //probably can optimize later
                var minYear = from gsPoint in _gisDataSet.OrderBy(o => o.timeContainer.Year).Take(1)
                              select gsPoint.timeContainer.Year;
                              

                var maxYear = from gsPoint in _gisDataSet.OrderByDescending(o => o.timeContainer.Year)
                              select gsPoint.timeContainer.Year;
                
                //check this!!!!!!!
                rangeOfTime = Common.GetEncodedTimesToInterpolate(this.DataSetTimeDomain, this.TimeEncodingFactor, minYear.First(), maxYear.First());
            }
            else
            {
                rangeOfTime = Common.GetEncodedTimesToInterpolate(this.DataSetTimeDomain, this.TimeEncodingFactor);
            }

            using (FileStream fs = new FileStream(InterpolationOutputFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                    watch.Start();

                   
                    foreach (DataPoint dp in _locationDataSet)
                    {
                        foreach (double currentTime in rangeOfTime)
                        {
                            Console.Out.WriteLine(dp.ToString(), GetDecodedTimeFromDictionaryAsString(currentTime), idwFunction.Interpolate(dp.x, dp.y, currentTime));
                            //sw.WriteLine("{0}\t{1}\t{2}", dp.ToString(), GetDecodedTimeFromDictionaryAsString(currentTime), idwFunction.Interpolate(dp, currentTime));
                            sw.WriteLine("{0}\t{1}\t{2}", dp.ToString(), GetDecodedTimeFromDictionaryAsString(currentTime), idwFunction.Interpolate(dp.x,dp.y, currentTime));
                            //GISForm gis = new GISForm();
                           
                            
                            sw.Flush(); //comment out for production
                        }
                    }            

                    watch.Stop();
                    Console.WriteLine(watch.Elapsed.TotalMinutes);
                }
            }
        }

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

                            while (!sr.EndOfStream)
                            {
                                string currentLine = sr.ReadLine();

                                string[] arrayValues = currentLine.Split('\t');

                                _locationDataSet.Add(new DataPoint(Int64.Parse(arrayValues[COLUMN_ID]),
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

        private bool InterpolateLocationFile()
        {
            return false;
        }

        
    }
}
