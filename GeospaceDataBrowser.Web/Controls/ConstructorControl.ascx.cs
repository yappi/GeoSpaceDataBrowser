namespace GeospaceDataBrowser.Web.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using GeospaceDataBrowser.Model;
    using GeospaceDataBrowser.Web.Data;

    public partial class ConstructorControl : System.Web.UI.UserControl
    {
        private const string DataViewIdTemplate = "DataViewControl{0}";
        private List<DataViewControl> dataViewControls = new List<DataViewControl>();

        #region Properties

        /// <summary>
        /// Gets or sets the constructor control mode.
        /// </summary>
        public ConstructorMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the data views count.
        /// </summary>
        private int DataViewCount
        {
            get
            {
                object resultObj = this.ViewState["DataViewCount"];
                return resultObj != null ? (int)resultObj : -1;
            }

            set { this.ViewState["DataViewCount"] = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds a view control to the constructor control.
        /// </summary>
        /// <param name="orderNumber">The view order number.</param>
        /// <param name="observatory">The observatory id.</param>
        /// <param name="instrument">The instrument id.</param>
        /// <param name="dataType">The data type id.</param>
        /// <param name="dateTime">The date time.</param>
        public void AddView(int orderNumber, int observatoryId, int instrumentId, int dataTypeId, DateTime dateTime)
        {
            DataViewControl dataView = CreateDataView(orderNumber);
            dataView.Initialize(orderNumber, observatoryId, instrumentId, dataTypeId, dateTime);
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            // Add data views from view state.
            int dataViewNumber = this.DataViewCount;
            for (int i = 0; i < dataViewNumber; i++)
            {
                CreateDataView(i);
            }
        }

        protected override object SaveViewState()
        {
            this.DataViewCount = this.dataViewControls.Count;

            return base.SaveViewState();
        }

        protected void DataView_AddButtonClicked(object sender, EventArgs e)
        {
            DataViewControl dataView = (DataViewControl)sender;

            // Re-arrange other controls in order they appear in correct order after postback.
            for (int i = 0; i <= dataView.OrderNumber; i++)
            {
                this.dataViewControls[i].OrderNumber = i;
                this.dataViewControls[i].ID = string.Format(ConstructorControl.DataViewIdTemplate, i);
            }

            for (int i = dataView.OrderNumber + 1; i < this.dataViewControls.Count; i++)
            {
                this.dataViewControls[i].OrderNumber = i + 1;
                this.dataViewControls[i].ID = string.Format(ConstructorControl.DataViewIdTemplate, i + 1);
            }

            AddView(dataView.OrderNumber + 1, dataView.ObservatoryId, dataView.InstrumentId, dataView.DataTypeId, dataView.DateTime);
        }

        protected void DataView_RemoveButtonClicked(object sender, EventArgs e)
        {
            DataViewControl dataView = (DataViewControl)sender;

            // Re-arrange other controls in order they appear in correct order after postback.
            for (int i = dataView.OrderNumber + 1; i < this.dataViewControls.Count; i++)
            {
                this.dataViewControls[i].OrderNumber = i - 1;
                this.dataViewControls[i].ID = string.Format(ConstructorControl.DataViewIdTemplate, i - 1);
            }

            this.PlaceHolder.Controls.Remove(dataView);
            this.dataViewControls.Remove(dataView);
        }

        /// <summary>
        /// Creates a data view control and places it on the constructor control.
        /// </summary>
        /// <returns>The created control.</returns>
        private DataViewControl CreateDataView(int index)
        {
            DataViewControl dataView = (DataViewControl)this.LoadControl("DataViewControl.ascx");
            dataView.ID = string.Format("DataViewControl{0}", index);
            dataView.AddButtonClicked += new EventHandler(DataView_AddButtonClicked);
            dataView.RemoveButtonClicked += new EventHandler(DataView_RemoveButtonClicked);

            this.PlaceHolder.Controls.AddAt(index, dataView);
            this.dataViewControls.Insert(index, dataView);
            return dataView;
        }

        #endregion Methods
    }
}