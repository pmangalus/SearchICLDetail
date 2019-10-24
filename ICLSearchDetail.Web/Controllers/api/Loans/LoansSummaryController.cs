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
            LoansSearchService loansService = new LoansSearchService();
            batchIdNum = loansService.GetDateCurrentDate();  

            return Ok(batchIdNum);
        } 

    }
}