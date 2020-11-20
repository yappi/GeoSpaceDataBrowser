using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GeospaceDataBrowser.Web
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.AboutMessage.Text = string.Format(Resources.LocalizedText.AboutMessageTemplate, DateTime.Now);
        }
    }
}