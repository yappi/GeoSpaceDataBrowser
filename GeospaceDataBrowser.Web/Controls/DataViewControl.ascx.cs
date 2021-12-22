namespace GeospaceDataBrowser.Web.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using GeospaceDataBrowser.Model;
    using GeospaceDataBrowser.Web.Data;
    using Resources;

    public partial class DataViewControl : System.Web.UI.UserControl
    {
        private const string OrderNumberName = "OrderNumber";

        #region Events

        public event EventHandler AddButtonClicked;

        public event EventHandler RemoveButtonClicked;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets an id of the selected observatory.
        /// </summary>
        public int ObservatoryId
        {
            get { return Convert.ToInt32(this.ObservatoryDropDown.SelectedValue); }
        }

        /// <summary>
        /// Gets an id of the selected instrument.
        /// </summary>
        public int InstrumentId
        {
            get { return Convert.ToInt32(this.InstrumentDropDown.SelectedValue); }
        }

        /// <summary>
        /// Gets an id of the selected data type.
        /// </summary>
        public int DataTypeId
        {
            get { return Convert.ToInt32(this.DataTypeDropDown.SelectedValue); }
        }

        /// <summary>
        /// Gets the selected date time.
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                return DateTime.ParseExact(this.DateTimeText.Text, this.NetDateTimeFormat.Value, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets a collection of controls that should trigger update panel.
        /// </summary>
        public IEnumerable<Control> TriggeringControls
        {
            get
            {
                return new List<Control>() { this.AddButton, this.RemoveButton };
            }
        }

        /// <summary>
        /// Gets or sets the order number of the data view control.
        /// </summary>
        public int OrderNumber
        {
            get
            {
                object resultObj = this.ViewState["OrderNumber"];

                return resultObj != null ? (int)resultObj : -1;
            }

            set { this.ViewState["OrderNumber"] = value; }
        }

        /// <summary>
        /// Gets the constructor control mode.
        /// </summary>
        private ConstructorMode Mode
        {
            get { return this.GetParent<ConstructorControl>().Mode; }
        }

        /// <summary>
        /// Gets the data type object.
        /// </summary>
        private DataType DataType
        {
            get
            {
                Observatory observatory = Repository.GetObservatory(this.ObservatoryId);
                Instrument instrument = Repository.GetInstrument(observatory, this.InstrumentId);
                return Repository.GetDataType(instrument.InstrumentType, this.DataTypeId);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initializes control.
        /// </summary>
        /// <param name="number">The order number.</param>
        /// <param name="observatory">The observatory id.</param>
        /// <param name="instrument">The instrument id.</param>
        /// <param name="dataType">The data type id.</param>
        /// <param name="dateTime">The date time.</param>
        public void Initialize(int orderNumber, int observatoryId, int instrumentId, int dataTypeId, DateTime dateTime)
        {
            this.OrderNumber = orderNumber;
            Observatory observatory = Repository.GetObservatory(observatoryId);
            Instrument instrument = Repository.GetInstrument(observatory, instrumentId);
            DataType dataType = Repository.GetDataType(instrument.InstrumentType, dataTypeId);

            SetDateTimeFormat(dataType);
            FillDateTimeText(this.DateTimeText, dateTime);

            FillDropDown(this.ObservatoryDropDown, Repository.Observatories);
            this.ObservatoryDropDown.SelectedValue = observatory.Id.ToString();

            FillDropDown(this.InstrumentDropDown, observatory.Instruments);
            this.InstrumentDropDown.SelectedValue = instrument.Id.ToString();

            FillDropDown(this.DataTypeDropDown, instrument.InstrumentType.DataTypes);
            this.DataTypeDropDown.SelectedValue = dataType.Id.ToString();

            // Hide 'Remove' button for the first view.
            this.RemoveButton.Visible = orderNumber != 0;

            FillImageControls(observatoryId, instrumentId, dataTypeId, dateTime);
        }

        public void SaveState()
        {
            this.SaveViewState();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UpdateControls();

            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "initDataViewControlScript",
                @"
                $(document).ready(function () {{
                InitControls();
                }});

                var pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();

                pageRequestManager.add_beginRequest(function () {{
                UninitControls();
                }});

                pageRequestManager.add_endRequest(function () {{
                InitControls();
                }});",
                true);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DataBindChildren();

            SetDateTimeFormat(this.DataType);

            FillEmptyDropDown(this.ObservatoryDropDown);
            FillEmptyDropDown(this.InstrumentDropDown);
            FillEmptyDropDown(this.DataTypeDropDown);
        }

        protected void ObservatoryDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int observatoryId = Convert.ToInt32(this.ObservatoryDropDown.SelectedValue);
            Observatory observatory = Repository.Observatories.Where(o => o.Id == observatoryId).First();

            FillDropDown(this.InstrumentDropDown, observatory.Instruments);

            FillDropDown(this.DataTypeDropDown, observatory.Instruments.First().InstrumentType.DataTypes);
        }

        protected void InstrumentDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int observatoryId = Convert.ToInt32(this.ObservatoryDropDown.SelectedValue);
            Observatory observatory = Repository.Observatories.Where(o => o.Id == observatoryId).First();

            int instrumentId = Convert.ToInt32(this.InstrumentDropDown.SelectedValue);
            Instrument insrument = observatory.Instruments.Where(i => i.Id == instrumentId).FirstOrDefault();

            if (insrument != null)
            {
                FillDropDown(this.DataTypeDropDown, insrument.InstrumentType.DataTypes);
            }
        }

        protected void ApplyButton_Click(object sender, EventArgs e)
        {
            UpdateImageControls();
        }

        protected void NextButton_Click(object sender, EventArgs e)
        {
            FillDateTimeText(this.DateTimeText, this.StepDate(StepType.Forward));

            UpdateImageControls();
        }

        protected void PreviousButton_Click(object sender, EventArgs e)
        {
            FillDateTimeText(this.DateTimeText, this.StepDate(StepType.Backward));

            UpdateImageControls();
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            OnEvent(this.AddButtonClicked);
        }

        protected void RemoveButton_Click(object sender, EventArgs e)
        {
            OnEvent(this.RemoveButtonClicked);
        }

        private void OnEvent(EventHandler handler)
        {
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        /// <summary>
        /// Sets date time controls format based on the data type properties.
        /// </summary>
        /// <param name="dataType">The data type object.</param>
        private void SetDateTimeFormat(DataType dataType)
        {
            // TODO: introduce logic to select date format type.
            if (dataType.XRangeLimit.MinValue == dataType.XRangeLimit.MaxValue &&
                dataType.XRangeLimit.MinValue >= TimeSpan.FromDays(1))
            {
                this.JsDateTimeFormat.Value = LocalizedText.JsStringDateFormat;
                this.NetDateTimeFormat.Value = LocalizedText.NetStringDateFormat;
            }
            else
            {
                this.JsDateTimeFormat.Value = LocalizedText.JsStringDateTimeFormat;
                this.NetDateTimeFormat.Value = LocalizedText.NetStringDateTimeFormat;
            }
        }

        /// <summary>
        /// Updates child controls visibility.
        /// </summary>
        private void UpdateControls()
        {
            bool customizationControlVisible = this.Mode == ConstructorMode.Customizable;

            this.FromDateLabel.Visible =
            this.FromDateText.Visible =
            this.ToDateLabel.Visible =
            this.ToDateText.Visible =
            this.YRangeFromLabel.Visible =
            this.YRangeFromText.Visible =
            this.YRangeToLabel.Visible =
            this.YRangeToText.Visible = customizationControlVisible;

            this.DateLabel.Visible =
            this.DateTimeText.Visible = !customizationControlVisible;
        }

        private void DownloadData(int observatoryId, int instrumentId, int dataTypeId, DateTime dateTime)
        {
            int fileFound = 0;
            try
            {
                using (Stream downloadStream = Repository.GetData(observatoryId, instrumentId, dataTypeId, dateTime))
                {
                    var list = Repository.GetDataToDownload(observatoryId, instrumentId, dataTypeId, dateTime);
                    string[] allFiles = Directory.GetFiles(list[0]);
                    foreach (var item in allFiles)
                    {
                        if (item.Contains(list[1]))
                        {
                            if (!item.Contains(".jpg") && !item.Contains(".png") && !item.Contains(".bmp"))
                            {
                                fileFound++;
                                FileInfo file = new FileInfo(item);
                                Response.Clear();
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                                Response.AddHeader("Content-Length", file.Length.ToString());
                                Response.ContentType = "multipart/form-data";
                                Response.Flush();
                                Response.TransmitFile(file.FullName);
                                Response.End();
                            }

                        }
                    }
                    string path = list[0].Substring(0, list[0].LastIndexOf("\\"));
                    string[] allFilesUpDir = Directory.GetFiles(path);
                    foreach (var item in allFilesUpDir)
                    {
                        if (item.Contains(list[1]))
                        {
                            if (!item.Contains(".jpg") && !item.Contains(".png") && !item.Contains(".bmp"))
                            {
                                fileFound++;
                                FileInfo file = new FileInfo(item);
                                Response.Clear();
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                                Response.AddHeader("Content-Length", file.Length.ToString());
                                Response.ContentType = "multipart/form-data";
                                Response.Flush();
                                Response.TransmitFile(file.FullName);
                                Response.End();
                            }

                        }
                    }

                    if (fileFound == 0)
                    {
                        ActionStatus.Text = string.Format("Data not found or data has not been loaded yet");
                    }
                }
            }
            catch
            {
                ActionStatus.Text = string.Format("Data not found or data has not been loaded yet");
            }
            
        }
                    



        /// <summary>
        /// Updates image controls.
        /// </summary>
        private void UpdateImageControls()
        {
            FillImageControls(this.ObservatoryId, this.InstrumentId, this.DataTypeId, this.DateTime);
        }

        private void FillImageControls(int observatoryId, int instrumentId, int dataTypeId, DateTime dateTime)
        {
            // Update data plot source.
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(string.Empty);
            queryCollection[ImageHandler.ObservatoryIdParameterName] = observatoryId.ToString();
            queryCollection[ImageHandler.InstrumentIdParameterName] = instrumentId.ToString();
            queryCollection[ImageHandler.DataTypeIdParameterName] = dataTypeId.ToString();
            queryCollection[ImageHandler.DateTimeParameterName] = dateTime.ToString(CultureInfo.InvariantCulture);

            this.DataPlot.ImageUrl = string.Format("{0}/Controls/ImageHandler.ashx?{1}",
                this.Request.ApplicationPath.TrimEnd('/'),
                queryCollection);
            // Update data plot description.
            DataType dataType = Repository.DatatTypes.Where(t => t.Id == dataTypeId).FirstOrDefault();
            this.DataTitleLiteral.Text = dataType != null ? dataType.Description : LocalizedText.NotAvailable;
        }

        private void FillDateTimeText(TextBox textBox, DateTime dateTime)
        {
            textBox.Text = dateTime.ToString(this.NetDateTimeFormat.Value);
        }

        private static void FillDropDown(DropDownList dropDown, object dataSource)
        {
            dropDown.DataSource = dataSource;
            dropDown.DataTextField = "ShortName";
            dropDown.DataValueField = "Id";
        }

        private static void FillEmptyDropDown(DropDownList dropDown)
        {
            if (dropDown.Items.Count == 0)
            {
                dropDown.Items.Add(LocalizedText.NotAvailable);
                dropDown.Enabled = false;
            }
            else
            {
                dropDown.Enabled = true;
            }
        }

        /// <summary>
        /// Calculates date time for the next step forward or backward.
        /// </summary>
        /// <param name="stepType">A step type.</param>
        /// <returns>Date time for the next step.</returns>
        private DateTime StepDate(StepType stepType)
        {
            int factor = stepType == StepType.Forward ? 1 : -1;

            DateTime result;
            if (this.DataType.XRangeLimit.MinValue < TimeSpan.FromDays(31))
            {
                result = this.DateTime.AddTicks(factor * this.DataType.XRangeLimit.MinValue.Ticks);
                result = new DateTime(this.DataType.XRangeLimit.MinValue.Ticks * (result.Ticks / this.DataType.XRangeLimit.MinValue.Ticks));
            }
            else
            {
                result = this.DateTime.AddMonths(factor * (int)(this.DataType.XRangeLimit.MinValue.Ticks / TimeSpan.FromDays(31).Ticks));
                result = result.AddDays(1 - result.Day);
            }

            return result;
        }

        #endregion Methods

        #region Nested classes

        private enum StepType
        {
            Forward,
            Backward
        }

        #endregion Nested classes

        protected void DownloadAnonymous_Click(object sender, EventArgs e)
        {
            //DownloadData(this.ObservatoryId, this.InstrumentId, this.DataTypeId, this.DateTime);
            ActionStatus.Text = string.Format("You do not have access to download data. Register to download data");
        }

        protected void DownloadLogIn_Click(object sender, EventArgs e)
        {
            ActionStatus.Text = string.Format("You do not have access to download data. Сontact the administrator");
        }

        protected void DownloadVerified_Click(object sender, EventArgs e)
        {
            DownloadData(this.ObservatoryId, this.InstrumentId, this.DataTypeId, this.DateTime);
        }
        protected void DownloadDatasetVerified_Click(object sender, EventArgs e)
        {
            Response.Redirect("DownloadDataset.aspx?ObservatoryId=" + this.ObservatoryId.ToString() + "&InstrumentId=" + this.InstrumentId.ToString() + "&DataTypeId=" + this.DataTypeId.ToString());
        }

    }
}