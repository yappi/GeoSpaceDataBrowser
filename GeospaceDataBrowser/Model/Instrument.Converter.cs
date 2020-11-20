namespace GeospaceDataBrowser.Model
{
    using System.Linq;
    using GeospaceDataBrowser.Data;

    /// <summary>
    /// Represents an instrument object.
    /// </summary>
    public partial class Instrument
    {
        /// <summary>
        /// Represents a converter <see cref="Instrument"> instances to DataSet and back.
        /// </summary>
        internal class Converter : EntityConverterBase<Converter, Instrument, GeospaceDataSet.InstrumentRow,
            GeospaceDataSet.InstrumentDataTable>
        {
            /// <summary>
            /// Converts data row to an entity object.
            /// </summary>
            /// <param name="row">A data row.</param>
            /// <returns>The data row converted to a entity object.</returns>
            protected override Instrument FromDataRowImpl(GeospaceDataSet.InstrumentRow row)
            {
                Instrument entity = new Instrument();
                entity.Id = row.Id;
                entity.Number = row.Number;
                entity.InstrumentType =
                    Repository.InstrumentTypes.Where(i => i.Id == row.InsrtumentTypeId).FirstOrDefault();

                if (row.Number == 1)
                {
                    entity.ShortName = entity.InstrumentType.ShortName;
                }
                else
                {
                    entity.ShortName = string.Format("{0} - {1}", entity.InstrumentType.ShortName, row.Number);
                }

                return entity;
            }
        }
    }
}