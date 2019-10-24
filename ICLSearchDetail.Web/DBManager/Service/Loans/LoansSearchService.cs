using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using ICLSearchDetail.Web.DBManager.Model;
using ICLSearchDetail.Web.DBManager.Model.LoansModels;
using Newtonsoft.Json;

namespace ICLSearchDetail.Web.DBManager.Service.Loans
{
    public class LoansSearchService
    {


        string createTimeSeconds1 = ConfigurationManager.AppSettings["createTimeSeconds1"];
        string createTimeSeconds2 = ConfigurationManager.AppSettings["createTimeSeconds2"];
        string BOFD_SORTCODE = ConfigurationManager.AppSettings["BOFD_SORTCODE"]; 
        public string GetDates()
        {
            var sqlString = "SELECT CONVERT(char(10),NEXT_LCY_DATE,126) as transaction_date, CONVERT(char(10), CUR_DATE, 126) as Create_time  from tbl_eod_bod_stat";
            string curr_date = "";

            ArrayList valuesList = new ArrayList();

            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlString, connection);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        valuesList.Add(Convert.ToString(reader[0].ToString()));
                        valuesList.Add(Convert.ToString(reader[1].ToString()));

                    curr_date = valuesList.ToArray().GetValue(0).ToString() + '|' + valuesList.ToArray().GetValue(1).ToString();
                         
                    }
                }
            }
            catch (Exception e)
            {
                curr_date = "error occured";
                
            }
            return curr_date;
        }

        public string getSummaryDetails(string neededDates, string batchIdNum)
        {

            string[] arrDates;
            arrDates = neededDates.Split('|');

            string txnDate = arrDates[0];
            string createTime = arrDates[1];
           

            List<LoanDataModel> loanDataResult = new List<LoanDataModel>();
            
            string ret = "";
            string sqlString = @"SELECT
                                TOW.BATCH_ID AS 'BATCH ID NO.',
                                TOW.CAR_AMOUNT AS 'CHECK AMOUNT',
                                TOW.SCAN_INSTRUMENT_NUMBER AS 'CHECK NUMBER',
                                TOW.BOFD_SORTCODE AS 'BRSTN',
                                TBK.NAME AS 'DRAWEE BANK',
                                TOW.SCAN_MICR_ACNO AS 'ACCOUNT NUMBER',
                                TIQ.IQA_FAILED_REASON AS 'IQA STATUS'
                                FROM TBL_OUTWARD TOW INNER JOIN TBL_BANK TBK
                                ON TOW.SCAN_PAYEE_BANK_CITY_CODE = TBK.MICR_CITY_CODE AND
                                TOW.SCAN_PAYEE_BANK_CODE = TBK.MICR_CODE
                                INNER JOIN TBL_IQA_FAILED_REASON TIQ
                                ON TOW.IQA_FLAG = TIQ.IQA_ID
                                WHERE TOW.TRANSACTION_DATE ='" + txnDate +
                                "' AND TOW.CREATE_TIME > '" + createTime + " " + createTimeSeconds1 + "' " +
                                " AND TOW.CREATE_TIME < '" + createTime + " " + createTimeSeconds2 + "' AND TOW.BATCH_ID = '" + batchIdNum + "'" +
                                " AND TOW.BOFD_SORTCODE = '" + BOFD_SORTCODE + "' ORDER BY TOW.CREATE_TIME ASC";

            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EXPRESS_SBC_CONN"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlString, connection);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        LoanDataModel loanDataResultList = new LoanDataModel();
                        loanDataResultList.BATCH_ID = reader["BATCH ID NO."].ToString();
                        loanDataResultList.CAR_AMOUNT = reader["CHECK AMOUNT"].ToString();
                        loanDataResultList.SCAN_INSTRUMENT_NUMBER = reader["CHECK NUMBER"].ToString();
                        loanDataResultList.BOFD_SORTCODE = reader["BRSTN"].ToString();
                        loanDataResultList.NAME = reader["DRAWEE BANK"].ToString();
                        loanDataResultList.SCAN_MICR_ACNO = reader["ACCOUNT NUMBER"].ToString();
                        loanDataResultList.IQA_FAILED_REASON = reader["IQA STATUS"].ToString();

                        loanDataResult.Add(loanDataResultList);
                    }
                    ret = JsonConvert.SerializeObject(loanDataResult);
                }
            }
            catch (Exception e)
            {

                LoanDataModel loanDataResultList = new LoanDataModel();
                loanDataResultList.ERROR_MSG = e.Message;
                loanDataResult.Add(loanDataResultList);
                ret = JsonConvert.SerializeObject(loanDataResult);
                return ret;
            }
            return ret;


        }



    }
}