using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GeospaceDataBrowser.Data;
using GeospaceDataBrowser.Model;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;
using System.Threading;
using System.Threading.Tasks;

namespace GeospaceDataBrowser.Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string ObservatoryId { get; set; }
        public string InstrumentId { get; set; }
        public string DataTypeId { get; set; }

        private string observatoryPath;

        private List<string> FilesPath = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            ObservatoryId = Request.QueryString["ObservatoryId"];
            InstrumentId = Request.QueryString["InstrumentId"];
            DataTypeId = Request.QueryString["DataTypeId"];

            Observatory observatory = Repository.GetObservatory(Int32.Parse(ObservatoryId));
            Instrument instrument = Repository.GetInstrument(observatory, Int32.Parse(InstrumentId));
            DataType dataType = Repository.GetDataType(instrument.InstrumentType, Int32.Parse(DataTypeId));
            DowloadDatasetObservatory.Text = string.Format($"Observatory: {observatory.ShortName}     Instrument: {instrument.ShortName}     Data Type: {dataType.ShortName}");
            observatoryPath = observatory.RootFolder + instrument.InstrumentType.FolderMask.Substring(0, instrument.InstrumentType.FolderMask.IndexOf('\\')) + "\\";
        }

        protected void DownloadButton_Click(object sender, EventArgs e)
        {
            if(InputDateValidation() && FromCalendar.SelectedDate.Year != 1 && ToCalendar.SelectedDate.Year != 1)
            {
                double fileSize = FindFilesPathAndCalculatingFileSize();
                if (fileSize < 500)                                        ////check for the allowable download size. Limit 500 MB
                {   
                    if(fileSize == 0)
                    {
                        DownloadStatus.ForeColor = System.Drawing.Color.Red;
                        DownloadStatus.Text = "No files were found for the selected period";
                    }
                    else
                    {
                        string zipFilePath = CreateZip();
                        DownloadZipFile(zipFilePath);
                        DownloadStatus.ForeColor = System.Drawing.Color.Green;
                        DownloadStatus.Text = "Start loading...";
                    }   
                }                
                else
                {
                    DownloadStatus.ForeColor = System.Drawing.Color.Red;
                    DownloadStatus.Text = "The file size for the selected period is "+ string.Format("{0:0}", fileSize) + " MB, which exceeds the 500 MB limit. Reduce the download period.";
                }
            }
            else
            {
                DownloadStatus.ForeColor = System.Drawing.Color.Red;
                DownloadStatus.Text = "Invalid date entry";
            }
        }

        protected double FindFilesPathAndCalculatingFileSize()          //Creating a list of paths to files and calculating their size.
        {
            DownloadStatus.ForeColor = System.Drawing.Color.Green;
            DownloadStatus.Text = "Looking for your files...";
            string directoryWithFiles;
            string fileNamesMustContain;
            double fileSize = 0;
            for (var i = FromCalendar.SelectedDate; i <= ToCalendar.SelectedDate; i = i.AddDays(1))
            {
                directoryWithFiles = observatoryPath + i.Year.ToString() + "\\";
                if (i.Month < 10)
                {
                    directoryWithFiles = directoryWithFiles + "0" + i.Month.ToString() + "\\";
                }
                else
                {
                    directoryWithFiles = directoryWithFiles + i.Month.ToString() + "\\";
                }


                fileNamesMustContain = i.Year.ToString();
                if(i.Month < 10)
                {
                    fileNamesMustContain = fileNamesMustContain + "0" + i.Month.ToString();
                }
                else
                {
                    fileNamesMustContain = fileNamesMustContain + i.Month.ToString();
                }
                if(i.Day < 10)
                {
                    fileNamesMustContain = fileNamesMustContain + "0" + i.Day.ToString();
                }
                else
                {
                    fileNamesMustContain = fileNamesMustContain + i.Day.ToString();
                }
                if (Directory.Exists(directoryWithFiles))
                {
                    string[] AllFiles = Directory.GetFiles(directoryWithFiles);
                    foreach (var item in AllFiles)
                    {
                        if (item.Contains(fileNamesMustContain))
                        {
                            FileInfo file = new FileInfo(item);
                            FilesPath.Add(file.FullName);
                            fileSize += file.Length / 1000000.0;
                        }
                    }
                }
            }
            return fileSize;
        }

        protected string CreateZip()
        {
            DownloadStatus.ForeColor = System.Drawing.Color.Green;
            DownloadStatus.Text = "We are creating an archive. Your file will start downloading soon ...";
            string zipFilePath = "d:\\ZipTemp\\" + FromCalendar.SelectedDate.Day.ToString() + FromCalendar.SelectedDate.Month.ToString() + FromCalendar.SelectedDate.Year.ToString() + "-" + ToCalendar.SelectedDate.Day.ToString() + ToCalendar.SelectedDate.Month.ToString() + ToCalendar.SelectedDate.Year.ToString() + ".zip";
            using (ZipFile zip = new ZipFile(zipFilePath))
            {
                foreach (var item in FilesPath)
                {
                    zip.AddFile(item);
                }
                zip.Save();
            }
            return zipFilePath;
        }

        protected void DownloadZipFile(string zipFilePath)
        {
            DownloadStatus.ForeColor = System.Drawing.Color.Green;
            DownloadStatus.Text = "Start loading...";
            FileInfo file = new FileInfo(zipFilePath);
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "multipart/form-data";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();
        }

        protected bool InputDateValidation()
        {
            if(FromCalendar.SelectedDate > ToCalendar.SelectedDate)
            {
                return false;
            }
            return true;
        }


    }
}