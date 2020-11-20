namespace GeospaceDataBrowser.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;
    using GeospaceDataBrowser.Model;

    public partial class Default : System.Web.UI.Page
    {
        private const string ObservatoryParameter = "observatory";
        private const string InstrumentParameter = "instrument";
        private const string DataTypeParameter = "datatype";
        private const string DateTimeParameter = "datetime";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                var observatoryIds = GetQueryParameterValues<int>(Default.ObservatoryParameter);
                var instrumentIds = GetQueryParameterValues<int>(Default.InstrumentParameter);
                var dataTypeIds = GetQueryParameterValues<int>(Default.DataTypeParameter);
                var dates = GetQueryParameterValues<DateTime>(Default.DateTimeParameter);

                for (int k = 0; k < observatoryIds.Length; k++)
                {
                    Observatory observatory = Repository.GetObservatory(observatoryIds[k]);
                    if (observatory == null)
                    {
                        observatory = Repository.Observatories.First();
                    }

                    Instrument instrument = k < instrumentIds.Length ? observatory.Instruments.FirstOrDefault(i => i.Id == instrumentIds[k]) : null;
                    if (instrument == null)
                    {
                        instrument = observatory.Instruments.First();
                    }

                    DataType dataType = k < dataTypeIds.Length ? instrument.InstrumentType.DataTypes.FirstOrDefault(t => t.Id == dataTypeIds[k]) : null;
                    if (dataType == null)
                    {
                        instrument.InstrumentType.DataTypes.First();
                    }

                    this.DataConstructor.AddView(k, observatory.Id, instrument.Id, dataType.Id, dates.Length > 0 ? dates[0] : DateTime.Now);
                }

                if (observatoryIds.Length == 0)
                {
                    var observatory = Repository.Observatories.First();
                    var instrument = observatory.Instruments.First();
                    var dataType = instrument.InstrumentType.DataTypes.First();

                    this.DataConstructor.AddView(0, observatory.Id, instrument.Id, dataType.Id, DateTime.Now);
                }
            }
        }

        private T[] GetQueryParameterValues<T>(string key) where T : struct
        {
            string parameter = this.Request.QueryString[key];
            if (string.IsNullOrEmpty(parameter))
            {
                return new T[0];
            }

            var values = parameter.Split(',');
            try
            {
                return values.Select(v => (T)Convert.ChangeType(v, typeof(T))).ToArray();
            }
            catch
            {
                return new T[0];
            }
        }
    }
}