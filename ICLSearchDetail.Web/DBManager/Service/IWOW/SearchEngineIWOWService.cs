using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using ICLSearchDetail.Web.DBManager.Model.SearchEngineModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;

namespace ICLSearchDetail.Web.DBManager.Service
{
    public class SearchEngineIWOWService
    {
        public string getUserIDDateDetails(string userIDDate)
        {
           string[] paramDetails;
           paramDetails = userIDDate.Split('|');

           string varUserID = paramDetails[0];
           string varDate = paramDetails[1];

            var fileLoc = ConfigurationManager.AppSettings["IWOWLoc"];
            
            var retSql = "";
            using (StreamReader file = new StreamReader(fileLoc))
            {
                //int counter = 0;
                string ln;
                while (((ln = file.ReadLine()) != null) && !ln.Contains("--"))
                {
                    retSql += ln;
                }
                file.Close();
            }

            string ret = "";
            var sqlQuery = retSql + " WHERE TIW.SVS_FIRST = '" + varUserID + "' AND TIW.TRANSACTION_DATE = '" + varDate + "'";
            //var sqlQuery = @"SELECT TIW.TRANSACTION_DATE AS 'TRANSACTION DATE',
            //                TIW.INSTRUMENT_NUMBER AS 'CHECK NO.',
            //                TIW.CAR_AMOUNT AS 'AMOUNT',
            //                TIW.ZP_AMT_CREATOR AS 'AMOUNT WISE USER',
            //                TIW.SVSFV_TIME AS 'SVS VERIFICATION TIME',
            //                TIW.SVS_FIRST AS 'SVS VERIFY BY USER ID',
            //                TUR.FNAME AS 'SVS VERIFY BY USER NAME',
            //                TIW.PAYEE_BANK_CITY_CODE +
            //                TIW.PAYEE_BANK_CODE +
            //                TIW.PAYEE_BANK_BRANCH_CODE +
            //                TIW.PAYEE_CHECK_DIGIT AS 'CHECK BRSTN' ,
            //                TEN.BRANCH_NAME AS 'CHECK BRANCH NAME'
            //                FROM TBL_INWARD TIW INNER JOIN TBL_USER TUR
            //                ON TIW.SVS_FIRST = TUR.ID
            //                INNER JOIN TBL_ENTITY TEN ON TIW.PAYEE_BANK_CITY_CODE +
            //                TIW.PAYEE_BANK_CODE +
            //                TIW.PAYEE_BANK_BRANCH_CODE +
            //                TIW.PAYEE_CHECK_DIGIT = TEN.ENTITY_ID
            //                WHERE TIW.SVS_FIRST = '" + varUserID + "' AND TIW.TRANSACTION_DATE ='" + varDate + "'";


            if (paramDetails.Length == 4)
            {
                string varHour = paramDetails[2];
                int varMxHour = Convert.ToInt16(varHour) + 1;
                string varMin = paramDetails[3];
                sqlQuery = sqlQuery + " AND TIW.SVSFV_TIME >= '" + varDate + " " + varHour + ":" + varMin + ":00.000000' AND TIW.SVSFV_TIME <= '" + varDate + " " + varMxHour + ":" + varMin + ":00.000000'";
            }

            sqlQuery = sqlQuery + " ORDER BY TIW.SVSFV_TIME ASC";
            List<IWOWModel> searchResult = new List<IWOWModel>();
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlQuery, connection);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        IWOWModel iwowResultList = new IWOWModel();
                        iwowResultList.CHECK_BRSTN = reader["CHECK BRSTN"].ToString();
                        iwowResultList.TRANSACTION_DATE = reader["TRANSACTION DATE"].ToString();
                        iwowResultList.INSTRUMENT_NUMBER = reader["CHECK NO."].ToString();
                        iwowResultList.ZP_AMOUNT = reader["AMOUNT"].ToString();
                        iwowResultList.ZP_AMT_CREATOR = reader["AMOUNT WISE USER"].ToString();
                        iwowResultList.SVSFV_TIME = reader["SVS VERIFICATION TIME"].ToString();
                        iwowResultList.SVS_FIRST = reader["SVS VERIFY BY USER ID"].ToString();
                        iwowResultList.FNAME = reader["SVS VERIFY BY USER NAME"].ToString();
                        iwowResultList.BRANCH_NAME = reader["CHECK BRANCH NAME"].ToString();

