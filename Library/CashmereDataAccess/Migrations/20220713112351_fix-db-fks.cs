using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cashmere.Library.CashmereDataAccess.Migrations
{
    public partial class fixdbfks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "exp");

            migrationBuilder.EnsureSchema(
                name: "xlns");

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AlertAttachmentType",
                columns: table => new
                {
                    code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    alert_type_id = table.Column<int>(type: "int", nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    mime_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    mime_subtype = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertAttachmentType", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "AlertEmailAttachment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    alert_email_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    type = table.Column<string>(type: "nchar(6)", fixedLength: true, maxLength: 6, nullable: false),
                    data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    hash = table.Column<byte[]>(type: "binary(64)", fixedLength: true, maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertEmailAttachment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AlertEvent",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    alert_type_id = table.Column<int>(type: "int", nullable: false),
                    date_detected = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_resolved = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_resolved = table.Column<bool>(type: "bit", nullable: false),
                    is_processed = table.Column<bool>(type: "bit", nullable: false),
                    alert_event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    is_processing = table.Column<bool>(type: "bit", nullable: false),
                    machine_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertEvent", x => x.id);
                    table.ForeignKey(
                        name: "FK_AlertEmailEvent_AlertEmailEvent",
                        column: x => x.alert_event_id,
                        principalTable: "AlertEvent",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AlertMessageType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    email_content_template = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    raw_email_content_template = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone_content_template = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertMessageType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationException",
                schema: "exp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    code = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    level = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    additional_info = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    stack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    machine_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationException", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationLog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    session_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "The session this log entry belongs to"),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    log_date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Datetime the system deems for the log entry."),
                    event_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The name of the log event"),
                    event_detail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, comment: "the details of the log message"),
                    event_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "the type of the log event used for grouping and sorting"),
                    component = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Which internal component produced the log entry e.g. GUI, APIs, DeviceController etc"),
                    log_level = table.Column<int>(type: "int", nullable: false, comment: "the LogLevel"),
                    machine_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationLog", x => x.id);
                },
                comment: "Stores the general application log that the GUI and other local systems write to");

            migrationBuilder.CreateTable(
                name: "ConfigCategory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigCategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigGroup",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    parent_group = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigGroup", x => x.id);
                    table.ForeignKey(
                        name: "FK_ConfigGroup_ConfigGroup",
                        column: x => x.parent_group,
                        principalTable: "ConfigGroup",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    country_code = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false, defaultValueSql: "('')"),
                    country_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.country_code);
                });

            migrationBuilder.CreateTable(
                name: "CrashEvent",
                schema: "exp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_detected = table.Column<DateTime>(type: "datetime2", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    machine_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrashEvent", x => x.id);
                },
                comment: "contains a crash report");

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    code = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    minor = table.Column<int>(type: "int", nullable: false),
                    flag = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    ISO_3_Numeric_Code = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "DeviceStatus",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    controller_state = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ba_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ba_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ba_currency = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    bag_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    bag_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    bag_note_level = table.Column<int>(type: "int", nullable: false),
                    bag_note_capacity = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    bag_value_level = table.Column<long>(type: "bigint", nullable: true),
                    bag_value_capacity = table.Column<long>(type: "bigint", nullable: true),
                    bag_percent_full = table.Column<int>(type: "int", nullable: false),
                    sensors_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    sensors_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    sensors_value = table.Column<int>(type: "int", nullable: false),
                    sensors_door = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    sensors_bag = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    escrow_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    escrow_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    escrow_position = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    transaction_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    transaction_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    machine_datetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    current_status = table.Column<int>(type: "int", nullable: false),
                    modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    machine_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceStatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    note_in = table.Column<bool>(type: "bit", nullable: false),
                    note_out = table.Column<bool>(type: "bit", nullable: false),
                    note_escrow = table.Column<bool>(type: "bit", nullable: false),
                    coin_in = table.Column<bool>(type: "bit", nullable: false),
                    coin_out = table.Column<bool>(type: "bit", nullable: false),
                    coin_escrow = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GUIPrepopList",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    AllowFreeText = table.Column<bool>(type: "bit", nullable: false),
                    DefaultIndex = table.Column<int>(type: "int", nullable: false),
                    UseDefault = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GUIPrepopList", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GUIScreenList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GUIScreenList", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GUIScreenType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GUIScreenType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    code = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    flag = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "PasswordPolicy",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    min_length = table.Column<int>(type: "int", nullable: false),
                    min_lowercase = table.Column<int>(type: "int", nullable: false),
                    min_digits = table.Column<int>(type: "int", nullable: false),
                    min_uppercase = table.Column<int>(type: "int", nullable: false),
                    min_special = table.Column<int>(type: "int", nullable: false),
                    allowed_special = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    expiry_days = table.Column<int>(type: "int", nullable: false),
                    history_size = table.Column<int>(type: "int", nullable: false),
                    use_history = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordPolicy", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PrinterStatus",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    printer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_error = table.Column<bool>(type: "bit", nullable: false),
                    has_paper = table.Column<bool>(type: "bit", nullable: false),
                    cover_open = table.Column<bool>(type: "bit", nullable: false),
                    error_code = table.Column<int>(type: "int", nullable: false),
                    error_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    error_message = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    modified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    machine_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrinterStatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SessionException",
                schema: "exp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    session_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    level = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    additional_info = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    stack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    machine_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionException", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SysTextItemCategory",
                schema: "xlns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Parent = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysTextItemCategory", x => x.id);
                    table.ForeignKey(
                        name: "FK_TextItemCategory_TextItemCategory",
                        column: x => x.Parent,
                        principalSchema: "xlns",
                        principalTable: "SysTextItemCategory",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SysTextItemType",
                schema: "xlns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysTextItemType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TextItemCategory",
                schema: "xlns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Parent = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextItemCategory", x => x.id);
                    table.ForeignKey(
                        name: "FK_UI_TextItemCategory_TextItemCategory",
                        column: x => x.Parent,
                        principalSchema: "xlns",
                        principalTable: "TextItemCategory",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TextItemType",
                schema: "xlns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextItemType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ThisDevice",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    device_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    device_location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    machine_name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    type_id = table.Column<int>(type: "int", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    config_group = table.Column<int>(type: "int", nullable: false),
                    user_group = table.Column<int>(type: "int", nullable: true),
                    GUIScreen_list = table.Column<int>(type: "int", nullable: false),
                    language_list = table.Column<int>(type: "int", nullable: true),
                    currency_list = table.Column<int>(type: "int", nullable: false),
                    transaction_type_list = table.Column<int>(type: "int", nullable: false),
                    login_cycles = table.Column<int>(type: "int", nullable: false),
                    login_attempts = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TransactionException",
                schema: "exp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    transaction_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<int>(type: "int", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false),
                    additional_info = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    machine_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionException", x => x.id);
                },
                comment: "Exceptions encountered during execution");

            migrationBuilder.CreateTable(
                name: "TransactionLimitList",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLimitList", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionType",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypeList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypeList", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UptimeComponentState",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    device = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    component_state = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UptimeComponentState", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UptimeMode",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    device = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    device_mode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UptimeMode", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    parent_group = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserGroup_UserGroup",
                        column: x => x.parent_group,
                        principalTable: "UserGroup",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ValidationList",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    category = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationList", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ValidationType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AlertEmail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    from = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    subject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    html_message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    raw_text_message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sent = table.Column<bool>(type: "bit", nullable: false),
                    send_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    alert_event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    send_error = table.Column<bool>(type: "bit", nullable: false),
                    send_error_message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertEmail", x => x.id);
                    table.ForeignKey(
                        name: "FK_AlertEmail_AlertEmailEvent",
                        column: x => x.alert_event_id,
                        principalTable: "AlertEvent",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AlertSMS",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    from = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sent = table.Column<bool>(type: "bit", nullable: false),
                    send_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    alert_event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    send_error = table.Column<bool>(type: "bit", nullable: false),
                    send_error_message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertSMS", x => x.id);
                    table.ForeignKey(
                        name: "FK_AlertSMS_AlertEvent",
                        column: x => x.alert_event_id,
                        principalTable: "AlertEvent",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Config",
                columns: table => new
                {
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    default_value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Config", x => x.name);
                    table.ForeignKey(
                        name: "FK_Config_ConfigCategory",
                        column: x => x.category_id,
                        principalTable: "ConfigCategory",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    bank_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    country_code = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bank_Country",
                        column: x => x.country_code,
                        principalTable: "Country",
                        principalColumn: "country_code");
                });

            migrationBuilder.CreateTable(
                name: "CurrencyList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    default_currency = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyList", x => x.id);
                    table.ForeignKey(
                        name: "FK_CurrencyList_Currency",
                        column: x => x.default_currency,
                        principalTable: "Currency",
                        principalColumn: "code");
                });

            migrationBuilder.CreateTable(
                name: "LanguageList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    default_language = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageList", x => x.id);
                    table.ForeignKey(
                        name: "FK_LanguageList_Language",
                        column: x => x.default_language,
                        principalTable: "Language",
                        principalColumn: "code");
                });

            migrationBuilder.CreateTable(
                name: "AlertMessageRegistry",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    alert_type_id = table.Column<int>(type: "int", nullable: false),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email_enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    phone_enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertMessageRegistry", x => x.id);
                    table.ForeignKey(
                        name: "FK_AlertMessageRegistry_AlertMessageType",
                        column: x => x.alert_type_id,
                        principalTable: "AlertMessageType",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_AlertMessageRegistry_Role",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    activity_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    standalone_allowed = table.Column<bool>(type: "bit", nullable: false),
                    standalone_authentication_required = table.Column<bool>(type: "bit", nullable: false),
                    standalone_can_Authenticate = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.id);
                    table.ForeignKey(
                        name: "FK_Permission_Activity",
                        column: x => x.activity_id,
                        principalTable: "Activity",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Permission_Role",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SysTextItem",
                schema: "xlns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DefaultTranslation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextItemTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysTextItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_SysTextItem_SysTextItemCategory",
                        column: x => x.Category,
                        principalSchema: "xlns",
                        principalTable: "SysTextItemCategory",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_sysTextItem_sysTextItemType",
                        column: x => x.TextItemTypeID,
                        principalSchema: "xlns",
                        principalTable: "SysTextItemType",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TextItem",
                schema: "xlns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DefaultTranslation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextItemTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_UI_TextItem_TextItemCategory",
                        column: x => x.Category,
                        principalSchema: "xlns",
                        principalTable: "TextItemCategory",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_UI_TextItem_TextItemType",
                        column: x => x.TextItemTypeID,
                        principalSchema: "xlns",
                        principalTable: "TextItemType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionLimitListItem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    transactionitemlist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    currency_code = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    show_funds_source = table.Column<bool>(type: "bit", nullable: false),
                    show_funds_form = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    funds_source_amount = table.Column<long>(type: "bigint", nullable: false),
                    prevent_overdeposit = table.Column<bool>(type: "bit", nullable: false),
                    overdeposit_amount = table.Column<long>(type: "bigint", nullable: false),
                    prevent_underdeposit = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    underdeposit_amount = table.Column<long>(type: "bigint", nullable: false),
                    prevent_overcount = table.Column<bool>(type: "bit", nullable: false),
                    overcount_amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLimitListItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_TransactionLimitListItem_Currency",
                        column: x => x.currency_code,
                        principalTable: "Currency",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK_TransactionLimitListItem_TransactionLimitList",
                        column: x => x.transactionitemlist_id,
                        principalTable: "TransactionLimitList",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "char(71)", unicode: false, fixedLength: true, maxLength: 71, nullable: false),
                    fname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    lname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    email_enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phone_enabled = table.Column<bool>(type: "bit", nullable: false),
                    password_reset_required = table.Column<bool>(type: "bit", nullable: false),
                    login_attempts = table.Column<int>(type: "int", nullable: false),
                    user_group = table.Column<int>(type: "int", nullable: false),
                    depositor_enabled = table.Column<bool>(type: "bit", nullable: true),
                    UserDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ApplicationUserLoginDetail = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    is_ad_user = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.id);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_Role",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ApplicationUser_UserGroup",
                        column: x => x.user_group,
                        principalTable: "UserGroup",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DeviceConfig",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    group_id = table.Column<int>(type: "int", nullable: false),
                    config_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    config_value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceConfig", x => x.id);
                    table.ForeignKey(
                        name: "FK_DeviceConfig_Config",
                        column: x => x.config_id,
                        principalTable: "Config",
                        principalColumn: "name");
                    table.ForeignKey(
                        name: "FK_DeviceConfig_ConfigGroup",
                        column: x => x.group_id,
                        principalTable: "ConfigGroup",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    branch_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    bank_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.id);
                    table.ForeignKey(
                        name: "FK_Branch_Bank",
                        column: x => x.bank_id,
                        principalTable: "Bank",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CurrencyListCurrency",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    currency_list = table.Column<int>(type: "int", nullable: false),
                    currency_item = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    currency_order = table.Column<int>(type: "int", nullable: false),
                    max_value = table.Column<long>(type: "bigint", nullable: false),
                    max_count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyListCurrency", x => x.id);
                    table.ForeignKey(
                        name: "FK_Currency_CurrencyList_Currency",
                        column: x => x.currency_item,
                        principalTable: "Currency",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK_Currency_CurrencyList_CurrencyList",
                        column: x => x.currency_list,
                        principalTable: "CurrencyList",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LanguageListLanguage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    language_list = table.Column<int>(type: "int", nullable: false),
                    language_item = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    language_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageListLanguage", x => x.id);
                    table.ForeignKey(
                        name: "FK_LanguageList_Language_Language",
                        column: x => x.language_item,
                        principalTable: "Language",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK_LanguageList_Language_LanguageList",
                        column: x => x.language_list,
                        principalTable: "LanguageList",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SysTextTranslation",
                schema: "xlns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    SysTextItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageCode = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    TranslationSysText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysTextTranslation", x => x.id);
                    table.ForeignKey(
                        name: "FK_sysTextTranslation_Language",
                        column: x => x.LanguageCode,
                        principalTable: "Language",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK_sysTextTranslation_sysTextItem",
                        column: x => x.SysTextItemID,
                        principalSchema: "xlns",
                        principalTable: "SysTextItem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GUIPrepopItem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    value = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GUIPrepopItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_GUIPrepopItem_TextItem",
                        column: x => x.value,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TextTranslation",
                schema: "xlns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    TextItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageCode = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    TranslationText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextTranslation", x => x.id);
                    table.ForeignKey(
                        name: "FK_UI_Translation_Language",
                        column: x => x.LanguageCode,
                        principalTable: "Language",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK_UI_Translation_TextItem",
                        column: x => x.TextItemID,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordHistory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    LogDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    User = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(71)", maxLength: 71, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordHistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_PasswordHistory_User",
                        column: x => x.User,
                        principalTable: "ApplicationUser",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserLock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    LogDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ApplicationUserLoginDetail = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LockType = table.Column<int>(type: "int", nullable: true),
                    WebPortalInitiated = table.Column<bool>(type: "bit", nullable: true),
                    InitiatingUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLock", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserLock_InitiatingUser",
                        column: x => x.InitiatingUser,
                        principalTable: "ApplicationUser",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    device_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    device_location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    machine_name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    type_id = table.Column<int>(type: "int", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    config_group = table.Column<int>(type: "int", nullable: false),
                    user_group = table.Column<int>(type: "int", nullable: true),
                    GUIScreen_list = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    language_list = table.Column<int>(type: "int", nullable: true),
                    currency_list = table.Column<int>(type: "int", nullable: false),
                    transaction_type_list = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    login_cycles = table.Column<int>(type: "int", nullable: false),
                    login_attempts = table.Column<int>(type: "int", nullable: false),
                    mac_address = table.Column<string>(type: "char(17)", unicode: false, fixedLength: true, maxLength: 17, nullable: true),
                    app_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    app_key = table.Column<byte[]>(type: "binary(32)", fixedLength: true, maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.id);
                    table.ForeignKey(
                        name: "FK_Device_LanguageList",
                        column: x => x.language_list,
                        principalTable: "LanguageList",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DeviceList_Branch",
                        column: x => x.branch_id,
                        principalTable: "Branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DeviceList_ConfigGroup",
                        column: x => x.config_group,
                        principalTable: "ConfigGroup",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DeviceList_CurrencyList",
                        column: x => x.currency_list,
                        principalTable: "CurrencyList",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DeviceList_DeviceType",
                        column: x => x.type_id,
                        principalTable: "DeviceType",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DeviceList_GUIScreenList",
                        column: x => x.GUIScreen_list,
                        principalTable: "GUIScreenList",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DeviceList_TransactionTypeList",
                        column: x => x.transaction_type_list,
                        principalTable: "TransactionTypeList",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DeviceList_UserGroup",
                        column: x => x.user_group,
                        principalTable: "UserGroup",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "GUIPrepopListItem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    List = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Item = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    List_Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GUIPrepopListItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_GUIPrepopList_Item_GUIPrepopItem",
                        column: x => x.Item,
                        principalTable: "GUIPrepopItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_GUIPrepopList_Item_GUIPrepopList",
                        column: x => x.List,
                        principalTable: "GUIPrepopList",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CIT",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cit_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    cit_complete_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    start_user = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    auth_user = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    fromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    toDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    old_bag_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    new_bag_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    seal_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    complete = table.Column<bool>(type: "bit", nullable: false),
                    cit_error = table.Column<int>(type: "int", nullable: false),
                    cit_error_message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CIT", x => x.id);
                    table.ForeignKey(
                        name: "FK_CIT_ApplicationUser_AuthUser",
                        column: x => x.auth_user,
                        principalTable: "ApplicationUser",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CIT_ApplicationUser_StartUser",
                        column: x => x.start_user,
                        principalTable: "ApplicationUser",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CIT_DeviceList",
                        column: x => x.device_id,
                        principalTable: "Device",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DepositorSession",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    session_start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    session_end = table.Column<DateTime>(type: "datetime2", nullable: true),
                    language_code = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: true),
                    complete = table.Column<bool>(type: "bit", nullable: false),
                    complete_success = table.Column<bool>(type: "bit", nullable: false),
                    error_code = table.Column<int>(type: "int", nullable: true),
                    error_message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    terms_accepted = table.Column<bool>(type: "bit", nullable: false),
                    account_verified = table.Column<bool>(type: "bit", nullable: false),
                    reference_account_verified = table.Column<bool>(type: "bit", nullable: false),
                    salt = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositorSession", x => x.id);
                    table.ForeignKey(
                        name: "FK_DepositorSession_DeviceList",
                        column: x => x.device_id,
                        principalTable: "Device",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DepositorSession_Language",
                        column: x => x.language_code,
                        principalTable: "Language",
                        principalColumn: "code");
                });

            migrationBuilder.CreateTable(
                name: "DeviceCITSuspenseAccount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    currency_code = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    account_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    account_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    account = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCITSuspenseAccount", x => x.id);
                    table.ForeignKey(
                        name: "FK_DeviceCITSuspenseAccount_Currency",
                        column: x => x.currency_code,
                        principalTable: "Currency",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK_DeviceCITSuspenseAccount_Device",
                        column: x => x.device_id,
                        principalTable: "Device",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DeviceLock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lock_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    locked = table.Column<bool>(type: "bit", nullable: false),
                    locking_user = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    web_locking_user = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    locked_by_device = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceLock", x => x.id);
                    table.ForeignKey(
                        name: "FK_DeviceLock_ApplicationUser_locking_user",
                        column: x => x.locking_user,
                        principalTable: "ApplicationUser",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DeviceLock_Device",
                        column: x => x.device_id,
                        principalTable: "Device",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DeviceLogin",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    LoginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogoutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    User = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: true),
                    DepositorEnabled = table.Column<bool>(type: "bit", nullable: true),
                    ChangePassword = table.Column<bool>(type: "bit", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ForcedLogout = table.Column<bool>(type: "bit", nullable: true),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceLogin", x => x.id);
                    table.ForeignKey(
                        name: "FK_DeviceLogin_ApplicationUser",
                        column: x => x.User,
                        principalTable: "ApplicationUser",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DeviceLogin_Device",
                        column: x => x.device_id,
                        principalTable: "Device",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DevicePrinter",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    is_infront = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    port = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    make = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    serial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevicePrinter", x => x.id);
                    table.ForeignKey(
                        name: "FK_DevicePrinter_DeviceList",
                        column: x => x.device_id,
                        principalTable: "Device",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DeviceSuspenseAccount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    currency_code = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    account_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    account_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    account = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceSuspenseAccount", x => x.id);
                    table.ForeignKey(
                        name: "FK_DeviceSuspenseAccount_Currency",
                        column: x => x.currency_code,
                        principalTable: "Currency",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK_DeviceSuspenseAccount_DeviceList",
                        column: x => x.device_id,
                        principalTable: "Device",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CITDenomination",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    cit_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    datetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    currency_id = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    denom = table.Column<int>(type: "int", nullable: false),
                    count = table.Column<long>(type: "bigint", nullable: false),
                    subtotal = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITDenomination", x => x.id);
                    table.ForeignKey(
                        name: "FK_CITDenominations_CIT",
                        column: x => x.cit_id,
                        principalTable: "CIT",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CITDenominations_Currency",
                        column: x => x.currency_id,
                        principalTable: "Currency",
                        principalColumn: "code");
                });

            migrationBuilder.CreateTable(
                name: "CITPrintout",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    cit_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    print_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    print_content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_copy = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITPrintout", x => x.id);
                    table.ForeignKey(
                        name: "FK_CITPrintout_CIT",
                        column: x => x.cit_id,
                        principalTable: "CIT",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CITTransaction",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cit_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    currency = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    suspense_account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    account_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    narration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cb_tx_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cb_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    cb_tx_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cb_status_detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    error_code = table.Column<int>(type: "int", nullable: false),
                    error_message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITTransaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_CITTransaction_CIT",
                        column: x => x.cit_id,
                        principalTable: "CIT",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DenominationDetail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    tx_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    denom = table.Column<int>(type: "int", nullable: false),
                    count = table.Column<long>(type: "bigint", nullable: false),
                    subtotal = table.Column<long>(type: "bigint", nullable: false),
                    datetime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DenominationDetail", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "EscrowJam",
                schema: "exp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    transaction_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date_detected = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dropped_amount = table.Column<long>(type: "bigint", nullable: false),
                    escrow_amount = table.Column<long>(type: "bigint", nullable: false),
                    posted_amount = table.Column<long>(type: "bigint", nullable: false),
                    retreived_amount = table.Column<long>(type: "bigint", nullable: false),
                    recovery_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    initialising_user = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    authorising_user = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    additional_info = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscrowJam", x => x.id);
                    table.ForeignKey(
                        name: "FK_EscrowJam_AppUser_Approver",
                        column: x => x.authorising_user,
                        principalTable: "ApplicationUser",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_EscrowJam_AppUser_Initiator",
                        column: x => x.initialising_user,
                        principalTable: "ApplicationUser",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "GUIScreen",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    type = table.Column<int>(type: "int", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    keyboard = table.Column<int>(type: "int", nullable: true),
                    is_masked = table.Column<bool>(type: "bit", nullable: true),
                    prefill_text = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    input_mask = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    gui_text = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GUIScreen", x => x.id);
                    table.ForeignKey(
                        name: "FK_GUIScreen_GUIScreenType",
                        column: x => x.type,
                        principalTable: "GUIScreenType",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "GuiScreenListScreen",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    screen = table.Column<int>(type: "int", nullable: false),
                    gui_screen_list = table.Column<int>(type: "int", nullable: false),
                    screen_order = table.Column<int>(type: "int", nullable: false),
                    required = table.Column<bool>(type: "bit", nullable: false),
                    validation_list_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    guiprepoplist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuiScreenListScreen", x => x.id);
                    table.ForeignKey(
                        name: "FK_GuiScreenList_Screen_GUIPrepopList",
                        column: x => x.guiprepoplist_id,
                        principalTable: "GUIPrepopList",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_GuiScreenList_Screen_GUIScreen",
                        column: x => x.screen,
                        principalTable: "GUIScreen",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_GuiScreenList_Screen_GUIScreenList",
                        column: x => x.gui_screen_list,
                        principalTable: "GUIScreenList",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_GuiScreenList_Screen_ValidationList",
                        column: x => x.validation_list_id,
                        principalTable: "ValidationList",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "GUIScreenText",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    guiscreen_id = table.Column<int>(type: "int", nullable: false),
                    screen_title = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    screen_title_instruction = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    full_instructions = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    btn_accept_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    btn_back_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    btn_cancel_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GUIScreenText", x => x.id);
                    table.ForeignKey(
                        name: "FK_GUIScreenText_btn_accept_caption",
                        column: x => x.btn_accept_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_GUIScreenText_btn_back_caption",
                        column: x => x.btn_back_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_GUIScreenText_btn_cancel_caption",
                        column: x => x.btn_cancel_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_GUIScreenText_full_instructions",
                        column: x => x.full_instructions,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_GUIScreenText_GUIScreen",
                        column: x => x.guiscreen_id,
                        principalTable: "GUIScreen",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_GUIScreenText_screen_title",
                        column: x => x.screen_title,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_GUIScreenText_screen_title_instruction",
                        column: x => x.screen_title_instruction,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Printout",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    datetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    tx_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    print_guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    print_content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_copy = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printout", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    tx_type = table.Column<int>(type: "int", nullable: true),
                    session_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tx_random_number = table.Column<int>(type: "int", nullable: true),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tx_start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tx_end_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    tx_completed = table.Column<bool>(type: "bit", nullable: false),
                    tx_currency = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    tx_amount = table.Column<long>(type: "bigint", nullable: true),
                    tx_account_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cb_account_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    tx_ref_account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cb_ref_account_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    tx_narration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tx_depositor_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tx_id_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tx_phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    funds_source = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    tx_result = table.Column<int>(type: "int", nullable: false),
                    tx_error_code = table.Column<int>(type: "int", nullable: false),
                    tx_error_message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    cb_tx_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cb_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    cb_tx_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cb_status_detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    notes_rejected = table.Column<bool>(type: "bit", nullable: false),
                    jam_detected = table.Column<bool>(type: "bit", nullable: false),
                    cit_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    escrow_jam = table.Column<bool>(type: "bit", nullable: false),
                    tx_suspense_account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    init_user = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    auth_user = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transaction_auth_user",
                        column: x => x.auth_user,
                        principalTable: "ApplicationUser",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Transaction_CIT",
                        column: x => x.cit_id,
                        principalTable: "CIT",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Transaction_Currency_Transaction",
                        column: x => x.tx_currency,
                        principalTable: "Currency",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK_Transaction_DepositorSession",
                        column: x => x.session_id,
                        principalTable: "DepositorSession",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Transaction_DeviceList",
                        column: x => x.device_id,
                        principalTable: "Device",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Transaction_init_user",
                        column: x => x.init_user,
                        principalTable: "ApplicationUser",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TransactionText",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    tx_item = table.Column<int>(type: "int", nullable: false),
                    disclaimer = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    terms = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    full_instructions = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    listItem_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    account_number_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    account_name_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    reference_account_number_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    reference_account_name_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    narration_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    alias_account_number_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    alias_account_name_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    depositor_name_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    phone_number_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    id_number_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    receipt_template = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FundsSource_caption = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    validation_text_success_message = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    validation_text_error_message = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionText", x => x.id);
                    table.ForeignKey(
                        name: "FK_TransactionText_Account_Name_Caption",
                        column: x => x.account_name_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Account_Number_Caption",
                        column: x => x.account_number_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Alias_Account_Name_Caption",
                        column: x => x.alias_account_name_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Alias_Account_Number_Caption",
                        column: x => x.alias_account_number_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Depositor_Name_Caption",
                        column: x => x.depositor_name_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Disclaimers",
                        column: x => x.disclaimer,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_full_instructions",
                        column: x => x.full_instructions,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Funds_Source_Caption",
                        column: x => x.FundsSource_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_IdNumberCaption",
                        column: x => x.id_number_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_ListItemCaption",
                        column: x => x.listItem_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_NarrationCaption",
                        column: x => x.narration_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_PhoneNumberCaption",
                        column: x => x.phone_number_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_ReceiptTemplate",
                        column: x => x.receipt_template,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_ReferenceAccountNameCaption",
                        column: x => x.reference_account_name_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_ReferenceAccountNumberCaption",
                        column: x => x.reference_account_number_caption,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionText_Terms",
                        column: x => x.terms,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypeListItem",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    validate_reference_account = table.Column<bool>(type: "bit", nullable: false),
                    default_account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    default_account_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    default_account_currency = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false, defaultValueSql: "('KES')"),
                    validate_default_account = table.Column<bool>(type: "bit", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    tx_type = table.Column<int>(type: "int", nullable: false),
                    tx_type_guiscreenlist = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    cb_tx_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    tx_limit_list = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    tx_text = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    account_permission = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    init_user_required = table.Column<bool>(type: "bit", nullable: false),
                    auth_user_required = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypeListItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_TransactionListItem_TransactionType",
                        column: x => x.tx_type,
                        principalTable: "TransactionType",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionTypeListItem_Currency",
                        column: x => x.default_account_currency,
                        principalTable: "Currency",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK_TransactionTypeListItem_GUIScreenList",
                        column: x => x.tx_type_guiscreenlist,
                        principalTable: "GUIScreenList",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionTypeListItem_TransactionLimitList",
                        column: x => x.tx_limit_list,
                        principalTable: "TransactionLimitList",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionTypeListItem_TransactionText",
                        column: x => x.tx_text,
                        principalTable: "TransactionText",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypeListTransactionTypeListItem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    txtype_list_item = table.Column<int>(type: "int", nullable: false),
                    txtype_list = table.Column<int>(type: "int", nullable: false),
                    list_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypeListTransactionTypeListItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_TransactionTypeList_TransactionTypeListItem_TransactionTypeList",
                        column: x => x.txtype_list,
                        principalTable: "TransactionTypeList",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TransactionTypeList_TransactionTypeListItem_TransactionTypeListItem",
                        column: x => x.txtype_list_item,
                        principalTable: "TransactionTypeListItem",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ValidationItem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    category = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    type_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    error_code = table.Column<int>(type: "int", nullable: true),
                    validation_text_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_ValidationItem_ValidationType",
                        column: x => x.type_id,
                        principalTable: "ValidationType",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ValidationItemValue",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    validation_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationItemValue", x => x.id);
                    table.ForeignKey(
                        name: "FK_ValidationItemValue_ValidationItem",
                        column: x => x.validation_item_id,
                        principalTable: "ValidationItem",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ValidationListValidationItem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    validation_list_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    validation_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationListValidationItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_ValidationList_ValidationItem_ValidationItem",
                        column: x => x.validation_item_id,
                        principalTable: "ValidationItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ValidationList_ValidationItem_ValidationList",
                        column: x => x.validation_list_id,
                        principalTable: "ValidationList",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ValidationText",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    validation_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    error_message = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    success_message = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationText", x => x.id);
                    table.ForeignKey(
                        name: "FK_ValidationText_TextItem_error_message",
                        column: x => x.error_message,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ValidationText_TextItem_success_message",
                        column: x => x.success_message,
                        principalSchema: "xlns",
                        principalTable: "TextItem",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ValidationText_ValidationItem",
                        column: x => x.validation_item_id,
                        principalTable: "ValidationItem",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertEmail_alert_event_id",
                table: "AlertEmail",
                column: "alert_event_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvent_alert_event_id",
                table: "AlertEvent",
                column: "alert_event_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlertMessageRegistry_alert_type_id",
                table: "AlertMessageRegistry",
                column: "alert_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlertMessageRegistry_role_id",
                table: "AlertMessageRegistry",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_AlertSMS_alert_event_id",
                table: "AlertSMS",
                column: "alert_event_id");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_role_id",
                table: "ApplicationUser",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_user_group",
                table: "ApplicationUser",
                column: "user_group");

            migrationBuilder.CreateIndex(
                name: "IX_Bank_country_code",
                table: "Bank",
                column: "country_code");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_bank_id",
                table: "Branch",
                column: "bank_id");

            migrationBuilder.CreateIndex(
                name: "IX_CIT_auth_user",
                table: "CIT",
                column: "auth_user");

            migrationBuilder.CreateIndex(
                name: "IX_CIT_device_id",
                table: "CIT",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_CIT_start_user",
                table: "CIT",
                column: "start_user");

            migrationBuilder.CreateIndex(
                name: "IX_CITDenomination_cit_id",
                table: "CITDenomination",
                column: "cit_id");

            migrationBuilder.CreateIndex(
                name: "IX_CITDenomination_currency_id",
                table: "CITDenomination",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "IX_CITPrintout_cit_id",
                table: "CITPrintout",
                column: "cit_id");

            migrationBuilder.CreateIndex(
                name: "IX_CITTransaction_cit_id",
                table: "CITTransaction",
                column: "cit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Config_category_id",
                table: "Config",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigGroup_parent_group",
                table: "ConfigGroup",
                column: "parent_group");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyList_default_currency",
                table: "CurrencyList",
                column: "default_currency");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyListCurrency_currency_item",
                table: "CurrencyListCurrency",
                column: "currency_item");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyListCurrency_currency_list",
                table: "CurrencyListCurrency",
                column: "currency_list");

            migrationBuilder.CreateIndex(
                name: "IX_DenominationDetail_tx_id",
                table: "DenominationDetail",
                column: "tx_id");

            migrationBuilder.CreateIndex(
                name: "IX_DepositorSession_device_id",
                table: "DepositorSession",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_DepositorSession_language_code",
                table: "DepositorSession",
                column: "language_code");

            migrationBuilder.CreateIndex(
                name: "IX_Device_branch_id",
                table: "Device",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_Device_config_group",
                table: "Device",
                column: "config_group");

            migrationBuilder.CreateIndex(
                name: "IX_Device_currency_list",
                table: "Device",
                column: "currency_list");

            migrationBuilder.CreateIndex(
                name: "IX_Device_GUIScreen_list",
                table: "Device",
                column: "GUIScreen_list");

            migrationBuilder.CreateIndex(
                name: "IX_Device_language_list",
                table: "Device",
                column: "language_list");

            migrationBuilder.CreateIndex(
                name: "IX_Device_transaction_type_list",
                table: "Device",
                column: "transaction_type_list");

            migrationBuilder.CreateIndex(
                name: "IX_Device_type_id",
                table: "Device",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Device_user_group",
                table: "Device",
                column: "user_group");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCITSuspenseAccount_currency_code",
                table: "DeviceCITSuspenseAccount",
                column: "currency_code");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCITSuspenseAccount_device_id",
                table: "DeviceCITSuspenseAccount",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConfig_config_id",
                table: "DeviceConfig",
                column: "config_id");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConfig_group_id",
                table: "DeviceConfig",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLock_device_id",
                table: "DeviceLock",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLock_locking_user",
                table: "DeviceLock",
                column: "locking_user");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLogin_device_id",
                table: "DeviceLogin",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLogin_User",
                table: "DeviceLogin",
                column: "User");

            migrationBuilder.CreateIndex(
                name: "IX_DevicePrinter_device_id",
                table: "DevicePrinter",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceSuspenseAccount_currency_code",
                table: "DeviceSuspenseAccount",
                column: "currency_code");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceSuspenseAccount_device_id",
                table: "DeviceSuspenseAccount",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_EscrowJam_authorising_user",
                schema: "exp",
                table: "EscrowJam",
                column: "authorising_user");

            migrationBuilder.CreateIndex(
                name: "IX_EscrowJam_initialising_user",
                schema: "exp",
                table: "EscrowJam",
                column: "initialising_user");

            migrationBuilder.CreateIndex(
                name: "IX_EscrowJam_transaction_id",
                schema: "exp",
                table: "EscrowJam",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_GUIPrepopItem_value",
                table: "GUIPrepopItem",
                column: "value");

            migrationBuilder.CreateIndex(
                name: "IX_GUIPrepopListItem_Item",
                table: "GUIPrepopListItem",
                column: "Item");

            migrationBuilder.CreateIndex(
                name: "IX_GUIPrepopListItem_List",
                table: "GUIPrepopListItem",
                column: "List");

            migrationBuilder.CreateIndex(
                name: "IX_GUIScreen_gui_text",
                table: "GUIScreen",
                column: "gui_text");

            migrationBuilder.CreateIndex(
                name: "IX_GUIScreen_type",
                table: "GUIScreen",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenListScreen_gui_screen_list",
                table: "GuiScreenListScreen",
                column: "gui_screen_list");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenListScreen_guiprepoplist_id",
                table: "GuiScreenListScreen",
                column: "guiprepoplist_id");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenListScreen_screen",
                table: "GuiScreenListScreen",
                column: "screen");

            migrationBuilder.CreateIndex(
                name: "IX_GuiScreenListScreen_validation_list_id",
                table: "GuiScreenListScreen",
                column: "validation_list_id");

            migrationBuilder.CreateIndex(
                name: "IX_GUIScreenText_btn_accept_caption",
                table: "GUIScreenText",
                column: "btn_accept_caption");

            migrationBuilder.CreateIndex(
                name: "IX_GUIScreenText_btn_back_caption",
                table: "GUIScreenText",
                column: "btn_back_caption");

            migrationBuilder.CreateIndex(
                name: "IX_GUIScreenText_btn_cancel_caption",
                table: "GUIScreenText",
                column: "btn_cancel_caption");

            migrationBuilder.CreateIndex(
                name: "IX_GUIScreenText_full_instructions",
                table: "GUIScreenText",
                column: "full_instructions");

            migrationBuilder.CreateIndex(
                name: "IX_GUIScreenText_guiscreen_id",
                table: "GUIScreenText",
                column: "guiscreen_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GUIScreenText_screen_title",
                table: "GUIScreenText",
                column: "screen_title");

            migrationBuilder.CreateIndex(
                name: "IX_GUIScreenText_screen_title_instruction",
                table: "GUIScreenText",
                column: "screen_title_instruction");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageList_default_language",
                table: "LanguageList",
                column: "default_language");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageListLanguage_language_item",
                table: "LanguageListLanguage",
                column: "language_item");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageListLanguage_language_list",
                table: "LanguageListLanguage",
                column: "language_list");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordHistory_User",
                table: "PasswordHistory",
                column: "User");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_activity_id",
                table: "Permission",
                column: "activity_id");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_role_id",
                table: "Permission",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_Printout_tx_id",
                table: "Printout",
                column: "tx_id");

            migrationBuilder.CreateIndex(
                name: "IX_SysTextItem_Category",
                schema: "xlns",
                table: "SysTextItem",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_SysTextItem_TextItemTypeID",
                schema: "xlns",
                table: "SysTextItem",
                column: "TextItemTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SysTextItemCategory_Parent",
                schema: "xlns",
                table: "SysTextItemCategory",
                column: "Parent");

            migrationBuilder.CreateIndex(
                name: "IX_SysTextTranslation_LanguageCode",
                schema: "xlns",
                table: "SysTextTranslation",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_SysTextTranslation_SysTextItemID",
                schema: "xlns",
                table: "SysTextTranslation",
                column: "SysTextItemID");

            migrationBuilder.CreateIndex(
                name: "IX_TextItem_Category",
                schema: "xlns",
                table: "TextItem",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_TextItem_TextItemTypeID",
                schema: "xlns",
                table: "TextItem",
                column: "TextItemTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TextItemCategory_Parent",
                schema: "xlns",
                table: "TextItemCategory",
                column: "Parent");

            migrationBuilder.CreateIndex(
                name: "IX_TextTranslation_LanguageCode",
                schema: "xlns",
                table: "TextTranslation",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_TextTranslation_TextItemID",
                schema: "xlns",
                table: "TextTranslation",
                column: "TextItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_auth_user",
                table: "Transaction",
                column: "auth_user");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_cit_id",
                table: "Transaction",
                column: "cit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_device_id",
                table: "Transaction",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_init_user",
                table: "Transaction",
                column: "init_user");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_session_id",
                table: "Transaction",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_tx_currency",
                table: "Transaction",
                column: "tx_currency");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_tx_type",
                table: "Transaction",
                column: "tx_type");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLimitListItem_currency_code",
                table: "TransactionLimitListItem",
                column: "currency_code");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLimitListItem_transactionitemlist_id",
                table: "TransactionLimitListItem",
                column: "transactionitemlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_account_name_caption",
                table: "TransactionText",
                column: "account_name_caption");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_account_number_caption",
                table: "TransactionText",
                column: "account_number_caption");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_alias_account_name_caption",
                table: "TransactionText",
                column: "alias_account_name_caption");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_alias_account_number_caption",
                table: "TransactionText",
                column: "alias_account_number_caption");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_depositor_name_caption",
                table: "TransactionText",
                column: "depositor_name_caption");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_disclaimer",
                table: "TransactionText",
                column: "disclaimer");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_full_instructions",
                table: "TransactionText",
                column: "full_instructions");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_FundsSource_caption",
                table: "TransactionText",
                column: "FundsSource_caption");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_id_number_caption",
                table: "TransactionText",
                column: "id_number_caption");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_listItem_caption",
                table: "TransactionText",
                column: "listItem_caption");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_narration_caption",
                table: "TransactionText",
                column: "narration_caption");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_phone_number_caption",
                table: "TransactionText",
                column: "phone_number_caption");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_receipt_template",
                table: "TransactionText",
                column: "receipt_template");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_reference_account_name_caption",
                table: "TransactionText",
                column: "reference_account_name_caption");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_reference_account_number_caption",
                table: "TransactionText",
                column: "reference_account_number_caption");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_terms",
                table: "TransactionText",
                column: "terms");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_tx_item",
                table: "TransactionText",
                column: "tx_item",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_validation_text_error_message",
                table: "TransactionText",
                column: "validation_text_error_message");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionText_validation_text_success_message",
                table: "TransactionText",
                column: "validation_text_success_message");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListItem_default_account_currency",
                table: "TransactionTypeListItem",
                column: "default_account_currency");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListItem_tx_limit_list",
                table: "TransactionTypeListItem",
                column: "tx_limit_list");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListItem_tx_text",
                table: "TransactionTypeListItem",
                column: "tx_text");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListItem_tx_type",
                table: "TransactionTypeListItem",
                column: "tx_type");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListItem_tx_type_guiscreenlist",
                table: "TransactionTypeListItem",
                column: "tx_type_guiscreenlist");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListTransactionTypeListItem_txtype_list",
                table: "TransactionTypeListTransactionTypeListItem",
                column: "txtype_list");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypeListTransactionTypeListItem_txtype_list_item",
                table: "TransactionTypeListTransactionTypeListItem",
                column: "txtype_list_item");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_parent_group",
                table: "UserGroup",
                column: "parent_group");

            migrationBuilder.CreateIndex(
                name: "IX_UserLock_InitiatingUser",
                table: "UserLock",
                column: "InitiatingUser");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationItem_type_id",
                table: "ValidationItem",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationItem_validation_text_id",
                table: "ValidationItem",
                column: "validation_text_id");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationItemValue_validation_item_id",
                table: "ValidationItemValue",
                column: "validation_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationListValidationItem_validation_item_id",
                table: "ValidationListValidationItem",
                column: "validation_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationListValidationItem_validation_list_id",
                table: "ValidationListValidationItem",
                column: "validation_list_id");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationText_error_message",
                table: "ValidationText",
                column: "error_message");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationText_success_message",
                table: "ValidationText",
                column: "success_message");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationText_validation_item_id",
                table: "ValidationText",
                column: "validation_item_id");

            migrationBuilder.AddForeignKey(
                name: "FK_DenominationDetail_Transaction",
                table: "DenominationDetail",
                column: "tx_id",
                principalTable: "Transaction",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_EscrowJam_Transaction",
                schema: "exp",
                table: "EscrowJam",
                column: "transaction_id",
                principalTable: "Transaction",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_GUIScreen_GUIScreenText",
                table: "GUIScreen",
                column: "gui_text",
                principalTable: "GUIScreenText",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Printout_Transaction",
                table: "Printout",
                column: "tx_id",
                principalTable: "Transaction",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_TransactionTypeListItem",
                table: "Transaction",
                column: "tx_type",
                principalTable: "TransactionTypeListItem",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionText_TransactionTypeListItem",
                table: "TransactionText",
                column: "tx_item",
                principalTable: "TransactionTypeListItem",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionText_ValidationTextErrorMessages",
                table: "TransactionText",
                column: "validation_text_error_message",
                principalTable: "ValidationText",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionText_ValidationTextSuccessMessages",
                table: "TransactionText",
                column: "validation_text_success_message",
                principalTable: "ValidationText",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ValidationItem_ValidationText",
                table: "ValidationItem",
                column: "validation_text_id",
                principalTable: "ValidationText",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionTypeListItem_Currency",
                table: "TransactionTypeListItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionTypeListItem_GUIScreenList",
                table: "TransactionTypeListItem");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreenText_btn_accept_caption",
                table: "GUIScreenText");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreenText_btn_back_caption",
                table: "GUIScreenText");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreenText_btn_cancel_caption",
                table: "GUIScreenText");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreenText_full_instructions",
                table: "GUIScreenText");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreenText_screen_title",
                table: "GUIScreenText");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreenText_screen_title_instruction",
                table: "GUIScreenText");

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
                name: "FK_ValidationText_TextItem_error_message",
                table: "ValidationText");

            migrationBuilder.DropForeignKey(
                name: "FK_ValidationText_TextItem_success_message",
                table: "ValidationText");

            migrationBuilder.DropForeignKey(
                name: "FK_GUIScreen_GUIScreenText",
                table: "GUIScreen");

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
                name: "AlertMessageRegistry");

            migrationBuilder.DropTable(
                name: "AlertSMS");

            migrationBuilder.DropTable(
                name: "ApplicationException",
                schema: "exp");

            migrationBuilder.DropTable(
                name: "ApplicationLog");

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
                name: "DenominationDetail");

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
                name: "EscrowJam",
                schema: "exp");

            migrationBuilder.DropTable(
                name: "GUIPrepopListItem");

            migrationBuilder.DropTable(
                name: "GuiScreenListScreen");

            migrationBuilder.DropTable(
                name: "LanguageListLanguage");

            migrationBuilder.DropTable(
                name: "PasswordHistory");

            migrationBuilder.DropTable(
                name: "PasswordPolicy");

            migrationBuilder.DropTable(
                name: "Permission");

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
                name: "TextTranslation",
                schema: "xlns");

            migrationBuilder.DropTable(
                name: "ThisDevice");

            migrationBuilder.DropTable(
                name: "TransactionException",
                schema: "exp");

            migrationBuilder.DropTable(
                name: "TransactionLimitListItem");

            migrationBuilder.DropTable(
                name: "TransactionTypeListTransactionTypeListItem");

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
                name: "AlertMessageType");

            migrationBuilder.DropTable(
                name: "AlertEvent");

            migrationBuilder.DropTable(
                name: "Config");

            migrationBuilder.DropTable(
                name: "GUIPrepopItem");

            migrationBuilder.DropTable(
                name: "GUIPrepopList");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "SysTextItem",
                schema: "xlns");

            migrationBuilder.DropTable(
                name: "ValidationList");

            migrationBuilder.DropTable(
                name: "ConfigCategory");

            migrationBuilder.DropTable(
                name: "CIT");

            migrationBuilder.DropTable(
                name: "DepositorSession");

            migrationBuilder.DropTable(
                name: "SysTextItemCategory",
                schema: "xlns");

            migrationBuilder.DropTable(
                name: "SysTextItemType",
                schema: "xlns");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "Role");

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
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "GUIScreenList");

            migrationBuilder.DropTable(
                name: "TextItem",
                schema: "xlns");

            migrationBuilder.DropTable(
                name: "TextItemCategory",
                schema: "xlns");

            migrationBuilder.DropTable(
                name: "TextItemType",
                schema: "xlns");

            migrationBuilder.DropTable(
                name: "GUIScreenText");

            migrationBuilder.DropTable(
                name: "GUIScreen");

            migrationBuilder.DropTable(
                name: "GUIScreenType");

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
