using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HWellnessEncr;
using HwHelpDesk.Shared.DataTransferObject;
using HwHelpDesk.Shared.DomainEntity;


namespace HwHelpDesk.Models
{
    public class Utility
    {
        StringBuilder xml = new StringBuilder();
        HWellness encr = new HWellness();
        SmtpClient smtp;
        //24 byte or 192 bit key and IV for TripleDES
        //One nice thing is you control what these numbers are so it is totally random!
        static private Byte[] KEY_192 = { 40, 50, 60, 89, 92, 6, 217, 30, 15, 16, 44, 60, 65, 25, 14, 12, 2, 14, 10, 20, 19, 9, 14, 17 };       

        static private Byte[] IV_192 = { 5, 13, 52, 4, 8, 1, 17, 3, 42, 5, 82, 83, 16, 7, 29, 13, 11, 3, 22, 8, 16, 10, 11, 25 };

        #region Triple Encryption
        public static string DecryptTripleDES(string value)
        {
            if ((value != "") && (value != null))
            {

                TripleDESCryptoServiceProvider cryptoProvider = new TripleDESCryptoServiceProvider();
                //convert from string to byte array
                Byte[] buffer = Convert.FromBase64String(value);
                MemoryStream ms = new MemoryStream(buffer);
                CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_192, IV_192), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);