                        searchResult.Add(iwowResultList);
                    }
                    ret = JsonConvert.SerializeObject(searchResult);
                    }
                }
            catch (Exception e)
            {

                //IWOWModel iwowResultList = new IWOWModel();
                //iwowResultList.ERROR_MSG = e.Message;
                //searchResult.Add(iwowResultList);
                ret = JsonConvert.SerializeObject(searchResult);
                return ret;
            }
            return ret;
        }

        public string getCheckDetails(string checkNumber)
        {
            string[] paramDetails;
            paramDetails = checkNumber.Split('|');
            string varCheckNo = paramDetails[0].PadLeft(10, '0');

            string varDate;
            string varUserID;
            var fileLoc = ConfigurationManager.AppSettings["IWOWLoc"];

            var retSql = "";
            using (StreamReader file = new StreamReader(fileLoc))
            {
                //int counter = 0;
                string ln;
                while (((ln = file.ReadLine()) != null) && !ln.Contains("--"))
                {
                    retSql += ln;
                }
                file.Close();
            }

            string ret = "";
            var sqlQuery = retSql + " WHERE TIW.INSTRUMENT_NUMBER = '" + varCheckNo + "'";

            //string ret = "";

            //var sqlQuery = @"SELECT TIW.TRANSACTION_DATE AS 'TRANSACTION DATE',
            //                TIW.INSTRUMENT_NUMBER AS 'CHECK NO.',
            //                TIW.CAR_AMOUNT AS 'AMOUNT',
            //                TIW.ZP_AMT_CREATOR AS 'AMOUNT WISE USER',
            //                TIW.SVSFV_TIME AS 'SVS VERIFICATION TIME',
            //                TIW.SVS_FIRST AS 'SVS VERIFY BY USER ID',
            //                TUR.FNAME AS 'SVS VERIFY BY USER NAME',
            //                TIW.PAYEE_BANK_CITY_CODE +
            //                TIW.PAYEE_BANK_CODE +
            //                TIW.PAYEE_BANK_BRANCH_CODE +
            //                TIW.PAYEE_CHECK_DIGIT AS 'CHECK BRSTN' ,
            //                TEN.BRANCH_NAME AS 'CHECK BRANCH NAME'
            //                FROM TBL_INWARD TIW INNER JOIN TBL_USER TUR
            //                ON TIW.SVS_FIRST = TUR.ID
            //                INNER JOIN TBL_ENTITY TEN ON TIW.PAYEE_BANK_CITY_CODE +
            //                TIW.PAYEE_BANK_CODE +
            //                TIW.PAYEE_BANK_BRANCH_CODE +
            //                TIW.PAYEE_CHECK_DIGIT = TEN.ENTITY_ID
            //                WHERE TIW.INSTRUMENT_NUMBER = '" + varCheckNo + "'";

            if (paramDetails.Length > 1)
            {
                if(paramDetails[1] != "" && paramDetails[2] != "")
                {
                    varDate = paramDetails[1];
                    varUserID = paramDetails[2];
                    sqlQuery = sqlQuery + " AND TIW.TRANSACTION_DATE = '" + varDate + "' AND TIW.SVS_FIRST = '" + varUserID + "'";
                }
                else
                {
                    if(paramDetails[1] != "")
                    {
                        varDate = paramDetails[1];
                        sqlQuery = sqlQuery + " AND TIW.TRANSACTION_DATE = '" + varDate + "'";
                    }else if(paramDetails[2] != "")
                    {
                        varUserID = paramDetails[2];
                        sqlQuery = sqlQuery + " AND TIW.SVS_FIRST = '" + varUserID + "'";
                    }
                }
                
            }

            sqlQuery = sqlQuery + " ORDER BY TIW.SVSFV_TIME ASC";
            List<IWOWModel> searchResult = new List<IWOWModel>();
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlQuery, connection);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        IWOWModel iwowResultList = new IWOWModel();
                        iwowResultList.CHECK_BRSTN = reader["CHECK BRSTN"].ToString();
                        iwowResultList.TRANSACTION_DATE = reader["TRANSACTION DATE"].ToString();
                        iwowResultList.INSTRUMENT_NUMBER = reader["CHECK NO."].ToString();
                        iwowResultList.ZP_AMOUNT = reader["AMOUNT"].ToString();
                        iwowResultList.ZP_AMT_CREATOR = reader["AMOUNT WISE USER"].ToString();
                        iwowResultList.SVSFV_TIME = reader["SVS VERIFICATION TIME"].ToString();
                        iwowResultList.SVS_FIRST = reader["SVS VERIFY BY USER ID"].ToString();
                        iwowResultList.FNAME = reader["SVS VERIFY BY USER NAME"].ToString();
                        iwowResultList.BRANCH_NAME = reader["CHECK BRANCH NAME"].ToString();

                        searchResult.Add(iwowResultList);
                    }
                    ret = JsonConvert.SerializeObject(searchResult);
                }
            }
            catch (Exception e)
            {
                IWOWModel iwowResultList = new IWOWModel();
                iwowResultList.ERROR_MSG = e.Message;
                searchResult.Add(iwowResultList);
                ret = JsonConvert.SerializeObject(searchResult);
                return ret;
            }
            return ret;
        }
