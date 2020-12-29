using System;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GeospaceDataBrowser.Web.Account
{
    public partial class PersonalPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindUserGrid();

                //Bind the users and roles 
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
            GridView UserGrid = (GridView)LoginView1.FindControl("UserGrid");
            if (UserGrid != null)
            {
                UserGrid.DataSource = allUsers;
                UserGrid.DataBind();
            }
        }

        private void BindUsersToUserList()
        {
            // Get all of the user accounts 
            MembershipUserCollection users = Membership.GetAllUsers();
            DropDownList UserList = (DropDownList)LoginView1.FindControl("UserList");
            if (UserList != null)
            {
                UserList.DataSource = users;
                UserList.DataBind();
            }
        }

        //protected void UserGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    // Determine the username of the user we are editing
        //    GridView UserGrid = (GridView)LoginView1.FindControl("UserGrid");
        //    string UserName = UserGrid.DataKeys[e.RowIndex].Value.ToString();

        //    // Delete the user
        //    Membership.DeleteUser(UserName);

        //    // Revert the grid's EditIndex to -1 and rebind the data
        //    UserGrid.EditIndex = -1;
        //    BindUserGrid();
        //}

        private void BindRolesToList()
        {
            // Get all of the roles 
            string[] roles = System.Web.Security.Roles.GetAllRoles();
            Repeater UsersRoleList = (Repeater)LoginView1.FindControl("UsersRoleList");
            if (UsersRoleList != null)
            {
                UsersRoleList.DataSource = roles;
                UsersRoleList.DataBind();
            }

            DropDownList RoleList = (DropDownList)LoginView1.FindControl("RoleList");
            if (RoleList != null)
            {
                RoleList.DataSource = roles;
                RoleList.DataBind();
            }
        }

        private void CheckRolesForSelectedUser()
        {
            // Determine what roles the selected user belongs to 
            DropDownList UserList = (DropDownList)LoginView1.FindControl("UserList");
            if (UserList != null)
            {
                string selectedUserName = UserList.SelectedValue;
                string[] selectedUsersRoles = System.Web.Security.Roles.GetRolesForUser(selectedUserName);


                // Loop through the Repeater's Items and check or uncheck the checkbox as needed 
                Repeater UsersRoleList = (Repeater)LoginView1.FindControl("UsersRoleList");
                if (UsersRoleList != null)
                {
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
            DropDownList UserList = (DropDownList)LoginView1.FindControl("UserList");
            if (UserList != null)
            {
                string selectedUserName = UserList.SelectedValue;

                string roleName = RoleCheckBox.Text;

                // Determine if we need to add or remove the user from this role 
                Label ActionStatus = (Label)LoginView1.FindControl("ActionStatus");
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
        }

        private void DisplayUsersBelongingToRole()
        {
            // Get the selected role 
            DropDownList RoleList = (DropDownList)LoginView1.FindControl("RoleList");
            if (RoleList != null)
            {
                string selectedRoleName = RoleList.SelectedValue;

                // Get the list of usernames that belong to the role 
                string[] usersBelongingToRole = System.Web.Security.Roles.GetUsersInRole(selectedRoleName);

                // Bind the list of users to the GridView 
                GridView RolesUserList = (GridView)LoginView1.FindControl("RolesUserList");
                RolesUserList.DataSource = usersBelongingToRole;
                RolesUserList.DataBind();
            }
        }

        protected void RoleList_SelectedIndexChanged1(object sender, EventArgs e)
        {
            DisplayUsersBelongingToRole();
        }

        protected void RolesUserList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the selected role 
            DropDownList RoleList = (DropDownList)LoginView1.FindControl("RoleList");
            if (RoleList != null)
            {
                string selectedRoleName = RoleList.SelectedValue;

                // Reference the UserNameLabel 
                GridView RolesUserList = (GridView)LoginView1.FindControl("RolesUserList");
                Label UserNameLabel = RolesUserList.Rows[e.RowIndex].FindControl("UserNameLabel") as Label;

                // Remove the user from the role 
                System.Web.Security.Roles.RemoveUserFromRole(UserNameLabel.Text, selectedRoleName);

                // Refresh the GridView 
                DisplayUsersBelongingToRole();

                // Display a status message 
                Label ActionStatus = (Label)LoginView1.FindControl("ActionStatus");
                ActionStatus.Text = string.Format("User {0} was removed from role {1}.", UserNameLabel.Text, selectedRoleName);
                CheckRolesForSelectedUser();
            }
        }

    }
}