
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ICLSearchDetail.Controllers
{
    public class SearchICLController : ApiController
    {
        [Route("api/icl/findIcl")]
        [HttpGet]
        public IHttpActionResult SaveRequest()
        {
            // string request = Request.Content.ReadAsStringAsync().Result;
            // pinChangeRequest = JsonConvert.DeserializeObject<PinChangeRequest>(request);
            //RequestModel model = JsonConvert.DeserializeObject<RequestModel>(request);

            //SavingResponse response = _requestManager.SaveRequest(model);

            return Ok("ola");
        }
        
    }
}