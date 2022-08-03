using System;
using System.IO;
using System.Net;
using System.Xml;

namespace Cashmere.Finacle.Integration.Models
{
    public class CoopFinancle
    {
        public static string ExecutePOST()
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                dateTime = dateTime.ToUniversalTime();
                dateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                Guid.NewGuid().ToString();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml("<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'\r\n                  xmlns:mes='urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader'\r\n                  xmlns:com='urn://co-opbank.co.ke/CommonServices/Data/Common'\r\n                  xmlns:ns='urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0'>\r\n\t<soapenv:Header>\r\n\t\t<mes:RequestHeader>\r\n\t\t\t<com:CreationTimestamp>2022-03-02T19:09:16.350</com:CreationTimestamp>\r\n\t\t\t<com:CorrelationID>02559070-8fa9-4651-bc9d-6714252cae40</com:CorrelationID>\r\n\t\t\t<mes:FaultTO/>\r\n\t\t\t<mes:MessageID>02559070-8fa9-4651-bc9d-6714252cae40</mes:MessageID>\r\n\t\t\t<mes:ReplyTO/>\r\n\t\t\t<mes:Credentials>\r\n\t\t\t\t<mes:SystemCode>000</mes:SystemCode>\r\n\t\t\t\t<mes:BankID>01</mes:BankID>\r\n\t\t\t</mes:Credentials>\r\n\t\t</mes:RequestHeader>\r\n\t</soapenv:Header>\r\n\t<soapenv:Body>\r\n\t\t<ns:FundsTransfer>\r\n\t\t\t<ns:MessageReference/>\r\n\t\t\t<ns:SystemCode/>\r\n\t\t\t<ns:TransactionDatetime>2022-03-02T12:00:00.001</ns:TransactionDatetime>\r\n\t\t\t<ns:ValueDate>2022-03-02T12:00:00.001</ns:ValueDate>\r\n\t\t\t<ns:TransactionID/>\r\n\t\t\t<ns:TransactionType>T</ns:TransactionType>\r\n\t\t\t<ns:TransactionSubType>BI</ns:TransactionSubType>\r\n\t\t\t<ns:TransactionResponseDetails>\r\n\t\t\t\t<ns:Remarks/>\r\n\t\t\t</ns:TransactionResponseDetails>\r\n\t\t\t<ns:TransactionItems>\r\n\t\t\t\t<ns:TransactionItem>\r\n\t\t\t\t\t<ns:TransactionReference>VICTEST01</ns:TransactionReference>\r\n\t\t\t\t\t<ns:TransactionItemKey>2</ns:TransactionItemKey>\r\n\t\t\t\t\t<ns:AccountNumber>01148743633300</ns:AccountNumber>\r\n\t\t\t\t\t<ns:DebitCreditFlag>D</ns:DebitCreditFlag>\r\n\t\t\t\t\t<ns:TransactionAmount>20</ns:TransactionAmount>\r\n\t\t\t\t\t<ns:TransactionCurrency>KES</ns:TransactionCurrency>\r\n\t\t\t\t\t<ns:Narrative>PARTICULARS FOR DEBIT</ns:Narrative>\r\n\t\t\t\t\t<ns:SourceBranch/>\r\n\t\t\t\t</ns:TransactionItem>\r\n\t\t\t\t<ns:TransactionItem>\r\n\t\t\t\t\t<ns:TransactionReference>VICTEST01</ns:TransactionReference>\r\n\t\t\t\t\t<ns:TransactionItemKey>2</ns:TransactionItemKey>\r\n\t\t\t\t\t<ns:AccountNumber>01103173040200</ns:AccountNumber>\r\n\t\t\t\t\t<ns:DebitCreditFlag>C</ns:DebitCreditFlag>\r\n\t\t\t\t\t<ns:TransactionAmount>20</ns:TransactionAmount>\r\n\t\t\t\t\t<ns:TransactionCurrency>KES</ns:TransactionCurrency>\r\n\t\t\t\t\t<ns:Narrative>PARTICULARS FOR CREDIT</ns:Narrative>\r\n\t\t\t\t\t<ns:SourceBranch/>\r\n\t\t\t\t</ns:TransactionItem>\r\n\t\t\t</ns:TransactionItems>\r\n\t\t</ns:FundsTransfer>\r\n\t</soapenv:Body>\r\n</soapenv:Envelope>");
                Console.WriteLine((object)xmlDocument);
                HttpWebRequest webRequest = CoopFinancle.CreateWebRequest();
                using (Stream requestStream = webRequest.GetRequestStream())
                    xmlDocument.Save(requestStream);
                using (WebResponse response = webRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        string end = streamReader.ReadToEnd();
                        Console.WriteLine(end);
                        return end;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://192.168.0.180/Account/FundsTransfer/SyncPost/4.0");
            webRequest.Headers.Add("SOAP:Action");
            webRequest.ContentType = "application/xml";
            webRequest.Headers["SOAPAction"] = "\"Post\"";
            webRequest.Headers["Authorization"] = "Basic b21uaTpvbW5pMTIz";
            webRequest.Method = "POST";
            return webRequest;
        }
    }
}
