namespace GeospaceDataBrowser.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents an observatory object.
    /// </summary>
    public partial class Observatory : EntityBase
    {
        private List<Instrument> instruments = new List<Instrument>();

        /// <summary>
        /// Gets observatory location.
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        /// Gets observatory coordinates.
        /// </summary>
        public string Coordinates { get; private set; }

        /// <summary>
        /// Gets path to the observatory root folder.
        /// </summary>
        public string RootFolder { get; private set; }

        /// <summary>
        /// Returns a collection of observatory instruments.
        /// </summary>
        public ReadOnlyCollection<Instrument> Instruments
        {
            get { return this.instruments.AsReadOnly(); }
        }
    }
}