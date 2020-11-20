namespace GeospaceDataBrowser.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// The base class that is used for converting model entities to data objects and back.
    /// Each model object converter should inherit from this class.
    /// </summary>
    /// <typeparam name="DT">The type of the derived class.</typeparam>
    /// <typeparam name="ET">The type of the entity.</typeparam>
    /// <typeparam name="RT">The type of the data row to convert entity.</typeparam>
    /// <typeparam name="TT">The type of the data table to convert entity collection.</typeparam>
    public abstract class EntityConverterBase<DT, ET, RT, TT> where TT : DataTable
    {
        /// <summary>
        /// Converts entity object to a data row.
        /// </summary>
        /// <param name="entity">An entity object.</param>
        /// <returns>The entity object converted to a data row.</returns>
        public static RT ToDataRow(ET entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            TT dataTable = Activator.CreateInstance<TT>();

            return EntityConverterBase<DT, ET, RT, TT>.CreateConverterInstance().ToDataRowImpl(entity, dataTable);
        }

        /// <summary>
        /// Converts data row to an entity object.
        /// </summary>
        /// <param name="row">A data row.</param>
        /// <returns>The data row converted to a entity object.</returns>
        public static ET FromDataRow(RT row)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            return EntityConverterBase<DT, ET, RT, TT>.CreateConverterInstance().FromDataRowImpl(row);
        }

        /// <summary>
        /// Converts entity collection to a data table.
        /// </summary>
        /// <param name="entity">A collection containing entity objects.</param>
        /// <returns>The data table with entity objects converted to data rows.</returns>
        public static TT ToDataTable(IEnumerable<ET> entityCollection)
        {
            EntityConverterBase<DT, ET, RT, TT> converter = EntityConverterBase<DT, ET, RT, TT>.CreateConverterInstance();

            TT result = Activator.CreateInstance<TT>();
            DataTable dataTable = result as DataTable;
            foreach (ET entity in entityCollection)
            {
                dataTable.Rows.Add(converter.ToDataRowImpl(entity, result) as DataRow);
            }

            return result;
        }

        /// <summary>
        /// Converts data table to an entity collection.
        /// </summary>
        /// <param name="table">A data table containing data rows.</param>
        /// <returns>The collection of entity objects converted from data rows.</returns>
        public static IList<ET> FromDataTable(TT table)
        {
            return EntityConverterBase<DT, ET, RT, TT>.FromDataTable(table.Rows);
        }

        /// <summary>
        /// Converts data table to an entity collection.
        /// </summary>
        /// <param name="rows">The data rows.</param>
        /// <returns>The collection of entity objects converted from data rows.</returns>
        public static IList<ET> FromDataTable(IEnumerable rows)
        {
            EntityConverterBase<DT, ET, RT, TT> converter = EntityConverterBase<DT, ET, RT, TT>.CreateConverterInstance();

            List<ET> result = new List<ET>();
            foreach (RT row in rows)
            {
                result.Add(converter.FromDataRowImpl(row));
            }

            return result;
        }

        /// <summary>
        /// Converts entity object to a data row.
        /// </summary>
        /// <param name="entity">An entity object.</param>
        /// <param name="dataTable">A data table to create data row in.</param>
        /// <returns>The entity object converted to a data row.</returns>
        protected virtual RT ToDataRowImpl(ET entity, TT dataTable)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts data row to an entity object.
        /// </summary>
        /// <param name="row">A data row.</param>
        /// <returns>The data row converted to a entity object.</returns>
        protected abstract ET FromDataRowImpl(RT row);

        /// <summary>
        /// Creates an instance of the generic converter.
        /// </summary>
        /// <returns></returns>
        protected static EntityConverterBase<DT, ET, RT, TT> CreateConverterInstance()
        {
            // Create a instance of the converter type.
            return (EntityConverterBase<DT, ET, RT, TT>)Activator.CreateInstance(typeof(DT));
        }
    }
}