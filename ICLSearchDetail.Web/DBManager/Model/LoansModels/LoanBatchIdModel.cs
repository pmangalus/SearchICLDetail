using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICLSearchDetail.Web.DBManager.Model.LoansModels
{
    public class LoanBatchIdModel
    {
        public string BATCH_ID { get; set; }
        public string TOTAL_INSTRUMENT { get; set; }
        public string fldNotEqual { get; set; }

        public string batch_cnt { get; set; }

        public string ERROR_MSG { get; set; }
    }
}