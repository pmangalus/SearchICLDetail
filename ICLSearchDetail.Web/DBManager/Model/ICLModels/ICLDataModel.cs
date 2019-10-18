using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICLSearchDetail.Web.DBManager.Model
{

    public class ICLDataModel
    {
        public string CHI_REF { get; set; }
        public string BRANCH { get; set; }
        public string CHECK_NUMBER { get; set; }
        public string AMOUNT { get; set; }
        public string CHECK_SCAN_TIME { get; set; }
        public string USER_AMT_KEYING { get; set; }
        public string DEBIT_ACCOUNT { get; set; }
        public string CREDIT_ACCOUNT { get; set; }

        public string ERROR_MSG { get; set; }
        //public List<ICLDataModel> ICLDataModelRoot { get; set; }
    }
}