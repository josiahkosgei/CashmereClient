
//Reporting.FailedPostsReporting.FailedPostReport


using System;
using System.Collections.Generic;

namespace CashmereUtil.Reporting.FailedPostsReporting
{
    public class FailedPostReport
    {
        public List<FailedTransaction> FailedTransactions { get; set; }

        public DateTime RunDate { get; set; }
    }
}
