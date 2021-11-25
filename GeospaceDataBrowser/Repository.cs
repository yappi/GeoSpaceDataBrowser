namespace GeospaceDataBrowser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using GeospaceDataBrowser.Data;
    using GeospaceDataBrowser.Model;

    /// <summary>
    /// Represents a repository to work with model objects.
    /// </summary>
    public static class Repository
    {
        /// <summary>
        /// Collection of the all available observatories.
        /// </summary>
        private static IEnumerable<Observatory> observatories;

        /// <summary>
        /// Synchronization object to access observatories collection.
        /// </summary>
        private static object syncObservatories = new object();

        /// <summary>
        /// Collection of the all available instruments.
        /// </summary>
        private static IEnumerable<InstrumentType> instrumentTypes;

        /// <summary>
        /// Synchronization object to access instrument collection.
        /// </summary>
        private static object syncInstrumentTypes = new object();

        /// <summary>
        /// Collection of the all available data types.
        /// </summary>
        private static IEnumerable<DataType> dataTypes;

        /// <summary>
        /// Synchronization object to access data type collection.
        /// </summary>
        private static object syncDataTypes = new object();

        #region Properties

        /// <summary>
        /// Gets the collection of all available observatory objects.
        /// </summary>
        public static IEnumerable<Observatory> Observatories
        {
            get
            {
                if (Repository.observatories == null)
                {
                    lock (Repository.syncObservatories)
                    {
                        if (Repository.observatories == null)
                        {
                            Repository.observatories = Repository.GetObservatories();
                        }
                    }
                }

                return Repository.observatories;
            }
        }

        /// <summary>
        /// Gets the collection of all available instrument types.
        /// </summary>
        public static IEnumerable<InstrumentType> InstrumentTypes
        {
            get
            {
                if (Repository.instrumentTypes == null)
                {
                    lock (Repository.syncInstrumentTypes)
                    {
                        if (Repository.instrumentTypes == null)
                        {
                            Repository.instrumentTypes = Repository.GetInstrumentTypes();
                        }
                    }
                }

                return Repository.instrumentTypes;
            }
        }

        /// <summary>
        /// Gets the collection of all available data type objects.
        /// </summary>
        public static IEnumerable<DataType> DatatTypes
        {
            get
            {
                if (Repository.dataTypes == null)
                {
                    lock (Repository.syncDataTypes)
                    {
                        if (Repository.dataTypes == null)
                        {
                            Repository.dataTypes = Repository.GetDataTypes();
                        }
                    }
                }

                return Repository.dataTypes;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns plotted data for the specified date time.
        /// </summary>
        /// <param name="observatoryId">The observatory id.</param>
        /// <param name="instrumentId">The instrument id.</param>
        /// <param name="dataTypeId">The data type id.</param>
        /// <param name="date">The date.</param>
        /// <returns>The stream containing plotted data.</returns>
        public static Stream GetData(int observatoryId, int instrumentId, int dataTypeId, DateTime date)
        {
            // Get observatory by id.
            Observatory observatory = Repository.GetObservatory(observatoryId);

            // Get instrument by id.
            Instrument instrument = Repository.GetInstrument(observatory, instrumentId);

            // Get data type by id.
            DataType dataType = Repository.GetDataType(instrument.InstrumentType, dataTypeId);

            // Build path to the data file.
            string filePath = Path.Combine(observatory.RootFolder, string.Format(instrument.InstrumentType.FolderMask, date),
                string.Format(dataType.FileMask, date, instrument.Number, observatory.Location.First()));

            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public static Stream GetShortNames(int observatoryId, int instrumentId, int dataTypeId, DateTime date)
        {
            // Get observatory by id.
            Observatory observatory = Repository.GetObservatory(observatoryId);

            // Get instrument by id.
            Instrument instrument = Repository.GetInstrument(observatory, instrumentId);

            // Get data type by id.
            DataType dataType = Repository.GetDataType(instrument.InstrumentType, dataTypeId);

            // Build path to the data file.
            string filePath = Path.Combine(observatory.RootFolder, string.Format(instrument.InstrumentType.FolderMask, date),
                string.Format(dataType.FileMask, date, instrument.Number, observatory.Location.First()));

            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public static List<string> GetDataToDownload(int observatoryId, int instrumentId, int dataTypeId, DateTime date)
        {
            List<string> pathAndName = new List<string>();
            // Get observatory by id.
            Observatory observatory = Repository.GetObservatory(observatoryId);

            // Get instrument by id.
            Instrument instrument = Repository.GetInstrument(observatory, instrumentId);

            // Get data type by id.
            DataType dataType = Repository.GetDataType(instrument.InstrumentType, dataTypeId);

            // Build path to the data file.
            string filePath = Path.Combine(observatory.RootFolder, string.Format(instrument.InstrumentType.FolderMask, date),
                string.Format(dataType.FileMask, date, instrument.Number, observatory.Location.First()));

            pathAndName.Add(Path.Combine(observatory.RootFolder, string.Format(instrument.InstrumentType.FolderMask, date)));
            string[] temp = dataType.FileMask.Split('{', '}');
            foreach (var item in temp)
            {
                if (item.Contains("yy"))
                {
                    pathAndName.Add(string.Format("{" + item.Trim(new char[] { 'm', 'H' }) + "}", date));
                }
            }            
            return pathAndName;
        }

        /// <summary>
        /// Gets an observatory by id.
        /// </summary>
        /// <param name="observatoryId">An observatory id.</param>
        /// <returns>The observatory object.</returns>
        public static Observatory GetObservatory(int observatoryId)
        {
            Observatory result = Repository.Observatories.Where(o => o.Id == observatoryId).FirstOrDefault();
            if (result == null)
            {
                throw new ArgumentException(string.Format("Observatory, id = '{0}', does not exists.", observatoryId));
            }

            return result;
        }

        /// <summary>
        /// Gets an observatory instrument by id.
        /// </summary>
        /// <param name="observatory">An observatory object.</param>
        /// <param name="instrumentId">An instrument id.</param>
        /// <returns>The instrument object.</returns>
        public static Instrument GetInstrument(Observatory observatory, int instrumentId)
        {
            if (observatory == null)
            {
                throw new ArgumentNullException("observatory");
            }

            Instrument result = observatory.Instruments.Where(i => i.Id == instrumentId).FirstOrDefault();
            if (result == null)
            {
                throw new ArgumentException(string.Format("Instrument, id = '{0}', does not exists or belong to the specified observatory '{1}'.",
                    instrumentId, observatory.ShortName));
            }

            return result;
        }

        /// <summary>
        /// Gets an instrument type data type by id.
        /// </summary>
        /// <param name="instrumentType">An instrument type.</param>
        /// <param name="dataTypeId">A data type id.</param>
        /// <returns>The data type object.</returns>
        public static DataType GetDataType(InstrumentType instrumentType, int dataTypeId)
        {
            if (instrumentType == null)
            {
                throw new ArgumentNullException("instrumentType");
            }

            DataType result = instrumentType.DataTypes.Where(d => d.Id == dataTypeId).FirstOrDefault();
            if (result == null)
            {
                throw new ArgumentException(string.Format("Data type, Id = '{0}', does not exist or belong to the specified instrument type '{1}'.",
                    dataTypeId, instrumentType.ShortName));
            }

            return result;
        }

        /// <summary>
        /// Gets a collection of data types for the specified instrument type.
        /// </summary>
        /// <param name="instrumentTypeId">The instrument type id.</param>
        /// <returns>Collection of data types objects.</returns>
        internal static IEnumerable<DataType> GetInstrumentDataTypes(int instrumentTypeId)
        {
            // Fill data table.
            using (GeospaceDataBrowser.Data.GeospaceDataSetTableAdapters.DataTypeTableAdapter adapter =
                new Data.GeospaceDataSetTableAdapters.DataTypeTableAdapter())
            {
                // Convert to model objects.
                return DataType.Converter.FromDataTable(adapter.GetDataByInstrumentTypeId(instrumentTypeId));
            }
        }

        /// <summary>
        /// Gets a collection of instruments for the specified observatory.
        /// </summary>
        /// <param name="observatoryId">The observatory id.</param>
        /// <returns>Collection of instrument objects.</returns>
        internal static IEnumerable<Instrument> GetObservatoryInstruments(int observatoryId)
        {
            // Fill data table.
            using (GeospaceDataBrowser.Data.GeospaceDataSetTableAdapters.InstrumentTableAdapter adapter =
                new Data.GeospaceDataSetTableAdapters.InstrumentTableAdapter())
            {
                // Convert to model objects.
                return Instrument.Converter.FromDataTable(adapter.GetDataByObservatoryId(observatoryId));
            }
        }

        /// <summary>
        /// Returns all available observatory objects from database.
        /// </summary>
        /// <returns>Collection of observatory objects.</returns>
        private static IEnumerable<Observatory> GetObservatories()
        {
            // Fill data table.
            using (GeospaceDataBrowser.Data.GeospaceDataSetTableAdapters.ObservatoryTableAdapter adapter =
                new Data.GeospaceDataSetTableAdapters.ObservatoryTableAdapter())
            {
                // Convert to model objects.
                return Observatory.Converter.FromDataTable(adapter.GetData());
            }
        }

        /// <summary>
        /// Returns all available instrument type objects from database.
        /// </summary>
        /// <returns>Collection of instrument type objects.</returns>
        private static IEnumerable<InstrumentType> GetInstrumentTypes()
        {
            // Fill data table.
            using (GeospaceDataBrowser.Data.GeospaceDataSetTableAdapters.InstrumentTypeTableAdapter adapter =
                new Data.GeospaceDataSetTableAdapters.InstrumentTypeTableAdapter())
            {
                using (GeospaceDataSet ds = new GeospaceDataSet())
                {
                    adapter.Fill(ds.InstrumentType);

                    // Convert to model objects.
                    return InstrumentType.Converter.FromDataTable(ds.InstrumentType);
                }
            }
        }

        /// <summary>
        /// Returns all available data type objects from database.
        /// </summary>
        /// <returns>Collection of instrument type objects.</returns>
        private static IEnumerable<DataType> GetDataTypes()
        {
            // Fill data table.
            using (GeospaceDataBrowser.Data.GeospaceDataSetTableAdapters.DataTypeTableAdapter adapter =
                new Data.GeospaceDataSetTableAdapters.DataTypeTableAdapter())
            {
                using (GeospaceDataSet ds = new GeospaceDataSet())
                {
                    adapter.Fill(ds.DataType);

                    // Convert to model objects.
                    return DataType.Converter.FromDataTable(ds.DataType);
                }
            }
        }

        #endregion Methods
    }
}