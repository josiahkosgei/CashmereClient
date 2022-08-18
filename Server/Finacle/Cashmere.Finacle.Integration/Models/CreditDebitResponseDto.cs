namespace Cashmere.Finacle.Integration.Models
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public partial class Envelope
    {

        private EnvelopeHeader headerField;

        private EnvelopeBody bodyField;

        /// <remarks/>
        public EnvelopeHeader Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        public EnvelopeBody Body
        {
            get
            {
                return this.bodyField;
            }
            set
            {
                this.bodyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class EnvelopeHeader
    {

        private ResponseHeader responseHeaderField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
        public ResponseHeader ResponseHeader
        {
            get
            {
                return this.responseHeaderField;
            }
            set
            {
                this.responseHeaderField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader", IsNullable = false)]
    public partial class ResponseHeader
    {

        private string correlationIDField;

        private string messageIDField;

        private string statusCodeField;

        private string statusDescriptionField;

        private ResponseHeaderStatusMessages statusMessagesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Common")]
        public string CorrelationID
        {
            get
            {
                return this.correlationIDField;
            }
            set
            {
                this.correlationIDField = value;
            }
        }

        /// <remarks/>
        public string MessageID
        {
            get
            {
                return this.messageIDField;
            }
            set
            {
                this.messageIDField = value;
            }
        }

        /// <remarks/>
        public string StatusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }

        /// <remarks/>
        public string StatusDescription
        {
            get
            {
                return this.statusDescriptionField;
            }
            set
            {
                this.statusDescriptionField = value;
            }
        }

        /// <remarks/>
        public ResponseHeaderStatusMessages StatusMessages
        {
            get
            {
                return this.statusMessagesField;
            }
            set
            {
                this.statusMessagesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
    public partial class ResponseHeaderStatusMessages
    {
        
        private byte applicationIDField;

        private byte messageCodeField;

        private string messageDescriptionField;

        private object messageTypeField;
        
        /// <remarks/>
        public byte ApplicationID
        {
            get
            {
                return this.applicationIDField;
            }
            set
            {
                this.applicationIDField = value;
            }
        }
        /// <remarks/>
        public byte MessageCode
        {
            get
            {
                return this.messageCodeField;
            }
            set
            {
                this.messageCodeField = value;
            }
        }

        /// <remarks/>
        public string MessageDescription
        {
            get
            {
                return this.messageDescriptionField;
            }
            set
            {
                this.messageDescriptionField = value;
            }
        }

        /// <remarks/>
        public object MessageType
        {
            get
            {
                return this.messageTypeField;
            }
            set
            {
                this.messageTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class EnvelopeBody
    {

        private FundsTransfer fundsTransferField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
        public FundsTransfer FundsTransfer
        {
            get
            {
                return this.fundsTransferField;
            }
            set
            {
                this.fundsTransferField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0", IsNullable = false)]
    public partial class FundsTransfer
    {

        private string messageReferenceField;

        private byte systemCodeField;

        private System.DateTime transactionDatetimeField;

        private System.DateTime valueDateField;

        private string transactionIDField;

        private string transactionTypeField;

        private string transactionSubTypeField;

        private FundsTransferTransactionResponseDetails transactionResponseDetailsField;

        private FundsTransferTransactionItem[] transactionItemsField;

        private FundsTransferTransactionCharges transactionChargesField;

        private FundsTransferTransactionItem1 transactionItemField;

        /// <remarks/>
        public string MessageReference
        {
            get
            {
                return this.messageReferenceField;
            }
            set
            {
                this.messageReferenceField = value;
            }
        }

        /// <remarks/>
        public byte SystemCode
        {
            get
            {
                return this.systemCodeField;
            }
            set
            {
                this.systemCodeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime TransactionDatetime
        {
            get
            {
                return this.transactionDatetimeField;
            }
            set
            {
                this.transactionDatetimeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ValueDate
        {
            get
            {
                return this.valueDateField;
            }
            set
            {
                this.valueDateField = value;
            }
        }

        /// <remarks/>
        public string TransactionID
        {
            get
            {
                return this.transactionIDField;
            }
            set
            {
                this.transactionIDField = value;
            }
        }

        /// <remarks/>
        public string TransactionType
        {
            get
            {
                return this.transactionTypeField;
            }
            set
            {
                this.transactionTypeField = value;
            }
        }

        /// <remarks/>
        public string TransactionSubType
        {
            get
            {
                return this.transactionSubTypeField;
            }
            set
            {
                this.transactionSubTypeField = value;
            }
        }

        /// <remarks/>
        public FundsTransferTransactionResponseDetails TransactionResponseDetails
        {
            get
            {
                return this.transactionResponseDetailsField;
            }
            set
            {
                this.transactionResponseDetailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("TransactionItem", IsNullable = false)]
        public FundsTransferTransactionItem[] TransactionItems
        {
            get
            {
                return this.transactionItemsField;
            }
            set
            {
                this.transactionItemsField = value;
            }
        }

        /// <remarks/>
        public FundsTransferTransactionCharges TransactionCharges
        {
            get
            {
                return this.transactionChargesField;
            }
            set
            {
                this.transactionChargesField = value;
            }
        }

        /// <remarks/>
        public FundsTransferTransactionItem1 TransactionItem
        {
            get
            {
                return this.transactionItemField;
            }
            set
            {
                this.transactionItemField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    public partial class FundsTransferTransactionResponseDetails
    {

        private string remarksField;

        /// <remarks/>
        public string Remarks
        {
            get
            {
                return this.remarksField;
            }
            set
            {
                this.remarksField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    public partial class FundsTransferTransactionItem
    {

        private ulong transactionReferenceField;

        private string transactionItemKeyField;

        private ulong accountNumberField;

        private string debitCreditFlagField;

        private decimal transactionAmountField;

        private string transactionCurrencyField;

        private string narrativeField;

        private string transactionCodeField;

        private decimal availableBalanceField;

        private decimal bookedBalanceField;

        private object temporaryODDetailsField;

        /// <remarks/>
        public ulong TransactionReference
        {
            get
            {
                return this.transactionReferenceField;
            }
            set
            {
                this.transactionReferenceField = value;
            }
        }

        /// <remarks/>
        public string TransactionItemKey
        {
            get
            {
                return this.transactionItemKeyField;
            }
            set
            {
                this.transactionItemKeyField = value;
            }
        }

        /// <remarks/>
        public ulong AccountNumber
        {
            get
            {
                return this.accountNumberField;
            }
            set
            {
                this.accountNumberField = value;
            }
        }

        /// <remarks/>
        public string DebitCreditFlag
        {
            get
            {
                return this.debitCreditFlagField;
            }
            set
            {
                this.debitCreditFlagField = value;
            }
        }

        /// <remarks/>
        public decimal TransactionAmount
        {
            get
            {
                return this.transactionAmountField;
            }
            set
            {
                this.transactionAmountField = value;
            }
        }

        /// <remarks/>
        public string TransactionCurrency
        {
            get
            {
                return this.transactionCurrencyField;
            }
            set
            {
                this.transactionCurrencyField = value;
            }
        }

        /// <remarks/>
        public string Narrative
        {
            get
            {
                return this.narrativeField;
            }
            set
            {
                this.narrativeField = value;
            }
        }

        /// <remarks/>
        public string TransactionCode
        {
            get
            {
                return this.transactionCodeField;
            }
            set
            {
                this.transactionCodeField = value;
            }
        }

        /// <remarks/>
        public decimal AvailableBalance
        {
            get
            {
                return this.availableBalanceField;
            }
            set
            {
                this.availableBalanceField = value;
            }
        }

        /// <remarks/>
        public decimal BookedBalance
        {
            get
            {
                return this.bookedBalanceField;
            }
            set
            {
                this.bookedBalanceField = value;
            }
        }

        /// <remarks/>
        public object TemporaryODDetails
        {
            get
            {
                return this.temporaryODDetailsField;
            }
            set
            {
                this.temporaryODDetailsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    public partial class FundsTransferTransactionCharges
    {

        private FundsTransferTransactionChargesCharge chargeField;

        /// <remarks/>
        public FundsTransferTransactionChargesCharge Charge
        {
            get
            {
                return this.chargeField;
            }
            set
            {
                this.chargeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    public partial class FundsTransferTransactionChargesCharge
    {

        private object eventTypeField;

        private object eventIdField;

        /// <remarks/>
        public object EventType
        {
            get
            {
                return this.eventTypeField;
            }
            set
            {
                this.eventTypeField = value;
            }
        }

        /// <remarks/>
        public object EventId
        {
            get
            {
                return this.eventIdField;
            }
            set
            {
                this.eventIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    public partial class FundsTransferTransactionItem1
    {

        private System.DateTime valueDateField;

        /// <remarks/>
        public System.DateTime ValueDate
        {
            get
            {
                return this.valueDateField;
            }
            set
            {
                this.valueDateField = value;
            }
        }
    }


}