<<<<<<< Updated upstream
=======
        private static string sbcCheckImageLoc = ConfigurationManager.AppSettings["sbcCheckImageLoc"];
        public CheckImageModel ManageCheckImage(string checkDetailParams)
        {
            CheckImageModel checkImageModel = new CheckImageModel();
            var chk = "";
            var instrument_id = "";
            byte[] imgByteView1 = null;
            byte[] imgByteView2 = null;
            string accountNo = checkDetailParams.Split('|')[0];
            string instrumentIdDate = checkDetailParams.Split('|')[1];
            string instrumentIdCheckNumber = checkDetailParams.Split('|')[2];
            string instrumentAmount = checkDetailParams.Split('|')[3];
            string instrumentBRSTN = checkDetailParams.Split('|')[4];


            string sqlQuery = @"SELECT A.instrument_id, A.VIEW1, A.VIEW2, b.CAR_AMOUNT
                                FROM tbl_images A JOIN tbl_inward B
                                ON A.instrument_id = b.instrument_id
                                WHERE B.payee_acno = '" + accountNo + "' AND B.TRANSACTION_DATE = '" + instrumentIdDate + "'AND B.instrument_number = '" + instrumentIdCheckNumber + "'";


            string str = "";
            

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataSet ds = new DataSet();
                    reader.Close();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        //imgByte = Encoding.ASCII.GetBytes(dr["VIEW2"].ToString());
                        //imgByte = Encoding.ASCII.GetBytes(dr["VIEW1"].ToString());
                        imgByteView1 = (byte[])dr["VIEW1"];
                        imgByteView2 = (byte[])dr["VIEW2"];
                        instrument_id = dr["instrument_id"].ToString();
                        //str = Convert.ToBase64String(imgByteView1);
                        chk = "data:Image/jpeg;base64," + str;
                    }
                    //File.WriteAllBytes(@"C:\\Users\\206867\\Desktop\\1_files\\sample2.jpg", imgByteView1);
                    if (!File.Exists(sbcCheckImageLoc + "\\" + instrument_id + "_front.jpg") && !File.Exists(sbcCheckImageLoc + "\\" + instrument_id + "_back.jpg"))
                    {
                        File.WriteAllBytes(sbcCheckImageLoc + "\\" + instrument_id + "_front.jpg", imgByteView1);
                        File.WriteAllBytes(sbcCheckImageLoc + "\\" + instrument_id + "_back.jpg", imgByteView2);

                        createPdf(instrumentIdCheckNumber, instrumentIdDate, instrumentAmount, accountNo, instrumentBRSTN, instrument_id);
                    }


                    
                }
                catch(Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }

            
            string fileName = sbcCheckImageLoc + "\\" + instrument_id + ".pdf";
            byte[] buff = File.ReadAllBytes(fileName);
            FileStream fs = new FileStream(fileName,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);


            checkImageModel.fileByte64 = buff;
            checkImageModel.fileName = instrumentIdCheckNumber;

            return checkImageModel;
        }

        public void createPdf(string instrumentIdCheckNumber, string instrumentIdDate, string instrumentAmount, string accountNo, string instrumentBRSTN, string instrument_id)
        {
            FileStream fs = new FileStream(sbcCheckImageLoc + "\\"+ instrument_id + ".pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document();
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                var newLine = "____________________________________________________";
                Paragraph title = new Paragraph("CHEQUE DETAILS \n "+ newLine);
                title.SpacingAfter = 9f;
                title.Alignment = Element.ALIGN_CENTER;                
                doc.Add(title);

                doc.Add(new Paragraph("CHECK NUMBER: " + instrumentIdCheckNumber));
                doc.Add(new Paragraph("DATE: " + instrumentIdDate));
                doc.Add(new Paragraph("AMOUNT: " + instrumentAmount));
                doc.Add(new Paragraph("ACCOUNT NUMBER: " + accountNo));
                doc.Add(new Paragraph("BRSTN: " + instrumentBRSTN));
                doc.Add(new Paragraph(Environment.NewLine));

                String imageFile = sbcCheckImageLoc + "\\" + instrument_id + "_front.jpg";
                Image image = Image.GetInstance(imageFile);
                image.Alignment = Image.ALIGN_CENTER;
                image.ScaleAbsolute(500f, 159f);
                doc.Add(image);

                var centerText = " | " + instrumentIdCheckNumber + " | " + instrumentBRSTN.Substring(0, 5) + " | " + instrumentBRSTN.Substring(5) + " | " + accountNo + " | 000 |";
                
                Paragraph para = new Paragraph(centerText,  new Font(Font.FontFamily.COURIER, 10));
                para.SpacingAfter = 9f;
                para.Alignment = Element.ALIGN_CENTER;
                doc.Add(para);

                //
                doc.Add(new Paragraph(Environment.NewLine));
                String imageFileBack = sbcCheckImageLoc + "\\" + instrument_id + "_back.jpg";
                Image imageBack = Image.GetInstance(imageFileBack);
                imageBack.Alignment = Image.ALIGN_CENTER;
                imageBack.ScaleAbsolute(500f, 159f);
                doc.Add(imageBack);
                doc.Close();
            }
            catch(Exception e)
            {
                string a = e.Message;
                doc.Close();
            }
            

        }

>>>>>>> Stashed changes
    }
}