                return sr.ReadToEnd();
            }
            else
            {
                return value;
            }
        }
        #endregion

        #region User Login
        public LoginResponse UserLogin(string userName,string password,string tokenID,string ipAddress)
        {
            List<userData> uD =new  List<userData>();
            LoginResponse response = new LoginResponse();
            SqlParameter[] param;
            try
            {
                string str = encrypt.QueryStringModule.Encrypt(password.Trim());
                //string strdec = encrypt.QueryStringModule.Decrypt(password.Trim());
                param = new SqlParameter[10];
                param[0] = MakeInParameter("@username", SqlDbType.VarChar, 50, userName);
                param[1] = MakeInParameter("@password1", SqlDbType.VarChar, 200, encrypt.QueryStringModule.Encrypt(password));
                param[2] = MakeOutParameter("@OUT_STATUS", SqlDbType.Int, 4);
                param[3] = MakeOutParameter("@OUT_type", SqlDbType.VarChar, 20);
                param[4] = MakeOutParameter("@out_username", SqlDbType.VarChar, 50);
                param[5] = MakeInParameter("@ipaddress", SqlDbType.VarChar, 50, ipAddress);
                param[6] = MakeOutParameter("@Out_c_day", SqlDbType.VarChar, 50);
                param[7] = MakeOutParameter("@DialerID", SqlDbType.Int, 6);
                param[8] = MakeOutParameter("@company_Id", SqlDbType.Int, 6);
                param[9] = MakeOutParameter("@user_promo", SqlDbType.VarChar, 25);
                RunProcedure("_sp_admin_login_new", param);
                if (Convert.ToInt32(param[2].Value) > 0)
                {
                    response.MsgCode = 200;
                    response.Msg = "success";
                }
                else
                {
                    response.MsgCode = 404;
                    response.Msg = "invalid";
                }
                userData userData = new userData();
                userData.userID = Convert.ToInt32(param[2].Value);
                userData.roleID = param[3].Value.ToString().Trim();
                userData.roleName = param[3].Value.ToString().Trim();
                userData.userName = param[4].Value.ToString().Trim();
                userData.lastLogin = param[6].Value.ToString().Trim();
                userData.dailerID = param[7].Value.ToString();
                userData.companyID = param[8].Value.ToString();
                uD.Add(userData);              
                response.userData = uD;
               
            }
            catch (Exception ex)
            {
                response.MsgCode = 0;
                response.Msg = ex.Message;
                response.userData = null;
            }
            return response;
        }
        #endregion

        #region Connection
        private SqlConnection Conn = null;
        public SqlConnection Connect()
        {
            string sConnStr = ConfigurationManager.ConnectionStrings["LocalContext"].ToString();
            if (object.Equals(Conn, null))
            {
                Conn = new SqlConnection(sConnStr);
                // Conn = new SqlConnection(DecryptTripleDES(sConnStr));               
            }
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }           
            return Conn;
        }

        public void FunCloseConnection()
        {
            if (Conn.State == ConnectionState.Open) Conn.Close();
            if (Conn != null)
            {
                Conn.Dispose();

                Conn = null;
            }

        }
        private SqlCommand FunCreateCommand(SqlConnection Conn, string sProcName, SqlParameter[] objaPrams)
        {

            SqlCommand objCommand = new SqlCommand(sProcName, Conn);
            objCommand.CommandTimeout = Conn.ConnectionTimeout;
            objCommand.CommandType = CommandType.StoredProcedure;


            if (objaPrams != null)
            {
                foreach (SqlParameter objParameter in objaPrams)
                    objCommand.Parameters.Add(objParameter);
            }

            objCommand.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));

            return objCommand;
        }

        private SqlCommand FunCreateCommand(SqlConnection Conn, string sProcName)
        {
            SqlCommand objCommand = new SqlCommand(sProcName, Conn);
            objCommand.CommandType = CommandType.StoredProcedure;
            return objCommand;
        }
        #endregion        

        #region Run Procedure
        public int RunProcedure(string sProcName, SqlParameter[] objaPrams)
        {
            SqlCommand objCommand = FunCreateCommand(Connect(), sProcName, objaPrams);
            objCommand.ExecuteNonQuery();
            this.FunCloseConnection();
            return (int)objCommand.Parameters["ReturnValue"].Value;
        }

        public void RunProcedure(string sProcName, SqlParameter[] objaPrams, out DataSet objDataSet)
        {
            SqlDataAdapter objDataAdapter = new SqlDataAdapter();
            SqlCommand objCommand = FunCreateCommand(Connect(), sProcName, objaPrams);
            objDataAdapter.SelectCommand = objCommand;
            objDataSet = new DataSet();
            objDataAdapter.Fill(objDataSet);
            this.FunCloseConnection();
        }

        public void RunProcedure(string sProcName, out DataSet objDataSet)
        {
            SqlDataAdapter objDataAdapter = new SqlDataAdapter();
            SqlCommand objCommand = FunCreateCommand(Connect(), sProcName);
            objDataAdapter.SelectCommand = objCommand;
            objDataSet = new DataSet();
            objDataAdapter.Fill(objDataSet);
            this.FunCloseConnection();
        }

        public void RunProcedure(string sProcName, SqlParameter[] objaPrams, out DataTable objDataTable)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand = FunCreateCommand(Connect(), sProcName, objaPrams);
            objDataTable = new DataTable();
            objDataTable.Load(objCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection));
        }
        #endregion        

        #region SQL Parameter
        public SqlParameter MakeInParameter(string sParamName, SqlDbType objDbType, int iSize, object objValue)
        {
            return MakeParameter(sParamName, objDbType, iSize, ParameterDirection.Input, objValue);
        }

        public SqlParameter MakeOutParameter(string sParamName, SqlDbType objDbType, int iSize)
        {
            return MakeParameter(sParamName, objDbType, iSize, ParameterDirection.Output, null);
        }
       
        public SqlParameter MakeParameter(string sParamName, SqlDbType objDbType, Int32 iSize, ParameterDirection objDirection, object objValue)
        {
            SqlParameter objParameter;

            if (iSize > 0)
                objParameter = new SqlParameter(sParamName, objDbType, iSize);
            else
                objParameter = new SqlParameter(sParamName, objDbType);

            objParameter.Direction = objDirection;
            if (!(objDirection == ParameterDirection.Output && objValue == null))
                objParameter.Value = objValue;
            return objParameter;
        }
        #endregion

        #region Send Email

        public static String ReadHtmlPage(string url)
        {
            string result = "";           
            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }

        public void Send_emailToVerifier_CancelOrder(int order_id, string Remark, string emailid)
        {
            DataSet ds = new DataSet();
           
            SqlParameter[] sqlParam = new SqlParameter[1];
            sqlParam[0] = MakeInParameter("@o_id", SqlDbType.Int, 8, order_id);
            RunProcedure("usp_fetch_orderdetailbyid", sqlParam, out ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                string Subject = ds.Tables[0].Rows[0]["orderstatus"].ToString();
                string html_mail_body = "";
                html_mail_body = "<div style='font:12px verdana'><p>Dear Sir/Mam</p> A order has been cancel. -<br />";
                html_mail_body = html_mail_body + "<table cellpadding='2' cellspacing='2' style='font:12px verdana; margin-top:10px'>";
                html_mail_body = html_mail_body + "<tr><td>Order No. - </td><td>" + ds.Tables[0].Rows[0]["o_number"].ToString() + "</td></tr>";
                html_mail_body = html_mail_body + "<tr><td>Customer Name - </td><td>" + ds.Tables[0].Rows[0]["Customer_Name"].ToString() + "</td></tr>";
                html_mail_body = html_mail_body + "<tr><td>Products Queried For - </td><td>" + ds.Tables[0].Rows[0]["Package_Name_Test"].ToString() + "</td></tr>";
                html_mail_body = html_mail_body + "<tr><td>Pickup Date - </td><td>" + ds.Tables[0].Rows[0]["Booking_Date"].ToString() + "</td></tr>";
                html_mail_body = html_mail_body + "<tr><td>Query - </td><td>" + Remark + "</td></tr>";
                html_mail_body = html_mail_body + "</table>";
                html_mail_body = html_mail_body + "</div>";
                bool trye = Mailwithattachment_HW(Subject, html_mail_body.ToString(), emailid, "", "");
            }
        }

        public void Send_email(int order_id, int msg_id)
        {
            DataSet ds = new DataSet();
           
            SqlParameter[] sqlParam = new SqlParameter[2];
            sqlParam[0] = MakeInParameter("@order_id", SqlDbType.Int, 8, order_id);
            sqlParam[1] = MakeInParameter("@e_id", SqlDbType.Int, 8, msg_id);
            RunProcedure("sp_get_customer_details_email", sqlParam, out ds);

            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                string e_subject = "";
                string html_mail_body = "";
                string package_name = "";
                string test_name = "";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    package_name += "<tr><td style=\"padding-bottom: 10px;padding-top: 10px;border-bottom: 1px solid #ddd;width:68%;\">" + ds.Tables[0].Rows[i]["pkg_name"].ToString() + "</td><td style=\"padding-bottom: 10px;padding-top: 10px;border-bottom: 1px solid #ddd;width:12%;\"> " + ds.Tables[0].Rows[i]["ot_price"].ToString() + "</td><td style=\"padding-bottom: 10px;padding-top: 10px;border-bottom: 1px solid #ddd;width:8%;\"> " + ds.Tables[0].Rows[i]["ot_discount_amount"].ToString() + "</td><td style=\"padding-bottom: 10px;padding-top: 10px;border-bottom: 1px solid #ddd;width:10%;\"> " + ds.Tables[0].Rows[i]["ot_final_amt"].ToString() + "</td></tr>";
                    test_name += "<tr><td style=\"text-align: left; width: 40%;\"></td><td style=\"text-align: left;\">" + ds.Tables[0].Rows[i]["pkg_name"].ToString() + " @ Rs." + ds.Tables[0].Rows[i]["ot_final_amt"].ToString() + "</td></tr>";
                }


                e_subject = ds.Tables[1].Rows[0]["e_subject"].ToString();
                html_mail_body = ds.Tables[1].Rows[0]["e_mail_body"].ToString();

                e_subject = e_subject.Replace("^", "'").Replace("##o_number##", ds.Tables[0].Rows[0]["o_number"].ToString())
                   .Replace("##o_date##", ds.Tables[0].Rows[0]["o_date"].ToString()).Replace("##o_time##", ds.Tables[0].Rows[0]["o_time"].ToString())
                   .Replace("##o_net_payable##", ds.Tables[0].Rows[0]["o_net_payable"].ToString()).Replace("##o_amt_recd##", ds.Tables[0].Rows[0]["o_amt_recd"].ToString())
                   .Replace("##Booking_Date##", ds.Tables[0].Rows[0]["Booking_Date"].ToString()).Replace("##o_discount##", ds.Tables[0].Rows[0]["o_discount"].ToString())
                   .Replace("##Customer_Name##", ds.Tables[0].Rows[0]["Customer_Name"].ToString()).Replace("##ca_mob##", encr.DecryptCRM(ds.Tables[0].Rows[0]["ca_mob"].ToString()))
                   .Replace("##ca_email##", encr.DecryptCRM(ds.Tables[0].Rows[0]["ca_email"].ToString())).Replace("##ot_price##", ds.Tables[0].Rows[0]["ot_price"].ToString())
                   .Replace("##ot_discount_amount##", ds.Tables[0].Rows[0]["ot_discount_amount"].ToString()).Replace("##ot_final_amt##", ds.Tables[0].Rows[0]["ot_final_amt"].ToString())
                   .Replace("##pkg_name##", ds.Tables[0].Rows[0]["pkg_name"].ToString()).Replace("##Pickup_Address##", ds.Tables[0].Rows[0]["Pickup_Address"].ToString())
                   .Replace("##Dietitian_Name##", ds.Tables[0].Rows[0]["Dietitian_Name"].ToString()).Replace("##Health_Assisment_Date##", ds.Tables[0].Rows[0]["Health_Assisment_Date"].ToString())
                   .Replace("##Dietitian_qualification##", ds.Tables[0].Rows[0]["Dietitian_qualification"].ToString()).Replace("##Dietitian_previous_experince##", ds.Tables[0].Rows[0]["Dietitian_previous_experince"].ToString())
                   .Replace("##Dietitian_pics##", ds.Tables[0].Rows[0]["Dietitian_pics"].ToString()).Replace("##about_dietitian##", ds.Tables[0].Rows[0]["about_dietitian"].ToString())
                   .Replace("##Doctor_Name##", ds.Tables[0].Rows[0]["Doctor_Name"].ToString()).Replace("##concall_date##", ds.Tables[0].Rows[0]["concall_date"].ToString())
                   .Replace("##concall_time##", ds.Tables[0].Rows[0]["concall_time"].ToString()).Replace("##Doctor_qualification##", ds.Tables[0].Rows[0]["Doctor_qualification"].ToString())
                   .Replace("##doctor_alma_mater##", ds.Tables[0].Rows[0]["doctor_alma_mater"].ToString()).Replace("##year_of_practice##", ds.Tables[0].Rows[0]["year_of_practice"].ToString())
                   .Replace("##previous_experince##", ds.Tables[0].Rows[0]["previous_experince"].ToString()).Replace("##speciality##", ds.Tables[0].Rows[0]["speciality"].ToString())
                   .Replace("##doctor_image##", ds.Tables[0].Rows[0]["doctor_image"].ToString()).Replace("##Cancel_Date##", ds.Tables[0].Rows[0]["Cancel_Date"].ToString())
                   .Replace("##Report_upload_date##", ds.Tables[0].Rows[0]["Report_upload_date"].ToString()).Replace("##package_name##", package_name).Replace("##test_name##", test_name)
                   .Replace("##hm_name##", ds.Tables[0].Rows[0]["hm_name"].ToString()).Replace("##sample_recive_time##", ds.Tables[0].Rows[0]["sample_recive_time"].ToString())
                   .Replace("##Phlebo_Name##", ds.Tables[0].Rows[0]["Phlebo_Name"].ToString()).Replace("##Package_Name_Test##", ds.Tables[0].Rows[0]["Package_Name_Test"].ToString()).Replace("##report_upload_time##", Convert.ToString(DateTime.Parse(ds.Tables[0].Rows[0]["Report_upload_time"].ToString()).ToString("hh:mm tt")));

                html_mail_body = html_mail_body.Replace("^", "'").Replace("##o_number##", ds.Tables[0].Rows[0]["o_number"].ToString())
                   .Replace("##o_date##", ds.Tables[0].Rows[0]["o_date"].ToString()).Replace("##o_time##", ds.Tables[0].Rows[0]["o_time"].ToString())
                   .Replace("##o_net_payable##", ds.Tables[0].Rows[0]["o_net_payable"].ToString()).Replace("##o_amt_recd##", ds.Tables[0].Rows[0]["o_amt_recd"].ToString())
                   .Replace("##Booking_Date##", ds.Tables[0].Rows[0]["Booking_Date"].ToString()).Replace("##o_discount##", ds.Tables[0].Rows[0]["o_discount"].ToString())
                   .Replace("##Customer_Name##", ds.Tables[0].Rows[0]["Customer_Name"].ToString()).Replace("##ca_mob##", encr.DecryptCRM(ds.Tables[0].Rows[0]["ca_mob"].ToString()))
                   .Replace("##ca_email##", encr.DecryptCRM(ds.Tables[0].Rows[0]["ca_email"].ToString())).Replace("##ot_price##", ds.Tables[0].Rows[0]["ot_price"].ToString())
                   .Replace("##ot_discount_amount##", ds.Tables[0].Rows[0]["ot_discount_amount"].ToString()).Replace("##ot_final_amt##", ds.Tables[0].Rows[0]["ot_final_amt"].ToString())
                   .Replace("##pkg_name##", ds.Tables[0].Rows[0]["pkg_name"].ToString()).Replace("##Pickup_Address##", ds.Tables[0].Rows[0]["Pickup_Address"].ToString())
                   .Replace("##Dietitian_Name##", ds.Tables[0].Rows[0]["Dietitian_Name"].ToString()).Replace("##Health_Assisment_Date##", ds.Tables[0].Rows[0]["Health_Assisment_Date"].ToString())
                   .Replace("##Dietitian_qualification##", ds.Tables[0].Rows[0]["Dietitian_qualification"].ToString()).Replace("##Dietitian_previous_experince##", ds.Tables[0].Rows[0]["Dietitian_previous_experince"].ToString())
                   .Replace("##Dietitian_pics##", ds.Tables[0].Rows[0]["Dietitian_pics"].ToString()).Replace("##about_dietitian##", ds.Tables[0].Rows[0]["about_dietitian"].ToString())
                   .Replace("##Doctor_Name##", ds.Tables[0].Rows[0]["Doctor_Name"].ToString()).Replace("##concall_date##", ds.Tables[0].Rows[0]["concall_date"].ToString())
                   .Replace("##concall_time##", ds.Tables[0].Rows[0]["concall_time"].ToString()).Replace("##Doctor_qualification##", ds.Tables[0].Rows[0]["Doctor_qualification"].ToString())
                   .Replace("##doctor_alma_mater##", ds.Tables[0].Rows[0]["doctor_alma_mater"].ToString()).Replace("##year_of_practice##", ds.Tables[0].Rows[0]["year_of_practice"].ToString())
                   .Replace("##previous_experince##", ds.Tables[0].Rows[0]["previous_experince"].ToString()).Replace("##speciality##", ds.Tables[0].Rows[0]["speciality"].ToString())
                   .Replace("##doctor_image##", ds.Tables[0].Rows[0]["doctor_image"].ToString()).Replace("##Cancel_Date##", ds.Tables[0].Rows[0]["Cancel_Date"].ToString())
                   .Replace("##Report_upload_date##", ds.Tables[0].Rows[0]["Report_upload_date"].ToString()).Replace("##package_name##", package_name).Replace("##test_name##", test_name)
                   .Replace("##hm_name##", ds.Tables[0].Rows[0]["hm_name"].ToString()).Replace("##sample_recive_time##", ds.Tables[0].Rows[0]["sample_recive_time"].ToString())
                   .Replace("##Phlebo_Name##", ds.Tables[0].Rows[0]["Phlebo_Name"].ToString()).Replace("##Package_Name_Test##", ds.Tables[0].Rows[0]["Package_Name_Test"].ToString()).Replace("##report_upload_time##", Convert.ToString(DateTime.Parse(ds.Tables[0].Rows[0]["Report_upload_time"].ToString()).ToString("hh:mm tt")));


                bool trye = Mailwithattachment_HW(e_subject, html_mail_body.ToString(), ds.Tables[0].Rows[0]["ca_email"].ToString(), "", "");
            }
        }
        public bool Mailwithattachment_HW(string subject, string body, string to, string Bcc, string path)
        {
            MailMessage mail = new MailMessage();
            try
            {
                if (object.Equals(smtp, null))
                {
                    smtp = new SmtpClient();
                }
                mail.From = new MailAddress("noreply@hindustanwellness.com", "Hindustan Wellness");
                mail.To.Add(encr.DecryptCRM(to));
                if (Bcc != "")
                {
                    mail.CC.Add(Bcc);
                }
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.ReplyTo = (new MailAddress("customer.service@hindustanwellness.com"));
                if (path.ToString() != "")
                {
                    try
                    {
                        mail.Attachments.Add(new Attachment(path));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                smtp.Host = "mail.smtp2go.com";
                smtp.EnableSsl = false;
                smtp.Credentials = new System.Net.NetworkCredential("noreply@hindustanwellness.com", "Y5DmHVaOA6Cg");
                try
                {
                    smtp.Send(mail);
                    mail.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Encrypt Decrypt
        //----******Encrypt Function To Encrypt Password
        private const string ENCRYPTION_KEY = "key";
        private readonly static byte[] SALT = Encoding.ASCII.GetBytes(ENCRYPTION_KEY.Length.ToString());
        
        public static string Encrypt(string inputText)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] plainText = Encoding.Unicode.GetBytes(inputText);
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(ENCRYPTION_KEY, SALT);
            using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16)))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainText, 0, plainText.Length);
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion

        #region Send Sms
        public void SendNew_sms(int order_id, int msg_id)
        {
            DataSet ds = new DataSet();
            
            SqlParameter[] sqlParam = new SqlParameter[2];
            sqlParam[0] = MakeInParameter("@order_id", SqlDbType.Int, 8, order_id);
            sqlParam[1] = MakeInParameter("@sms_id", SqlDbType.Int, 8, msg_id);
            RunProcedure("sp_get_customer_details_sms_live", sqlParam, out ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
                    {
                        string sms_body = "";
                        string line_braker = "";
                        sms_body = ds.Tables[1].Rows[0]["sms_template"].ToString();
                        line_braker = ds.Tables[1].Rows[0]["line_braker"].ToString();

                        string login_id = "";
                        string password = "";

                        try
                        {
                            login_id = encr.DecryptCRM(ds.Tables[0].Rows[0]["cl_login_id"].ToString());
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            password = DecryptTripleDES(ds.Tables[0].Rows[0]["cl_password"].ToString());
                        }
                        catch (Exception)
                        {
                        }

                        sms_body = sms_body.Replace("|", line_braker)
                            .Replace("##customer_name##", ds.Tables[0].Rows[0]["Customer_Name"].ToString())
                            .Replace("##customer_dob##", ds.Tables[0].Rows[0]["ca_dob"].ToString())
                            .Replace("##customer_mobile##", encr.DecryptCRM(ds.Tables[0].Rows[0]["ca_mob"].ToString()))
                            .Replace("##customer_alt_mobile##", encr.DecryptCRM(ds.Tables[0].Rows[0]["ca_alternate_number"].ToString()))
                            .Replace("##customer_email##", encr.DecryptCRM(ds.Tables[0].Rows[0]["ca_email"].ToString()))
                            .Replace("##order_number##", ds.Tables[0].Rows[0]["o_number"].ToString())
                            .Replace("##pickup_date##", ds.Tables[0].Rows[0]["o_date"].ToString())
                            .Replace("##pickup_time##", ds.Tables[0].Rows[0]["o_time"].ToString())
                            .Replace("##booking_date##", ds.Tables[0].Rows[0]["Booking_Date"].ToString())
                            .Replace("##order_status##", ds.Tables[0].Rows[0]["os_name"].ToString())
                            .Replace("##payable_amount##", ds.Tables[0].Rows[0]["o_net_payable"].ToString())
                            .Replace("##recived_amount##", ds.Tables[0].Rows[0]["o_amt_recd"].ToString())
                            .Replace("##pkg_text_name##", ds.Tables[0].Rows[0]["Package_Name_Test"].ToString())
                            .Replace("##remaning_amount##", ds.Tables[0].Rows[0]["Remaning_amount"].ToString())
                            .Replace("##customer_pickup_address##", ds.Tables[0].Rows[0]["Pickup_Address"].ToString())
                            .Replace("##phlebo_name##", ds.Tables[0].Rows[0]["Phlebo_Name"].ToString())
                            .Replace("##phlebo_mobile##", ds.Tables[0].Rows[0]["phlebo_Number"].ToString())
                            .Replace("##report_upload_date##", ds.Tables[0].Rows[0]["Report_upload_date"].ToString())
                            .Replace("##report_upload_time##", ds.Tables[0].Rows[0]["Report_upload_time"].ToString())
                            .Replace("##dccs_name##", ds.Tables[0].Rows[0]["dccs_name"].ToString())
                            .Replace("##doctor_name##", ds.Tables[0].Rows[0]["Doctor_Name"].ToString())
                            .Replace("##doctor_concall_date##", ds.Tables[0].Rows[0]["concall_date"].ToString())
                            .Replace("##doctor_concall_time##", ds.Tables[0].Rows[0]["concall_time"].ToString())
                            .Replace("##dietitian_name##", ds.Tables[0].Rows[0]["Dietitian_Name"].ToString())
                            .Replace("##hm_name##", ds.Tables[0].Rows[0]["hm_name"].ToString())
                            .Replace("##Payment_Mode##", ds.Tables[0].Rows[0]["Payment_Method"].ToString())
                            .Replace("##customer_login_id##", login_id.ToString())
                            .Replace("##customer_password##", password.ToString())
                            .Replace("##customer_age##", ds.Tables[0].Rows[0]["ca_age"].ToString())
                            .Replace("##customer_gender##", ds.Tables[0].Rows[0]["ca_gender"].ToString());

                        Gosms(ds.Tables[0].Rows[0]["ca_mob"].ToString(), sms_body.ToString());
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public string Gosms(string mobileno, string message)
        {
            #region mobile app notification
            message = message.Replace("%0D%0A", "%n").Replace("%n", "\n");

            try
            {
                SqlParameter[] sqlParam1 = new SqlParameter[2];
                sqlParam1[0] = MakeInParameter("@mob", SqlDbType.VarChar, 200, mobileno);
                sqlParam1[1] = MakeInParameter("@message", SqlDbType.VarChar, 600, message);
                RunProcedure("sp_customerapp_sms_notification", sqlParam1);
            }
            catch (Exception ex)
            {

            }
            #endregion
            string answer = "";
            try
            {
                int count = 0;
                string mobno = string.Empty;
                string url = string.Empty;
                String surl = string.Empty;
                string sSendrnumber = string.Empty;
                string Adddigit91 = "";
                try
                {
                    DataSet ds = new DataSet();
                    RunProcedure("Udp_getActiveSmsSetUp", out ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        surl = ds.Tables[0].Rows[0]["URL"].ToString();
                        sSendrnumber = ds.Tables[0].Rows[0]["Sender"].ToString();
                        Adddigit91 = ds.Tables[0].Rows[0]["AddDigit91"].ToString();
                        surl = DecryptTripleDES(surl);
                    }
                }
                catch (Exception ex) { }
                int chkcount = 0;
                int checksms = 0;
                xml.Append("<root>");
                string[] mob = mobileno.ToString().Trim().Split(',');

                if (mob.Length > 0)
                {
                    for (int ln = 0; ln < mob.Length; ln++)
                    {
                        checksms = 1;
                        chkcount = chkcount + 1;
                        mobileno = encr.DecryptCRM(mob[ln].Trim());//dr["MobileNo"].ToString();
                        int j = mobileno.Length;
                        int counts = mobileno.Length;
                        int k = j - 10;
                        if (j < 10)
                        {
                            mobileno = "";
                        }
                        if (j == 10)
                        {
                            if (Adddigit91 == "Y")
                                mobno = "91" + "" + mobileno;
                            else
                                mobno = mobileno;
                        }
                        if (j > 10)
                        {
                            mobileno = mobileno.Remove(0, k);
                            if (Adddigit91 == "Y")
                                mobno = "91" + "" + mobileno;
                            else
                                mobno = mobileno;
                        }

                        //  message = message.Replace("&", "%26");
                        if (mobileno != "&nbsp;" && mobileno != "")
                        {
                            xml.Append("<SMS>");
                            xml.Append("<subject>" + message + "</subject>");
                            xml.Append("<Mobile_No>" + mobileno + "</Mobile_No>");
                            xml.Append("<Sender>" + sSendrnumber + "</Sender>");
                            xml.Append("<Update_Datetime>" + System.DateTime.Now + "</Update_Datetime>");
                            #region Genrate URL
                            url = surl;
                            url = url.Replace("@MN@", mobno);
                            url = url.Replace("@MText@", HttpUtility.UrlEncode(message));
                            string status = ReadHtmlPage(url);
                            #endregion
                            count = count + 1;
                            int length = message.Length;
                            int divlength = length / 157;
                            decimal remilngth = length % 157;
                            if (divlength == 0)
                            {
                                length = 1;
                            }
                            else
                            {
                                length = divlength;
                                if (remilngth != 0)
                                {
                                    length = length + 1;
                                }
                            }
                            xml.Append("</SMS>");
                            int legthxml = xml.Length;
                            if (legthxml > 7000 && legthxml < 7950)
                            {
                                xml.Append("</root>");
                                xml = new StringBuilder();
                                xml.Append("<root>");
                            }
                        }
                    }
                }


                xml.Append("</root>");
                try
                {

                    if (count == 0 && chkcount != 0)
                    {
                        answer = "Applicant mobile no. not available.";
                    }
                    if (count == 0 && chkcount != 0 && mobileno == "")
                    {
                        answer = "Applicant mobile no. not valid.";
                    }
                    if (count > 0)
                    {
                        answer = count + " SMS send successfully to your Applicant.";
                    }
                }
                catch (Exception)
                {
                    answer = "Sorry ! your information is not send, please contact administrator";
                }
            }
            catch (Exception e)
            {
                answer = "Sorry ! your information is not send, please contact administrator";
            }

            return answer;

        }

        public DataSet GET_SMS_DETAIL(int OrderID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[1];
                sqlParam[0] = MakeInParameter("@OrderID", SqlDbType.Int, 10, OrderID);
                RunProcedure("GET_SMS_DETAIL", sqlParam, out ds);
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        #endregion

        #region Email validation
        public string ValidateEmail(string emailID)
        {            
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(emailID);
            if (match.Success)
                return "success";
            else
                return "failed";
        }
        #endregion

        #region Get Customer Age
        public int GetCustomerAge(string dob)
        {
            // Save today's date.
            var today = DateTime.Today;

            var birthday = Convert.ToDateTime(dob);
            // Calculate the age.
            var age = today.Year - birthday.Year;
            // Go back to the year the person was born in case of a leap year
            if (birthday.Date > today.AddYears(-age)) age--;
            return age;
        }
        #endregion

        #region mobile Validation
        public string MobileValidate(string mobileNo)
        {
            if (Convert.ToInt32(mobileNo.Length) == 10)
            {
                bool check = IsNumeric(mobileNo);
                if (check == false)
                {
                    return "failed";
                }
                else
                {
                    return "success";
                }
            }
            else
            {
                return "failed";
            }
        }
        #endregion

        #region Date Format Validation

        public bool ValidateDate(string date)
        {
            bool status;
            DateTime dDate;
            if (DateTime.TryParse(date,out dDate))
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }
        #endregion

        #region isNumeric
        public bool IsNumeric(string value)
        {
            bool isNumeric = true;
            isNumeric = Regex.IsMatch(value, @"^([0|\+[0-9]{1,5})?([7-9][0-9]{9})$");
            return isNumeric;  
        }
        #endregion

        
    }
}