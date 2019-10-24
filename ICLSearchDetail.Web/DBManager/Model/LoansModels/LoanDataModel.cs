using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICLSearchDetail.Web.DBManager.Model.LoansModels
{
    public class LoanDataModel
    {
        public string BATCH_ID { get; set; }
        public string CAR_AMOUNT { get; set; }
        public string SCAN_INSTRUMENT_NUMBER { get; set; }
        public string BOFD_SORTCODE { get; set; }
        public string NAME { get; set; }
        public string SCAN_MICR_ACNO { get; set; }
        public string IQA_FAILED_REASON { get; set; }
        public string TBL_OUTWARD { get; set; }
        public string ERROR_MSG { get; set; }

    }
}