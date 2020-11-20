namespace GeospaceDataBrowser.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents an instrument object.
    /// </summary>
    public partial class Instrument
    {
        /// <summary>
        /// Gets an instrument id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets an instrument short name.
        /// </summary>
        public string ShortName { get; private set; }

        /// <summary>
        /// Gets an instrument number.
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Gets an instrument type.
        /// </summary>
        public InstrumentType InstrumentType { get; private set; }
    }
}