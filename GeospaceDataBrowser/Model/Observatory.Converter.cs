namespace GeospaceDataBrowser.Model
{
    using System.Linq;
    using GeospaceDataBrowser.Data;

    /// <summary>
    /// Represents an observatory object.
    /// </summary>
    public partial class Observatory : EntityBase
    {
        /// <summary>
        /// Represents a converter <see cref="Observatory"> instances to DataSet and back.
        /// </summary>
        internal class Converter : EntityConverterBase<Converter, Observatory, GeospaceDataSet.ObservatoryRow,
            GeospaceDataSet.ObservatoryDataTable>
        {
            /// <summary>
            /// Converts data row to an entity object.
            /// </summary>
            /// <param name="row">A data row.</param>
            /// <returns>The data row converted to a entity object.</returns>
            protected override Observatory FromDataRowImpl(GeospaceDataSet.ObservatoryRow row)
            {
                Observatory entity = new Observatory();
                entity.Id = row.Id;
                entity.ShortName = row.ShortName;
                entity.FullName = row.FullName;
                entity.RootFolder = row.RootFolder;
                entity.Location = row.Location;

                if (!row.IsDescriptionNull())
                {
                    entity.Description = row.Description;
                }

                if (!row.IsCoordinatesNull())
                {
                    entity.Coordinates = row.Coordinates;
                }

                entity.instruments.AddRange(Repository.GetObservatoryInstruments(entity.Id));

                return entity;
            }
        }
    }
}