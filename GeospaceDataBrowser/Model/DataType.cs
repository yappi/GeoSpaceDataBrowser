namespace GeospaceDataBrowser.Model
{
    using System;

    /// <summary>
    /// Represents a data of a certain type.
    /// </summary>
    public partial class DataType : EntityBase
    {
        /// <summary>
        /// Gets an X axis max and min limits.
        /// </summary>
        public Limit<TimeSpan> XRangeLimit;

        /// <summary>
        /// Gets an Y axis max and min values.
        /// </summary>
        public Limit<double> YValueLimit;

        /// <summary>
        /// Gets a file maks to read data from.
        /// </summary>
        public string FileMask { get; private set; }
    }
}