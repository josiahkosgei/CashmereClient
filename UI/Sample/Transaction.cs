﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Stores the summary of a transaction attempt. A transaction can have various stages of completion if an error is encountered.
    /// </summary>
    [Table("Transaction")]
    [Index(nameof(CITId), Name = "icit_id_Transaction")]
    [Index(nameof(DeviceId), Name = "idevice_Transaction")]
    [Index(nameof(SessionId), Name = "isession_id_Transaction")]
    [Index(nameof(TxCurrency), Name = "itx_currency_Transaction")]
    [Index(nameof(TxType), Name = "itx_type_Transaction")]
    public partial class Transaction
    {
        public Transaction()
        {
            DenominationDetails = new HashSet<DenominationDetail>();
            EscrowJams = new HashSet<EscrowJam>();
            Printouts = new HashSet<Printout>();
            TransactionExceptions = new HashSet<TransactionException>();
        }


        /// <summary>
        /// Globally Unique Identifier for replication
        /// </summary>
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The transaction type chosen by the user from TransactionTypeListItem
        /// </summary>
        [Column("tx_type")]
        public int? TxType { get; set; }

        /// <summary>
        /// The session this transaction fullfills
        /// </summary>
        [Column("session_id")]
        public Guid SessionId { get; set; }
        [Column("tx_random_number")]
        public int? TxRandomNumber { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }

        /// <summary>
        /// The date and time the transaction was recorded in the database. Can be different from core banking&apos;s transaction date
        /// </summary>
        [Column("tx_start_date")]
        public DateTime TxStartDate { get; set; }

        /// <summary>
        /// The date and time the transaction was recorded in the database. Can be different from core banking&apos;s transaction date
        /// </summary>
        [Column("tx_end_date")]
        public DateTime? TxEndDate { get; set; }

        /// <summary>
        /// Indicate if the transaction has completed or is in progress
        /// </summary>
        [Column("tx_completed")]
        public bool TxCompleted { get; set; }

        /// <summary>
        /// User selected currency. A transaction can only have one currency at a time
        /// </summary>
        [Column("tx_currency")]
        [StringLength(3)]
        [Unicode(false)]
        public string TxCurrency { get; set; }
        [Column("tx_amount")]
        public long? TxAmount { get; set; }

        /// <summary>
        /// Account Number for crediting. This can be a suspense account
        /// </summary>
        [Column("tx_account_number")]
        [StringLength(50)]
        [Unicode(false)]
        public string TxAccountNumber { get; set; }

        /// <summary>
        /// The account name returned by core banking.
        /// </summary>
        [Column("cb_account_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string CbAccountName { get; set; }

        /// <summary>
        /// Used for double validation transactions where the user enters a second account number. E.g Mpesa Agent Number
        /// </summary>
        [Column("tx_ref_account")]
        [StringLength(50)]
        [Unicode(false)]
        public string TxRefAccount { get; set; }

        /// <summary>
        /// Core banking returned Reference Account Name if any following a validation request for a Reference Account Number
        /// </summary>
        [Column("cb_ref_account_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string CbRefAccountName { get; set; }

        /// <summary>
        /// The narration from the deposit slip. Usually set to 16 characters in core banking
        /// </summary>
        [Column("tx_narration")]
        [StringLength(50)]
        [Unicode(false)]
        public string TxNarration { get; set; }

        /// <summary>
        /// Customer&apos;s name
        /// </summary>
        [Column("tx_depositor_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string TxDepositorName { get; set; }

        /// <summary>
        /// Customer&apos;s ID number
        /// </summary>
        [Column("tx_id_number")]
        [StringLength(50)]
        [Unicode(false)]
        public string TxIdNumber { get; set; }

        /// <summary>
        /// Customer entered phone number
        /// </summary>
        [Column("tx_phone")]
        [StringLength(50)]
        [Unicode(false)]
        public string TxPhone { get; set; }
        [Column("funds_source")]
        [StringLength(255)]
        public string FundsSource { get; set; }

        /// <summary>
        /// Boolean for if the transaction succeeded 100% without encountering a critical terminating error
        /// </summary>
        [Column("tx_result")]
        public int TxResult { get; set; }

        /// <summary>
        /// Last error code encountered during the transaction
        /// </summary>
        [Column("tx_error_code")]
        public int TxErrorCode { get; set; }

        /// <summary>
        /// Last error message encountered during the transaction
        /// </summary>
        [Column("tx_error_message")]
        [StringLength(255)]
        [Unicode(false)]
        public string TxErrorMessage { get; set; }

        /// <summary>
        /// Core banking returned transaction number
        /// </summary>
        [Column("cb_tx_number")]
        [StringLength(50)]
        [Unicode(false)]
        public string CbTxNumber { get; set; }

        /// <summary>
        /// Core banking returned transaction date and time
        /// </summary>
        [Column("cb_date")]
        public DateTime? CbDate { get; set; }

        /// <summary>
        /// Core banking returned transaction status e.g. SUCCESS or FAILURE
        /// </summary>
        [Column("cb_tx_status")]
        [StringLength(16)]
        [Unicode(false)]
        public string CbTxStatus { get; set; }

        /// <summary>
        /// Additional status details returned by core banking e.g. &apos;Amount must be less that MAX_AMOUNT&apos;
        /// </summary>
        [Column("cb_status_detail")]
        public string CbStatusDetail { get; set; }
        [Column("notes_rejected")]
        public bool NotesRejected { get; set; }
        [Column("jam_detected")]
        public bool JamDetected { get; set; }
        [Column("cit_id")]
        public Guid? CITId { get; set; }
        [Column("escrow_jam")]
        public bool EscrowJam { get; set; }

        [ForeignKey(nameof(CITId))]
        [InverseProperty("Transactions")]
        public virtual CIT CIT { get; set; }
        [ForeignKey(nameof(DeviceId))]
        [InverseProperty("Transactions")]
        public virtual Device Device { get; set; }
        [ForeignKey(nameof(SessionId))]
        [InverseProperty(nameof(DepositorSession.Transactions))]
        public virtual DepositorSession Session { get; set; }
        [ForeignKey(nameof(TxCurrency))]
        [InverseProperty(nameof(Currency.Transactions))]
        public virtual Currency TxCurrencyNavigation { get; set; }
        [ForeignKey(nameof(TxType))]
        [InverseProperty(nameof(TransactionTypeListItem.Transactions))]
        public virtual TransactionTypeListItem TxTypeNavigation { get; set; }
        [InverseProperty(nameof(DenominationDetail.Tx))]
        public virtual ICollection<DenominationDetail> DenominationDetails { get; set; }
        [InverseProperty("Transaction")]
        public virtual ICollection<EscrowJam> EscrowJams { get; set; }
        [InverseProperty(nameof(Printout.Tx))]
        public virtual ICollection<Printout> Printouts { get; set; }
        [InverseProperty(nameof(TransactionException.Transaction))]
        public virtual ICollection<TransactionException> TransactionExceptions { get; set; }
    }
}