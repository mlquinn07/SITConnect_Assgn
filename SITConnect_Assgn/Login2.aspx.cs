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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace SITConnect_Assgn
{
    public partial class Login2 : System.Web.UI.Page
    {
        string MYSITConnectionString =
    System.Configuration.ConfigurationManager.ConnectionStrings["MYSITConnect"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //ValidateCaptcha part
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                ("https://www.google.com/recaptcha/api/siteverify?secret=xxxxxxxxxxxxxxxxxxx &response=" + captchaResponse);

            try
            {
                using (WebResponse wresponse = req.GetResponse())
                {
                    using (StreamReader readstream = new StreamReader(wresponse.GetResponseStream()))
                    {
                        string jsonresponse = readstream.ReadToEnd();

                        lbl_gScore.Text = jsonresponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        MyObject jsonobject = js.Deserialize<MyObject>(jsonresponse);

                        result = Convert.ToBoolean(jsonobject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }

        }

        //get the data to transfer to here

        protected void LoginMe(object sender, EventArgs e)
        {
            string pwd = tb_pwd.Text.ToString().Trim();
            string userid = tb_email.Text.ToString().Trim();
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);

            if (ValidateCaptcha())
            {
                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);

                        if (userHash.Equals(dbHash))
                        {
                            Session["UserID"] = userid;

                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;

                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                            Response.Redirect("homepage.aspx", false);
                        }
                        else
                        {
                            lblMessage.Text = "Invalid. Please try again.";
                            //Response.Redirect("Login2.aspx", false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally { }
            }
            //try
            //{
            //    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
            //    {
            //        string pwdWithSalt = pwd + dbSalt;
            //        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            //        string userHash = Convert.ToBase64String(hashWithSalt);

            //        if (userHash.Equals(dbHash))
            //        {
            //            Session["UserID"] = userid;

            //            string guid = Guid.NewGuid().ToString();
            //            Session["AuthToken"] = guid;

            //            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
            //            Response.Redirect("homepage.aspx", false);
            //        }
            //        else
            //        {
            //            lblMessage.Text = "Invalid. Please try again.";
            //            //Response.Redirect("Login2.aspx", false);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.ToString());
            //}
            //finally { }
            //if (tb_email.Text.Trim().Equals("u") && tb_pwd.Text.Trim().Equals("p"))
            //{
            //    Session["LoggedIn"] = tb_email.Text.Trim();

            //    string guid = Guid.NewGuid().ToString();
            //    Session["AuthToken"] = guid;

            //    Response.Cookies.Add(new HttpCookie("AuthToken", guid));

            //    Response.Redirect("homepage.aspx", false);
            //}
            //else
            //{
            //    lblMessage.Text = "Invalid. Please try again.";
            //}
        }
        protected string getDBHash(string userid)
        {
            string h = null;

            SqlConnection conn = new SqlConnection(MYSITConnectionString);
            string sql = "select PasswordHash FROM Stationery WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != DBNull.Value)
                        {
                            h = reader["PasswordHash"].ToString();
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
            return h;
        }
        protected string getDBSalt(string userid)
        {
            string s = null;

            SqlConnection conn = new SqlConnection(MYSITConnectionString);
            string sql = "select PasswordSalt FROM Stationery WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != DBNull.Value)
                        {
                            s = reader["PASSWORDSALT"].ToString();
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
            return s;
        }
    }
}
