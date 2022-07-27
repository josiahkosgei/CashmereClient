
//Reporting.CITReporting.CITReport


using System.Collections.Generic;

namespace CashmereUtil.Reporting.CITReporting
{
    public class CITReport
    {
        public CIT CIT { get; set; }

        public List<CITDenomination> CITDenominations { get; set; }

        public IList<Transaction> Transactions { get; set; }

        public IList<EscrowJam> EscrowJams { get; set; }
    }
}
