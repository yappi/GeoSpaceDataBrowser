namespace GeospaceDataBrowser.Model
{
    using System.Collections.Generic;
    using GeospaceDataBrowser.Data;

    /// <summary>
    /// Represents an instrument object.
    /// </summary>
    public partial class InstrumentType : EntityBase
    {
        /// <summary>
        /// Represents a converter <see cref="Instrument"> instances to DataSet and back.
        /// </summary>
        internal class Converter : EntityConverterBase<Converter, InstrumentType, GeospaceDataSet.InstrumentTypeRow,
            GeospaceDataSet.InstrumentTypeDataTable>
        {
            /// <summary>
            /// Converts data row to an entity object.
            /// </summary>
            /// <param name="row">A data row.</param>
            /// <returns>The data row converted to a entity object.</returns>
            protected override InstrumentType FromDataRowImpl(GeospaceDataSet.InstrumentTypeRow row)
            {
                InstrumentType entity = new InstrumentType();
                entity.Id = row.Id;
                entity.ShortName = row.ShortName;
                entity.FullName = row.FullName;
                entity.FolderMask = row.FolderMask;

                if (!row.IsDescriptionNull())
                {
                    entity.Description = row.Description;
                }

                entity.dataTypes.AddRange(Repository.GetInstrumentDataTypes(entity.Id));
                return entity;
            }
        }
    }
}