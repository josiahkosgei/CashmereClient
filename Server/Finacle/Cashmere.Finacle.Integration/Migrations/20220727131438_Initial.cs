using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cashmere.Finacle.Integration.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountPermissionItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountPermission = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPermissionItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountPermissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    txType = table.Column<int>(type: "int", nullable: false),
                    ListType = table.Column<int>(type: "int", nullable: false),
                    error_message = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPermissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Icon = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enabed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    bank_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    country_code = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.id);
                },
                comment: "The bank that owns the depositor");

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    code = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false, comment: "ISO 4217 Three Character Currency Code"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, comment: "Name of the currency"),
                    minor = table.Column<int>(type: "int", nullable: false, comment: "Expresses the relationship between a major currency unit and its corresponding minor currency unit. This mechanism is called the currency \"exponent\" and assumes a base of 10. Will be used with converters in the GUI"),
                    flag = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "two character country code for the national flag to display for the language"),
                    enabled = table.Column<bool>(type: "bit", nullable: false, comment: "whether the system supports the language"),
                    iso_3_numeric_code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.code);
                },
                comment: "Currency enumeration");

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
                name: "TransactionTypeListItem",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "common name for the transaction e.g. Mpesa Deposit"),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "common description for the transaction type"),
                    icon_path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "the location of the picture representing the transaction in the GUI"),
                    validate_reference_account = table.Column<bool>(type: "bit", nullable: false),
                    default_account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "the default account that pre-polulates the AccountNumber of a transaction"),
                    default_account_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    default_account_currency = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false, defaultValueSql: "('KES')"),
                    validate_default_account = table.Column<bool>(type: "bit", nullable: false),
                    enabled = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    cb_tx_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "A string passed to core banking with transaction details so core banking can route the deposit to the correct handler"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    show_funds_source = table.Column<bool>(type: "bit", nullable: false, comment: "Whether to show the source of funds screen after deposit limit is reached or passed"),
                    funds_source_amount = table.Column<int>(type: "int", nullable: false, comment: "The amount after which the Source of Funds screen will be shown"),
                    prevent_overdeposit = table.Column<bool>(type: "bit", nullable: false, comment: "CDM will not accept further deposits past the maximum"),
                    overdeposit_amount = table.Column<int>(type: "int", nullable: false, comment: "The amount after which the CDM will disable the counter")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypeListItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_TransactionTypeListItem_Currency_default_account_currency",
                        column: x => x.default_account_currency,
                        principalTable: "Currency",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Transactions that the system can perform e.g. regular deposit, Mpesa deposit, etc");

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    device_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    device_location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    machine_name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    enabled = table.Column<bool>(type: "bit", nullable: false),
                    login_cycles = table.Column<int>(type: "int", nullable: false),
                    login_attempts = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    password = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.id);
                    table.ForeignKey(
                        name: "FK_Device_Branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "Branch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ibank_id_Branch",
                table: "Branch",
                column: "bank_id");

            migrationBuilder.CreateIndex(
                name: "ibranch_id_DeviceList",
                table: "Device",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "idefault_account_currency_TransactionTypeListItem",
                table: "TransactionTypeListItem",
                column: "default_account_currency");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountPermissionItems");

            migrationBuilder.DropTable(
                name: "AccountPermissions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "TransactionTypeListItem");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "Bank");
        }
    }
}
