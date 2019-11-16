using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICLSearchDetail.Web.DBManager.Model
{
    public class OCSumDetailsModel
    {
        public string INSTRUMENT_ID { get; set; }
        public string BUSINESS_DATE { get; set; }
        public string AMOUNT { get; set; }
        public string ACCOUNT { get; set; }
        public string BRSTN { get; set; }
        public string SERIAL_NUMBER { get; set; }
        public string BOFD_RT { get; set; }
        public string BRANCH_NAME { get; set; }
        public string BOFD_ACC { get; set; }
        public string ICL_FILENAME { get; set; }
        public string SCANNED_TIME { get; set; }
        public string SCANNED_BY { get; set; }
        public string AMOUNT_KEYING_TIME { get; set; }
        public string ACCOUNT_NO_KEYING_TIME { get; set; }
        
    }

    public class LNSumDetailsModel
    {
        public string MODULE { get; set; }
        public string BATCH_ID { get; set; }
        public string BUSINESS_DATE { get; set; }
        public string CHECK_NUMBER { get; set; }
        public string AMOUNT { get; set; }
        public string SCAN_ACCOUNT { get; set; }
        public string SCAN_BRSTN { get; set; }
        public string SCANNED_TIME { get; set; }
        public string SCANNED_BY { get; set; }
        public string AMOUNT_KEYING_TIME { get; set; }
        public string AMOUNT_KEYING_USR { get; set; }
        public string ACCOUNT_NO_KEYING_TIME { get; set; }
        public string ACCOUNT_NO_KEYING_USR { get; set; }
        public string PDC_VERIFICATION_USER { get; set; }
        public string PDC_VERIFICATION_TIME { get; set; }
        public string PDC_ALLOW_HOLD_USER { get; set; }
        public string PDC_ALLOW_HOLD_TIME { get; set; }
        public string BRANCH_NAME { get; set; }

    }
}