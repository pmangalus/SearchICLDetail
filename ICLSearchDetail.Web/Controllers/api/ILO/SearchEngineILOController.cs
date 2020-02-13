using ICLSearchDetail.Web.DBManager.Service.ILO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ICLSearchDetail.Web.Controllers.api.ILO
{
    public class SearchEngineILOController : ApiController
    {
        [System.Web.Http.Route("api/searchEngine/searchILO/brstn/{brstnDate}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetBrstnDateDetails(String brstnDate)
        {

            SearchEngineILOService searchEngineService = new SearchEngineILOService();

            string returnUserIDDateDetails = searchEngineService.getBrstnDateDetails(brstnDate);

            return Ok(returnUserIDDateDetails);
        }

        [System.Web.Http.Route("api/searchEngine/searchILO/batchID/{batchDate}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetBatchDateDetails(String batchDate)
        {

            SearchEngineILOService searchEngineService = new SearchEngineILOService();

            string returnCheckDetails = searchEngineService.getBatchDateDetails(batchDate);

            return Ok(returnCheckDetails);
        }


        [System.Web.Http.Route("api/searchEngine/searchILO/iclFile/{iclFile}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetByICLFileName(String iclFile)
        {

            SearchEngineILOService searchEngineService = new SearchEngineILOService();

            string returnCheckDetails = searchEngineService.getICLFileName(iclFile);

            return Ok(returnCheckDetails);
        }

    }
}
