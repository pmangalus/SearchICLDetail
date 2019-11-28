using ICLSearchDetail.Web.DBManager.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ICLSearchDetail.Web.DBManager.Service
{
    public class OutwardCheckService
    {
        List<OutwardCheckSummaryModel> retList = new List<OutwardCheckSummaryModel>();
        List<OCSumDetailsModel> oCSumDetailsModelList = new List<OCSumDetailsModel>();
        List<LNSumDetailsModel> lNSumDetailsModelList = new List<LNSumDetailsModel>();
        /*public String GetOutwardCheckSummaryBackUp(String fileLoc)
        {
            var ret = "";
            
            var a = "";
            using (StreamReader file = new StreamReader(fileLoc))
            {
                int counter = 0;
                string ln;
                while ((ln = file.ReadLine()) != null)
                {
                    ret += ln + Environment.NewLine;
                    counter++;
                }
                file.Close();
                Console.WriteLine($"File has {counter} lines.");
            }
            string curr_date = GetDateCurrentDate();
            ret = ret.Replace("{date}", curr_date);
            if (!curr_date.Contains("error"))
            {
                try
                {
                    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
                    {

                        SqlCommand cmd = new SqlCommand(ret, connection);
                        int ctr = 1;
                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            OutwardCheckSummaryModel listSummary = new OutwardCheckSummaryModel();
                            listSummary.idx = ctr;
                            listSummary.summary = reader.GetValue(0).ToString();
                            retList.Add(listSummary);
                            ctr++;
                        }
                        ret = JsonConvert.SerializeObject(retList);
                        Console.WriteLine("sfasfaf");
                    }
                }
                catch (Exception e)
                {
                    ret = e.Message;

                }
            }
            else
            {
                OutwardCheckSummaryModel listSummary = new OutwardCheckSummaryModel();
                listSummary.idx = -1;
                listSummary.summary = "error getting current date";
                retList.Add(listSummary);
                ret = JsonConvert.SerializeObject(retList);
            }
            return ret;
        }

        */

        public String GetDateCurrentDate()
        {
            var sqlString = "select CUR_DATE from tbl_eod_bod_stat;";
            var curr_date = "";
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlString, connection);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        curr_date = DateTime.Parse(reader["CUR_DATE"].ToString()).ToShortDateString();
                    }
                }
            }
            catch (Exception e)
            {
                curr_date = "error occured" + e.ToString();
            }
            return curr_date;
        }
        public String GetOutwardCheckSummary(String fileLoc)
        {

            List<string> ret = new List<string>();
            var returnRetList = "";
            var curr_date = GetDateCurrentDate();
            if (!curr_date.Contains("error"))
            {

                using (StreamReader file = new StreamReader(fileLoc))
                {
                    //int counter = 0;
                    string ln;
                    while ((ln = file.ReadLine()) != null)
                    {
                        ln = ln.Replace("{date}", curr_date);
                        ret.Add(ln);
                    }
                    file.Close();
                }

                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
                {
                    connection.Open();
                    for (int i = 0; i < ret.Count; i++)
                    {
                        SqlCommand cmd = new SqlCommand(ret[i], connection);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            OutwardCheckSummaryModel listSummary = new OutwardCheckSummaryModel();
                            listSummary.idx = i;
                            listSummary.summary = reader.GetValue(0).ToString();
                            retList.Add(listSummary);

                        }
                    }
                    returnRetList = JsonConvert.SerializeObject(retList);
                }
            }
            else
            {
                OutwardCheckSummaryModel listSummary = new OutwardCheckSummaryModel();
                listSummary.idx = -1;
                listSummary.summary = "error getting current date";
                retList.Add(listSummary);
                returnRetList = JsonConvert.SerializeObject(retList);
            }
            return returnRetList;
        }
        public String GetDetailsSummary(String idx, String fps, String offSet, String npLength)
        {
            //throw new NotImplementedException();
            //http://localhost:53325/api/cics/getDetailsSummary/

            var fpi = Int32.Parse(offSet);
            var fpR = Int32.Parse(npLength);
            var total = fpR - fpi;

            if (fpR < fpi)
            {
                fps = total.ToString();
                offSet = total.ToString();
            }

            var fileLoc = ConfigurationManager.AppSettings["OutcheckDetailsLoc"];
            var curr_date = GetDateCurrentDate();
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
            retSql = retSql.Replace("{date}", curr_date);
            var andClause = getConditionalStatement(Int32.Parse(idx));

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
            {
                connection.Open();
                var sqlString = retSql + " " + andClause;

                sqlString = sqlString.Replace("{fps}", offSet)
                                      .Replace("{ffr}", fps);

                SqlCommand cmd = new SqlCommand(sqlString, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OCSumDetailsModel ocCSumDetailsModel = new OCSumDetailsModel();
                    ocCSumDetailsModel.INSTRUMENT_ID = reader["INSTRUMENT_ID"].ToString();
                    ocCSumDetailsModel.BUSINESS_DATE = DateTime.Parse(reader["BUSINESS_DATE"].ToString()).ToShortDateString();
                    ocCSumDetailsModel.AMOUNT = reader["AMOUNT"].ToString();
                    ocCSumDetailsModel.ACCOUNT = reader["ACCOUNT"].ToString();
                    ocCSumDetailsModel.BRSTN = reader["BRSTN"].ToString();
                    ocCSumDetailsModel.SERIAL_NUMBER = reader["SERIAL_NUMBER"].ToString();
                    ocCSumDetailsModel.BOFD_RT = reader["BOFD_RT"].ToString();
                    ocCSumDetailsModel.BRANCH_NAME = reader["BRANCH_NAME"].ToString();
                    ocCSumDetailsModel.BOFD_ACC = reader["BOFD_ACC"].ToString();
                    ocCSumDetailsModel.ICL_FILENAME = reader["ICL_FILENAME"].ToString();
                    ocCSumDetailsModel.SCANNED_TIME = reader["SCANNED_TIME"].ToString();
                    ocCSumDetailsModel.SCANNED_BY = reader["SCANNED_BY"].ToString();
                    ocCSumDetailsModel.AMOUNT_KEYING_TIME = reader["AMOUNT_KEYING_TIME"].ToString();
                    ocCSumDetailsModel.ACCOUNT_NO_KEYING_TIME = reader["ACCOUNT_NO_KEYING_TIME"].ToString();
                    oCSumDetailsModelList.Add(ocCSumDetailsModel);

                }
                retSql = JsonConvert.SerializeObject(oCSumDetailsModelList);
            }
            return retSql;
        }
        public string getConditionalStatement(int idx)
        {
            //var sqlWhere = "";
            var fileLoc = ConfigurationManager.AppSettings["OutcheckDetailsLoc"];
            List<string> where = new List<string>();
            using (StreamReader file = new StreamReader(fileLoc))
            {
                string ln;
                while ((ln = file.ReadLine()) != null)
                {
                    if (ln.Contains("{and}"))
                    {
                        where.Add(ln.Replace("{and}", "AND"));
                    }
                }
                
            }
            return where[idx];
        }

        /* Sprint 4: 11/11/2019 
         * Author: JCPB
         * Export To Excel Direct Button
         */

        public String ExportToExcel(String idx)
        {

            var fileLoc = ConfigurationManager.AppSettings["LoanTransmitLoc"];
            var curr_date = GetDateCurrentDate();
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
            retSql = retSql.Replace("{date}", curr_date);
            //var andClause = getConditionalStatement(Int32.Parse(idx));

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
            {
                connection.Open();
                var sqlString = retSql;
                SqlCommand cmd = new SqlCommand(sqlString, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    LNSumDetailsModel lnCSumDetailsModel = new LNSumDetailsModel();
                    lnCSumDetailsModel.MODULE = reader["MODULE"].ToString();
                    lnCSumDetailsModel.BATCH_ID = reader["BATCH ID NO."].ToString();
                    lnCSumDetailsModel.BUSINESS_DATE = DateTime.Parse(reader["BUSINESS DATE"].ToString()).ToShortDateString();
                    lnCSumDetailsModel.CHECK_NUMBER = reader["CHECK NUMBER"].ToString();
                    lnCSumDetailsModel.AMOUNT = reader["AMOUNT"].ToString();
                    lnCSumDetailsModel.SCAN_ACCOUNT = reader["SCAN ACCOUNT NO."].ToString();
                    lnCSumDetailsModel.SCAN_BRSTN = reader["SCAN BRSTN"].ToString();

                    if (reader["SCAN TIME"].ToString().Equals(""))
                    {
                        lnCSumDetailsModel.SCANNED_TIME = reader["SCAN TIME"].ToString();
                    }
                    else
                    {
                        lnCSumDetailsModel.SCANNED_TIME = DateTime.Parse(reader["SCAN TIME"].ToString()).ToString(); //reader["ACCOUNT NO. KEYING TIME"].ToString();
                    }
                    

                    lnCSumDetailsModel.SCANNED_BY = reader["SCAN BY"].ToString();


                    if (reader["AMOUNT KEYING/PDC PROCESSING TIME"].ToString().Equals(""))
                    {
                        lnCSumDetailsModel.AMOUNT_KEYING_TIME = reader["AMOUNT KEYING/PDC PROCESSING TIME"].ToString();
                    }
                    else
                    {
                        lnCSumDetailsModel.AMOUNT_KEYING_TIME = DateTime.Parse(reader["AMOUNT KEYING/PDC PROCESSING TIME"].ToString()).ToString(); //reader["ACCOUNT NO. KEYING TIME"].ToString();
                    }

                    lnCSumDetailsModel.AMOUNT_KEYING_USR = reader["AMOUNT KEYING/PDC PROCESSING USER"].ToString();

                    if(reader["ACCOUNT NO. KEYING TIME"].ToString().Equals(""))
                    {
                        lnCSumDetailsModel.ACCOUNT_NO_KEYING_TIME = reader["ACCOUNT NO. KEYING TIME"].ToString();
                    } else
                    {
                        lnCSumDetailsModel.ACCOUNT_NO_KEYING_TIME = DateTime.Parse(reader["ACCOUNT NO. KEYING TIME"].ToString()).ToString(); //reader["ACCOUNT NO. KEYING TIME"].ToString();
                    }
                    


                    lnCSumDetailsModel.ACCOUNT_NO_KEYING_USR = reader["ACCOUNT NO. KEYING USER"].ToString();
                    lnCSumDetailsModel.PDC_VERIFICATION_USER = reader["PDC VERIFICATION USER"].ToString();

                    if (reader["PDC VERIFICATION TIME"].ToString().Equals(""))
                    {
                        lnCSumDetailsModel.PDC_VERIFICATION_TIME = reader["PDC VERIFICATION TIME"].ToString();
                    }
                    else
                    {
                        lnCSumDetailsModel.PDC_VERIFICATION_TIME = DateTime.Parse(reader["PDC VERIFICATION TIME"].ToString()).ToString(); //reader["ACCOUNT NO. KEYING TIME"].ToString();
                    }
                    
                    lnCSumDetailsModel.PDC_ALLOW_HOLD_USER = reader["PDC ALLOW/HOLD USER"].ToString();

                    if (reader["PDC ALLOW/HOLD TIME"].ToString().Equals(""))
                    {
                        lnCSumDetailsModel.PDC_ALLOW_HOLD_TIME = reader["PDC ALLOW/HOLD TIME"].ToString();
                    }
                    else
                    {
                        lnCSumDetailsModel.PDC_ALLOW_HOLD_TIME = DateTime.Parse(reader["PDC ALLOW/HOLD TIME"].ToString()).ToString(); //reader["ACCOUNT NO. KEYING TIME"].ToString();
                    }
                    
                    lnCSumDetailsModel.BRANCH_NAME = reader["BRANCH_NAME"].ToString();
                    lNSumDetailsModelList.Add(lnCSumDetailsModel);

                }
                retSql = JsonConvert.SerializeObject(lNSumDetailsModelList);
            }
      
            return retSql;
        }
    }
}