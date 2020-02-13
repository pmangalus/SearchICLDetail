
using ICLSearchDetail.Web.DBManager.Service.Bypass;
using System.Web.Http;
using System.Web.Mvc;

namespace ICLSearchDetail.Web.Controllers.api.Bypass
{
    public class BypassController : ApiController
    {
        [System.Web.Http.Route("api/bypass/batchAccountNoKeyUpdate/")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult SetbatchAccountNoKey()
        {
            BypassService bypassService = new BypassService();
            string a = bypassService.ReturnBypassUpdateAmountNoKey();
            return Ok(a);
        }

        [System.Web.Http.Route("api/bypass/batchAccountNoKeySelect/")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult BatchAccountNoKeySelect()
        {
            BypassService bypassService = new BypassService();
            string a = bypassService.ReturnBypassSelectTotalAmountNoKey();
            return Ok(a);
        }
    }
}