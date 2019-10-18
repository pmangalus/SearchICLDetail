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
        [Route("api/cics/getDetailsSummary/{idx}")]
        [HttpGet]
        public IHttpActionResult GetDetailsSummary(String idx)
        {
            OutwardCheckService outCheck = new OutwardCheckService();
            var result = outCheck.GetDetailsSummary(idx);
            return Ok(result);
        }
    }
}
