using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;

using CashmereDeposit.Utils;
using CashmereDeposit.ViewModels;

namespace CashmereDeposit.Models
{
    public class DepositorPrinter : PropertyChangedBase
    {
        private bool logoTest;
        public static char Esc = '\x001B';
        private DepositorLogger Log;
        private PrinterState _state;
        private SerialPort _port;
        private DispatcherTimer dispTimer = new(DispatcherPriority.Send);
        private bool _CTSHolding;

        public PrinterState State => _state;

        private readonly DepositorDBContext _depositorDBContext;
         private readonly IDeviceRepository _deviceRepository;
        private ApplicationViewModel ApplicationViewModel { get; }

        public DepositorPrinter(
          ApplicationViewModel applicationViewModel,
          DepositorLogger log,
          string portName,
          int baudRate = 9600,
          Parity parity = Parity.None,
          int databits = 8,
          StopBits stopBits = StopBits.One)
        {
            _deviceRepository = IoC.Get<IDeviceRepository>();
            _depositorDBContext = IoC.Get<DepositorDBContext>();
            ApplicationViewModel = applicationViewModel;
            Log = log;
            Log?.Info(GetType().Name, "Port Listener Initialising", "Initialisation", "Initialising the port listener");
            Directory.CreateDirectory(ApplicationViewModel.DeviceConfiguration.RECEIPT_FOLDER);
            Port = new SerialPort(portName, baudRate, parity, databits, stopBits);
            dispTimer.Interval = TimeSpan.FromSeconds(5.0);
            dispTimer.Tick += new EventHandler(dispTimer_Tick);
            dispTimer.IsEnabled = true;
            Log?.Info(GetType().Name, "Port Listener Initialising Result", "Initialisation", "SUCCESS");
        }

        private SerialPort Port
        {
            get => _port;
            set => _port = value;
        }

        public object PrintCITReceiptLock { get; set; } = new();

        public bool CTSHolding
        {
            get => _CTSHolding;
            set
            {
                if (value == _CTSHolding)
                    return;
                _CTSHolding = value;
                NotifyOfPropertyChange(nameof(CTSHolding));
                OnPrinterStateChangedEventEvent(this, new PrinterStateChangedEventArgs()
                {
                    state = new PrinterState() { HasError = value }
                });
            }
        }

        private void dispTimer_Tick(object sender, EventArgs e) => CTSHolding = GetCTSHolding();

