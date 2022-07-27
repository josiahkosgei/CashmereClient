
// Summary


using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "Summary")]
    public class Summary
    {
        [XmlElement(ElementName = "Transaction")]
        public List<BagRemovalReportTransaction> Transaction { get; set; }

        [XmlElement(ElementName = "Information")]
        public BagRemovalReportInformation Information { get; set; }

        [XmlElement(ElementName = "NoteCounts")]
        public List<BagRemovalReportNoteCount> NoteCounts { get; set; }
    }
}
