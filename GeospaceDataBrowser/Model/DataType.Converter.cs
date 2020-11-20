namespace GeospaceDataBrowser.Model
{
    using System;
    using System.Collections.Generic;
    using GeospaceDataBrowser.Data;

    /// <summary>
    /// Represents a data of a certain type.
    /// </summary>
    public partial class DataType
    {
        /// <summary>
        /// Represents a converter <see cref="DataType"> instances to DataSet and back.
        /// </summary>
        internal class Converter : EntityConverterBase<Converter, DataType, GeospaceDataSet.DataTypeRow,
            GeospaceDataSet.DataTypeDataTable>
        {
            /// <summary>
            /// Converts data row to an entity object.
            /// </summary>
            /// <param name="row">A data row.</param>
            /// <returns>The data row converted to a entity object.</returns>
            protected override DataType FromDataRowImpl(GeospaceDataSet.DataTypeRow row)
            {
                DataType entity = new DataType();
                entity.Id = row.Id;
                entity.ShortName = row.Name;
                entity.FullName = row.FullName;
                entity.FileMask = row.FileMask;

                if (!row.IsDescriptionNull())
                {
                    entity.Description = row.Description;
                }

                entity.XRangeLimit = new Limit<TimeSpan>(
                    new TimeSpan(row.IsTimeRangeMinNull() ? TimeSpan.MinValue.Ticks : row.TimeRangeMin),
                    new TimeSpan(row.IsTimeRangeMaxNull() ? TimeSpan.MaxValue.Ticks : row.TimeRangeMax));

                return entity;
            }
        }
    }
}