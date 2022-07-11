using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cashmere.Library.CashmereDataAccess.Migrations
{
    public partial class definelangcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "exp");

            migrationBuilder.EnsureSchema(
                name: "cb");

            migrationBuilder.EnsureSchema(
                name: "xlns");

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The name of the activity. will be used in lookups"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Short description of the activity being performed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                },
                comment: "a task a user needs permission to perform");

            migrationBuilder.CreateTable(
                name: "AlertAttachmentType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlertTypeId = table.Column<int>(type: "int", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MimeSubtype = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertAttachmentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlertEmailAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlertEmailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Hash = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertEmailAttachment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlertEmailResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    AlertEmailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValueSql: "(N'NEW')"),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
                    Recipient = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Error = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false),
                    HtmlMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RawTextMessage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertEmailResult", x => x.Id);
                },
                comment: "Result of sending an alert email");

            migrationBuilder.CreateTable(
                name: "AlertEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The Device that raised the alert"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())", comment: "The exact moment the alert was raised"),
                    AlertTypeId = table.Column<int>(type: "int", nullable: false, comment: "the type of alert"),
                    DateDetected = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When was the alert detected, in case it is different from the created date. e.g. may indicate the event occured some other time, possibly before it was created in the db"),
                    DateResolved = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "If tied to another Alert, this is when the the paired Alert was resolved e.g. a door close alert may resolve a previous door open alert"),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false, comment: "whether the Alert in qustion has been resolved or is still open"),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false, comment: "has this alert been processed and messages created accordingly"),
                    AlertEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "if this alert is paired with a previous alert, it is linked here"),
                    IsProcessing = table.Column<bool>(type: "bit", nullable: false, comment: "is this alert currently being processed, used for concurrency control"),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertEvent", x => x.Id);
                },
                comment: "An event that has raised an alert. Various messages can be sent based on the alert raised e.g. SMS EMail etc");

            migrationBuilder.CreateTable(
                name: "AlertMessageType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Name of the AlertMessage"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Title displayed in th eheader sction of messages"),
                    EmailContentTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The HTML template that will be merged into later"),
                    RawEmailContentTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The raw text template that will be merged into later"),
                    PhoneContentTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The SMS template that will be merged into later"),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))", comment: "whether or not the alert message type in enabled and can be instantiated")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertMessageType", x => x.Id);
                },
                comment: "Types of messages for alerts sent via email or phone");

            migrationBuilder.CreateTable(
                name: "CashmereCommunicationServiceStatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email_status = table.Column<int>(type: "int", nullable: false),
                    email_error = table.Column<int>(type: "int", nullable: false),
                    email_error_message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sms_status = table.Column<int>(type: "int", nullable: false),
                    sms_error = table.Column<int>(type: "int", nullable: false),
                    sms_error_message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    modified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashmereCommunicationServiceStatus", x => x.id);
                },
                comment: "status of the communication service for email, sms etc");

            migrationBuilder.CreateTable(
                name: "ConfigCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Name of the AlertMessage"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigCategory", x => x.Id);
                },
                comment: "Categorisation of configuration opions");

            migrationBuilder.CreateTable(
                name: "ConfigGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigGroup_ConfigGroup",
                        column: x => x.ParentGroupId,
                        principalTable: "ConfigGroup",
                        principalColumn: "Id");
                },
                comment: "Group together configurations so devices can share configs");

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryCode = table.Column<string>(type: "char(900)", unicode: false, fixedLength: true, nullable: false, defaultValueSql: "('')"),
                    CountryName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryCode);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Code = table.Column<string>(type: "char(900)", unicode: false, fixedLength: true, nullable: false, comment: "ISO 4217 Three Character Currency Code"),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "Name of the currency"),
                    Minor = table.Column<int>(type: "int", nullable: false, comment: "Expresses the relationship between a major currency unit and its corresponding minor currency unit. This mechanism is called the currency \"exponent\" and assumes a base of 10. Will be used with converters in the GUI"),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "two character country code for the national flag to display for the language"),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, comment: "whether the system supports the language"),
                    Iso3NumericCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Code);
                },
                comment: "Currency enumeration");

            migrationBuilder.CreateTable(
                name: "DashboardDatum",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SynchronizeTitle = table.Column<bool>(type: "bit", nullable: true),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardDatum", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "DenominationView",
                columns: table => new
                {
                    tx_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    subtotal = table.Column<long>(type: "bigint", nullable: true),
                    _50 = table.Column<long>(name: "50", type: "bigint", nullable: false),
                    _100 = table.Column<long>(name: "100", type: "bigint", nullable: false),
                    _200 = table.Column<long>(name: "200", type: "bigint", nullable: false),
                    _500 = table.Column<long>(name: "500", type: "bigint", nullable: false),
                    _1000 = table.Column<long>(name: "1000", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DeviceStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ControllerState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaCurrency = table.Column<string>(type: "char", unicode: false, fixedLength: true, nullable: false),
                    BagNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BagStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BagNoteLevel = table.Column<int>(type: "int", nullable: false),
                    BagNoteCapacity = table.Column<string>(type: "nchar", fixedLength: true, nullable: false),
                    BagValueLevel = table.Column<long>(type: "bigint", nullable: true),
                    BagValueCapacity = table.Column<long>(type: "bigint", nullable: true),
                    BagPercentFull = table.Column<int>(type: "int", nullable: false),
                    SensorsType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SensorsStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SensorsValue = table.Column<int>(type: "int", nullable: false),
                    SensorsDoor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SensorsBag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscrowType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscrowStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscrowPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MachineDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentStatus = table.Column<int>(type: "int", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceStatus", x => x.Id);
                },
                comment: "Current State of the device");

            migrationBuilder.CreateTable(
                name: "DeviceType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    NoteIn = table.Column<bool>(type: "bit", nullable: false),
                    NoteOut = table.Column<bool>(type: "bit", nullable: false),
                    NoteEscrow = table.Column<bool>(type: "bit", nullable: false),
                    CoinIn = table.Column<bool>(type: "bit", nullable: false),
                    CoinOut = table.Column<bool>(type: "bit", nullable: false),
                    CoinEscrow = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceType", x => x.Id);
                },
                comment: "Describes the type of device");

            migrationBuilder.CreateTable(
                name: "GuiPrepopList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    AllowFreeText = table.Column<bool>(type: "bit", nullable: false),
                    DefaultIndex = table.Column<int>(type: "int", nullable: false),
                    UseDefault = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuiPrepopList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuiScreenType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuiScreenType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Code = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "two character country code for the national flag to display for the language"),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, comment: "whether the system supports the language")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Code);
                },
                comment: "Available languages in the system");

            migrationBuilder.CreateTable(
                name: "ModelDifference",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContextId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: true),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelDifference", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "PasswordPolicy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    MinLength = table.Column<int>(type: "int", nullable: false),
                    MinLowercase = table.Column<int>(type: "int", nullable: false),
                    MinDigits = table.Column<int>(type: "int", nullable: false),
                    MinUppercase = table.Column<int>(type: "int", nullable: false),
                    MinSpecial = table.Column<int>(type: "int", nullable: false),
                    AllowedSpecial = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExpiryDays = table.Column<int>(type: "int", nullable: false),
                    HistorySize = table.Column<int>(type: "int", nullable: false),
                    UseHistory = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordPolicy", x => x.Id);
                },
                comment: "The system password policy");

            migrationBuilder.CreateTable(
                name: "PingRequest",
                schema: "cb",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestUUID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    ServerOnline = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsError = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PingRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrinterStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    PrinterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsError = table.Column<bool>(type: "bit", nullable: false),
                    HasPaper = table.Column<bool>(type: "bit", nullable: false),
                    CoverOpen = table.Column<bool>(type: "bit", nullable: false),
                    ErrorCode = table.Column<int>(type: "int", nullable: false),
                    ErrorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrinterStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                },
                comment: "a user's role storing all their permissions");

            migrationBuilder.CreateTable(
                name: "SysTextItemCategory",
                schema: "xlns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysTextItemCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextItemCategory_TextItemCategory",
                        column: x => x.ParentId,
                        principalSchema: "xlns",
                        principalTable: "SysTextItemCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SysTextItemType",
                schema: "xlns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysTextItemType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TextItemCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextItemCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UI_TextItemCategory_TextItemCategory",
                        column: x => x.ParentId,
                        principalTable: "TextItemCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TextItemType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextItemType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThisDevice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeviceLocation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    ConfigGroup = table.Column<int>(type: "int", nullable: false),
                    UserGroup = table.Column<int>(type: "int", nullable: true),
                    GuiscreenList = table.Column<int>(type: "int", nullable: false),
                    LanguageList = table.Column<int>(type: "int", nullable: true),
                    CurrencyList = table.Column<int>(type: "int", nullable: false),
                    TransactionTypeList = table.Column<int>(type: "int", nullable: false),
                    LoginCycles = table.Column<int>(type: "int", nullable: false),
                    LoginAttempts = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TransactionLimitList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLimitList", x => x.Id);
                },
                comment: "Sets the transaction limit amounts for each currency");

            migrationBuilder.CreateTable(
                name: "TransactionType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Vendor supplied ScreenType GUID"),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "common name for the transaction e.g. Mpesa Deposit"),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "common description for the transaction type"),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionType", x => x.Id);
                },
                comment: "");

            migrationBuilder.CreateTable(
                name: "TransactionTypeList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypeList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionView",
                columns: table => new
                {
                    RandomNumber = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeviceName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DeviceLocation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CbTransactionNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    SubTotal = table.Column<long>(type: "bigint", nullable: true),
                    Denomination50 = table.Column<long>(type: "bigint", nullable: false),
                    Denomination100 = table.Column<long>(type: "bigint", nullable: false),
                    Denomination200 = table.Column<long>(type: "bigint", nullable: false),
                    Denomination500 = table.Column<long>(type: "bigint", nullable: false),
                    Denomination1000 = table.Column<long>(type: "bigint", nullable: false),
                    AccountNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AccountName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ReferenceAccountNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ReferenceAccountName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Narration = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DepositorName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    IdNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false),
                    ErrorCode = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    CbStatus = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    JamDetected = table.Column<bool>(type: "bit", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CITId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UptimeComponentState",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Device = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ComponentState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UptimeComponentState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UptimeMode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Device = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeviceMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UptimeMode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ParentGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroup_UserGroup",
                        column: x => x.ParentGroupId,
                        principalTable: "UserGroup",
                        principalColumn: "Id");
                },
                comment: "groups together users ho have privileges on the same machine");

            migrationBuilder.CreateTable(
                name: "ValidationList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "common name for the transaction e.g. Mpesa Deposit"),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "common description for the transaction type"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationList", x => x.Id);
                },
                comment: "List of validations to be performed on a field");

            migrationBuilder.CreateTable(
                name: "ValidationType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "common name for the transaction e.g. Mpesa Deposit"),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "common description for the transaction type"),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationType", x => x.Id);
                },
                comment: "The type of validation e.g. regex, etc");

            migrationBuilder.CreateTable(
                name: "ViewConfig",
                columns: table => new
                {
                    ConfigId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConfigValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ViewPermission",
                columns: table => new
                {
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Activity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StandaloneAllowed = table.Column<bool>(type: "bit", nullable: false),
                    StandaloneAuthenticationRequired = table.Column<bool>(type: "bit", nullable: false),
                    StandaloneCanAuthenticate = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "WebUserLoginCount",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoginCount = table.Column<int>(type: "int", nullable: true),
                    User = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: true),
                    Gcrecord = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebUserLoginCount", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "WebUserPasswordHistory",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Datetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: true),
                    Gcrecord = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebUserPasswordHistory", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "XpobjectType",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    AssemblyName = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XpobjectType", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "AlertEmail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())", comment: "Datetime when the email message was created"),
                    From = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Email address of the sender"),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Fills the \"To:\" heading of the email"),
                    Subject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Subject of the email"),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Pipe delimited List of filenames for files to attach when sending. Files must be accessible from the server"),
                    HtmlMessage = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The HTML formatted message"),
                    RawTextMessage = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The raw ANSI text version of the email for clients that do not support HTML emails e.g. mobile phones etc"),
                    Sent = table.Column<bool>(type: "bit", nullable: false, comment: "Whether or not the email message has been processed by the server"),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Datetime when the email message was processed by the server"),
                    AlertEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Corresponding Alert that is tied to this email message"),
                    SendError = table.Column<bool>(type: "bit", nullable: false, comment: "Was there a fatal error during processing this email message"),
                    SendErrorMessage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Error message returned by the server when email sending failed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertEmail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertEmail_AlertEmailEvent",
                        column: x => x.AlertEventId,
                        principalTable: "AlertEvent",
                        principalColumn: "Id");
                },
                comment: "Stores emails sent by the system");

            migrationBuilder.CreateTable(
                name: "AlertSMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())", comment: "Datetime when the SMS alert message was created by the system"),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "the number from which the SMS originates"),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Pipe delimited List of phone numbers to receive SMSes"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "the SMS text message to deliver"),
                    Sent = table.Column<bool>(type: "bit", nullable: false, comment: "whether or not the SMS message was processed"),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "the datetime when the SMS message was processed"),
                    AlertEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "the associated AlertEvent for this SMS message"),
                    SendError = table.Column<bool>(type: "bit", nullable: false, comment: "was there a fatal rror during processing?"),
                    SendErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "error mssage returned by the system while processing the SMS message")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertSMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertSMS_AlertEvent",
                        column: x => x.AlertEventId,
                        principalTable: "AlertEvent",
                        principalColumn: "Id");
                },
                comment: "AlertSmses");

            migrationBuilder.CreateTable(
                name: "Config",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Config", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Config_ConfigCategory",
                        column: x => x.CategoryId,
                        principalTable: "ConfigCategory",
                        principalColumn: "Id");
                },
                comment: "Configuration List");

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryCode = table.Column<string>(type: "char(900)", unicode: false, fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bank_Country",
                        column: x => x.CountryCode,
                        principalTable: "Country",
                        principalColumn: "CountryCode");
                },
                comment: "The bank that owns the depositor");

            migrationBuilder.CreateTable(
                name: "CurrencyList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    DefaultCurrencyId = table.Column<string>(type: "char(900)", unicode: false, fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyList_Currency",
                        column: x => x.DefaultCurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Code");
                },
                comment: "Enumeration of allowed Currencies. A device can then associate with a currency List");

            migrationBuilder.CreateTable(
                name: "LanguageList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    DefaultLanguageId = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageList_Language",
                        column: x => x.DefaultLanguageId,
                        principalTable: "Language",
                        principalColumn: "Code");
                },
                comment: "A List of languages a device supports");

            migrationBuilder.CreateTable(
                name: "ModelDifferenceAspect",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Xml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelDifferenceAspect", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_ModelDifferenceAspect_Owner",
                        column: x => x.OwnerId,
                        principalTable: "ModelDifference",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateTable(
                name: "AlertMessageRegistry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    AlertTypeId = table.Column<int>(type: "int", nullable: false, comment: "The type of alert the role can receive"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The role that will be given rights to the AlertMssage type"),
                    EmailEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))", comment: "Can the role receive email"),
                    PhoneEnabled = table.Column<bool>(type: "bit", nullable: false, comment: "Can the role receive an SMS message for the alert message type")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertMessageRegistry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertMessageRegistry_AlertMessageType",
                        column: x => x.AlertTypeId,
                        principalTable: "AlertMessageType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AlertMessageRegistry_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                },
                comment: "Register a role to receive an alert");

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StandaloneAllowed = table.Column<bool>(type: "bit", nullable: false),
                    StandaloneAuthenticationRequired = table.Column<bool>(type: "bit", nullable: false),
                    StandaloneCanAuthenticate = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_Activity",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Permission_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                },
                comment: "grant a role to perform an activity");

            migrationBuilder.CreateTable(
                name: "SysTextItem",
                schema: "xlns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultTranslation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextItemTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysTextItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysTextItem_SysTextItemCategory",
                        column: x => x.CategoryId,
                        principalSchema: "xlns",
                        principalTable: "SysTextItemCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_sysTextItem_sysTextItemType",
                        column: x => x.TextItemTypeId,
                        principalSchema: "xlns",
                        principalTable: "SysTextItemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DefaultTranslation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextItemTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UI_TextItem_TextItemCategory",
                        column: x => x.CategoryId,
                        principalTable: "TextItemCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UI_TextItem_TextItemType",
                        column: x => x.TextItemTypeId,
                        principalTable: "TextItemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionLimitListItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    TransactionLimitListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyCode = table.Column<string>(type: "char(900)", unicode: false, fixedLength: true, nullable: false, comment: "ISO 4217 Three Character Currency Code"),
                    ShowFundsSource = table.Column<bool>(type: "bit", nullable: false, comment: "Whether to show the source of funds screen after deposit limit is reached or passed"),
                    ShowFundsForm = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FundsSourceAmount = table.Column<long>(type: "bigint", nullable: false, comment: "The amount after which the Source of Funds screen will be shown"),
                    PreventOverdeposit = table.Column<bool>(type: "bit", nullable: false, comment: "CDM will not accept further deposits past the maximum"),
                    OverdepositAmount = table.Column<long>(type: "bigint", nullable: false, comment: "The amount after which the CDM will disable the counter"),
                    PreventUnderdeposit = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    UnderdepositAmount = table.Column<long>(type: "bigint", nullable: false),
                    PreventOvercount = table.Column<bool>(type: "bit", nullable: false),
                    OvercountAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLimitListItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionLimitListItem_Currency",
                        column: x => x.CurrencyCode,
                        principalTable: "Currency",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_TransactionLimitListItem_TransactionLimitList",
                        column: x => x.TransactionLimitListId,
                        principalTable: "TransactionLimitList",
                        principalColumn: "Id");
                },
                comment: "Limit values for each currency");

            migrationBuilder.CreateTable(
                name: "PermissionPolicyRole",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsAdministrative = table.Column<bool>(type: "bit", nullable: true),
                    CanEditModel = table.Column<bool>(type: "bit", nullable: true),
                    PermissionPolicy = table.Column<int>(type: "int", nullable: true),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: true),
                    Gcrecord = table.Column<int>(type: "int", nullable: true),
                    ObjectTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyRole", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_PermissionPolicyRole_ObjectType",
                        column: x => x.ObjectTypeId,
                        principalTable: "XpobjectType",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    ConfigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceConfig_Config",
                        column: x => x.ConfigId,
                        principalTable: "Config",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceConfig_ConfigGroup",
                        column: x => x.GroupId,
                        principalTable: "ConfigGroup",
                        principalColumn: "Id");
                },
                comment: "Link a Device to its configuration");

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branch_Bank",
                        column: x => x.BankId,
                        principalTable: "Bank",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CurrencyListCurrency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    CurrencyListId = table.Column<int>(type: "int", nullable: false, comment: "The Currency List to which the currency is associated"),
                    CurrencyItemId = table.Column<string>(type: "char(900)", unicode: false, fixedLength: true, nullable: false, comment: "The currency in the List"),
                    CurrencyOrder = table.Column<int>(type: "int", nullable: false, comment: "ASC Order of sorting for currencies in List."),
                    MaxValue = table.Column<long>(type: "bigint", nullable: false),
                    MaxCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyListCurrency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Currency_CurrencyList_Currency",
                        column: x => x.CurrencyItemId,
                        principalTable: "Currency",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Currency_CurrencyList_CurrencyList",
                        column: x => x.CurrencyListId,
                        principalTable: "CurrencyList",
                        principalColumn: "Id");
                },
                comment: "[m2m] Currency and CurrencyList");

            migrationBuilder.CreateTable(
                name: "LanguageListLanguage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    LanguageListId = table.Column<int>(type: "int", nullable: false),
                    LanguageItemId = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, nullable: false),
                    LanguageOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageListLanguage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageList_Language_Language",
                        column: x => x.LanguageItemId,
                        principalTable: "Language",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_LanguageList_Language_LanguageList",
                        column: x => x.LanguageListId,
                        principalTable: "LanguageList",
                        principalColumn: "Id");
                },
                comment: "[m2m] LanguageList and Language");

            migrationBuilder.CreateTable(
                name: "SysTextTranslation",
                schema: "xlns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    SysTextItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageCode = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, nullable: false),
                    TranslationSysText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysTextTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sysTextTranslation_Language",
                        column: x => x.LanguageCode,
                        principalTable: "Language",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_sysTextTranslation_sysTextItem",
                        column: x => x.SysTextItemId,
                        principalSchema: "xlns",
                        principalTable: "SysTextItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuiPrepopItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuiPrepopItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GUIPrepopItem_TextItem",
                        column: x => x.ValueId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TextTranslation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    TextItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageCode = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, nullable: false),
                    TranslationText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UI_Translation_Language",
                        column: x => x.LanguageCode,
                        principalTable: "Language",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_UI_Translation_TextItem",
                        column: x => x.TextItemId,
                        principalTable: "TextItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionPolicyNavigationPermissionsObject",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NavigateState = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyNavigationPermissionsObject", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_PermissionPolicyPermissionsObject_Role",
                        column: x => x.RoleId,
                        principalTable: "PermissionPolicyRole",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionPolicyTypePermissionsObject",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReadState = table.Column<int>(type: "int", nullable: true),
                    WriteState = table.Column<int>(type: "int", nullable: true),
                    CreateState = table.Column<int>(type: "int", nullable: true),
                    DeleteState = table.Column<int>(type: "int", nullable: true),
                    NavigateState = table.Column<int>(type: "int", nullable: true),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyTypePermissionsObject", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_PermissionPolicyTypePermissionsObject_Role",
                        column: x => x.RoleId,
                        principalTable: "PermissionPolicyRole",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebPortalRole",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebPortalRole", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_WebPortalRole_Oid",
                        column: x => x.Oid,
                        principalTable: "PermissionPolicyRole",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateTable(
                name: "GuiPrepopListItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    GuiPrepopListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuiPrepopItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuiPrepopListItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GUIPrepopList_Item_GUIPrepopItem",
                        column: x => x.GuiPrepopItemId,
                        principalTable: "GuiPrepopItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GUIPrepopList_Item_GUIPrepopList",
                        column: x => x.GuiPrepopListId,
                        principalTable: "GuiPrepopList",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PermissionPolicyMemberPermissionsObject",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Members = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReadState = table.Column<int>(type: "int", nullable: true),
                    WriteState = table.Column<int>(type: "int", nullable: true),
                    Criteria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypePermissionObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyMemberPermissionsObject", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_PermissionPolicyMemberPermissionsObject_TypePermissionObject",
                        column: x => x.TypePermissionObjectId,
                        principalTable: "PermissionPolicyTypePermissionsObject",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionPolicyObjectPermissionsObject",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Criteria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReadState = table.Column<int>(type: "int", nullable: true),
                    WriteState = table.Column<int>(type: "int", nullable: true),
                    DeleteState = table.Column<int>(type: "int", nullable: true),
                    NavigateState = table.Column<int>(type: "int", nullable: true),
                    TypePermissionObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionPolicyObjectPermissionsObject", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_PermissionPolicyObjectPermissionsObject_TypePermissionObject",
                        column: x => x.TypePermissionObjectId,
                        principalTable: "PermissionPolicyTypePermissionsObject",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationException",
                schema: "exp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stack = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationException", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The session this log entry belongs to"),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Datetime the system deems for the log entry."),
                    EventName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "The name of the log event"),
                    EventDetail = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "the details of the log message"),
                    EventType = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "the type of the log event used for grouping and sorting"),
                    Component = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "Which internal component produced the log entry e.g. GUI, APIs, DeviceController etc"),
                    LogLevel = table.Column<int>(type: "int", nullable: false, comment: "the LogLevel"),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationLog", x => x.Id);
                },
                comment: "Stores the general application log that the GUI and other local systems write to");

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "username for logging into the system"),
                    Password = table.Column<string>(type: "char(71)", unicode: false, fixedLength: true, maxLength: 71, nullable: false, comment: "salted and hashed password utilising a password library"),
                    Fname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "First names"),
                    Lname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Last name"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The role the user has e.g. Custodian, Branch Manager tc"),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "user email address, used to receive emails from the system"),
                    EmailEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))", comment: "whether or not the user is allowed to receive emails"),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "the phone number for the user to rceive SMSes from the system"),
                    PhoneEnabled = table.Column<bool>(type: "bit", nullable: false, comment: "can the user receive SMSes from the system"),
                    PasswordResetRequired = table.Column<bool>(type: "bit", nullable: false, comment: "should the user rset their password at their next login"),
                    LoginAttempts = table.Column<int>(type: "int", nullable: false, comment: "how many unsuccessful login attempts has the user mad in a row. used to lock the user automatically"),
                    UserGroupId = table.Column<int>(type: "int", nullable: false),
                    DepositorEnabled = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsAdUser = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationUserLoginDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuthorisinguserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InitialisinguserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationUser_UserGroup",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserChangePassword",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: true),
                    OldPassword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NewPassword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PasswordPolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserChangePassword", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_ApplicationUserChangePassword_PasswordPolicy",
                        column: x => x.PasswordPolicyId,
                        principalTable: "PasswordPolicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserChangePassword_User",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordHistory_User",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WebPortalRoleRolesApplicationUserApplicationUser",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: true),
                    ApplicationUsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebPortalRoleRolesApplicationUserApplicationUser", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_WebPortalRoleRoles_ApplicationUserApplicationUsers_ApplicationUsers",
                        column: x => x.ApplicationUsersId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebPortalRoleRoles_ApplicationUserApplicationUsers_Roles",
                        column: x => x.RolesId,
                        principalTable: "WebPortalRole",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserLoginDetail",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastPasswordDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginLogEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FailedLoginCount = table.Column<int>(type: "int", nullable: true),
                    OTP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OTPExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OTPEnabled = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResetEmailCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ResetEmailExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResetEmailEnabled = table.Column<bool>(type: "bit", nullable: true),
                    OptimisticLockField = table.Column<int>(type: "int", nullable: true),
                    GCRecord = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserLoginDetail", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_ApplicationUserLoginDetail_User",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicationUserLoginDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LockType = table.Column<int>(type: "int", nullable: true),
                    WebPortalInitiated = table.Column<bool>(type: "bit", nullable: true),
                    InitiatingUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLock_ApplicationUserLoginDetail",
                        column: x => x.ApplicationUserLoginDetailId,
                        principalTable: "ApplicationUserLoginDetail",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLock_InitiatingUser",
                        column: x => x.InitiatingUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WebPortalLogin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicationUserLoginDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebPortalLoginAction = table.Column<int>(type: "int", nullable: true),
                    Success = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ChangePassword = table.Column<bool>(type: "bit", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SessionID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SFBegone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebPortalLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebPortalLogin_ApplicationUserLoginDetail",
                        column: x => x.ApplicationUserLoginDetailId,
                        principalTable: "ApplicationUserLoginDetail",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CIT",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Device that conducted the CIT"),
                    CITDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())", comment: "Datetime of the CIT"),
                    CITCompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Datetime when the CIT was completed"),
                    StartUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ApplicationUser who initiated the CIT"),
                    AuthUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Application User who authorised the CIT event"),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "The datetime from which the CIT calculations will be carrid out"),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The datetime until which the CIT calculations will be carrid out"),
                    OldBagNumber = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The asset number of the Bag that was removed i.e. the full bag"),
                    NewBagNumber = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The asset number of the empty bag that was inserted"),
                    SealNumber = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The numbr on the tamper evident seal tag used to seal the bag"),
                    Complete = table.Column<bool>(type: "bit", nullable: false, comment: "Has the CIT been completed, used for calculating incomplete CITs"),
                    CITError = table.Column<int>(type: "int", nullable: false, comment: "The error code encountered during CIT"),
                    CITErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Error message encounterd during CIT"),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CIT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CIT_ApplicationUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CIT_ApplicationUser_StartUser",
                        column: x => x.StartUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                },
                comment: "store a CIT transaction");

            migrationBuilder.CreateTable(
                name: "CITDenomination",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    CITId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The CIT the record belongs to"),
                    Datetime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this item was recorded"),
                    CurrencyId = table.Column<string>(type: "char(900)", unicode: false, fixedLength: true, nullable: false, comment: "The currency code"),
                    Denom = table.Column<int>(type: "int", nullable: false, comment: "denomination of note or coin in major currency"),
                    Count = table.Column<long>(type: "bigint", nullable: false, comment: "How many of the denomination were counted"),
                    Subtotal = table.Column<long>(type: "bigint", nullable: false, comment: "The subtotal of the denomination calculated as denom*count")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITDenomination", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CITDenominations_CIT",
                        column: x => x.CITId,
                        principalTable: "CIT",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CITDenominations_Currency",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Code");
                },
                comment: "currency and deomination breakdown of the CIT bag");

            migrationBuilder.CreateTable(
                name: "CITPrintout",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    CITId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The CIT this rceipt belongs to"),
                    PrintGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Receipt SHA512 hash"),
                    PrintContent = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Text of the receipt"),
                    IsCopy = table.Column<bool>(type: "bit", nullable: false, comment: "Is this CIT Receipt a copy, used for marking duplicate receipts")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITPrintout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CITPrintout_CIT",
                        column: x => x.CITId,
                        principalTable: "CIT",
                        principalColumn: "Id");
                },
                comment: "Stores CIT receipts");

            migrationBuilder.CreateTable(
                name: "CITTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CITId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    SuspenseAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CbTxNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CbDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CbTxStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CbStatusDetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorCode = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CITTransaction_CIT_CITId",
                        column: x => x.CITId,
                        principalTable: "CIT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CrashEvent",
                schema: "exp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDetected = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrashEvent", x => x.Id);
                },
                comment: "contains a crash report");

            migrationBuilder.CreateTable(
                name: "DenominationDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    TxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Denom = table.Column<int>(type: "int", nullable: false, comment: "denomination of note or coin in major currency"),
                    Count = table.Column<long>(type: "bigint", nullable: false, comment: "How many of the denomination were counted"),
                    Subtotal = table.Column<long>(type: "bigint", nullable: false, comment: "The subtotal of the denomination calculated as denom*count"),
                    Datetime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DenominationDetail", x => x.Id);
                },
                comment: "Denomination enumeration for a Transaction");

            migrationBuilder.CreateTable(
                name: "DepositorSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SessionEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LanguageCode = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, nullable: false),
                    Complete = table.Column<bool>(type: "bit", nullable: false),
                    CompleteSuccess = table.Column<bool>(type: "bit", nullable: false),
                    ErrorCode = table.Column<int>(type: "int", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    AccountVerified = table.Column<bool>(type: "bit", nullable: false),
                    ReferenceAccountVerified = table.Column<bool>(type: "bit", nullable: false),
                    Salt = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositorSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositorSession_Language",
                        column: x => x.LanguageCode,
                        principalTable: "Language",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Stores details of a customer deposit session. Asuccessful session ends in a successful transaction");

            migrationBuilder.CreateTable(
                name: "SessionException",
                schema: "exp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stack = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionException", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionException_DepositorSession",
                        column: x => x.SessionId,
                        principalTable: "DepositorSession",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    DeviceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeviceLocation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    ConfigGroupId = table.Column<int>(type: "int", nullable: false),
                    UserGroupId = table.Column<int>(type: "int", nullable: false),
                    GuiScreenListId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    LanguageListId = table.Column<int>(type: "int", nullable: false),
                    CurrencyListId = table.Column<int>(type: "int", nullable: false),
                    TransactionTypeListId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    LoginCycles = table.Column<int>(type: "int", nullable: false, comment: "how many cycles of failed logins have been detected. used to lock the machine in case of password guessing"),
                    LoginAttempts = table.Column<int>(type: "int", nullable: false, comment: "how many times in a row a login attempt has failed"),
                    Secret = table.Column<string>(type: "char(128)", unicode: false, fixedLength: true, maxLength: 128, nullable: false),
                    Password = table.Column<string>(type: "char(128)", unicode: false, fixedLength: true, maxLength: 128, nullable: false),
                    MacAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Device_LanguageList",
                        column: x => x.LanguageListId,
                        principalTable: "LanguageList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceList_Branch",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceList_ConfigGroup",
                        column: x => x.ConfigGroupId,
                        principalTable: "ConfigGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceList_CurrencyList",
                        column: x => x.CurrencyListId,
                        principalTable: "CurrencyList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceList_DeviceType",
                        column: x => x.TypeId,
                        principalTable: "DeviceType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceList_TransactionTypeList",
                        column: x => x.TransactionTypeListId,
                        principalTable: "TransactionTypeList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceList_UserGroup",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceCITSuspenseAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyCode = table.Column<string>(type: "char(900)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Account = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCITSuspenseAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceCITSuspenseAccount_Currency_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalTable: "Currency",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceCITSuspenseAccount_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceLock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LockDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Locked = table.Column<bool>(type: "bit", nullable: false),
                    LockingUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WebLockingUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LockedByDevice = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceLock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceLock_ApplicationUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceLock_Device",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id");
                },
                comment: "Record device locking and unlocking activity");

            migrationBuilder.CreateTable(
                name: "DeviceLogin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    LoginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogoutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: true),
                    DepositorEnabled = table.Column<bool>(type: "bit", nullable: true),
                    ChangePassword = table.Column<bool>(type: "bit", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForcedLogout = table.Column<bool>(type: "bit", nullable: true),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceLogin_ApplicationUser",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceLogin_Device",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DevicePrinter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsInfront = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))", comment: "Is the printer in the front i.e. customer facing or in the rear i.e. custodian facing"),
                    Port = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Serial = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevicePrinter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevicePrinter_DeviceList",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DeviceSuspenseAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyCode = table.Column<string>(type: "char(900)", unicode: false, fixedLength: true, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Account = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceSuspenseAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceSuspenseAccount_Currency",
                        column: x => x.CurrencyCode,
                        principalTable: "Currency",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_DeviceSuspenseAccount_DeviceList",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EscrowJam",
                schema: "exp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateDetected = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DroppedAmount = table.Column<long>(type: "bigint", nullable: false),
                    EscrowAmount = table.Column<long>(type: "bigint", nullable: false),
                    PostedAmount = table.Column<long>(type: "bigint", nullable: false),
                    RetreivedAmount = table.Column<long>(type: "bigint", nullable: false),
                    RecoveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InitialisinguserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuthorisinguserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationUserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscrowJam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EscrowJam_ApplicationUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EscrowJam_ApplicationUser_ApplicationUserId1",
                        column: x => x.ApplicationUserId1,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GuiScreen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    GuiScreenTypeId = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Keyboard = table.Column<int>(type: "int", nullable: true),
                    IsMasked = table.Column<bool>(type: "bit", nullable: true),
                    PrefillText = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Text to prefil in the textbox"),
                    InputMask = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    GuiScreenTextId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuiScreen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GUIScreen_GUIScreenType",
                        column: x => x.GuiScreenTypeId,
                        principalTable: "GuiScreenType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GuiScreenText",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    GuiScreenId = table.Column<int>(type: "int", nullable: false, comment: "The GUIScreen this entry corresponds to"),
                    ScreenTitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScreenTitleInstructionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullInstructionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BtnAcceptCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BtnBackCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BtnCancelCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuiScreenText", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GUIScreenText_BtnAcceptCaption",
                        column: x => x.BtnAcceptCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GUIScreenText_BtnBackCaption",
                        column: x => x.BtnBackCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GUIScreenText_BtnCancelCaption",
                        column: x => x.BtnCancelCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GUIScreenText_FullInstructions",
                        column: x => x.FullInstructionsId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GUIScreenText_GUIScreen",
                        column: x => x.GuiScreenId,
                        principalTable: "GuiScreen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GUIScreenText_ScreenTitle",
                        column: x => x.ScreenTitleId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GUIScreenText_ScreenTitleInstruction",
                        column: x => x.ScreenTitleInstructionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                },
                comment: "Stores the text for a screen for a language");

            migrationBuilder.CreateTable(
                name: "GuiScreenList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    GuiScreenListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuiScreenList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuiScreenListScreen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    GuiScreenId = table.Column<int>(type: "int", nullable: false),
                    GuiScreenListId = table.Column<int>(type: "int", nullable: false),
                    ScreenOrder = table.Column<int>(type: "int", nullable: false),
                    Required = table.Column<bool>(type: "bit", nullable: false),
                    ValidationListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuiPrepopListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuiScreenListScreen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuiScreenList_Screen_GUIPrepopList",
                        column: x => x.GuiPrepopListId,
                        principalTable: "GuiPrepopList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuiScreenList_Screen_GUIScreen",
                        column: x => x.GuiScreenId,
                        principalTable: "GuiScreen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GuiScreenList_Screen_ValidationList",
                        column: x => x.ValidationListId,
                        principalTable: "ValidationList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuiScreenListScreen_GuiScreenList_GuiScreenListId",
                        column: x => x.GuiScreenListId,
                        principalTable: "GuiScreenList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Printout",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    TxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrintGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrintContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCopy = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printout", x => x.Id);
                },
                comment: "Stores contents of a printout for a transaction");

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())", comment: "Globally Unique Identifier for replication"),
                    TxType = table.Column<int>(type: "int", nullable: false, comment: "The transaction type chosen by the user from TransactionTypeListItem"),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The session this transaction fullfills"),
                    TxRandomNumber = table.Column<int>(type: "int", nullable: true),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TxStartDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date and time the transaction was recorded in the database. Can be different from core banking's transaction date"),
                    TxEndDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "The date and time the transaction was recorded in the database. Can be different from core banking's transaction date"),
                    TxCompleted = table.Column<bool>(type: "bit", nullable: false, comment: "Indicate if the transaction has completed or is in progress"),
                    TxCurrency = table.Column<string>(type: "char(900)", unicode: false, fixedLength: true, nullable: false, comment: "User selected currency. A transaction can only have one currency at a time"),
                    TxAmount = table.Column<long>(type: "bigint", nullable: true),
                    TxAccountNumber = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "Account Number for crediting. This can be a suspense account"),
                    CbAccountName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "The account name returned by core banking."),
                    TxRefAccount = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "Used for double validation transactions where the user enters a second account number. E.g Mpesa Agent Number"),
                    CbRefAccountName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "Core banking returned Reference Account Name if any following a validation request for a Reference Account Number"),
                    TxNarration = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "The narration from the deposit slip. Usually set to 16 characters in core banking"),
                    TxDepositorName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "Customer's name"),
                    TxIdNumber = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "Customer's ID number"),
                    TxPhone = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "Customer entered phone number"),
                    FundsSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TxResult = table.Column<int>(type: "int", nullable: false, comment: "Boolean for if the transaction succeeded 100% without encountering a critical terminating error"),
                    TxErrorCode = table.Column<int>(type: "int", nullable: false, comment: "Last error code encountered during the transaction"),
                    TxErrorMessage = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "Last error message encountered during the transaction"),
                    CbTxNumber = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "Core banking returned transaction number"),
                    CbDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Core banking returned transaction date and time"),
                    CbTxStatus = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "Core banking returned transaction status e.g. SUCCESS or FAILURE"),
                    CbStatusDetail = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Additional status details returned by core banking e.g. 'Amount must be less that MAX_AMOUNT'"),
                    NotesRejected = table.Column<bool>(type: "bit", nullable: false),
                    JamDetected = table.Column<bool>(type: "bit", nullable: false),
                    CITId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EscrowJam = table.Column<bool>(type: "bit", nullable: false),
                    TxSuspenseAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InitUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuthUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_CIT",
                        column: x => x.CITId,
                        principalTable: "CIT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_Currency_Transaction",
                        column: x => x.TxCurrency,
                        principalTable: "Currency",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_DepositorSession",
                        column: x => x.SessionId,
                        principalTable: "DepositorSession",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transaction_DeviceList",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id");
                },
                comment: "Stores the summary of a transaction attempt. A transaction can have various stages of completion if an error is encountered.");

            migrationBuilder.CreateTable(
                name: "TransactionException",
                schema: "exp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionException", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionException_Transaction",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id");
                },
                comment: "Exceptions encountered during execution");

            migrationBuilder.CreateTable(
                name: "TransactionPosting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    CbTxNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DrAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrAmount = table.Column<long>(type: "bigint", nullable: false),
                    CrAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CrCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CrAmount = table.Column<long>(type: "bigint", nullable: false),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CbDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CbTxStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CbStatusDetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceInitiated = table.Column<bool>(type: "bit", nullable: false),
                    PostStatus = table.Column<int>(type: "int", nullable: false),
                    InitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InitialisingUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorisingUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthResponse = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionPosting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionPosting_AuthorisingUser",
                        column: x => x.AuthorisingUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionPosting_InitialisingUser",
                        column: x => x.InitialisingUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionPosting_Transaction",
                        column: x => x.TxId,
                        principalTable: "Transaction",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransactionText",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    TxItemId = table.Column<int>(type: "int", nullable: false),
                    DisclaimerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TermsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullInstructionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListItemCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumberCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNameCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceAccountNumberCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceAccountNameCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NarrationCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AliasAccountNumberCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AliasAccountNameCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepositorNameCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumberCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNumberCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiptTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FundsSourceCaptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionText", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionText_Account_Name_Caption",
                        column: x => x.AccountNameCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Account_Number_Caption",
                        column: x => x.AccountNumberCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Alias_Account_Name_Caption",
                        column: x => x.AliasAccountNameCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Alias_Account_Number_Caption",
                        column: x => x.AliasAccountNumberCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Depositor_Name_Caption",
                        column: x => x.DepositorNameCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Disclaimers",
                        column: x => x.DisclaimerId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_full_instructions",
                        column: x => x.FullInstructionsId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Funds_Source_Caption",
                        column: x => x.FundsSourceCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_IdNumberCaption",
                        column: x => x.IdNumberCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionText_ListItemCaption",
                        column: x => x.ListItemCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_NarrationCaption",
                        column: x => x.NarrationCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_PhoneNumberCaption",
                        column: x => x.PhoneNumberCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_ReceiptTemplate",
                        column: x => x.ReceiptTemplateId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_ReferenceAccountNameCaption",
                        column: x => x.ReferenceAccountNameCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_ReferenceAccountNumberCaption",
                        column: x => x.ReferenceAccountNumberCaptionId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Terms",
                        column: x => x.TermsId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                },
                comment: "Stores the multi language texts for a tx");

            migrationBuilder.CreateTable(
                name: "TransactionTypeListItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "common name for the transaction e.g. Mpesa Deposit"),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "common description for the transaction type"),
                    ValidateReferenceAccount = table.Column<bool>(type: "bit", nullable: false),
                    DefaultAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "the default account that pre-polulates the AccountNumber of a transaction"),
                    DefaultAccountName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DefaultAccountCurrencyId = table.Column<string>(type: "char(900)", unicode: false, fixedLength: true, nullable: false, defaultValueSql: "('KES')"),
                    ValidateDefaultAccount = table.Column<bool>(type: "bit", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    TxTypeId = table.Column<int>(type: "int", nullable: false),
                    TxTypeGuiScreenListId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    CbTxType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "A string passed to core banking with transaction details so core banking can route the deposit to the correct handler"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    TxLimitListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TxTextId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitUserRequired = table.Column<bool>(type: "bit", nullable: false),
                    AuthUserRequired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypeListItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionListItem_TransactionType",
                        column: x => x.TxTypeId,
                        principalTable: "TransactionType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionTypeListItem_Currency",
                        column: x => x.DefaultAccountCurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_TransactionTypeListItem_GUIScreenList",
                        column: x => x.TxTypeGuiScreenListId,
                        principalTable: "GuiScreenList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionTypeListItem_TransactionLimitList",
                        column: x => x.TxLimitListId,
                        principalTable: "TransactionLimitList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionTypeListItem_TransactionText",
                        column: x => x.TxTextId,
                        principalTable: "TransactionText",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Transactions that the system can perform e.g. regular deposit, Mpesa deposit, etc");

            migrationBuilder.CreateTable(
                name: "TransactionTypeListTransactionTypeListItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    TxtypeListItemId = table.Column<int>(type: "int", nullable: false),
                    TxtypeListId = table.Column<int>(type: "int", nullable: false),
                    ListOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypeListTransactionTypeListItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionTypeList_TransactionTypeListItem_TransactionTypeList",
                        column: x => x.TxtypeListId,
                        principalTable: "TransactionTypeList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionTypeList_TransactionTypeListItem_TransactionTypeListItem",
                        column: x => x.TxtypeListItemId,
                        principalTable: "TransactionTypeListItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ValidationItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, comment: "common name for the transaction e.g. Mpesa Deposit"),
                    Category = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, comment: "common description for the transaction type"),
                    ValidationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    ErrorCode = table.Column<int>(type: "int", nullable: true),
                    ValidationTextId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValidationItem_ValidationType",
                        column: x => x.ValidationTypeId,
                        principalTable: "ValidationType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ValidationItemValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    ValidationItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationItemValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValidationItemValue_ValidationItem",
                        column: x => x.ValidationItemId,
                        principalTable: "ValidationItem",
                        principalColumn: "Id");
                },
                comment: "Individual values for the validation");

            migrationBuilder.CreateTable(
                name: "ValidationListValidationItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    ValidationListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValidationItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationListValidationItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValidationList_ValidationItem_ValidationItem",
                        column: x => x.ValidationItemId,
                        principalTable: "ValidationItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ValidationList_ValidationItem_ValidationList",
                        column: x => x.ValidationListId,
                        principalTable: "ValidationList",
                        principalColumn: "Id");
                },
                comment: "Link a ValidationItem to a ValidationList");

            migrationBuilder.CreateTable(
                name: "ValidationText",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    ValidationItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ErrorMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SuccessMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationText", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValidationText_ErrorMessage",
                        column: x => x.ErrorMessageId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ValidationText_SuccessMessage",
                        column: x => x.SuccessMessageId,
                        principalTable: "TextItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ValidationText_ValidationItem",
                        column: x => x.ValidationItemId,
                        principalTable: "ValidationItem",
                        principalColumn: "Id");
                },
                comment: "Multilanguage validation result text");

            migrationBuilder.CreateIndex(
                name: "IX_AlertEmail_AlertEventId",
                table: "AlertEmail",
                column: "AlertEventId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertMessageRegistry_AlertTypeId",
                table: "AlertMessageRegistry",
                column: "AlertTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertMessageRegistry_RoleId",
                table: "AlertMessageRegistry",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertSMS_AlertEventId",
                table: "AlertSMS",
                column: "AlertEventId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationException_DeviceId",
                schema: "exp",
                table: "ApplicationException",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLog_DeviceId",
                table: "ApplicationLog",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLog_SessionId",
                table: "ApplicationLog",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_ApplicationUserLoginDetailId",
                table: "ApplicationUser",
                column: "ApplicationUserLoginDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_AuthorisinguserId",
                table: "ApplicationUser",
                column: "AuthorisinguserId",
                unique: true,
                filter: "[AuthorisinguserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_AuthUserId",
                table: "ApplicationUser",
                column: "AuthUserId",
                unique: true,
                filter: "[AuthUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_InitialisinguserId",
                table: "ApplicationUser",
                column: "InitialisinguserId",
                unique: true,
                filter: "[InitialisinguserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_RoleId",
                table: "ApplicationUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_UserGroupId",
                table: "ApplicationUser",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserChangePassword_PasswordPolicyId",
                table: "ApplicationUserChangePassword",
                column: "PasswordPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserChangePassword_UserId",
                table: "ApplicationUserChangePassword",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserLoginDetail_LastLoginLogEntryId",
                table: "ApplicationUserLoginDetail",
                column: "LastLoginLogEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserLoginDetail_UserId",
                table: "ApplicationUserLoginDetail",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bank_CountryCode",
                table: "Bank",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_BankId",
                table: "Branch",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_CIT_ApplicationUserId",
                table: "CIT",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CIT_DeviceId",
                table: "CIT",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_CIT_StartUserId",
                table: "CIT",
                column: "StartUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CITDenomination_CITId",
                table: "CITDenomination",
                column: "CITId");

            migrationBuilder.CreateIndex(
                name: "IX_CITDenomination_CurrencyId",
                table: "CITDenomination",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CITPrintout_CITId",
                table: "CITPrintout",
                column: "CITId");

            migrationBuilder.CreateIndex(
                name: "IX_CITTransaction_CITId",
                table: "CITTransaction",
                column: "CITId");

            migrationBuilder.CreateIndex(
                name: "IX_Config_CategoryId",
                table: "Config",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigGroup_ParentGroupId",
                table: "ConfigGroup",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CrashEvent_DeviceId",
                schema: "exp",
                table: "CrashEvent",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyList_DefaultCurrencyId",
                table: "CurrencyList",
                column: "DefaultCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyListCurrency_CurrencyItemId",
                table: "CurrencyListCurrency",
                column: "CurrencyItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyListCurrency_CurrencyListId",
                table: "CurrencyListCurrency",
                column: "CurrencyListId");

            migrationBuilder.CreateIndex(
                name: "iGCRecord_DashboardData",
                table: "DashboardDatum",
                column: "GCRecord");

            migrationBuilder.CreateIndex(
                name: "IX_DenominationDetail_TxId",
                table: "DenominationDetail",
                column: "TxId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositorSession_DeviceId",
                table: "DepositorSession",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositorSession_LanguageCode",
                table: "DepositorSession",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_Device_BranchId",
                table: "Device",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_ConfigGroupId",
                table: "Device",
                column: "ConfigGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_CurrencyListId",
                table: "Device",
                column: "CurrencyListId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_GuiScreenListId",
                table: "Device",
                column: "GuiScreenListId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_LanguageListId",
                table: "Device",
                column: "LanguageListId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_TransactionTypeListId",
                table: "Device",
                column: "TransactionTypeListId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_TypeId",
                table: "Device",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_UserGroupId",
                table: "Device",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCITSuspenseAccount_CurrencyCode",
                table: "DeviceCITSuspenseAccount",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCITSuspenseAccount_DeviceId",
                table: "DeviceCITSuspenseAccount",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConfig_ConfigId",
                table: "DeviceConfig",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConfig_GroupId",
                table: "DeviceConfig",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLock_ApplicationUserId",
                table: "DeviceLock",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLock_DeviceId",
                table: "DeviceLock",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLogin_DeviceId",
                table: "DeviceLogin",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLogin_UserId",
                table: "DeviceLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DevicePrinter_DeviceId",
                table: "DevicePrinter",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceSuspenseAccount_CurrencyCode",
                table: "DeviceSuspenseAccount",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceSuspenseAccount_DeviceId",
                table: "DeviceSuspenseAccount",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_EscrowJam_ApplicationUserId",
                schema: "exp",
                table: "EscrowJam",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EscrowJam_ApplicationUserId1",
                schema: "exp",
                table: "EscrowJam",
                column: "ApplicationUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_EscrowJam_TransactionId",
                schema: "exp",
                table: "EscrowJam",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiPrepopItem_ValueId",
                table: "GuiPrepopItem",
                column: "ValueId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiPrepopListItem_GuiPrepopItemId",
                table: "GuiPrepopListItem",
                column: "GuiPrepopItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiPrepopListItem_GuiPrepopListId",
                table: "GuiPrepopListItem",
                column: "GuiPrepopListId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreen_GuiScreenTextId",
                table: "GuiScreen",
                column: "GuiScreenTextId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreen_GuiScreenTypeId",
                table: "GuiScreen",
                column: "GuiScreenTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenList_GuiScreenListId",
                table: "GuiScreenList",
                column: "GuiScreenListId",
                unique: true,
                filter: "[GuiScreenListId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenListScreen_GuiPrepopListId",
                table: "GuiScreenListScreen",
                column: "GuiPrepopListId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenListScreen_GuiScreenId",
                table: "GuiScreenListScreen",
                column: "GuiScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenListScreen_GuiScreenListId",
                table: "GuiScreenListScreen",
                column: "GuiScreenListId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenListScreen_ValidationListId",
                table: "GuiScreenListScreen",
                column: "ValidationListId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenText_BtnAcceptCaptionId",
                table: "GuiScreenText",
                column: "BtnAcceptCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenText_BtnBackCaptionId",
                table: "GuiScreenText",
                column: "BtnBackCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenText_BtnCancelCaptionId",
                table: "GuiScreenText",
                column: "BtnCancelCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenText_FullInstructionsId",
                table: "GuiScreenText",
                column: "FullInstructionsId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenText_GuiScreenId",
                table: "GuiScreenText",
                column: "GuiScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenText_ScreenTitleId",
                table: "GuiScreenText",
                column: "ScreenTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenText_ScreenTitleInstructionId",
                table: "GuiScreenText",
                column: "ScreenTitleInstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageList_DefaultLanguageId",
                table: "LanguageList",
                column: "DefaultLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageListLanguage_LanguageItemId",
                table: "LanguageListLanguage",
                column: "LanguageItemId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageListLanguage_LanguageListId",
                table: "LanguageListLanguage",
                column: "LanguageListId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelDifferenceAspect_OwnerId",
                table: "ModelDifferenceAspect",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordHistory_ApplicationUserId",
                table: "PasswordHistory",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_ActivityId",
                table: "Permission",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_RoleId",
                table: "Permission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyMemberPermissionsObject_TypePermissionObjectId",
                table: "PermissionPolicyMemberPermissionsObject",
                column: "TypePermissionObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyNavigationPermissionsObject_RoleId",
                table: "PermissionPolicyNavigationPermissionsObject",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyObjectPermissionsObject_TypePermissionObjectId",
                table: "PermissionPolicyObjectPermissionsObject",
                column: "TypePermissionObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyRole_ObjectTypeId",
                table: "PermissionPolicyRole",
                column: "ObjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionPolicyTypePermissionsObject_RoleId",
                table: "PermissionPolicyTypePermissionsObject",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Printout_TxId",
                table: "Printout",
                column: "TxId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionException_SessionId",
                schema: "exp",
                table: "SessionException",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SysTextItem_CategoryId",
                schema: "xlns",
                table: "SysTextItem",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SysTextItem_TextItemTypeId",
                schema: "xlns",
                table: "SysTextItem",
                column: "TextItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SysTextItemCategory_ParentId",
                schema: "xlns",
                table: "SysTextItemCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SysTextTranslation_LanguageCode",
                schema: "xlns",
                table: "SysTextTranslation",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_SysTextTranslation_SysTextItemId",
                schema: "xlns",
                table: "SysTextTranslation",
                column: "SysTextItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TextItem_CategoryId",
                table: "TextItem",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TextItem_TextItemTypeId",
                table: "TextItem",
                column: "TextItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TextItemCategory_ParentId",
                table: "TextItemCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_TextTranslation_LanguageCode",
                table: "TextTranslation",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_TextTranslation_TextItemId",
                table: "TextTranslation",
                column: "TextItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CITId",
                table: "Transaction",
                column: "CITId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DeviceId",
                table: "Transaction",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_SessionId",
                table: "Transaction",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TxCurrency",
                table: "Transaction",
                column: "TxCurrency");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TxType",
                table: "Transaction",
                column: "TxType");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionException_TransactionId",
                schema: "exp",
                table: "TransactionException",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLimitListItem_CurrencyCode",
                table: "TransactionLimitListItem",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLimitListItem_TransactionLimitListId",
                table: "TransactionLimitListItem",
                column: "TransactionLimitListId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionPosting_AuthorisingUserId",
                table: "TransactionPosting",
                column: "AuthorisingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionPosting_InitialisingUserId",
                table: "TransactionPosting",
                column: "InitialisingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionPosting_TxId",
                table: "TransactionPosting",
                column: "TxId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_AccountNameCaptionId",
                table: "TransactionText",
                column: "AccountNameCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_AccountNumberCaptionId",
                table: "TransactionText",
                column: "AccountNumberCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_AliasAccountNameCaptionId",
                table: "TransactionText",
                column: "AliasAccountNameCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_AliasAccountNumberCaptionId",
                table: "TransactionText",
                column: "AliasAccountNumberCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_DepositorNameCaptionId",
                table: "TransactionText",
                column: "DepositorNameCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_DisclaimerId",
                table: "TransactionText",
                column: "DisclaimerId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_FullInstructionsId",
                table: "TransactionText",
                column: "FullInstructionsId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_FundsSourceCaptionId",
                table: "TransactionText",
                column: "FundsSourceCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_IdNumberCaptionId",
                table: "TransactionText",
                column: "IdNumberCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_ListItemCaptionId",
                table: "TransactionText",
                column: "ListItemCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_NarrationCaptionId",
                table: "TransactionText",
                column: "NarrationCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_PhoneNumberCaptionId",
                table: "TransactionText",
                column: "PhoneNumberCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_ReceiptTemplateId",
                table: "TransactionText",
                column: "ReceiptTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_ReferenceAccountNameCaptionId",
                table: "TransactionText",
                column: "ReferenceAccountNameCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_ReferenceAccountNumberCaptionId",
                table: "TransactionText",
                column: "ReferenceAccountNumberCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_TermsId",
                table: "TransactionText",
                column: "TermsId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_TxItemId",
                table: "TransactionText",
                column: "TxItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListItem_DefaultAccountCurrencyId",
                table: "TransactionTypeListItem",
                column: "DefaultAccountCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListItem_TxLimitListId",
                table: "TransactionTypeListItem",
                column: "TxLimitListId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListItem_TxTextId",
                table: "TransactionTypeListItem",
                column: "TxTextId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListItem_TxTypeGuiScreenListId",
                table: "TransactionTypeListItem",
                column: "TxTypeGuiScreenListId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListItem_TxTypeId",
                table: "TransactionTypeListItem",
                column: "TxTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListTransactionTypeListItem_TxtypeListId",
                table: "TransactionTypeListTransactionTypeListItem",
                column: "TxtypeListId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListTransactionTypeListItem_TxtypeListItemId",
                table: "TransactionTypeListTransactionTypeListItem",
                column: "TxtypeListItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_ParentGroupId",
                table: "UserGroup",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLock_ApplicationUserLoginDetailId",
                table: "UserLock",
                column: "ApplicationUserLoginDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLock_InitiatingUserId",
                table: "UserLock",
                column: "InitiatingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationItem_ValidationTextId",
                table: "ValidationItem",
                column: "ValidationTextId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationItem_ValidationTypeId",
                table: "ValidationItem",
                column: "ValidationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationItemValue_ValidationItemId",
                table: "ValidationItemValue",
                column: "ValidationItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationListValidationItem_ValidationItemId",
                table: "ValidationListValidationItem",
                column: "ValidationItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationListValidationItem_ValidationListId",
                table: "ValidationListValidationItem",
                column: "ValidationListId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationText_ErrorMessageId",
                table: "ValidationText",
                column: "ErrorMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationText_SuccessMessageId",
                table: "ValidationText",
                column: "SuccessMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationText_ValidationItemId",
                table: "ValidationText",
                column: "ValidationItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WebPortalLogin_ApplicationUserLoginDetailId",
                table: "WebPortalLogin",
                column: "ApplicationUserLoginDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_WebPortalRoleRolesApplicationUserApplicationUser_ApplicationUsersId",
                table: "WebPortalRoleRolesApplicationUserApplicationUser",
                column: "ApplicationUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_WebPortalRoleRolesApplicationUserApplicationUser_RolesId",
                table: "WebPortalRoleRolesApplicationUserApplicationUser",
                column: "RolesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationException_Device",
                schema: "exp",
                table: "ApplicationException",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationLog_DepositorSession",
                table: "ApplicationLog",
                column: "SessionId",
                principalTable: "DepositorSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationLog_Device",
                table: "ApplicationLog",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_ApplicationUserLoginDetail",
                table: "ApplicationUser",
                column: "ApplicationUserLoginDetailId",
                principalTable: "ApplicationUserLoginDetail",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CIT_ApplicationUser_AuthUser",
                table: "ApplicationUser",
                column: "AuthUserId",
                principalTable: "CIT",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EscrowJam_AppUser_Approver",
                table: "ApplicationUser",
                column: "AuthorisinguserId",
                principalSchema: "exp",
                principalTable: "EscrowJam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EscrowJam_AppUser_Initiator",
                table: "ApplicationUser",
                column: "InitialisinguserId",
                principalSchema: "exp",
                principalTable: "EscrowJam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserLoginDetail_LastLoginLogEntry",
                table: "ApplicationUserLoginDetail",
                column: "LastLoginLogEntryId",
                principalTable: "WebPortalLogin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CIT_DeviceList",
                table: "CIT",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CrashEvent_Device",
                schema: "exp",
                table: "CrashEvent",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DenominationDetail_Transaction",
                table: "DenominationDetail",
                column: "TxId",
                principalTable: "Transaction",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositorSession_DeviceList",
                table: "DepositorSession",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceList_GUIScreenList",
                table: "Device",
                column: "GuiScreenListId",
                principalTable: "GuiScreenList",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EscrowJam_Transaction",
                schema: "exp",
                table: "EscrowJam",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GUIScreen_GUIScreenText",
                table: "GuiScreen",
                column: "GuiScreenTextId",
                principalTable: "GuiScreenText",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuiScreenList_Screen_GUIScreenList",
                table: "GuiScreenList",
                column: "GuiScreenListId",
                principalTable: "GuiScreenListScreen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Printout_Transaction",
                table: "Printout",
                column: "TxId",
                principalTable: "Transaction",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_TransactionTypeListItem",
                table: "Transaction",
                column: "TxType",
                principalTable: "TransactionTypeListItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionText_TransactionTypeListItem",
                table: "TransactionText",
                column: "TxItemId",
                principalTable: "TransactionTypeListItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ValidationItem_ValidationText",
                table: "ValidationItem",
                column: "ValidationTextId",
                principalTable: "ValidationText",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUser_Role",
                table: "ApplicationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CIT_DeviceList",
                table: "CIT");

            migrationBuilder.DropForeignKey(
                name: "FK_DepositorSession_DeviceList",
                table: "DepositorSession");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_DeviceList",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_DepositorSession",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUser_ApplicationUserLoginDetail",
                table: "ApplicationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_WebPortalLogin_ApplicationUserLoginDetail",
                table: "WebPortalLogin");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUser_UserGroup",
                table: "ApplicationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CIT_ApplicationUser_AuthUser",
                table: "ApplicationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_CIT",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_EscrowJam_AppUser_Approver",
                table: "ApplicationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_EscrowJam_AppUser_Initiator",
                table: "ApplicationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionTypeListItem_Currency",
                table: "TransactionTypeListItem");

            migrationBuilder.DropForeignKey(
                name: "FK_GuiScreenListScreen_GuiScreenList_GuiScreenListId",
                table: "GuiScreenListScreen");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionTypeListItem_GUIScreenList",
                table: "TransactionTypeListItem");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreenText_BtnAcceptCaption",
                table: "GuiScreenText");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreenText_BtnBackCaption",
                table: "GuiScreenText");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreenText_BtnCancelCaption",
                table: "GuiScreenText");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreenText_FullInstructions",
                table: "GuiScreenText");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreenText_ScreenTitle",
                table: "GuiScreenText");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreenText_ScreenTitleInstruction",
                table: "GuiScreenText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_Account_Name_Caption",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_Account_Number_Caption",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_Alias_Account_Name_Caption",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_Alias_Account_Number_Caption",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_Depositor_Name_Caption",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_Disclaimers",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_full_instructions",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_Funds_Source_Caption",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_IdNumberCaption",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_ListItemCaption",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_NarrationCaption",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_PhoneNumberCaption",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_ReceiptTemplate",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_ReferenceAccountNameCaption",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_ReferenceAccountNumberCaption",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_Terms",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_ValidationText_ErrorMessage",
                table: "ValidationText");

            migrationBuilder.DropForeignKey(
                name: "FK_ValidationText_SuccessMessage",
                table: "ValidationText");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreen_GUIScreenText",
                table: "GuiScreen");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionText_TransactionTypeListItem",
                table: "TransactionText");

            migrationBuilder.DropForeignKey(
                name: "FK_ValidationItem_ValidationText",
                table: "ValidationItem");

            migrationBuilder.DropTable(
                name: "AlertAttachmentType");

            migrationBuilder.DropTable(
                name: "AlertEmail");

            migrationBuilder.DropTable(
                name: "AlertEmailAttachment");

            migrationBuilder.DropTable(
                name: "AlertEmailResult");

            migrationBuilder.DropTable(
                name: "AlertMessageRegistry");

            migrationBuilder.DropTable(
                name: "AlertSMS");

            migrationBuilder.DropTable(
                name: "ApplicationException",
                schema: "exp");

            migrationBuilder.DropTable(
                name: "ApplicationLog");

            migrationBuilder.DropTable(
                name: "ApplicationUserChangePassword");

            migrationBuilder.DropTable(
                name: "CashmereCommunicationServiceStatus");

            migrationBuilder.DropTable(
                name: "CITDenomination");

            migrationBuilder.DropTable(
                name: "CITPrintout");

            migrationBuilder.DropTable(
                name: "CITTransaction");

            migrationBuilder.DropTable(
                name: "CrashEvent",
                schema: "exp");

            migrationBuilder.DropTable(
                name: "CurrencyListCurrency");

            migrationBuilder.DropTable(
                name: "DashboardDatum");

            migrationBuilder.DropTable(
                name: "DenominationDetail");

            migrationBuilder.DropTable(
                name: "DenominationView");

            migrationBuilder.DropTable(
                name: "DeviceCITSuspenseAccount");

            migrationBuilder.DropTable(
                name: "DeviceConfig");

            migrationBuilder.DropTable(
                name: "DeviceLock");

            migrationBuilder.DropTable(
                name: "DeviceLogin");

            migrationBuilder.DropTable(
                name: "DevicePrinter");

            migrationBuilder.DropTable(
                name: "DeviceStatus");

            migrationBuilder.DropTable(
                name: "DeviceSuspenseAccount");

            migrationBuilder.DropTable(
                name: "GuiPrepopListItem");

            migrationBuilder.DropTable(
                name: "LanguageListLanguage");

            migrationBuilder.DropTable(
                name: "ModelDifferenceAspect");

            migrationBuilder.DropTable(
                name: "PasswordHistory");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "PermissionPolicyMemberPermissionsObject");

            migrationBuilder.DropTable(
                name: "PermissionPolicyNavigationPermissionsObject");

            migrationBuilder.DropTable(
                name: "PermissionPolicyObjectPermissionsObject");

            migrationBuilder.DropTable(
                name: "PingRequest",
                schema: "cb");

            migrationBuilder.DropTable(
                name: "PrinterStatus");

            migrationBuilder.DropTable(
                name: "Printout");

            migrationBuilder.DropTable(
                name: "SessionException",
                schema: "exp");

            migrationBuilder.DropTable(
                name: "SysTextTranslation",
                schema: "xlns");

            migrationBuilder.DropTable(
                name: "TextTranslation");

            migrationBuilder.DropTable(
                name: "ThisDevice");

            migrationBuilder.DropTable(
                name: "TransactionException",
                schema: "exp");

            migrationBuilder.DropTable(
                name: "TransactionLimitListItem");

            migrationBuilder.DropTable(
                name: "TransactionPosting");

            migrationBuilder.DropTable(
                name: "TransactionTypeListTransactionTypeListItem");

            migrationBuilder.DropTable(
                name: "TransactionView");

            migrationBuilder.DropTable(
                name: "UptimeComponentState");

            migrationBuilder.DropTable(
                name: "UptimeMode");

            migrationBuilder.DropTable(
                name: "UserLock");

            migrationBuilder.DropTable(
                name: "ValidationItemValue");

            migrationBuilder.DropTable(
                name: "ValidationListValidationItem");

            migrationBuilder.DropTable(
                name: "ViewConfig");

            migrationBuilder.DropTable(
                name: "ViewPermission");

            migrationBuilder.DropTable(
                name: "WebPortalRoleRolesApplicationUserApplicationUser");

            migrationBuilder.DropTable(
                name: "WebUserLoginCount");

            migrationBuilder.DropTable(
                name: "WebUserPasswordHistory");

            migrationBuilder.DropTable(
                name: "AlertMessageType");

            migrationBuilder.DropTable(
                name: "AlertEvent");

            migrationBuilder.DropTable(
                name: "PasswordPolicy");

            migrationBuilder.DropTable(
                name: "Config");

            migrationBuilder.DropTable(
                name: "GuiPrepopItem");

            migrationBuilder.DropTable(
                name: "ModelDifference");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "PermissionPolicyTypePermissionsObject");

            migrationBuilder.DropTable(
                name: "SysTextItem",
                schema: "xlns");

            migrationBuilder.DropTable(
                name: "WebPortalRole");

            migrationBuilder.DropTable(
                name: "ConfigCategory");

            migrationBuilder.DropTable(
                name: "SysTextItemCategory",
                schema: "xlns");

            migrationBuilder.DropTable(
                name: "SysTextItemType",
                schema: "xlns");

            migrationBuilder.DropTable(
                name: "PermissionPolicyRole");

            migrationBuilder.DropTable(
                name: "XpobjectType");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "LanguageList");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "ConfigGroup");

            migrationBuilder.DropTable(
                name: "CurrencyList");

            migrationBuilder.DropTable(
                name: "DeviceType");

            migrationBuilder.DropTable(
                name: "TransactionTypeList");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "DepositorSession");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "ApplicationUserLoginDetail");

            migrationBuilder.DropTable(
                name: "WebPortalLogin");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "CIT");

            migrationBuilder.DropTable(
                name: "EscrowJam",
                schema: "exp");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "GuiScreenList");

            migrationBuilder.DropTable(
                name: "GuiScreenListScreen");

            migrationBuilder.DropTable(
                name: "GuiPrepopList");

            migrationBuilder.DropTable(
                name: "ValidationList");

            migrationBuilder.DropTable(
                name: "TextItem");

            migrationBuilder.DropTable(
                name: "TextItemCategory");

            migrationBuilder.DropTable(
                name: "TextItemType");

            migrationBuilder.DropTable(
                name: "GuiScreenText");

            migrationBuilder.DropTable(
                name: "GuiScreen");

            migrationBuilder.DropTable(
                name: "GuiScreenType");

            migrationBuilder.DropTable(
                name: "TransactionTypeListItem");

            migrationBuilder.DropTable(
                name: "TransactionType");

            migrationBuilder.DropTable(
                name: "TransactionLimitList");

            migrationBuilder.DropTable(
                name: "TransactionText");

            migrationBuilder.DropTable(
                name: "ValidationText");

            migrationBuilder.DropTable(
                name: "ValidationItem");

            migrationBuilder.DropTable(
                name: "ValidationType");
        }
    }
}
