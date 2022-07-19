using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Logging;
using CashmereDeposit.Utils;
using CashmereDeposit.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CashmereDeposit.Models
{
    public class DeviceConfiguration
    {

        private readonly IAsyncRepository<DeviceConfig> _repository;
        private bool _ALLOW_WEB_SERVER;
        private int _MIN_HDD_SPACE;
        private int _LOGGING_LEVEL;
        private bool _CIT_ALLOW_POST;
        private int _MESSAGEKEEPALIVETIME;
        private int _SERVER_POLL_INTERVAL;
        private string _TRANSACTION_LOG_FOLDER;
        private bool _AUTODROP_CHANGE_ALLOWED;
        private bool _AUTODROP_CHECKED;
        private bool _AUTOCOUNT_CHANGE_ALLOWED;
        private bool _AUTOCOUNT_CHECKED;
        private bool _USE_MAX_DEPOSIT_COUNT;
        private bool _USE_MAX_DEPOSIT_VALUE;
        private bool _ALLOW_CROSS_CURRENCY_TX;
        private bool _ALLOW_DEPOSIT_PAST_MAX;
        private bool _POST_ON_NOTEJAM;
        private bool _POST_ON_ESCROWJAM;
        private int _USER_SCREEN_TIMEOUT;
        private int _THANK_YOU_TIMEOUT;
        private int _BAGFULL_OVERFLOW_COUNT;
        private bool _AD_ALLOW;
        private bool _EMAIL_CAN_SEND;
        private bool _SMS_CAN_SEND;
        private int _EMAIL_SEND_INTERVAL;
        private int _ALERT_DB_POLL_INTERVAL;
        private bool _EMAIL_SEND_ATTACHMENT;
        private string _EMAIL_LOCAL_FOLDER;
        private int _ALERT_BATCH_SIZE;
        private bool _ALLOW_OFFLINE_AUTH;
        private bool _LOGINFAIL_DEVICELOCK;
        private int _LOGINFAIL_DEVICELOCK_RETRY_COUNT;
        private int _LOGINFAIL_MAX_CYCLES;
        private string _UI_CULTURE;
        private string _APPLICATION_DATE_FORMAT;
        private string _SMS_DATE_FORMAT;
        private string _SMS_AMOUNT_FORMAT;
        private string _APPLICATION_GUID_FORMAT;
        private string _APPLICATION_INTEGER_FORMAT;
        private string _APPLICATION_DOUBLE_FORMAT;
        private string _APPLICATION_MONEY_FORMAT;
        private string _PHONE_MASK;
        private string _PHONE_CODE;
        private int _DEVICECONTROLLER_PORT;
        private string _DEVICECONTROLLER_HOST;
        private string _FIX_DEVICE_PORT;
        private string _FIX_CONTROLLER_PORT;
        private string _CONTROLLER_TYPE;
        private string _CONTROLLER_LOG_DIRECTORY;
        private bool _SENSOR_INVERT_DOOR;
        private int _BAGFULL_WARN_PERCENT;
        private string _RECEIPT_LOGO;
        private bool _RECEIPT_INVERT_ORDER;
        private int _RECEIPT_WIDTH;
        private int _CIT_RECEIPT_ORIGINAL_COUNT;
        private int _RECEIPT_ORIGINAL_COUNT;
        private string _RECEIPT_COPY_TEXT;
        private string _RECEIPT_BANK_NAME;
        private string _RECEIPT_CIT_TITLE;
        private char _RECEIPT_ACCOUNT_NO_PAD_CHAR;
        private int _RECEIPT_ACCOUNT_NUMBER_VISIBLE_DIGITS;
        private string _RECEIPT_FOLDER;
        private string _RECEIPT_PRINTERPORT;
        private string _RECEIPT_DATE_FORMAT;
        private string _API_CDM_GUI_URI;
        private string _API_COMMSERV_URI;
        private string _API_INTEGRATION_URI;
        private string _API_AUTH_API_URI;
        public bool USE_REAR_SCREEN;

        private static DeviceConfiguration? deviceConfiguration { get; set; }

        public UtilDepositorLogger Log { get; set; } = new UtilDepositorLogger(Assembly.GetExecutingAssembly().GetName().Version, "CashmereDepositLog");

        internal ApplicationViewModel ApplicationViewModel { get; }

        internal Tokeniser Tokeniser { get; }

        public bool ALLOW_WEB_SERVER => _ALLOW_WEB_SERVER;

        public int MIN_HDD_SPACE => _MIN_HDD_SPACE;

        public int LOGGING_LEVEL => _LOGGING_LEVEL;

        public bool CIT_ALLOW_POST
        {
            get => _CIT_ALLOW_POST;
            internal set => _CIT_ALLOW_POST = value;
        }

        public int MESSAGEKEEPALIVETIME
        {
            get => _MESSAGEKEEPALIVETIME;
            internal set => _MESSAGEKEEPALIVETIME = value;
        }

        public int SERVER_POLL_INTERVAL
        {
            get => _SERVER_POLL_INTERVAL;
            internal set => _SERVER_POLL_INTERVAL = value;
        }

        public string TRANSACTION_LOG_FOLDER
        {
            get => _TRANSACTION_LOG_FOLDER;
            internal set => _TRANSACTION_LOG_FOLDER = value;
        }

        public bool AUTODROP_CHANGE_ALLOWED => _AUTODROP_CHANGE_ALLOWED;

        public bool AUTODROP_CHECKED => _AUTODROP_CHECKED;

        public bool AUTOCOUNT_CHANGE_ALLOWED => _AUTOCOUNT_CHANGE_ALLOWED;

        public bool AUTOCOUNT_CHECKED => _AUTOCOUNT_CHECKED;

        public bool USE_MAX_DEPOSIT_COUNT => _USE_MAX_DEPOSIT_COUNT;

        public bool USE_MAX_DEPOSIT_VALUE => _USE_MAX_DEPOSIT_VALUE;

        public bool ALLOW_CROSS_CURRENCY_TX => _ALLOW_CROSS_CURRENCY_TX;

        public bool ALLOW_DEPOSIT_PAST_MAX => _ALLOW_DEPOSIT_PAST_MAX;

        public bool POST_ON_NOTEJAM => _POST_ON_NOTEJAM;

        public bool POST_ON_ESCROWJAM => _POST_ON_ESCROWJAM;

        public int USER_SCREEN_TIMEOUT => _USER_SCREEN_TIMEOUT;

        public int THANK_YOU_TIMEOUT
        {
            get => _THANK_YOU_TIMEOUT;
            internal set => _THANK_YOU_TIMEOUT = value;
        }

        public int BAGFULL_OVERFLOW_COUNT => _BAGFULL_OVERFLOW_COUNT;

        public bool AD_ALLOW => _AD_ALLOW;

        public bool EMAIL_CAN_SEND
        {
            get => _EMAIL_CAN_SEND;
            internal set => _EMAIL_CAN_SEND = value;
        }

        public bool SMS_CAN_SEND
        {
            get => _SMS_CAN_SEND;
            internal set => _SMS_CAN_SEND = value;
        }

        public int EMAIL_SEND_INTERVAL
        {
            get => _EMAIL_SEND_INTERVAL;
            internal set => _EMAIL_SEND_INTERVAL = value;
        }

        public int ALERT_DB_POLL_INTERVAL
        {
            get => _ALERT_DB_POLL_INTERVAL;
            internal set => _ALERT_DB_POLL_INTERVAL = value;
        }

        public bool EMAIL_SEND_ATTACHMENT
        {
            get => _EMAIL_SEND_ATTACHMENT;
            internal set => _EMAIL_SEND_ATTACHMENT = value;
        }

        public string EMAIL_LOCAL_FOLDER
        {
            get => _EMAIL_LOCAL_FOLDER;
            internal set => _EMAIL_LOCAL_FOLDER = value;
        }

        public int ALERT_BATCH_SIZE
        {
            get => _ALERT_BATCH_SIZE;
            internal set => _ALERT_BATCH_SIZE = value;
        }

        public bool ALLOW_OFFLINE_AUTH
        {
            get => _ALLOW_OFFLINE_AUTH;
            internal set => _ALLOW_OFFLINE_AUTH = value;
        }

        public bool LOGINFAIL_DEVICELOCK
        {
            get => _LOGINFAIL_DEVICELOCK;
            internal set => _LOGINFAIL_DEVICELOCK = value;
        }

        public int LOGINFAIL_DEVICELOCK_RETRY_COUNT
        {
            get => _LOGINFAIL_DEVICELOCK_RETRY_COUNT;
            internal set => _LOGINFAIL_DEVICELOCK_RETRY_COUNT = value;
        }

        public int LOGINFAIL_MAX_CYCLES
        {
            get => _LOGINFAIL_MAX_CYCLES;
            internal set => _LOGINFAIL_MAX_CYCLES = value;
        }

        public string UI_CULTURE
        {
            get => _UI_CULTURE;
            internal set => _UI_CULTURE = value;
        }

        public string APPLICATION_DATE_FORMAT => _APPLICATION_DATE_FORMAT;

        public string SMS_DATE_FORMAT => _SMS_DATE_FORMAT;

        public string SMS_AMOUNT_FORMAT => _SMS_AMOUNT_FORMAT;

        public string APPLICATION_GUID_FORMAT => _APPLICATION_GUID_FORMAT;

        public string APPLICATION_INTEGER_FORMAT => _APPLICATION_INTEGER_FORMAT;

        public string APPLICATION_DOUBLE_FORMAT => _APPLICATION_DOUBLE_FORMAT;

        public string APPLICATION_MONEY_FORMAT => _APPLICATION_MONEY_FORMAT;

        public string PHONE_MASK => _PHONE_MASK;

        public string PHONE_CODE => _PHONE_CODE;

        public int DEVICECONTROLLER_PORT
        {
            get => _DEVICECONTROLLER_PORT;
            internal set => _DEVICECONTROLLER_PORT = value;
        }

        public string DEVICECONTROLLER_HOST
        {
            get => _DEVICECONTROLLER_HOST;
            internal set => _DEVICECONTROLLER_HOST = value;
        }

        public string FIX_DEVICE_PORT
        {
            get => _FIX_DEVICE_PORT;
            internal set => _FIX_DEVICE_PORT = value;
        }

        public string FIX_CONTROLLER_PORT
        {
            get => _FIX_CONTROLLER_PORT;
            internal set => _FIX_CONTROLLER_PORT = value;
        }

        public string CONTROLLER_TYPE
        {
            get => _CONTROLLER_TYPE;
            internal set => _CONTROLLER_TYPE = value;
        }

        public string CONTROLLER_LOG_DIRECTORY
        {
            get => _CONTROLLER_LOG_DIRECTORY;
            internal set => _CONTROLLER_LOG_DIRECTORY = value;
        }

        public bool SENSOR_INVERT_DOOR
        {
            get => _SENSOR_INVERT_DOOR;
            internal set => _SENSOR_INVERT_DOOR = value;
        }

        public int BAGFULL_WARN_PERCENT
        {
            get => _BAGFULL_WARN_PERCENT;
            internal set => _BAGFULL_WARN_PERCENT = value;
        }

        public string RECEIPT_LOGO
        {
            get => _RECEIPT_LOGO;
            internal set => _RECEIPT_LOGO = value;
        }

        public bool RECEIPT_INVERT_ORDER
        {
            get => _RECEIPT_INVERT_ORDER;
            internal set => _RECEIPT_INVERT_ORDER = value;
        }

        public int RECEIPT_WIDTH
        {
            get => _RECEIPT_WIDTH;
            internal set => _RECEIPT_WIDTH = value;
        }

        public int CIT_RECEIPT_ORIGINAL_COUNT => _CIT_RECEIPT_ORIGINAL_COUNT;

        public int RECEIPT_ORIGINAL_COUNT => _RECEIPT_ORIGINAL_COUNT;

        public string RECEIPT_COPY_TEXT => _RECEIPT_COPY_TEXT;

        public string RECEIPT_BANK_NAME => _RECEIPT_BANK_NAME;

        public string RECEIPT_CIT_TITLE => _RECEIPT_CIT_TITLE;

        public char RECEIPT_ACCOUNT_NO_PAD_CHAR => _RECEIPT_ACCOUNT_NO_PAD_CHAR;

        public int RECEIPT_ACCOUNT_NUMBER_VISIBLE_DIGITS => _RECEIPT_ACCOUNT_NUMBER_VISIBLE_DIGITS;

        public string RECEIPT_FOLDER
        {
            get => _RECEIPT_FOLDER;
            internal set => _RECEIPT_FOLDER = value;
        }

        public string RECEIPT_PRINTERPORT
        {
            get => _RECEIPT_PRINTERPORT;
            internal set => _RECEIPT_PRINTERPORT = value;
        }

        public string RECEIPT_DATE_FORMAT => _RECEIPT_DATE_FORMAT;

        public string API_CDM_GUI_URI
        {
            get => _API_CDM_GUI_URI;
            internal set => _API_CDM_GUI_URI = value;
        }

        public string API_COMMSERV_URI
        {
            get => _API_COMMSERV_URI;
            internal set => _API_COMMSERV_URI = value;
        }

        public string API_INTEGRATION_URI
        {
            get => _API_INTEGRATION_URI;
            internal set => _API_INTEGRATION_URI = value;
        }

        public string API_AUTH_API_URI
        {
            get => _API_AUTH_API_URI;
            internal set => _API_AUTH_API_URI = value;
        }

        private DeviceConfiguration()
        {
            _repository = IoC.Get<IAsyncRepository<DeviceConfig>>();
            Tokeniser = new Tokeniser(this);
            Init();
        }

        public static DeviceConfiguration GetInstance()
        {
            return deviceConfiguration ?? new DeviceConfiguration();
        }

        private DeviceConfiguration Init()
        {
            InitialiseConfigs(GenerateConfigs());
            return this;
        }

        public static DeviceConfiguration Initialise() => GetInstance().Init();

        private void InitialiseConfigs(IList<(string config_id, string config_value)> config)
        {
            InitialiseConfig(config, "MIN_HDD_SPACE", "MIN_HDD_SPACE", ref _MIN_HDD_SPACE, 1073741274);
            InitialiseConfig(config, "API_CDM_GUI_URI", "API_CDM_GUI_URI", ref _API_CDM_GUI_URI, string.Empty);
            InitialiseConfig(config, "API_COMMSERV_URI", "API_COMMSERV_URI", ref _API_COMMSERV_URI, string.Empty);
            InitialiseConfig(config, "API_INTEGRATION_URI", "API_INTEGRATION_URI", ref _API_INTEGRATION_URI, string.Empty);
            InitialiseConfig(config, "API_AUTH_API_URI", "API_AUTH_API_URI", ref _API_AUTH_API_URI, string.Empty);
            InitialiseConfig(config, "ALLOW_WEB_SERVER", "ALLOW_WEB_SERVER", ref _ALLOW_WEB_SERVER, false);
            InitialiseConfig(config, "RECEIPT_BANK_NAME", "RECEIPT_BANK_NAME", ref _RECEIPT_BANK_NAME, "Cash Deposit Machine");
            InitialiseConfig(config, "RECEIPT_INVERT_ORDER", "RECEIPT_INVERT_ORDER", ref _RECEIPT_INVERT_ORDER, false);
            InitialiseConfig(config, "RECEIPT_LOGO", "RECEIPT_LOGO", ref _RECEIPT_LOGO, "Resources/logo.txt");
            InitialiseConfig(config, "RECEIPT_PRINTERPORT", "RECEIPT_PRINTERPORT", ref _RECEIPT_PRINTERPORT, "COM1");
            InitialiseConfig(config, "RECEIPT_CIT_TITLE", "RECEIPT_CIT_TITLE", ref _RECEIPT_CIT_TITLE, "CIT Receipt");
            InitialiseConfig(config, "RECEIPT_FOLDER", "RECEIPT_FOLDER", ref _RECEIPT_FOLDER, "C:\\CashmereDeposit\\Receipts");
            InitialiseConfig(config, "RECEIPT_ACCOUNT_NO_PAD_CHAR", "RECEIPT_ACCOUNT_NO_PAD_CHAR", ref _RECEIPT_ACCOUNT_NO_PAD_CHAR, '*');
            InitialiseConfig(config, "RECEIPT_DATE_FORMAT", "RECEIPT_DATE_FORMAT", ref _RECEIPT_DATE_FORMAT, "yyyy-MM-dd HH:mm:ss.fff");
            InitialiseConfig(config, "RECEIPT_COPY_TEXT", "RECEIPT_COPY_TEXT", ref _RECEIPT_COPY_TEXT, "RECEIPT COPY");
            InitialiseConfig(config, "CIT_RECEIPT_ORIGINAL_COUNT", "CIT_RECEIPT_ORIGINAL_COUNT", ref _CIT_RECEIPT_ORIGINAL_COUNT, 2);
            InitialiseConfig(config, "RECEIPT_ORIGINAL_COUNT", "RECEIPT_ORIGINAL_COUNT", ref _RECEIPT_ORIGINAL_COUNT, 1);
            InitialiseConfig(config, "RECEIPT_WIDTH", "RECEIPT_WIDTH", ref _RECEIPT_WIDTH, 24);
            InitialiseConfig(config, "RECEIPT_ACCOUNT_NUMBER_VISIBLE_DIGITS", "RECEIPT_ACCOUNT_NUMBER_VISIBLE_DIGITS", ref _RECEIPT_ACCOUNT_NUMBER_VISIBLE_DIGITS, 4);
            InitialiseConfig(config, "ALLOW_CROSS_CURRENCY_TX", "ALLOW_CROSS_CURRENCY_TX", ref _ALLOW_CROSS_CURRENCY_TX, false);
            InitialiseConfig(config, "LOGGING_LEVEL", "LOGGING_LEVEL", ref _LOGGING_LEVEL, 1);
            InitialiseConfig(config, "USE_MAX_DEPOSIT_COUNT", "USE_MAX_DEPOSIT_COUNT", ref _USE_MAX_DEPOSIT_COUNT, false);
            InitialiseConfig(config, "USE_MAX_DEPOSIT_VALUE", "USE_MAX_DEPOSIT_VALUE", ref _USE_MAX_DEPOSIT_VALUE, false);
            InitialiseConfig(config, "ALLOW_DEPOSIT_PAST_MAX", "ALLOW_DEPOSIT_PAST_MAX", ref _ALLOW_DEPOSIT_PAST_MAX, true);
            InitialiseConfig(config, "POST_ON_NOTEJAM", "POST_ON_NOTEJAM", ref _POST_ON_NOTEJAM, true);
            InitialiseConfig(config, "POST_ON_ESCROWJAM", "POST_ON_ESCROWJAM", ref _POST_ON_ESCROWJAM, false);
            InitialiseConfig(config, "USER_SCREEN_TIMEOUT", "USER_SCREEN_TIMEOUT", ref _USER_SCREEN_TIMEOUT, 15);
            InitialiseConfig(config, "APPLICATION_DATE_FORMAT", "APPLICATION_DATE_FORMAT", ref _APPLICATION_DATE_FORMAT, "yyyy-MM-dd HH:mm:ss.fff");
            InitialiseConfig(config, "SMS_DATE_FORMAT", "SMS_DATE_FORMAT", ref _SMS_DATE_FORMAT, "d/M/yy 'at' h:mm tt");
            InitialiseConfig(config, "SMS_AMOUNT_FORMAT", "SMS_AMOUNT_FORMAT", ref _SMS_AMOUNT_FORMAT, "#,#0.00");
            InitialiseConfig(config, "APPLICATION_GUID_FORMAT", "APPLICATION_GUID_FORMAT", ref _APPLICATION_GUID_FORMAT, (string)null);
            InitialiseConfig(config, "APPLICATION_INTEGER_FORMAT", "APPLICATION_INTEGER_FORMAT", ref _APPLICATION_INTEGER_FORMAT, (string)null);
            InitialiseConfig(config, "APPLICATION_DOUBLE_FORMAT", "APPLICATION_DOUBLE_FORMAT", ref _APPLICATION_DOUBLE_FORMAT, (string)null);
            InitialiseConfig(config, "APPLICATION_MONEY_FORMAT", "APPLICATION_MONEY_FORMAT", ref _APPLICATION_MONEY_FORMAT, "########0");
            InitialiseConfig(config, "PHONE_MASK", "PHONE_MASK", ref _PHONE_MASK, "254 000 000 000");
            InitialiseConfig(config, "PHONE_CODE", "PHONE_CODE", ref _PHONE_CODE, "254");
            InitialiseConfig(config, "AUTODROP_CHANGE_ALLOWED", "AUTODROP_CHANGE_ALLOWED", ref _AUTODROP_CHANGE_ALLOWED, true);
            InitialiseConfig(config, "AUTODROP_CHECKED", "AUTODROP_CHECKED", ref _AUTODROP_CHECKED, false);
            InitialiseConfig(config, "AUTOCOUNT_CHANGE_ALLOWED", "AUTOCOUNT_CHANGE_ALLOWED", ref _AUTOCOUNT_CHANGE_ALLOWED, true);
            InitialiseConfig(config, "AUTOCOUNT_CHECKED", "AUTOCOUNT_CHECKED", ref _AUTOCOUNT_CHECKED, false);
            InitialiseConfig(config, "BAGFULL_OVERFLOW_COUNT", "BAGFULL_OVERFLOW_COUNT", ref _BAGFULL_OVERFLOW_COUNT, 1000);
            InitialiseConfig(config, "THANK_YOU_TIMEOUT", "THANK_YOU_TIMEOUT", ref _THANK_YOU_TIMEOUT, 5);
            InitialiseConfig(config, "CIT_ALLOW_POST", "CIT_ALLOW_POST", ref _CIT_ALLOW_POST, true);
            InitialiseConfig(config, "AD_ALLOW", "AD_ALLOW", ref _AD_ALLOW, false);
            InitialiseConfig(config, "EMAIL_CAN_SEND", "EMAIL_CAN_SEND", ref _EMAIL_CAN_SEND, false);
            InitialiseConfig(config, "EMAIL_SEND_ATTACHMENT", "EMAIL_SEND_ATTACHMENT", ref _EMAIL_SEND_ATTACHMENT, false);
            InitialiseConfig(config, "EMAIL_SEND_INTERVAL", "EMAIL_SEND_INTERVAL", ref _EMAIL_SEND_INTERVAL, 3000);
            InitialiseConfig(config, "SMS_CAN_SEND", "SMS_CAN_SEND", ref _SMS_CAN_SEND, false);
            InitialiseConfig(config, "ALERT_DB_POLL_INTERVAL", "ALERT_DB_POLL_INTERVAL", ref _ALERT_DB_POLL_INTERVAL, 3000);
            InitialiseConfig(config, "EMAIL_LOCAL_FOLDER", "EMAIL_LOCAL_FOLDER", ref _EMAIL_LOCAL_FOLDER, "C:\\CashmereDeposit\\EMAIL");
            InitialiseConfig(config, "ALERT_BATCH_SIZE", "ALERT_BATCH_SIZE", ref _ALERT_BATCH_SIZE, 10);
            InitialiseConfig(config, "ALLOW_OFFLINE_AUTH", "ALLOW_OFFLINE_AUTH", ref _ALLOW_OFFLINE_AUTH, false);
            InitialiseConfig(config, "LOGINFAIL_DEVICELOCK", "LOGINFAIL_DEVICELOCK", ref _LOGINFAIL_DEVICELOCK, true);
            InitialiseConfig(config, "LOGINFAIL_DEVICELOCK_RETRY_COUNT", "LOGINFAIL_DEVICELOCK_RETRY_COUNT", ref _LOGINFAIL_DEVICELOCK_RETRY_COUNT, 3);
            InitialiseConfig(config, "LOGINFAIL_MAX_CYCLES", "LOGINFAIL_MAX_CYCLES", ref _LOGINFAIL_MAX_CYCLES, 3);
            InitialiseConfig(config, "TRANSACTION_LOG_FOLDER", "TRANSACTION_LOG_FOLDER", ref _TRANSACTION_LOG_FOLDER, "C:\\CashmereDeposit\\Transactions");
            InitialiseConfig(config, "DEVICECONTROLLER_PORT", "DEVICECONTROLLER_PORT", ref _DEVICECONTROLLER_PORT, 20201);
            InitialiseConfig(config, "DEVICECONTROLLER_HOST", "DEVICECONTROLLER_HOST", ref _DEVICECONTROLLER_HOST, "localhost");
            InitialiseConfig(config, "FIX_CONTROLLER_PORT", "FIX_CONTROLLER_PORT", ref _FIX_CONTROLLER_PORT, (string)null);
            InitialiseConfig(config, "FIX_DEVICE_PORT", "FIX_DEVICE_PORT", ref _FIX_DEVICE_PORT, (string)null);
            InitialiseConfig(config, "CONTROLLER_TYPE", "CONTROLLER_TYPE", ref _CONTROLLER_TYPE, (string)null);
            InitialiseConfig(config, "CONTROLLER_LOG_DIRECTORY", "CONTROLLER_LOG_DIRECTORY", ref _CONTROLLER_LOG_DIRECTORY, (string)null);
            InitialiseConfig(config, "SENSOR_INVERT_DOOR", "SENSOR_INVERT_DOOR", ref _SENSOR_INVERT_DOOR, false);
            InitialiseConfig(config, "BAGFULL_WARN_PERCENT", "BAGFULL_WARN_PERCENT", ref _BAGFULL_WARN_PERCENT, 80);
            InitialiseConfig(config, "MESSAGEKEEPALIVETIME", "MESSAGEKEEPALIVETIME", ref _MESSAGEKEEPALIVETIME, 900);
            InitialiseConfig(config, "SERVER_POLL_INTERVAL", "SERVER_POLL_INTERVAL", ref _SERVER_POLL_INTERVAL, 900);
            InitialiseConfig(config, "UI_CULTURE", "UI_CULTURE", ref _UI_CULTURE, "en-gb");
        }

    private List<(string name, string? default_value)> GenerateConfigs()
        {
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
            {
                var configs = depositorDbContext.Configs.ToList();
                var config_group = depositorDbContext.Devices.FirstOrDefault()?.ConfigGroup;
                //var userType = depositorDbContext.Set().FromSql("dbo.SomeSproc @Id = {0}, @Name = {1}", 45, "Ada");
                depositorDbContext.Set<DeviceConfig>().FromSqlRaw("EXECUTE  dbo.GetDeviceConfigByUserGroup @ConfigGroup = {0}", config_group).ToList().ForEach(deviceConfig => configs.First<Config>(x => x.Name.Equals(deviceConfig.ConfigId, StringComparison.OrdinalIgnoreCase)).DefaultValue = deviceConfig.ConfigValue);
                //depositorDbContext.DeviceConfigs.Where(x => config_group != null && x.GroupId == config_group).AsQueryable().ToList()
                //    .ForEach(deviceConfig =>
                //        configs.First(x =>
                //                x.Name.Equals(deviceConfig.ConfigId.ToString(), StringComparison.OrdinalIgnoreCase))
                //            .DefaultValue = deviceConfig.ConfigValue);

                //depositorDbContext
                //    .GetDeviceConfigByUserGroup(depositorDbContext.Devices.FirstOrDefault()?.config_group).ToList()
                //    .ForEach(deviceConfig =>
                //        configs.First(x => x.name.Equals(deviceConfig.config_id, StringComparison.OrdinalIgnoreCase))
                //            .default_value = deviceConfig.config_value);
                return configs.Select(x => (name: x.Name, default_value: x.DefaultValue))
                    .ToList();
            }
        }
        public void InitialiseConfig(
          IList<(string config_id, string config_value)> config,
          string configName,
          string propertyName,
          ref bool field,
          bool defaultValue)
        {
            string str = config.FirstOrDefault(x => x.config_id == configName).Item2;
            if (!bool.TryParse(str, out field))
            {
                field = defaultValue;
                Log.WarningFormat(nameof(DeviceConfiguration), "Setting FAIL", nameof(InitialiseConfig), "Couldn't set config {0} to {1} so used default value {2}", (object)configName, (object)str, (object)defaultValue);
            }
            else
                Log.InfoFormat(nameof(DeviceConfiguration), "Setting SUCCESS", nameof(InitialiseConfig), "Set config {0} to {1}", (object)configName, (object)field);
        }

        public void InitialiseConfig(
          IList<(string config_id, string config_value)> config,
          string configName,
          string propertyName,
          ref int field,
          int defaultValue)
        {
            string s = config.FirstOrDefault(x => x.config_id == configName).Item2;
            if (!int.TryParse(s, out field))
            {
                field = defaultValue;
                Log.WarningFormat(nameof(DeviceConfiguration), "Setting FAIL", nameof(InitialiseConfig), "Couldn't set config {0} to {1} so used default value {2}", (object)configName, (object)s, (object)defaultValue);
            }
            else
                Log.InfoFormat(nameof(DeviceConfiguration), "Setting SUCCESS", nameof(InitialiseConfig), "Set config {0} to {1}", (object)configName, (object)field);
        }

        public void InitialiseConfig(
          IList<(string config_id, string config_value)> config,
          string configName,
          string propertyName,
          ref char field,
          char defaultValue)
        {
            string str = config.FirstOrDefault(x => x.config_id == configName).Item2;
            if (string.IsNullOrWhiteSpace(str) && str.Length == 1)
            {
                field = defaultValue;
                Log.WarningFormat(nameof(DeviceConfiguration), "Setting FAIL", nameof(InitialiseConfig), "Couldn't set config {0} to {1} so used default value {2}", (object)configName, (object)str, (object)defaultValue);
            }
            else
            {
                field = str[0];
                Log.InfoFormat(nameof(DeviceConfiguration), "Setting SUCCESS", nameof(InitialiseConfig), "Set config {0} to {1}", (object)configName, (object)field);
            }
        }

        public void InitialiseConfig(
          IList<(string config_id, string config_value)> config,
          string configName,
          string propertyName,
          ref string field,
          string defaultValue)
        {
            string str = config.FirstOrDefault(x => x.config_id == configName).Item2;
            if (string.IsNullOrWhiteSpace(str))
            {
                field = defaultValue;
                Log.WarningFormat(nameof(DeviceConfiguration), "Setting FAIL", nameof(InitialiseConfig), "Couldn't set config {0} to {1} so used default value {2}", (object)configName, (object)str, (object)defaultValue);
            }
            else
            {
                field = str;
                Log.InfoFormat(nameof(DeviceConfiguration), "Setting SUCCESS", nameof(InitialiseConfig), "Set config {0} to {1}", (object)configName, (object)field);
            }
        }
    }
}
