using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ICLSearchDetail.Web.DBManager.Model.SearchEngineModels;
using ICLSearchDetail.Web.DBManager.Service;

namespace ICLSearchDetail.Web.Controllers
{
    public class SearchEngineIWOWController : ApiController
    {
        [System.Web.Http.Route("api/searchEngine/searchIWOW/userID/{userIDDate}")]
        [System.Web.Http.HttpGet] 
        public IHttpActionResult GetUserIDDateDetails(String userIDDate)
        {

            SearchEngineIWOWService searchEngineService = new SearchEngineIWOWService();

           string returnUserIDDateDetails = searchEngineService.getUserIDDateDetails(userIDDate);

           return Ok(returnUserIDDateDetails);
        }

        [System.Web.Http.Route("api/searchEngine/searchIWOW/checkNo/{checkNumber}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetCheckDetails(String checkNumber)
        {

            SearchEngineIWOWService searchEngineService = new SearchEngineIWOWService();

            string returnCheckDetails = searchEngineService.getCheckDetails(checkNumber);

            return Ok(returnCheckDetails);
        }
<<<<<<< Updated upstream
=======

        [System.Web.Http.Route("api/searchEngine/searchIWOW/scanAcctNo/{scanAcctNo}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetByAcctNo(String scanAcctNo)
        {

            SearchEngineIWOWService searchEngineService = new SearchEngineIWOWService();

            string returnCheckDetails = searchEngineService.getByAcctNo(scanAcctNo);

            return Ok(returnCheckDetails);
        }

        [System.Web.Http.Route("api/searchEngine/searchIWOW/getImageCheck/{instrumentIdAndDate}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult ConvertToImage(String instrumentIdAndDate)
        {
            CheckImageModel checkImageModel = new CheckImageModel();

            SearchEngineIWOWService searchEngineService = new SearchEngineIWOWService();
            checkImageModel = searchEngineService.ManageCheckImage(instrumentIdAndDate);
            //byte[] file = GetFile(id);
            //HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
            string content = Convert.ToBase64String(checkImageModel.fileByte64) + "FILENAME:" + checkImageModel.fileName;

            return Ok(content);
        }
>>>>>>> Stashed changes
    }
}