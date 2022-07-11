﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// status of the communication service for email, sms etc
    /// </summary>
    [Table("CashmereCommunicationServiceStatus")]
    public partial class CashmereCommunicationServiceStatus
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("email_status")]
        public int EmailStatus { get; set; }
        [Column("email_error")]
        public int EmailError { get; set; }
        [Column("email_error_message")]
        public string EmailErrorMessage { get; set; }
        [Column("sms_status")]
        public int SmsStatus { get; set; }
        [Column("sms_error")]
        public int SmsError { get; set; }
        [Column("sms_error_message")]
        public string SmsErrorMessage { get; set; }
        [Column("modified")]
        public DateTime Modified { get; set; }
    }
}