using ICLSearchDetail.Web.DBManager.Model;
using ICLSearchDetail.Web.DBManager.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ICLSearchDetail.Web.api
{
    public class SearchIclController : ApiController
    {
        [Route("api/icl/searchIcl/{iclNumber}")]
        [HttpGet]
        public IHttpActionResult GetIclInfo(String iclNumber)
        {
            ICLSearchService iclSearchService = new ICLSearchService();
            String returnIcl = iclSearchService.returnICLSearchResult(iclNumber);

            return Ok(returnIcl);
        }

        [Route("api/cics/outwardChechStatusSummary/")]
        [HttpGet]
        public IHttpActionResult GetOutwardChechStatusSummary()
        {
            var query = ConfigurationManager.AppSettings["OutwardCheckStatusLoc"];
            OutwardCheckService outCheck = new OutwardCheckService();
            var result = outCheck.GetOutwardCheckSummary(query);
            return Ok(result);
        }

        //GetDetailsSummary
        [Route("api/cics/getDetailsSummary/{idx}/{fps}/{offSet}/{npLength}")]
        [HttpGet]
        public IHttpActionResult GetDetailsSummary(String idx, String fps, String offSet, String npLength)
        {
            var result = "";
            if (idx.Equals("6")) // 11/11/2019 sprint 4 export to excel
            {
                OutwardCheckService outCheck = new OutwardCheckService();
                 result = outCheck.ExportToExcel(idx);
            } else
            {
                OutwardCheckService outCheck = new OutwardCheckService();
                result = outCheck.GetDetailsSummary(idx, fps, offSet, npLength);
            }
            
            return Ok(result);
        }


        //Loan Export
        [Route("api/cics/getDetailsSummary/{idx}")]
        [HttpGet]
        public IHttpActionResult LoanExportSummary(String idx)
        {
            var result = "";
                OutwardCheckService outCheck = new OutwardCheckService();
                result = outCheck.ExportToExcel(idx);

            return Ok(result);
        }

    }
}
