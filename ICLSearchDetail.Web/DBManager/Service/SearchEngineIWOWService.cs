﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ICLSearchDetail.Web.DBManager.Model.SearchEngineModels;
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

            string ret = "";
            var sqlQuery = @"SELECT TIW.TRANSACTION_DATE AS 'TRANSACTION DATE',
                            TIW.INSTRUMENT_NUMBER AS 'CHECK NO.',
                            TIW.ZP_AMOUNT AS 'AMOUNT',
                            TIW.ZP_AMT_CREATOR AS 'AMOUNT WISE USER',
                            TIW.SVSFV_TIME AS 'SVS VERIFICATION TIME',
                            TIW.SVS_FIRST AS 'SVS VERIFY BY USER ID',
                            TUR.FNAME AS 'SVS VERIFY BY USER NAME',
                            TIW.PAYEE_BANK_CITY_CODE +
                            TIW.PAYEE_BANK_CODE +
                            TIW.PAYEE_BANK_BRANCH_CODE +
                            TIW.PAYEE_CHECK_DIGIT AS 'CHECK BRSTN' ,
                            TEN.BRANCH_NAME AS 'CHECK BRANCH NAME'
                            FROM TBL_INWARD TIW INNER JOIN TBL_USER TUR
                            ON TIW.SVS_FIRST = TUR.ID
                            INNER JOIN TBL_ENTITY TEN ON TIW.PAYEE_BANK_CITY_CODE +
                            TIW.PAYEE_BANK_CODE +
                            TIW.PAYEE_BANK_BRANCH_CODE +
                            TIW.PAYEE_CHECK_DIGIT = TEN.ENTITY_ID
                            WHERE TIW.SVS_FIRST = '" + varUserID + "' AND TIW.TRANSACTION_DATE ='" + varDate + "'";

            if (paramDetails.Length == 3)
            {
                string varTime = paramDetails[2];
                int varHour = Convert.ToInt16(varTime) + 1;
                sqlQuery = sqlQuery + " AND TIW.SVSFV_TIME >= '" + varDate + " " + varTime + ":00:00.000000' AND TIW.SVSFV_TIME < '" + varDate + " " + varHour + ":00:00.000000'";
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

        public string getCheckDetails(string checkNumber)
        {
            string[] paramDetails;
            paramDetails = checkNumber.Split('|');
            string varCheckNo = paramDetails[0];

            string varDate;
            string ret = "";

            var sqlQuery = @"SELECT TIW.TRANSACTION_DATE AS 'TRANSACTION DATE',
                            TIW.INSTRUMENT_NUMBER AS 'CHECK NO.',
                            TIW.ZP_AMOUNT AS 'AMOUNT',
                            TIW.ZP_AMT_CREATOR AS 'AMOUNT WISE USER',
                            TIW.SVSFV_TIME AS 'SVS VERIFICATION TIME',
                            TIW.SVS_FIRST AS 'SVS VERIFY BY USER ID',
                            TUR.FNAME AS 'SVS VERIFY BY USER NAME',
                            TIW.PAYEE_BANK_CITY_CODE +
                            TIW.PAYEE_BANK_CODE +
                            TIW.PAYEE_BANK_BRANCH_CODE +
                            TIW.PAYEE_CHECK_DIGIT AS 'CHECK BRSTN' ,
                            TEN.BRANCH_NAME AS 'CHECK BRANCH NAME'
                            FROM TBL_INWARD TIW INNER JOIN TBL_USER TUR
                            ON TIW.SVS_FIRST = TUR.ID
                            INNER JOIN TBL_ENTITY TEN ON TIW.PAYEE_BANK_CITY_CODE +
                            TIW.PAYEE_BANK_CODE +
                            TIW.PAYEE_BANK_BRANCH_CODE +
                            TIW.PAYEE_CHECK_DIGIT = TEN.ENTITY_ID
                            WHERE TIW.INSTRUMENT_NUMBER = '" + varCheckNo + "'";

            if (paramDetails.Length == 3)
            {
                varDate = paramDetails[1];
                string varTime = paramDetails[2];
                int varHour = Convert.ToInt16(varTime) + 1;
                sqlQuery = sqlQuery + " AND TIW.SVSFV_TIME >= '" + varDate + " " + varTime + ":00:00.000000' AND TIW.SVSFV_TIME < '" + varDate + " " + varHour + ":00:00.000000' AND TIW.TRANSACTION_DATE = '" + varDate + "'";
            }
            else if (paramDetails.Length == 2)
            {
                varDate = paramDetails[1];
                sqlQuery = sqlQuery + "AND TIW.TRANSACTION_DATE = '" + varDate + "'";
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
    }
}