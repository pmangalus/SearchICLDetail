using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ICLSearchDetail.Web.DBManager.Service.Loans;

namespace ICLSearchDetail.Web.Controllers.api.Loans
{
    public class LoansSummaryController : ApiController
    {
        [System.Web.Http.Route("api/loans/summaryLoan/{batchIdNum}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetSummaryLoan(String batchIdNum)
        {
            String returnDates;

            LoansSearchService loansService = new LoansSearchService();

            returnDates = loansService.GetDates();

            //returnDates = "2019-10-21|2019-10-18";
            string returnSummaryDetails = loansService.getSummaryDetails(returnDates, batchIdNum);

            return Ok(returnSummaryDetails);

        }

        [System.Web.Http.Route("api/loans/summaryLoan/")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetBatchIDs()
        {
            String returnDates;

            LoansSearchService loansService = new LoansSearchService();

            returnDates = loansService.GetDates();
            //returnDates = "2019-10-21|2019-10-18";
            string returnBatchIDs = loansService.getBatchIds(returnDates);

            return Ok(returnBatchIDs);
        }

        [System.Web.Http.Route("api/loans/summaryLoan/exportAll/")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetBatchIDsAll()
        {
            String returnDates;

            LoansSearchService loansService = new LoansSearchService();

            returnDates = loansService.GetDates();
            //returnDates = "2019-10-21|2019-10-18";
            //string returnBatchIDs = loansService.getAllSummaryForExportAll(returnDates);

            return Ok(loansService.getAllSummaryForExportAll(returnDates));
        }



    }
}
