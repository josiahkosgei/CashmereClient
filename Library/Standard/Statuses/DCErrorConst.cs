﻿// DCErrorConst


namespace Cashmere.Library.Standard.Statuses
{
    public static class DCErrorConst
    {
        public const int CASHDEVICE_ERROR = 1000;
        public const int ALARMDEVICE_ERROR = 3000;
        public const int PRINTER_ERROR = 4000;
        public const int CARDREADER_ERROR = 5000;
        public const int TEXTIO_ERROR = 6000;
        public const int PIN_ERROR = 7000;
        public const int CHK_ERROR = 8000;
        public const int ERROR_NONE = 0;
        public const int ERROR_UNKNOWN = -1;
        public const int LEVEL_INFO = 1;
        public const int LEVEL_WARNING = 1;
        public const int LEVEL_RECOVERABLE = 2;
        public const int LEVEL_FATAL = 3;
        public const int CONF_ERROR_OFFSET = 100;
        public const int DEVICE_ERROR_OFFSET = 200;
        public const int APP_ERROR_OFFSET = 300;
        public const int QUEUE_ERROR_OFFSET = 400;
        public const int CONF_CURRENCY_NOT_AVAILABLE = 101;
        public const int CONF_DEVICE_NOT_STORED = 102;
        public const int CONF_ERROR_ROOT_NOT_FOUND = 103;
        public const int CONF_FILE_NOT_FOUND = 104;
        public const int CONF_FIRST_ELEMENT_NOT_FOUND = 105;
        public const int CONF_ILLEGAL_CASHUNIT_STATUS = 106;
        public const int CONF_ILLEGAL_CASHUNIT_TYPE = 107;
        public const int CONF_ILLEGAL_DEVICE_ID = 108;
        public const int CONF_ILLEGAL_DEVICE_KEY = 109;
        public const int CONF_ILLEGAL_DEVICE_NAME = 110;
        public const int CONF_ILLEGAL_JXFS_CASHUNIT_STATUS = 111;
        public const int CONF_ILLEGAL_JXFS_CASHUNIT_TYPE = 112;
        public const int CONF_IO = 113;
        public const int CONF_KEY_LENGTH_NULL = 114;
        public const int CONF_KEY_NOT_FOUND = 115;
        public const int CONF_LANGUAGE_PROPERTY_NOT_FOUND = 116;
        public const int CONF_MALFORMED_UNICODE = 117;
        public const int CONF_MALFORMED_URL = 118;
        public const int CONF_MULTIPLE_JXFS_CURRENCIES = 119;
        public const int CONF_NO_URL_SPECIFIED = 120;
        public const int CONF_NULL_POINTER = 121;
        public const int CONF_OBJECT_ALREADY_STORED = 122;
        public const int CONF_OBJECT_NOT_AVAILABLE = 123;
        public const int CONF_SECOND_ELEMENT_NOT_FOUND = 124;
        public const int CONF_SECTION_NOT_FOUND = 125;
        public const int DEVICE_BUSY = 201;
        public const int DEVICE_CANCELLED = 202;
        public const int DEVICE_CLAIMED = 203;
        public const int DEVICE_CLOSED = 204;
        public const int DEVICE_DISABLED = 205;
        public const int DEVICE_EXISTS = 206;
        public const int DEVICE_FAILURE = 207;
        public const int DEVICE_FIRMWARE = 208;
        public const int DEVICE_ILLEGAL = 209;
        public const int DEVICE_IO = 210;
        public const int DEVICE_NO_CONTROL = 211;
        public const int DEVICE_NO_EXIST = 212;
        public const int DEVICE_NO_HARDWARE = 213;
        public const int DEVICE_NO_SERVICE = 214;
        public const int DEVICE_NOT_CLAIMED = 215;
        public const int DEVICE_NOT_SHARED = 216;
        public const int DEVICE_NOT_SUPPORTED = 217;
        public const int DEVICE_OFFLINE = 218;
        public const int DEVICE_OPEN = 219;
        public const int DEVICE_PARAMETER_INVALID = 220;
        public const int DEVICE_REMOTE = 221;
        public const int DEVICE_TIMEOUT = 222;
        public const int DEVICE_UNREGISTERED = 223;
        public const int DEVICE_HARDWARE = 224;
        public const int DEVICE_SYSTEM = 225;
        public const int DEVICE_RELEASED = 226;
        public const int DEVICE_NOTE_JAM = 227;
        public const int APP_ALREADY_INITIALIZED = 301;
        public const int APP_BASE_DENOM_NOT_DEFINED = 302;
        public const int APP_DENOM_NOT_CONTAINED = 303;
        public const int APP_DIFFERENCE_TO_ACTUAL_DENOM = 304;
        public const int APP_DIFFERENCE_TO_CURRENT_DENOM = 305;
        public const int APP_ELEMENT_NOT_FOUND = 306;
        public const int APP_ILLEGAL_ASSET_TYPE = 307;
        public const int APP_ILLEGAL_ARGUMENT = 308;
        public const int APP_ILLEGAL_CURRENCY_ITEM = 309;
        public const int APP_ILLEGAL_ORDER = 310;
        public const int APP_ILLEGAL_QUEUE_ID = 311;
        public const int APP_ITEM_ALREADY_CONFIGURED = 312;
        public const int APP_ITEM_NOT_CONFIGURED = 313;
        public const int APP_LISTENER_ALREADY_REGISTERED = 314;
        public const int APP_LISTENER_NOT_REGISTERED = 315;
        public const int APP_NOT_DEPOSITABLE = 316;
        public const int APP_NOT_ENOUGH_CASH_ITEMS = 317;
        public const int APP_TELLER_STATISTIC_NOT_CONFIGURED = 318;
        public const int APP_WRONG_VALUE_USED = 319;
        public const int APP_DENOMINATE_WHOLE_ASSET = 320;
        public const int APP_JXFS_DEVICE_NOT_AVAILABLE = 321;
        public const int APP_NO_READ_BACKUP_LOCATION = 322;
        public const int APP_NO_WRITE_BACKUP_LOCATION = 323;
        public const int APP_NO_READ_DEVICECONTROLLER_LIST = 324;
        public const int APP_NO_WRITE_DEVICECONTROLLER_LIST = 325;
        public const int APP_NO_READ_PINPAD_PORT = 326;
        public const int APP_NO_WRITE_PINPAD_PORT = 327;
        public const int APP_LICENCE_NO_FILE = 328;
        public const int APP_LICENCE_CANT_READ_FILE = 329;
        public const int APP_LICENCE_CANT_WRITE_FILE = 330;
        public const int APP_NO_LICENCE_KEY = 331;
        public const int APP_INVALID_LICENCE_KEY = 332;
        public const int APP_INVALID_ACTIVATION_KEY = 333;
        public const int APP_NOT_LICENCED = 334;
        public const int QUEUE_FILE_NOT_READ = 401;
        public const int QUEUE_FILE_NOT_WRITTEN = 402;
        public const int QUEUE_ILLEGAL_TRANSACTION_TYPE = 403;
        public const int QUEUE_ID_ALREADY_IN_USE = 404;
        public const int QUEUE_ORDER_NOT_FOUND = 405;
        public const int DEVICE_BILLS_TAKEN = 250;
        public const int DEVICE_CASH_DEVICE_ERROR = 251;
        public const int DEVICE_CASHIN_ACTIVE = 252;
        public const int DEVICE_CASHUNIT_ERROR = 253;
        public const int DEVICE_DELAYED_DISPENSE = 254;
        public const int DEVICE_CMD_NOT_SUPPORTED = 255;
        public const int DEVICE_ERROR_BILLS = 256;
        public const int DEVICE_ERROR_NO_BILLS = 257;
        public const int DEVICE_EXCHANGE_ACTIVE = 258;
        public const int DEVICE_ILLEGAL_DISPENSE_ORDER = 259;
        public const int DEVICE_INPUT_REFUSED = 260;
        public const int DEVICE_INVALID_CASHUNIT = 261;
        public const int DEVICE_INVALID_CURRENCY = 262;
        public const int DEVICE_INVALID_BILL = 263;
        public const int DEVICE_INVALID_COIN = 264;
        public const int DEVICE_INVALID_DENOMINATION = 265;
        public const int DEVICE_INVALID_MIXNUMBER = 266;
        public const int DEVICE_INVALID_MIXTABLE = 267;
        public const int DEVICE_INVALID_RETRACT = 268;
        public const int DEVICE_NOT_DISPENSABLE = 269;
        public const int DEVICE_NO_BILLS = 270;
        public const int DEVICE_NO_CASHIN_STARTED = 271;
        public const int DEVICE_NO_EXCHANGE_ACTIVE = 272;
        public const int DEVICE_POSITION_LOCKED = 273;
        public const int DEVICE_RESET_REQUIRED = 274;
        public const int DEVICE_SAFEDOOR_OPEN = 275;
        public const int DEVICE_SHUTTER_CLOSED = 276;
        public const int DEVICE_SHUTTER_NOT_CLOSED = 277;
        public const int DEVICE_SHUTTER_NOT_OPEN = 278;
        public const int DEVICE_SHUTTER_OPEN = 279;
        public const int DEVICE_TOO_MANY_BILLS = 280;
        public const int DEVICE_TOO_MANY_COINS = 281;
        public const int DEVICE_UNABLE_OPEN_SHUTTER = 282;
        public const int DEVICE_UNABLE_TO_PROCESS = 283;
        public const int DEVICE_UNKNOWN = 284;
        public const int DEVICE_UVV_IN_PROCESS = 285;
        public const int DEVICE_FATAL_DISPENSE_ERROR = 286;
        public const int DEVICE_DISPENSE_IO_ERROR = 287;
        public const int DEVICE_FATAL_ASSET_CORRUPT = 288;
        public const int DEVICE_SAFEDOOR_UNLOCKED = 289;
        public const int DEVICE_SAFEDOOR_LOCKED = 290;
        public const int DEVICE_ESCROW_FULL = 291;
        public const int ALARM_DEVICE_ERROR = 3000;
        public const int ALARM_DEVICE_NOT_SUPPORTED = 3001;
        public const int PRINTER_EXTEND_NOT_SUPPORTED = 4001;
        public const int PRINTER_FIELD_ERROR = 4002;
        public const int PRINTER_FIELD_GRAPHIC = 4003;
        public const int PRINTER_FIELD_HW_ERROR = 4004;
        public const int PRINTER_FIELD_INVALID = 4005;
        public const int PRINTER_FIELD_NOT_FOUND = 4006;
        public const int PRINTER_FIELD_NOT_READ = 4007;
        public const int PRINTER_FIELD_NOT_WRITE = 4008;
        public const int PRINTER_FIELD_OVERFLOW = 4009;
        public const int PRINTER_FIELD_REQUIRED = 4010;
        public const int PRINTER_FIELD_SPEC_FAILURE = 4011;
        public const int PRINTER_FIELD__OVWR = 4012;
        public const int PRINTER_FIELD_TYPE_NOT_SUPP = 4013;
        public const int PRINTER_FLUSH_FAIL = 4014;
        public const int PRINTER_FORM_INVALID = 4015;
        public const int PRINTER_FORM_NOT_FOUND = 4016;
        public const int PRINTER_MEDIA_INVALID = 4017;
        public const int PRINTER_MEDIA_NOT_FOUND = 4018;
        public const int PRINTER_MEDIA_OVERFLOW = 4019;
        public const int PRINTER_MEDIA_SKEWED = 4020;
        public const int PRINTER_MEDIA_TURN_FAIL = 4021;
        public const int PRINTER_NO_MEDIA_PRESENT = 4022;
        public const int PRINTER_NOFORMS = 4023;
        public const int PRINTER_NOMEDIA = 4024;
        public const int PRINTER_PAPEROUT = 4025;
        public const int PRINTER_RETRACT_BIN_FULL = 4026;
        public const int CARD_DEVICE_ERROR = 5001;
        public const int CARD_DEVICE_BAD_DATA = 5002;
        public const int CARD_DEVICE_INVALID_MEDIA = 5003;
        public const int CARD_DEVICE_MEDIA_JAMMED = 5004;
        public const int CARD_DEVICE_MOTOR_BIN_FULL = 5005;
        public const int CARD_DEVICE_MOTOR_MEDIA_JAMMED = 5006;
        public const int CARD_DEVICE_MOTOR_NO_MEDIA = 5007;
        public const int CARD_DEVICE_MOTOR_SHUTTER_FAILURE = 5008;
        public const int CARD_DEVICE_NO_MEDIA = 5009;
        public const int CARD_DEVICE_NO_SECURE_CAP = 5010;
        public const int CARD_DEVICE_NO_STRIPE = 5011;
        public const int CARD_DEVICE_NO_TRACKS = 5012;
        public const int CARD_DEVICE_NOTSUPPORTED_TRACK = 5013;
        public const int CARD_DEVICE_PARITY = 5014;
        public const int CARD_DEVICE_READ_EOF = 5015;
        public const int CARD_DEVICE_READ_FAILURE = 5016;
        public const int CARD_DEVICE_READ_OTHER = 5017;
        public const int CARD_DEVICE_SHUTTER_FAILURE = 5018;
        public const int CARD_DEVICE_WRITE_FAILURE = 5019;
        public const int TIO_DEVICE_ERROR = 6001;
        public const int TIO_DEVICE_BEEP = 6002;
        public const int TIO_DEVICE_CLEAR = 6003;
        public const int TIO_DEVICE_DISPLAY = 6004;
        public const int TIO_DEVICE_LED = 6005;
        public const int TIO_DEVICE_LIGHT = 6006;
        public const int TIO_DEVICE_READ = 6007;
        public const int PIN_ACCESS_DENIED = 7001;
        public const int PIN_CRYPTNOTSUPPORTED = 7002;
        public const int PIN_DUPLICATE_KEY = 7003;
        public const int PIN_FORMAT_NOTSUPPORTED = 7004;
        public const int PIN_KEY_NO_VALUE = 7005;
        public const int PIN_KEY_NOT_FOUND = 7006;
        public const int PIN_KEYINVALID = 7007;
        public const int PIN_KEYNOTSUPPORTED = 7008;
        public const int PIN_LENGTH_ERROR = 7009;
        public const int PIN_MINIMUMLENGTH = 7010;
        public const int PIN_NO_PIN = 7011;
        public const int PIN_NOACTIVEKEYS = 7012;
        public const int PIN_NOT_ALLOWED = 7013;
        public const int PIN_NOTSUPPORTEDCAP = 7014;
        public const int PIN_READ_FAILURE = 7015;
        public const int PIN_USE_VIOLATION = 7016;
        public const int CHK_INVALID_MEDIA = 8001;
        public const int CHK_MEDIA_JAMMED = 8002;
        public const int CHK_NO_MEDIA = 8003;
        public const int CHK_NOT_SUPPORTED = 8004;
        public const int CHK_PRINTER_ERROR = 8005;
        public const int CHK_READ_FAILURE = 8006;
        public const int CHK_SWITCH_FAILURE = 8007;
    }
}
