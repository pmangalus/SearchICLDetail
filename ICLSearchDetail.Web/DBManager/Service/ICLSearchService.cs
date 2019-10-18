using ICLSearchDetail.Web.DBManager.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ICLSearchDetail.Web.DBManager.Service
{
    public class ICLSearchService
    {
        private string iclNumber;

        public ICLSearchService()
        {
        }

        public ICLSearchService(string iclNumber)
        {
            this.iclNumber = iclNumber;
        }

        public String returnICLSearchResult(string iclNumber)
        {
            List<ICLDataModel> iclResult = new List<ICLDataModel>();
            ICLDataModel iclResultList = new ICLDataModel();
            string ret = "";
            string sqlString = @"SELECT t_out.CHI_REF,
                                  CASE
                                    WHEN br.branch_name IS NULL
                                    THEN ''
                                    ELSE br.branch_name
                                  END +' ('+ t_out.bofd_sortcode +')' AS [Branch],
                                  t_out.scan_instrument_number        AS [CHECK_NUMBER],
                                  t_out.car_amount                    AS [AMOUNT],
                                  t_out.create_time                   AS [CHECK_SCAN_TIME],
                                  t_out.amt_user                      AS [USER_AMT_KEYING],
                                  t_out.scan_micr_acno                AS [DEBIT_ACCOUNT],
                                  t_out.BATCH_ACCOUNT_NUMBER_FIN      AS [CREDIT_ACCOUNT]
                                FROM TBL_OUTWARD t_out
                                LEFT JOIN tbl_entity br
                                ON t_out.bofd_sortcode = br.branch_code
                               WHERE CHI_REF          = '" + iclNumber + ".ICL'";
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EsourceConnString"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlString, connection);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        iclResultList.CHI_REF = reader["CHI_REF"].ToString();
                        iclResultList.BRANCH = reader["BRANCH"].ToString();
                        iclResultList.CHECK_NUMBER = reader["CHECK_NUMBER"].ToString();
                        iclResultList.AMOUNT = reader["AMOUNT"].ToString();
                        iclResultList.CHECK_SCAN_TIME = reader["CHECK_SCAN_TIME"].ToString();
                        iclResultList.USER_AMT_KEYING = reader["USER_AMT_KEYING"].ToString();
                        iclResultList.DEBIT_ACCOUNT = reader["DEBIT_ACCOUNT"].ToString();
                        iclResultList.CREDIT_ACCOUNT = reader["CREDIT_ACCOUNT"].ToString();
                        iclResult.Add(iclResultList);
                    }
                    ret = JsonConvert.SerializeObject(iclResult);
                }
            }
            catch (Exception e)
            {
                iclResultList.ERROR_MSG = e.Message;
                iclResult.Add(iclResultList);
                ret = JsonConvert.SerializeObject(iclResult);
                return ret;
            }
            return ret;
        }

        
    }
}