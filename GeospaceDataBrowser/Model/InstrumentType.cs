namespace GeospaceDataBrowser.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents an instrument type object.
    /// </summary>
    public partial class InstrumentType : EntityBase
    {
        private List<DataType> dataTypes = new List<DataType>();

        /// <summary>
        /// Gets folder mask for the Instrument.
        /// </summary>
        public string FolderMask { get; private set; }

        /// <summary>
        /// Returns a collection of data types.
        /// </summary>
        public ReadOnlyCollection<DataType> DataTypes
        {
            get { return this.dataTypes.AsReadOnly(); }
        }
    }
}