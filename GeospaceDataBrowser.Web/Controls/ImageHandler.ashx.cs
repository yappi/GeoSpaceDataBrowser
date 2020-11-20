namespace GeospaceDataBrowser.Web.Controls
{
    using System;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Web;
    using GeospaceDataBrowser.Model;
    using Resources;

    /// <summary>
    /// Summary description for GetData
    /// </summary>
    public class ImageHandler : IHttpHandler
    {
        /// <summary>
        /// Date time request parameter name.
        /// </summary>
        public const string DateTimeParameterName = "DateTime";

        /// <summary>
        /// Observatory id request parameter name.
        /// </summary>
        public const string ObservatoryIdParameterName = "ObservatoryId";

        /// <summary>
        /// Instrument id request parameter name.
        /// </summary>
        public const string InstrumentIdParameterName = "InstrumentId";

        /// <summary>
        /// Data type id request parameter name.
        /// </summary>
        public const string DataTypeIdParameterName = "DataTypeId";

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                // Get input parameters.
                DateTime dateTime = ImageHandler.GetParameter<DateTime>(context, ImageHandler.DateTimeParameterName);
                int observatoryId = ImageHandler.GetParameter<int>(context, ImageHandler.ObservatoryIdParameterName);
                int instrumentId = ImageHandler.GetParameter<int>(context, ImageHandler.InstrumentIdParameterName);
                int dataTypeId = ImageHandler.GetParameter<int>(context, ImageHandler.DataTypeIdParameterName);

                // Return data plot.
                using (Stream imageStream = Repository.GetData(observatoryId, instrumentId, dataTypeId, dateTime))
                {
                    context.Response.ContentType = "image/png";
                    imageStream.CopyTo(context.Response.OutputStream);
                }
            }
            catch
            {
                // Return 'Data Unavailable' image.
                context.Response.ContentType = "image/jpg";
                LocalizedText.NoData.Save(context.Response.OutputStream, ImageFormat.Jpeg);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a request parameter value if present in the context, otherwise throws exception.
        /// </summary>
        /// <typeparam name="T">The expected type of the parameter value.</typeparam>
        /// <param name="context">The HTTP context.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <returns>The value of the parameter.</returns>
        private static T GetParameter<T>(HttpContext context, string parameterName)
        {
            object value = context.Request.Params[parameterName];
            if (value == null)
            {
                throw new InvalidOperationException(string.Format(LocalizedText.ErrorMissingRequiredParameterTemplate, parameterName));
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new ArgumentException(string.Format(LocalizedText.ErrorInvalidParameterFormatTemplate, parameterName));
            }
        }
    }
}