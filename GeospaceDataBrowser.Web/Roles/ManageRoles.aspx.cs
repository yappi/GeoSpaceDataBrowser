using System;
using System.Web.Security;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GeospaceDataBrowser.Web.Roles
{
    public partial class ManageRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                DisplayRolesInGrid();
        }

        protected void CreateRoleButton_Click(object sender, EventArgs e)
        {
            string newRoleName = RoleName.Text.Trim();

            if (!System.Web.Security.Roles.RoleExists(newRoleName))
                // Create the role
                System.Web.Security.Roles.CreateRole(newRoleName);

            RoleName.Text = string.Empty;
        }

        private void DisplayRolesInGrid()
        {
            RoleList.DataSource = System.Web.Security.Roles.GetAllRoles();
            RoleList.DataBind();
        }
    }
}