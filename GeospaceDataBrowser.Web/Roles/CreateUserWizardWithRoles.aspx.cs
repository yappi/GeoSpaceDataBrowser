using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
namespace GeospaceDataBrowser.Web.Roles
{
    public partial class CreateUserWizardWithRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindUserGrid();
                
                // Bind the users and roles 
                BindUsersToUserList();
                BindRolesToList();

                // Check the selected user's roles 
                CheckRolesForSelectedUser();

                // Display those users belonging to the currently selected role 
                DisplayUsersBelongingToRole();
            }
        }
        private void BindUserGrid()
        {
            MembershipUserCollection allUsers = Membership.GetAllUsers();
            UserGrid.DataSource = allUsers;
            UserGrid.DataBind();
        }

        private void BindUsersToUserList()
        {
            // Get all of the user accounts 
            MembershipUserCollection users = Membership.GetAllUsers();
            UserList.DataSource = users;
            UserList.DataBind();
        }

        private void BindRolesToList()
        {
            // Get all of the roles 
            string[] roles = System.Web.Security.Roles.GetAllRoles();
            UsersRoleList.DataSource = roles;
            UsersRoleList.DataBind();

            RoleList.DataSource = roles;
            RoleList.DataBind();
        }

        private void CheckRolesForSelectedUser()
        {
            // Determine what roles the selected user belongs to 
            string selectedUserName = UserList.SelectedValue;
            string[] selectedUsersRoles = System.Web.Security.Roles.GetRolesForUser(selectedUserName);

            // Loop through the Repeater's Items and check or uncheck the checkbox as needed 

            foreach (RepeaterItem ri in UsersRoleList.Items)
            {
                // Programmatically reference the CheckBox 
                CheckBox RoleCheckBox = ri.FindControl("RoleCheckBox") as CheckBox;
                // See if RoleCheckBox.Text is in selectedUsersRoles 
                if (selectedUsersRoles.Contains<string>(RoleCheckBox.Text))
                    RoleCheckBox.Checked = true;
                else
                    RoleCheckBox.Checked = false;
            }
        }

        protected void UserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckRolesForSelectedUser();
        }

        protected void RoleCheckBox_CheckChanged(object sender, EventArgs e)
        {
            // Reference the CheckBox that raised this event 
            CheckBox RoleCheckBox = sender as CheckBox;

            // Get the currently selected user and role 
            string selectedUserName = UserList.SelectedValue;

            string roleName = RoleCheckBox.Text;

            // Determine if we need to add or remove the user from this role 
            if (RoleCheckBox.Checked)
            {
                // Add the user to the role 
                System.Web.Security.Roles.AddUserToRole(selectedUserName, roleName);
                // Display a status message 
                ActionStatus.Text = string.Format("User {0} was added to role {1}.", selectedUserName, roleName);
            }
            else
            {
                // Remove the user from the role 
                System.Web.Security.Roles.RemoveUserFromRole(selectedUserName, roleName);
                // Display a status message 
                ActionStatus.Text = string.Format("User {0} was removed from role {1}.", selectedUserName, roleName);

            }
        }
        protected void UserList_SelectedIndexChanged1(object sender, EventArgs e)
        {
            CheckRolesForSelectedUser();
        }

        private void DisplayUsersBelongingToRole()
        {
            // Get the selected role 
            string selectedRoleName = RoleList.SelectedValue;

            // Get the list of usernames that belong to the role 
            string[] usersBelongingToRole = System.Web.Security.Roles.GetUsersInRole(selectedRoleName);

            // Bind the list of users to the GridView 
            RolesUserList.DataSource = usersBelongingToRole;
            RolesUserList.DataBind();
        }

        protected void RoleList_SelectedIndexChanged1(object sender, EventArgs e)
        {
            DisplayUsersBelongingToRole();
        }

        protected void RolesUserList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the selected role 
            string selectedRoleName = RoleList.SelectedValue;

            // Reference the UserNameLabel 
            Label UserNameLabel = RolesUserList.Rows[e.RowIndex].FindControl("UserNameLabel") as Label;

            // Remove the user from the role 
            System.Web.Security.Roles.RemoveUserFromRole(UserNameLabel.Text, selectedRoleName);

            // Refresh the GridView 
            DisplayUsersBelongingToRole();

            // Display a status message 
            ActionStatus.Text = string.Format("User {0} was removed from role {1}.", UserNameLabel.Text, selectedRoleName);
        }

        protected void UserGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Set the grid's EditIndex and rebind the data

            UserGrid.EditIndex = e.NewEditIndex;
            BindUserGrid();
        }

        protected void UserGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Revert the grid's EditIndex to -1 and rebind the data
            UserGrid.EditIndex = -1;
            BindUserGrid();
        }

        protected void UserGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Exit if the page is not valid
            if (!Page.IsValid)
                return;

            // Determine the username of the user we are editing
            string UserName = UserGrid.DataKeys[e.RowIndex].Value.ToString();

            // Read in the entered information and update the user
            TextBox EmailTextBox = UserGrid.Rows[e.RowIndex].FindControl("Email") as TextBox;
            TextBox CommentTextBox = UserGrid.Rows[e.RowIndex].FindControl("Comment") as TextBox;

            // Return information about the user
            MembershipUser UserInfo = Membership.GetUser(UserName);

            // Update the User account information
            UserInfo.Email = EmailTextBox.Text.Trim();
            UserInfo.Comment = CommentTextBox.Text.Trim();

            Membership.UpdateUser(UserInfo);

            // Revert the grid's EditIndex to -1 and rebind the data
            UserGrid.EditIndex = -1;
            BindUserGrid();
        }
        protected void UserGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Determine the username of the user we are editing
            string UserName = UserGrid.DataKeys[e.RowIndex].Value.ToString();

            // Delete the user
            Membership.DeleteUser(UserName);

            // Revert the grid's EditIndex to -1 and rebind the data
            UserGrid.EditIndex = -1;
            BindUserGrid();
        }


    }
}