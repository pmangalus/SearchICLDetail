using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICLSearchDetail.Web.DBManager.Model.SearchEngineModels
{
    public class IWOWModel
    { 
        public string TRANSACTION_DATE { get; set; }
        public string INSTRUMENT_NUMBER { get; set; }
        public string ZP_AMOUNT { get; set; }
        public string ZP_AMT_CREATOR { get; set; }
        public string SVSFV_TIME { get; set; }
        public string SVS_FIRST { get; set; }
        public string FNAME { get; set; }
        public string CHECK_BRSTN { get; set; }
        public string BRANCH_NAME { get; set; }
        public string ERROR_MSG { get; set; }

    }
}