        public void PrintCIT(CIT CIT, bool isCopy)
        {
            CITPrintout printout = new CITPrintout()
            {
                Id = GuidExt.UuidCreateSequential(),
                PrintGuid = Guid.NewGuid(),
                IsCopy = isCopy,
                Datetime = DateTime.Now
            };
            string printoutFromCIT;
            try
            {
                printoutFromCIT = GeneratePrintoutFromCIT(CIT, printout);
            }
            catch (IOException ex)
            {
                Log?.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "IOException generating CIT receipt: {0}", ex.MessageString());
                throw;
            }
            catch (NullReferenceException ex)
            {
                Log?.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "NullReferenceException generating CIT receipt: {0}", ex.MessageString());
                throw;
            }
            catch (Exception ex)
            {
                Log?.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Error generating CIT receipt: {0}", ex.MessageString());
                throw;
            }
            CIT.CITPrintouts.Add(printout);
            try
            {
                _depositorDBContext.SaveChangesAsync().Wait();
            }
            catch (ValidationException ex)
            {
                string ErrorDetail = "Error Saving to Database: " + string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message);
                foreach (var entityValidationError in ex.ValidationResult.MemberNames)
                {
                    ErrorDetail += ">validation error>";
                    //foreach (ValidationError validationError in (IEnumerable<ValidationError>) entityValidationError)
                    ErrorDetail = ErrorDetail + "ErrorMessage=>" + entityValidationError;
                }
                Console.WriteLine(ErrorDetail);
            }
            int receiptOriginalCount = ApplicationViewModel.DeviceConfiguration.CIT_RECEIPT_ORIGINAL_COUNT;
            int num = receiptOriginalCount < 1 ? 1 : receiptOriginalCount;
            for (int index = 0; index < ApplicationViewModel.DeviceConfiguration.CIT_RECEIPT_ORIGINAL_COUNT; ++index)
                Print(printoutFromCIT);
        }

        public void PrintTransaction(Transaction transaction, bool isCopy = false)
        {
            Printout printout = new Printout()
            {
                Id = GuidExt.UuidCreateSequential(),
                PrintGuid = Guid.NewGuid(),
                IsCopy = isCopy,
                Datetime = DateTime.Now
            };
            string printoutFromTransaction;
            try
            {
                printoutFromTransaction = GeneratePrintoutFromTransaction(transaction, printout);
            }
            catch (IOException ex)
            {
                Log.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "IOException generating  receipt: {0}", ex.MessageString());
                return;
            }
            catch (NullReferenceException ex)
            {
                Log.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "NullReferenceException generating  receipt: {0}>>{1}>>{2}>stack>{3}", ex.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message, ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Error generating  receipt:  {0}>>{1}>>{2}>stack>{3}", ex.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message, ex.StackTrace);
                throw;
            }
            transaction.Printouts.Add(printout);
            //depositorDBContext.SaveChangesAsync().Wait();
            int receiptOriginalCount = ApplicationViewModel.DeviceConfiguration.RECEIPT_ORIGINAL_COUNT;
            int num = receiptOriginalCount < 1 ? 1 : receiptOriginalCount;
            for (int index = 0; index < num; ++index)
                Print(printoutFromTransaction);
        }

        public void Print(string path)
        {
            try
            {
                Port.Open();
                using FileStream fileStream = File.OpenRead(path);
                Port.Write(new BinaryReader(fileStream).ReadBytes((int)fileStream.Length), 0, (int)fileStream.Length);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(GetType().Name, 90, ApplicationErrorConst.ERROR_FILE_IO.ToString(), "Print(string path) Error generating  receipt:  {0}", ex.MessageString());
            }
            finally
            {
                if (Port.IsOpen)
                    Port.Close();
            }
        }

        private string GeneratePrintoutFromTransaction(
          Transaction transaction,
          Printout printout)
        {
            TransactionText transactionText = transaction?.TxTypeNavigation?.TxTextNavigationText;
            if (transactionText == null)
                throw new NullReferenceException("GeneratePrintoutFromTransaction(): transactionText cannot be null");
            string str1 = "\r\n" + ApplicationViewModel.CashmereTranslationService?.TranslateUserText(GetType().Name + ".GeneratePrintoutFromTransaction receiptTemplate", transactionText.ReceiptTemplate, null).Replace("\r\n", "\n").Replace("\n", "\r\n");
            if (str1 == null)
                throw new NullReferenceException("GeneratePrintoutFromTransaction(): receipt_template cannot be null");
            Device device = _deviceRepository.GetByIdAsync(transaction.DeviceId).ContinueWith(x=>x.Result).Result;
            if (device == null)
                throw new NullReferenceException("GeneratePrintoutFromTransaction(): transactionDevice cannot be null");
            string str2 = str1.Replace("{device_name}", device.Name).Replace("{device_machine_name}", device.MachineName).Replace("{device_device_number}", device.DeviceNumber).Replace("{receipt_bank_name}", ApplicationViewModel.DeviceConfiguration.RECEIPT_BANK_NAME);
            string str3 = (!printout.IsCopy ? str2.Replace("{receipt_copy_text}" + Environment.NewLine, "").Replace("{receipt_copy_print_date}" + Environment.NewLine, "") : str2.Replace("{receipt_copy_text}", ApplicationViewModel.DeviceConfiguration.RECEIPT_COPY_TEXT).Replace("{receipt_copy_print_date}", "Printed on: " + DateTime.Now.ToString(ApplicationViewModel.DeviceConfiguration.RECEIPT_DATE_FORMAT))).Replace("{transactiontypelistitem_name}", transaction.TxTypeNavigation.Name);
            Transaction transaction1 = transaction;
            DateTime dateTime;
            string newValue1;
            if (transaction1 == null)
            {
                newValue1 = null;
            }
            else
            {
                dateTime = transaction1.TxEndDate.Value;
                newValue1 = dateTime.ToString(ApplicationViewModel.DeviceConfiguration.RECEIPT_DATE_FORMAT);
            }
            string str4 = str3.Replace("{tx_end_date}", newValue1);
            Transaction transaction2 = transaction;
            string newValue2;
            if (transaction2 == null)
            {
                newValue2 = null;
            }
            else
            {
                dateTime = transaction2.TxStartDate;
                newValue2 = dateTime.ToString(ApplicationViewModel.DeviceConfiguration.RECEIPT_DATE_FORMAT);
            }
            string str5 = str4.Replace("{tx_start_date}", newValue2);
            int val = transaction.TxAccountNumber.Length - ApplicationViewModel.DeviceConfiguration.RECEIPT_ACCOUNT_NUMBER_VISIBLE_DIGITS;
            string str6 = str5.Replace("{tx_account_number}", transaction.TxAccountNumber.Substring(val.Clamp(0, transaction.TxAccountNumber.Length - 1)).PadLeft(transaction.TxAccountNumber.Length, ApplicationViewModel.DeviceConfiguration.RECEIPT_ACCOUNT_NO_PAD_CHAR)).Replace("{cb_account_name}", transaction.CbAccountName);
            string str7 = transaction.TxRefAccount == null ? str6.Replace("{tx_ref_account}" + Environment.NewLine, "") : str6.Replace("{tx_ref_account}", string.Format("{0}: {1}", ApplicationViewModel.CashmereTranslationService.TranslateUserText("DevicePrinter.PrintTransaction.reference_account_number_caption", transactionText?.ReferenceAccountNumberCaption, "Reference Account"), transaction.TxRefAccount));
            string str8 = (transaction.CbRefAccountName == null ? str7.Replace("{cb_ref_account_name}" + Environment.NewLine, "") : str7.Replace("{cb_ref_account_name}", string.Format("{0}: {1}", ApplicationViewModel.CashmereTranslationService.TranslateUserText("DevicePrinter.PrintTransaction.reference_account_name_caption", transactionText?.ReferenceAccountNameCaption, "Reference Name"), transaction.CbRefAccountName))).Replace("{branch_name}", device.Branch.Name).Replace("{tx_random_number}", transaction.TxRandomNumber.Value.ToString() ?? "");
            string str9 = (transaction.CbTxNumber == null ? str8.Replace("{cb_tx_number}" + Environment.NewLine, "") : str8.Replace("{cb_tx_number}", transaction.CbTxNumber ?? "")).Replace("{tx_currency}", transaction.TxCurrency.ToUpper());
            string str10 = transaction.TxNarration == null ? str9.Replace("{tx_narration}" + Environment.NewLine, "") : str9.Replace("{tx_narration}", string.Format("{0}: {1}", ApplicationViewModel.CashmereTranslationService.TranslateUserText("DepositorPrinter.GeneratePrintoutFromTransaction.narration_caption", transactionText?.NarrationCaption, "Narration"), transaction.TxNarration));
            string str11 = transaction.FundsSource == null ? str10.Replace("{tx_funds_source}" + Environment.NewLine, "") : str10.Replace("{tx_funds_source}", string.Format("{0}: {1}", ApplicationViewModel.CashmereTranslationService.TranslateUserText("DepositorPrinter.GeneratePrintoutFromTransaction.funds_source_caption", transactionText?.FundsSourceCaption, "Funds Source"), transaction.FundsSource));
            string str12 = transaction.TxDepositorName == null ? str11.Replace("{tx_depositor_name}" + Environment.NewLine, "") : str11.Replace("{tx_depositor_name}", string.Format("{0}: {1}", ApplicationViewModel.CashmereTranslationService.TranslateUserText("DepositorPrinter.GeneratePrintoutFromTransaction.depositor_name_caption", transactionText?.DepositorNameCaption, "Depositor Name"), transaction.TxDepositorName));
            string str13 = string.IsNullOrWhiteSpace(transaction.TxIdNumber) ? str12.Replace("{tx_id_number}" + Environment.NewLine, "") : str12.Replace("{tx_id_number}", string.Format("{0}: {1}", ApplicationViewModel.CashmereTranslationService.TranslateUserText("DepositorPrinter.GeneratePrintoutFromTransaction.id_number_caption", transactionText?.IdNumberCaption, "ID Number"), transaction.TxIdNumber));
            List<string> list = (transaction.TxPhone == null ? str13.Replace("{tx_phone}" + Environment.NewLine, "") : str13.Replace("{tx_phone}", string.Format("{0}: {1}", ApplicationViewModel.CashmereTranslationService.TranslateUserText("DepositorPrinter.GeneratePrintoutFromTransaction.phone_number_caption", transactionText?.PhoneNumberCaption, "Phone"), transaction.TxPhone))).Replace("{denomination_breakdown}", GenerateDenominationBreakdown(transaction)).Replace("{Printout_UUID}", printout.PrintGuid.ToString()).Split(new string[1]
            {
          Environment.NewLine
            }, StringSplitOptions.None).ToList();
            object[] objArray = new object[4]
            {
        ApplicationViewModel.DeviceConfiguration.RECEIPT_FOLDER,
        null,
        null,
        null
            };
            dateTime = DateTime.Now;
            objArray[1] = dateTime.ToString("yyy-MM-dd HH.mm.ss.fff");
            objArray[2] = transaction.Printouts.Count;
            objArray[3] = string.Format("{0}_{1}_{2:0}", transaction?.TxAccountNumber, transaction?.TxRefAccount, transaction?.TxAmount);
            string path1 = string.Format("{0}\\[{1}]_receipt_{2}_{3}.txt", objArray);
            File.WriteAllText(path1 + "1", string.Join("\n", list.ToArray()));
            if (ApplicationViewModel.DeviceConfiguration.RECEIPT_INVERT_ORDER)
                list.Reverse();
            string str14 = string.Join("\r", list.ToArray());
            printout.PrintContent = string.Join(Environment.NewLine, list.ToArray());
            if (ApplicationViewModel.DeviceConfiguration.RECEIPT_INVERT_ORDER)
            {
                File.AppendAllText(path1, logoTest ? "\r" : str14);
                string path2 = AppDomain.CurrentDomain.BaseDirectory + ApplicationViewModel.DeviceConfiguration.RECEIPT_LOGO;
                FileIOExtentions.AppendAllBytes(path1, File.ReadAllBytes(path2));
            }
            else
            {
                string path2 = AppDomain.CurrentDomain.BaseDirectory + ApplicationViewModel.DeviceConfiguration.RECEIPT_LOGO;
                FileIOExtentions.AppendAllBytes(path1, File.ReadAllBytes(path2));
                File.AppendAllText(path1, logoTest ? "\r cutcyrcytc \r\r\r\r" : str14);
            }
            File.AppendAllText(path1, "\n\n\n\n");
            return path1;
        }

        private string GeneratePrintoutFromCIT(CIT CIT, CITPrintout printout)
        {
            lock (PrintCITReceiptLock)
            {
                try
                {
                    List<string> stringList1 = new List<string>();
                    new DepositorDBContext().Devices.FirstOrDefault(x => x.Id == CIT.DeviceId);
                    stringList1.Add(MakeTitleText(ApplicationViewModel.DeviceConfiguration.RECEIPT_BANK_NAME));
                    if (printout.IsCopy)
                        stringList1.Add("RECEIPT COPY");
                    stringList1.Add("========================");
                    stringList1.Add(ApplicationViewModel.DeviceConfiguration.RECEIPT_CIT_TITLE);
                    stringList1.Add("========================");
                    stringList1.Add("Printed on: ");
                    List<string> stringList2 = stringList1;
                    DateTime dateTime = DateTime.Now;
                    string str1 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    stringList2.Add(str1);
                    stringList1.Add("Start Date: ");
                    List<string> stringList3 = stringList1;
                    string str2;
                    if (!CIT.FromDate.HasValue)
                    {
                        dateTime = DateTime.MinValue;
                        str2 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    }
                    else
                    {
                        dateTime = CIT.FromDate.Value;
                        str2 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    }
                    stringList3.Add(str2);
                    stringList1.Add("End Date: ");
                    List<string> stringList4 = stringList1;
                    dateTime = CIT.ToDate;
                    string str3 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    stringList4.Add(str3);
                    stringList1.Add("User: ");
                    stringList1.Add(CIT.StartUserNavigation.Username);
                    stringList1.Add("Authorising User: ");
                    stringList1.Add(CIT.AuthUserNavigation.Username);
                    stringList1.Add("Branch:");
                    stringList1.Add(CIT.Device.Branch.Name);
                    stringList1.Add("Device Name:");
                    stringList1.Add(CIT.Device.Name);
                    stringList1.Add("Device Location:");
                    stringList1.Add(CIT.Device.DeviceLocation);
                    stringList1.Add("Bag Number:");
                    stringList1.Add(CIT.OldBagNumber);
                    stringList1.Add("Seal Number:");
                    stringList1.Add(CIT.SealNumber);
                    stringList1.Add("Total Transaction Count:");
                    stringList1.Add(CIT.Transactions.Count().ToString() ?? "");
                    stringList1.Add("Total Note Count:");
                    stringList1.Add(CIT.Transactions.Sum(x => x.DenominationDetails.Sum(y => y.Count)).ToString() ?? "");
                    stringList1.Add("Total Currency Count:");
                    List<string> stringList5 = stringList1;
                    int num1 = CIT.Transactions.Select(x => x.TxCurrencyNavigation).Distinct().Count();
                    string str4 = num1.ToString() ?? "";
                    stringList5.Add(str4);
                    stringList1.Add(DrawSingleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                    stringList1.Add(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                    stringList1.Add(DrawSingleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                    foreach (Currency currency1 in CIT.Transactions.Select(x => x.TxCurrencyNavigation).Distinct().ToList())
                    {
                        Currency currency = currency1;
                        List<CITDenomination> list = CIT.CITDenominations.Where(x => x.CurrencyId == currency.Code).ToList();
                        stringList1.Add("Currency:");
                        stringList1.Add(currency.Code.ToUpper());
                        stringList1.Add("Transaction Count:");
                        List<string> stringList6 = stringList1;
                        num1 = CIT.Transactions.Where(x => x.TxCurrencyNavigation == currency).Count();
                        string str5 = num1.ToString() ?? "";
                        stringList6.Add(str5);
                        stringList1.Add(DrawSingleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                        stringList1.Add(MakeTitleText(currency.Code.ToUpper() + " Denominations"));
                        stringList1.Add(DrawSingleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                        foreach (CITDenomination citDenomination in list.OrderBy(x => x.Denom))
                        {
                            num1 = citDenomination.Denom / 100;
                            string str6 = num1.ToString() ?? "";
                            long num2 = citDenomination.Count;
                            string str7 = num2.ToString() ?? "";
                            num2 = citDenomination.Denom * citDenomination.Count / 100L;
                            string str8 = num2.ToString() ?? "";
                            int num3 = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH - 11;
                            stringList1.Add(string.Format(string.Format("{{0,-6}}{{1,5}}{{2,{0}}}", num3), str6, str7, str8));
                        }
                        stringList1.Add(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                        stringList1.Add(string.Format(string.Format("{{0,-6}}" + Environment.NewLine + "{{1,{0}}}", ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH - 4), currency.Code.ToUpper() + " TOTAL:", MakeTitleText(string.Format("{0:#,#0.##}", list.Sum(x => x.Subtotal) / 100.0M))));
                        stringList1.Add(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                        try
                        {
                            ICollection<Transaction> transactions = CIT.Transactions;
                            long? nullable1;
                            if (transactions == null)
                            {
                                nullable1 = new long?();
                            }
                            else
                            {
                                IEnumerable<Transaction> source = transactions.Where(x => x.TxCurrency.Equals(currency.Code, StringComparison.OrdinalIgnoreCase));
                                nullable1 = source != null ? new long?(source.Sum(y => y.EscrowJams.Sum(z => z.RetreivedAmount))) : new long?();
                            }
                            long? nullable2 = nullable1;
                            Decimal? nullable3 = nullable2.HasValue ? new Decimal?(nullable2.GetValueOrDefault() / 100M) : new Decimal?();
                            Decimal? nullable4 = nullable3;
                            Decimal num2 = 0M;
                            if (nullable4.GetValueOrDefault() > num2 & nullable4.HasValue)
                            {
                                stringList1.Add(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                                stringList1.Add(string.Format(string.Format("{{0,-6}}" + Environment.NewLine + "{{1,{0}}}", ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH - 4), currency.Code.ToUpper() + " Recovered:", MakeTitleText(string.Format("{0:#,#0.##}", nullable3))));
                                stringList1.Add(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
                            }
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                    stringList1.Add("\n");
                    stringList1.Add("\n");
                    stringList1.Add(CIT.Id.ToString().ToUpper());
                    stringList1.Add("\n");
                    string path1 = string.Format("{0}\\[{1}]_citreceipt_{2}.txt", ApplicationViewModel.DeviceConfiguration.RECEIPT_FOLDER, DateTime.Now.ToString("yyy-MM-dd HH.mm.ss.fff"), CIT.Id);
                    File.WriteAllText(path1 + "1", string.Join("\r", stringList1.ToArray()));
                    printout.PrintContent = string.Join(Environment.NewLine, stringList1.ToArray());
                    CIT.CITPrintouts.Add(printout);
                    if (ApplicationViewModel.DeviceConfiguration.RECEIPT_INVERT_ORDER)
                        stringList1.Reverse();
                    string str9 = string.Join("\r", stringList1.ToArray());
                    if (ApplicationViewModel.DeviceConfiguration.RECEIPT_INVERT_ORDER)
                    {
                        File.AppendAllText(path1, logoTest ? "\r" : str9);
                        string path2 = AppDomain.CurrentDomain.BaseDirectory + ApplicationViewModel.DeviceConfiguration.RECEIPT_LOGO;
                        Log.Debug(GetType().Name, "GeneratePrintoutFromTransaction", "Printer", "Adding the logo from " + path2);
                        FileIOExtentions.AppendAllBytes(path1, File.ReadAllBytes(path2));
                    }
                    else
                    {
                        string path2 = AppDomain.CurrentDomain.BaseDirectory + ApplicationViewModel.DeviceConfiguration.RECEIPT_LOGO;
                        Log.Debug(GetType().Name, "GeneratePrintoutFromTransaction", "Printer", "Adding the logo from " + path2);
                        FileIOExtentions.AppendAllBytes(path1, File.ReadAllBytes(path2));
                        File.AppendAllText(path1, logoTest ? "\r" : str9);
                    }
                    File.AppendAllText(path1, "\n\n\n\n");
                    return path1;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private string DrawSingleLine(int length) => MakeTitleText("".PadLeft(length, '_'));

        private string MakeTitleText(string text) => text;

        private string DrawDoubleLine(int length) => MakeTitleText("".PadLeft(length, '='));

        private string MakeBold(string v) => string.Format("{0}E1{1}{2}E0", Esc, v, Esc);

        public bool GetCTSHolding()
        {
            bool flag = false;
            try
            {
                flag = Port.CtsHolding;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetCTSHolding() Error: {0}", string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message));
            }
            return flag;
        }

        public string GenerateDenominationBreakdown(Transaction transaction)
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            foreach (DenominationDetail denominationDetail in transaction.DenominationDetails)
            {
                string str1 = (denominationDetail.Denom / 100).ToString() ?? "";
                string str2 = denominationDetail.Count.ToString() ?? "";
                string str3 = (denominationDetail.Denom * denominationDetail.Count / 100L).ToString() ?? "";
                int num = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH - 11;
                stringBuilder1.AppendLine(string.Format(string.Format("{{0,-6}}{{1,5}}{{2, {0}}}", num), str1, str2, str3));
            }
            stringBuilder1.AppendLine(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
            StringBuilder stringBuilder2 = stringBuilder1;
            string format = string.Format("{{0,-6}}{{1, {0}}}", ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH - 6);
            long? txAmount = transaction.TxAmount;
            long num1 = 100;
            string str4 = MakeTitleText((txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num1) : new long?()).Value.ToString(ApplicationViewModel.DeviceConfiguration.APPLICATION_MONEY_FORMAT));
            string str5 = string.Format(format, "TOTAL:", str4);
            stringBuilder2.AppendLine(str5);
            stringBuilder1.AppendLine(DrawDoubleLine(ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH));
            return stringBuilder1.ToString();
        }

        public event EventHandler<PrinterStateChangedEventArgs> PrinterStateChangedEvent;

        private void OnPrinterStateChangedEventEvent(
          object sender,
          PrinterStateChangedEventArgs e)
        {
            if (PrinterStateChangedEvent == null)
                return;
            PrinterStateChangedEvent(this, e);
        }

        public class PrinterStateChangedEventArgs : EventArgs
        {
            public PrinterState state = new()
            {
                CoverOpen = false,
                HasPaper = true,
                HasError = false,
                ErrorCode = 0,
                ErrorType = PrinterErrorType.NONE,
                ErrorMessage = "No Error"
            };
        }
    }
}
