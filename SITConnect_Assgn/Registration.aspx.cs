using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Drawing;

namespace SITConnect_Assgn
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYSITConnectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["MYSITConnect"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private int checkPassword(string password)
        {
            int score = 0;
            //score 1 - very weak
            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            //score 2 - weak
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            //score 3 - medium
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            //score 4 - strong
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            //score 5 - excellent
            if (Regex.IsMatch(password, "[^A-Za-z0-9]"))
            {
                score++;
            }

            return score;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            // Authentication
            string pwd = tb_password.Text.ToString().Trim();

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];

            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);

            SHA512Managed hashing = new SHA512Managed();

            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

            finalHash = Convert.ToBase64String(hashWithSalt);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;

            createStationery();

            //prevent XSS by sanitising the user input.
            int scores = checkPassword(HttpUtility.HtmlEncode(tb_password.Text));
            string status = "";
            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;

                case 2:
                    status = "Weak";
                    break;

                case 3:
                    status = "Medium";
                    break;

                case 4:
                    status = "Strong";
                    break;

                case 5:
                    status = "Excellent";
                    break;

                default:
                    break;
            }
            lb_pwdchecker.Text = "Status: " + status;
            if (scores < 4)
            {
                lb_pwdchecker.ForeColor = Color.Red;
                return;
            }
            lb_pwdchecker.ForeColor = Color.Green;
        }

        public void createStationery()
        {
            //DateTime DOB = how to convert to datetime input??
            //DateTime DOB = Convert.ToDateTime
            try
            {
                using (SqlConnection con = new SqlConnection(MYSITConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Stationery VALUES(@Fname, @Lname, @Card, @Email, @PasswordHash, @PasswordSalt, @Dob, @CardVerified, @EmailVerified)"))
                    {
                        //cmd.Parameters.AddWithValue() - encryptData when it is private info and not to be seen by others. 
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Fname", tb_fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@Lname", tb_lname.Text.Trim());
                            cmd.Parameters.AddWithValue("@Card", encryptData(tb_CCI.Text.Trim()));
                            cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            //check whether this works?
                            cmd.Parameters.AddWithValue("@Dob", Convert.ToDateTime(tb_dob.Text));
                            cmd.Parameters.AddWithValue("@CardVerified", DBNull.Value);
                            cmd.Parameters.AddWithValue("@EmailVerified", DBNull.Value);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected byte [] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor(); :>
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {

            }
            return cipherText;
        }
    }
}