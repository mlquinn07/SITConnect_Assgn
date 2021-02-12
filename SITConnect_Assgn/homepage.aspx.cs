using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace SITConnect_Assgn
{
    public partial class homepage : System.Web.UI.Page
    {
        string MYSITConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["MYSITConnect"].ConnectionString;
        string userID = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    userID = (string)Session["userID"];

                    displayUserProfile(userID);

                }
            }
            //if (Session["LoggedIn"] != null)
            //{
                //lblMessage.Text = "Yay!. Logged in.";
                //lblMessage.ForeColor = System.Drawing.Color.Pink;
                //btnLogout.Visible = true;
            //}
            //if any errors occur, delete this part.
            //else
            //{
                //Response.Redirect("Login2.aspx", false);
            //}
        }

        protected void LogoutMe(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login2.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }

        protected void displayUserProfile(string userid)
        {
            SqlConnection conn = new SqlConnection(MYSITConnectionString);
            string sql = "SELECT * FROM Stationery WHERE Email=@userId";
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@userId", userid);

            try
            {
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.Read())
                        {
                            if (reader["Email"] != DBNull.Value)
                            {
                                lb_email.Text = reader["Email"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
    }
}