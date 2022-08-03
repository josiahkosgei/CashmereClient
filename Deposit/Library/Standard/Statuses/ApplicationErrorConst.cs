﻿//ApplicationErrorConst

namespace Cashmere.Library.Standard.Statuses
{
  public enum ApplicationErrorConst
  {
    ERROR_NOT_APPLICABLE = -1, // 0xFFFFFFFF
    ERROR_NONE = 0,
    ERROR_GENERAL = 1,
    ERROR_TRANSACTION_CLASS_NOT_LOADED = 2,
    ERROR_SYSTEM = 3,
    ERROR_OPERATION_NOT_SUPPORTED = 4,
    ERROR_NO_DEVICE_CONFIGURED = 5,
    ERROR_ILLEGAL_VALUE = 6,
    ERROR_LOGIN = 8,
    ERROR_PASSWORD_NO_MATCH = 9,
    ERROR_DEVICE_NOT_CONFIGURED = 10, // 0x0000000A
    ERROR_WHEN_JOURNALIZING = 11, // 0x0000000B
    ERROR_USER_PERIOD_RUNNING = 14, // 0x0000000E
    ERROR_TRANSACTIONS_PENDING = 16, // 0x00000010
    ERROR_USER_CONFIGURATION = 22, // 0x00000016
    ERROR_NO_USER_SELECTED = 24, // 0x00000018
    ERROR_EMAIL_SEND_FAILED = 25, // 0x00000019
    ERROR_NEGATIVE = 26, // 0x0000001A
    ERROR_TRANSACTION_LIMIT_EXCEEDED = 27, // 0x0000001B
    ERROR_PERMISSION_DENIED = 28, // 0x0000001C
    ERROR_WRONG_AMOUNT_DEPOSITED = 29, // 0x0000001D
    ERROR_NO_ROLE_SELECTED = 30, // 0x0000001E
    ERROR_INVALID_DATA = 31, // 0x0000001F
    ERROR_OPEN_CMD_FILE = 32, // 0x00000020
    ERROR_CANT_READ_CMD_LINE = 33, // 0x00000021
    ERROR_WRONG_FORMAT = 34, // 0x00000022
    ERROR_CANT_DELETE_CMD_FILE = 35, // 0x00000023
    ERROR_HANDLE_CMD_FILE = 36, // 0x00000024
    ERROR_HANDLE_COMMAND = 37, // 0x00000025
    ERROR_USER_NOT_LOGGED_IN = 38, // 0x00000026
    ERROR_WRITE_RESPONSE_FILE = 39, // 0x00000027
    ERROR_CLOSE_RESPONSE_FILE = 40, // 0x00000028
    ERROR_NO_TRANSACTION_MANAGER = 41, // 0x00000029
    ERROR_BUSY = 42, // 0x0000002A
    ERROR_NO_USER_LOGGED_IN = 43, // 0x0000002B
    ERROR_NO_ORDER_SELECTED = 44, // 0x0000002C
    ERROR_NO_DEVICE_SELECTED = 45, // 0x0000002D
    ERROR_DEVICE_DISABLED = 46, // 0x0000002E
    ERROR_DEVICE_LOCKED = 47, // 0x0000002F
    ERROR_DISTRESS_CONFIGURATION = 48, // 0x00000030
    ERROR_CANT_DELETE_THEMSELF = 49, // 0x00000031
    ERROR_TOO_LARGE_DEPOSIT_REQUESTED = 50, // 0x00000032
    ERROR_DEVICE_NOT_AVAILABLE = 51, // 0x00000033
    ERROR_PASSWORD_INVALID = 52, // 0x00000034
    ERROR_NOT_OWNER = 53, // 0x00000035
    ERROR_MIX_NOT_POSSIBLE = 54, // 0x00000036
    ERROR_SECOND_USER_REQUIRED = 55, // 0x00000037
    ERROR_OVERRIDE_REQUIRED = 56, // 0x00000038
    ERROR_ILLEGAL_TRANSACTION_ID = 57, // 0x00000039
    ERROR_DISPENSE_DELAYED = 58, // 0x0000003A
    ERROR_ILLEGAL_SESSION_ID = 59, // 0x0000003B
    ERROR_PASSWORD_RULE = 60, // 0x0000003C
    ERROR_DEVICE_ALREADY_CLAIMED = 61, // 0x0000003D
    ERROR_FREE_DEVICE_FAILED = 62, // 0x0000003E
    ERROR_DEVICE_ALREADY_LOCKED = 63, // 0x0000003F
    ERROR_DEVICE_ALREADY_UNLOCKED = 64, // 0x00000040
    ERROR_DEVICE_ALREADY_DISABLED = 65, // 0x00000041
    ERROR_DEVICE_ALREADY_ENABLED = 66, // 0x00000042
    ERROR_DEVICE_NOT_FOUND = 67, // 0x00000043
    ERROR_USER_NOT_FOUND = 68, // 0x00000044
    ERROR_START_AFTER_END_TIME = 69, // 0x00000045
    ERROR_START_TIME_NOT_DEFINED = 70, // 0x00000046
    ERROR_END_TIME_NOT_DEFINED = 71, // 0x00000047
    ERROR_INVALID_CASHUNIT_ID = 72, // 0x00000048
    ERROR_DEVICE_NOT_A_DISPENSER = 73, // 0x00000049
    ERROR_DEVICE_NOT_A_RECYCLER = 74, // 0x0000004A
    ERROR_PRINTER_ERROR = 75, // 0x0000004B
    ERROR_STC_NOT_ENABLED = 76, // 0x0000004C
    ERROR_STC_DOOR_NOT_CLOSED = 77, // 0x0000004D
    ERROR_INSUFFICIENT_CASH_FOR_TRANSFER = 78, // 0x0000004E
    ERROR_NO_NOTES_TO_TRANSFER = 79, // 0x0000004F
    ERROR_ORDER_NOT_FOUND = 80, // 0x00000050
    ERROR_PUSHER_PLATE_ON_TOP = 81, // 0x00000051
    ERROR_PUSHER_PLATE_CONNECTION_LOST = 82, // 0x00000052
    ERROR_UNKNOWN_PUSHER_PLATE_POSITION = 83, // 0x00000053
    ERROR_INCOMPLETE_SESSION = 84, // 0x00000054
    ERROR_INCOMPLETE_TRANSACTION = 85, // 0x00000055
    ERROR_TRANSACTION_IN_PROGRESS = 86, // 0x00000056
    ERROR_DEVICE_NOTEJAM = 87, // 0x00000057
    ERROR_DATABASE_OFFLINE = 88, // 0x00000058
    ERROR_DATABASE_GENERAL = 89, // 0x00000059
    ERROR_FILE_IO = 90, // 0x0000005A
    ERROR_TRANSACTION_POST_FAILURE = 91, // 0x0000005B
    ERROR_CORE_BANKING = 92, // 0x0000005C
    ERROR_DEVICE_IN_TRANSACTION_DURING_APP_STARTUP = 93, // 0x0000005D
    ERROR_DEVICE_BUSY = 94, // 0x0000005E
    ERROR_INCOMPLETE_CIT = 95, // 0x0000005F
    ERROR_APPLICATION_STARTUP_FAILED = 96, // 0x00000060
    ERROR_DEVICE_DOES_NOT_EXIST = 97, // 0x00000061
    ERROR_DEVICE_MANAGER_IS_NULL = 98, // 0x00000062
    ERROR_TRANSACTION_ACCOUNT_INVALID = 99, // 0x00000063
    ERROR_ALERT_SEND_FAILED = 100, // 0x00000064
    ERROR_CANT_PASS_REQUEST = 101, // 0x00000065
    ERROR_CANT_PASS_RESPONSE = 102, // 0x00000066
    ERROR_TRANSACTION_ID_NOT_FOUND = 103, // 0x00000067
    ERROR_SCREEN_RENDER_FAILED = 104, // 0x00000068
    ERROR_INVALID_OPERATION = 105, // 0x00000069
    ERROR_NULL_REFERENCE_EXCEPTION = 106, // 0x0000006A
    ERROR_DEVICE_ESCROWJAM = 107, // 0x0000006B
    TEXTTRANSLATIONERROR = 108, // 0x0000006C
    ERROR_DEVICE_COMMS = 109, // 0x0000006D
    ERROR_ACCOUNT_BLOCKED = 110, // 0x0000006E
    ERROR_AUTHENTICATION_FAILED = 111, // 0x0000006F
    ERROR_CRASH = 112, // 0x00000070
    ERROR_CIT_POST_FAILURE = 113, // 0x00000071
    ERROR_TIMEOUT = 114, // 0x00000072
    ERROR_DATABASE_SEARCH_NULL = 115, // 0x00000073
    ERROR_ARGUMENT_EXCEPTION = 116, // 0x00000074
    ERROR_CONFIG_GENERAL = 117, // 0x00000075
    WARN_NO_BLIND_EXCHANGE = 1001, // 0x000003E9
    WARN_DISPENSE_CANCELED = 1002, // 0x000003EA
    WARN_NOTHING_CHANGED = 1003, // 0x000003EB
    WARN_OWN_CASHBOX = 1004, // 0x000003EC
    WARN_NOTHING_DISPENSED = 1005, // 0x000003ED
    WARN_PROCESSING = 1006, // 0x000003EE
    WARN_NOTES_ON_RESET = 1007, // 0x000003EF
    WARN_PASSWORD_EXPIRES = 1008, // 0x000003F0
    WARN_STC_CAPACITY_EXCEEDED = 1009, // 0x000003F1
    WARN_TERMS_REJECT_BY_USER = 1010, // 0x000003F2
    WARN_DEPOSIT_CANCELED = 1011, // 0x000003F3
    WARN_LOCKING_USER = 1012, // 0x000003F4
    WARN_UNLOCKING_USER = 1013, // 0x000003F5
    WARN_DEPOSIT_TIMEOUT = 1014, // 0x000003F6
    WARN_BAGFULL = 1015, // 0x000003F7
    WARN_OVERDEPOSIT = 1016, // 0x000003F8
    WARN_OVERCOUNT = 1017, // 0x000003F9
    WARN_VALIDATION_FAILED = 1018, // 0x000003FA
    WARN_UNDERDEPOSIT = 1019, // 0x000003FB
  }
}
