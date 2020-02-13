using ICLSearchDetail.Web.DBManager.Model.ILOModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace ICLSearchDetail.Web.DBManager.Service.ILO
{
    public class SearchEngineILOService
    {
        public string getBrstnDateDetails(string brstnDate)
        {
            string[] paramDetails;
            paramDetails = brstnDate.Split('|');

            string varBrstn = paramDetails[0];
            string varDate = paramDetails[1];
            string varCheckNumber = paramDetails[2];

            var fileLoc = ConfigurationManager.AppSettings["ILOLoc"];

            var retSql = "";
            var sqlQuery = "";
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

            if(varDate.Contains("to"))
            {
                String [] dateRange = varDate.Split(new[] { "to" }, StringSplitOptions.None);

                sqlQuery = retSql + " WHERE (TOW.TRANSACTION_DATE >= '" + dateRange[0].TrimEnd() + "' and TOW.TRANSACTION_DATE <= '" + dateRange[1].TrimStart() + "') AND TOW.BOFD_SORTCODE = '" + varBrstn + "'";


            } else
            {
                sqlQuery = retSql + " WHERE TOW.TRANSACTION_DATE = '" + varDate + "' AND TOW.BOFD_SORTCODE = '" + varBrstn + "'";
            }
            
          
            if (!varCheckNumber.Equals(""))
            {
                sqlQuery += " AND TOW.SCAN_INSTRUMENT_NUMBER = '" + varCheckNumber.PadLeft(10, '0') + "' ";
            }

            List<ILOModel> searchResult = new List<ILOModel>();
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlQuery, connection);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ILOModel iloResultList = new ILOModel();
                        iloResultList.BUSINESS_DATE = reader["BUSINESS DATE"].ToString();
                        iloResultList.MODULE = reader["MODULE"].ToString();
                        iloResultList.BATCH_ID = reader["BATCH ID NO."].ToString();
                        iloResultList.CHECK_NUMBER = reader["CHECK NUMBER"].ToString();
                        iloResultList.CHECK_STATUS = reader["CHECK STATUS"].ToString();
                        iloResultList.AMOUNT = reader["AMOUNT"].ToString();
                        iloResultList.SCAN_ACCOUNT = reader["SCAN ACCOUNT NO."].ToString();
                        iloResultList.SCAN_BRSTN = reader["SCAN BRSTN"].ToString();
                        iloResultList.SCANNED_BY = reader["SCAN BY"].ToString();
                        iloResultList.SCANNED_TIME = reader["SCAN TIME"].ToString();
                        iloResultList.AMOUNT_KEYING_USR = reader["AMOUNT KEYING/PDC PROCESSING USER"].ToString();
                        iloResultList.AMOUNT_KEYING_TIME = reader["AMOUNT KEYING/PDC PROCESSING TIME"].ToString();
                        iloResultList.ACCOUNT_NO_KEYING_TIME = reader["ACCOUNT NO. KEYING TIME"].ToString();
                        iloResultList.ACCOUNT_NO_KEYING_USR = reader["ACCOUNT NO. KEYING USER"].ToString();
                        iloResultList.PDC_VERIFICATION_USER = reader["PDC VERIFICATION USER"].ToString();
                        iloResultList.PDC_VERIFICATION_TIME = reader["PDC VERIFICATION TIME"].ToString();
                        iloResultList.PDC_ALLOW_HOLD_USER = reader["PDC ALLOW/HOLD USER"].ToString();
                        iloResultList.PDC_ALLOW_HOLD_TIME = reader["PDC ALLOW/HOLD TIME"].ToString();
                        iloResultList.BRANCH_NAME = reader["BRANCH_NAME"].ToString();

                        searchResult.Add(iloResultList);
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

        public string getBatchDateDetails(string batchDate)
        {
            string[] paramDetails;
            paramDetails = batchDate.Split('|');

            string varBatchID = paramDetails[0];
            string varDate = paramDetails[1];
            string varCheckNumber = paramDetails[2];

            var fileLoc = ConfigurationManager.AppSettings["ILOLoc"];

            var retSql = "";
            var sqlQuery = "";
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

            if(varDate.Contains("to"))
            {
                String[] dateRange = varDate.Split(new[] { "to" }, StringSplitOptions.None);

                sqlQuery = retSql + " WHERE (TOW.TRANSACTION_DATE >= '" + dateRange[0].TrimEnd() + "' and TOW.TRANSACTION_DATE <= '" + dateRange[1].TrimStart() + "') AND TOW.BATCH_ID = '" + varBatchID + "'";

            } else
            {
                sqlQuery = retSql + " WHERE TOW.TRANSACTION_DATE = '" + varDate + "' AND TOW.BATCH_ID = '" + varBatchID + "'";
            }
            

            if (!varCheckNumber.Equals(""))
            {
                sqlQuery += " AND TOW.SCAN_INSTRUMENT_NUMBER = '" + varCheckNumber.PadLeft(10, '0') + "' ";
            }

            List<ILOModel> searchResult = new List<ILOModel>();
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlQuery, connection);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ILOModel iloResultList = new ILOModel();
                        iloResultList.BUSINESS_DATE = reader["BUSINESS DATE"].ToString();
                        iloResultList.MODULE = reader["MODULE"].ToString();
                        iloResultList.BATCH_ID = reader["BATCH ID NO."].ToString();
                        iloResultList.CHECK_NUMBER = reader["CHECK NUMBER"].ToString();
                        iloResultList.CHECK_STATUS = reader["CHECK STATUS"].ToString();
                        iloResultList.AMOUNT = reader["AMOUNT"].ToString();
                        iloResultList.SCAN_ACCOUNT = reader["SCAN ACCOUNT NO."].ToString();
                        iloResultList.SCAN_BRSTN = reader["SCAN BRSTN"].ToString();
                        iloResultList.SCANNED_BY = reader["SCAN BY"].ToString();
                        iloResultList.SCANNED_TIME = reader["SCAN TIME"].ToString();
                        iloResultList.AMOUNT_KEYING_USR = reader["AMOUNT KEYING/PDC PROCESSING USER"].ToString();
                        iloResultList.AMOUNT_KEYING_TIME = reader["AMOUNT KEYING/PDC PROCESSING TIME"].ToString();
                        iloResultList.ACCOUNT_NO_KEYING_TIME = reader["ACCOUNT NO. KEYING TIME"].ToString();
                        iloResultList.ACCOUNT_NO_KEYING_USR = reader["ACCOUNT NO. KEYING USER"].ToString();
                        iloResultList.PDC_VERIFICATION_USER = reader["PDC VERIFICATION USER"].ToString();
                        iloResultList.PDC_VERIFICATION_TIME = reader["PDC VERIFICATION TIME"].ToString();
                        iloResultList.PDC_ALLOW_HOLD_USER = reader["PDC ALLOW/HOLD USER"].ToString();
                        iloResultList.PDC_ALLOW_HOLD_TIME = reader["PDC ALLOW/HOLD TIME"].ToString();
                        iloResultList.BRANCH_NAME = reader["BRANCH_NAME"].ToString();

                        searchResult.Add(iloResultList);
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

        public string getICLFileName(string iclFileName)
        {

            string[] paramDetails;
            paramDetails = iclFileName.Split('|');

            String ret = "";
            String strICLFileName = paramDetails[0].Replace("!!!",".")
                                                   .Replace(",","','");
            String strTranDate = paramDetails[1];

            var fileLoc = ConfigurationManager.AppSettings["ILOLoc"];

            var retSql = "";
            var sqlQuery = "";
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

            sqlQuery = retSql + " WHERE TOW.chi_ref in ('" + strICLFileName + "')";



            if (strTranDate.Contains("to"))
            {
                String[] dateRange = strTranDate.Split(new[] { "to" }, StringSplitOptions.None);

                sqlQuery += " AND (transaction_date >= '" + dateRange[0].TrimEnd() + "' and transaction_date <= '" + dateRange[1].TrimStart() + "') ";
                //AND transaction_date = '" + strTranDate +"'
            }
            else
            {
                sqlQuery += " AND transaction_date = '" + strTranDate + "'";
            }




            List<ILOModel> searchResult = new List<ILOModel>();
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlQuery, connection);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ILOModel iloResultList = new ILOModel();
                        iloResultList.BUSINESS_DATE = reader["BUSINESS DATE"].ToString();
                        iloResultList.MODULE = reader["MODULE"].ToString();
                        iloResultList.BATCH_ID = reader["BATCH ID NO."].ToString();
                        iloResultList.CHECK_NUMBER = reader["CHECK NUMBER"].ToString();
                        iloResultList.CHECK_STATUS = reader["CHECK STATUS"].ToString();
                        iloResultList.AMOUNT = reader["AMOUNT"].ToString();
                        iloResultList.SCAN_ACCOUNT = reader["SCAN ACCOUNT NO."].ToString();
                        iloResultList.SCAN_BRSTN = reader["SCAN BRSTN"].ToString();
                        iloResultList.SCANNED_BY = reader["SCAN BY"].ToString();
                        iloResultList.SCANNED_TIME = reader["SCAN TIME"].ToString();
                        iloResultList.AMOUNT_KEYING_USR = reader["AMOUNT KEYING/PDC PROCESSING USER"].ToString();
                        iloResultList.AMOUNT_KEYING_TIME = reader["AMOUNT KEYING/PDC PROCESSING TIME"].ToString();
                        iloResultList.ACCOUNT_NO_KEYING_TIME = reader["ACCOUNT NO. KEYING TIME"].ToString();
                        iloResultList.ACCOUNT_NO_KEYING_USR = reader["ACCOUNT NO. KEYING USER"].ToString();
                        iloResultList.PDC_VERIFICATION_USER = reader["PDC VERIFICATION USER"].ToString();
                        iloResultList.PDC_VERIFICATION_TIME = reader["PDC VERIFICATION TIME"].ToString();
                        iloResultList.PDC_ALLOW_HOLD_USER = reader["PDC ALLOW/HOLD USER"].ToString();
                        iloResultList.PDC_ALLOW_HOLD_TIME = reader["PDC ALLOW/HOLD TIME"].ToString();
                        iloResultList.BRANCH_NAME = reader["BRANCH_NAME"].ToString();

                        searchResult.Add(iloResultList);
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

    }

}