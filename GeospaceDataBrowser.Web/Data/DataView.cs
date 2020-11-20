namespace GeospaceDataBrowser.Web.Data
{
    using System;
    using GeospaceDataBrowser.Model;

    /// <summary>
    /// Represents a data view with a certain display properties.
    /// </summary>
    [Serializable]
    public class DataView
    {
        /// <summary>
        /// The observatory id.
        /// </summary>
        public int ObservatoryId { get; set; }

        /// <summary>
        /// The instrument.
        /// </summary>
        public int InstrumentId { get; set; }

        /// <summary>
        /// The data type.
        /// </summary>
        public int DataTypeId { get; set; }

        /// <summary>
        /// The date to display data for.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// The date to display data from.
        /// </summary>
        public DateTime FromDateTime { get; set; }

        /// <summary>
        /// The date to display data to.
        /// </summary>
        public DateTime ToDateTime { get; set; }
    }
}