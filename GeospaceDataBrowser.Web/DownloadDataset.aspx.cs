using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GeospaceDataBrowser.Data;
using GeospaceDataBrowser.Model;

namespace GeospaceDataBrowser.Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string ObservatoryId { get; set; }
        public string InstrumentId { get; set; }
        public string DataTypeId { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            ObservatoryId = Request.QueryString["ObservatoryId"];
            InstrumentId = Request.QueryString["InstrumentId"];
            DataTypeId = Request.QueryString["DataTypeId"];

            Observatory observatory = Repository.GetObservatory(Int32.Parse(ObservatoryId));
            Instrument instrument = Repository.GetInstrument(observatory, Int32.Parse(InstrumentId));
            DataType dataType = Repository.GetDataType(instrument.InstrumentType, Int32.Parse(DataTypeId));
            DowloadDatasetObservatory.Text = string.Format($"Observatory: {observatory.ShortName}     Instrument: {instrument.ShortName}     Data Type: {dataType.ShortName}");
        }

        protected void DownloadButton_Click(object sender, EventArgs e)
        {

        }
    }
}