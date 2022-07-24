using Cashmere.Library.Standard.Statuses;
using CashmereDeposit.ViewModels;
using CashmereUtil.Licensing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Caliburn.Micro;

namespace CashmereDeposit.Models.Submodule
{
    [Guid("1E83A30B-2338-43A5-AB11-EEA2190F7CAE")]
    public class CashmereTranslationService : SubmoduleBase
    {
        private readonly DepositorDBContext _depositorDBContext;
        public bool isMultiLanguage { get; } = false;

        public CashmereTranslationService(ApplicationViewModel applicationViewModel, CDMLicense license)
          : base(applicationViewModel, license, new Guid("1E83A30B-2338-43A5-AB11-EEA2190F7CAE"), nameof(CashmereTranslationService))
        {
            _depositorDBContext = IoC.Get<DepositorDBContext>();
            if (license == null)
                return;
            if (!license.Grant(new LicenseFeatureItem()
            {
                Name = nameof(CashmereTranslationService),
                Value = "TRUE",
                Enabled = true
            }))
                return;
            isMultiLanguage = true;
        }

        public string TokenReplace(string input) => input?.Replace("\\r\\n", "\\n").Replace("\\n", Environment.NewLine);

        public string TranslateSystemText(
          string caller,
          string tokenID,
          string defaultText,
          string languageCode = null)
        {
            if (tokenID == null)
            {
                ApplicationViewModel.Log.ErrorFormat(GetType().Name, 108, nameof(TranslateSystemText), "Caller = {0}: TokenID cannot be null.", caller);
            }
            else
            {
                languageCode = languageCode ?? ApplicationViewModel?.CurrentLanguage ?? "en-gb";
                if (string.IsNullOrWhiteSpace(languageCode))
                {
                    ApplicationViewModel.Log.ErrorFormat(GetType().Name, 108, nameof(TranslateSystemText), "Caller = {0}: Error Language.IsNullOrWhiteSpace()", caller);
                }
                else
                {
                    using (DepositorDBContext depositorDbContext = new DepositorDBContext())
                    {
                        try
                        {
                            SysTextItem sysTextItem = _depositorDBContext.SysTextItems.FirstOrDefault<SysTextItem>(x => x.Token == tokenID);
                            if (sysTextItem == null)
                            {
                                ApplicationViewModel.Log.WarningFormat(GetType().Name, nameof(TranslateSystemText), "TranslationError", "Caller = {0}>: sysTextItem is null in db", caller);
                            }
                            else
                            {
                                ApplicationViewModel.Log.TraceFormat(GetType().Name, nameof(TranslateSystemText), "Translating", "Caller = {0}: Translating item {1} into language {2}", caller, sysTextItem.Name, languageCode);
                                string str;
                                if (!isMultiLanguage)
                                    str = sysTextItem?.DefaultTranslation;
                                else if (sysTextItem == null)
                                {
                                    str = null;
                                }
                                else
                                {
                                    ICollection<SysTextTranslation> textTranslations = sysTextItem.SysTextTranslations;
                                    str = textTranslations != null ? textTranslations.FirstOrDefault(x => x.LanguageCode.Equals(languageCode, StringComparison.InvariantCultureIgnoreCase))?.TranslationSysText : null;
                                }
                                if (str == null)
                                    str = sysTextItem?.DefaultTranslation ?? defaultText;
                                defaultText = str;
                            }
                        }
                        catch (Exception ex)
                        {
                            ApplicationViewModel.Log.ErrorFormat(GetType().Name, 108, nameof(TranslateSystemText), "Caller = {0}: Error translating text [{1}] into language {2}: {3}>>{4}", caller, tokenID, languageCode, ex?.Message, ex?.InnerException?.Message);
                        }
                    }
                }
            }
            return TokenReplace(defaultText);
        }

        internal string TranslateUserText(
          string caller,
          Guid? textItem,
          string defaultText,
          string languageCode = null)
        {
            languageCode = languageCode ?? ApplicationViewModel?.CurrentLanguage ?? "en-gb";
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                ApplicationViewModel.Log.ErrorFormat(GetType().Name, 108, "TranslateSystemText", "Caller = {0}: Error Language.IsNullOrWhiteSpace()", caller);
            }
            else
            {
                using (DepositorDBContext depositorDbContext = new DepositorDBContext())
                {
                    try
                    {
                        if (!textItem.HasValue)
                            throw new ArgumentNullException(nameof(textItem));
                        TextItem textItem1 = _depositorDBContext.TextItems.FirstOrDefault<TextItem>(x => x.Id == textItem);
                        if (textItem1 == null)
                        {
                            ApplicationViewModel.Log.WarningFormat(GetType().Name, nameof(TranslateUserText), "TranslationError", "Caller = {0}>: sysTextItem is null in db", caller);
                        }
                        else
                        {
                            ApplicationViewModel.Log.TraceFormat(GetType().Name, nameof(TranslateUserText), "Translating", "Caller = {0}: Translating item [{1}] into language {2}", caller, textItem1.Name, languageCode);
                            string str;
                            if (!isMultiLanguage)
                                str = textItem1?.DefaultTranslation;
                            else if (textItem1 == null)
                            {
                                str = null;
                            }
                            else
                            {
                                ICollection<TextTranslation> textTranslations = textItem1.TextTranslations;
                                str = textTranslations != null ? textTranslations.FirstOrDefault<TextTranslation>(x => x.LanguageCode.Equals(languageCode, StringComparison.InvariantCultureIgnoreCase))?.TranslationText : null;
                            }
                            if (str == null)
                                str = textItem1?.DefaultTranslation ?? defaultText;
                            defaultText = str;
                        }
                        if (!(defaultText == "[Translation Error]"))
                            ;
                    }
                    catch (ArgumentNullException ex)
                    {
                        ApplicationViewModel.Log.TraceFormat(GetType().Name, nameof(TranslateUserText), ApplicationErrorConst.TEXTTRANSLATIONERROR.ToString(), "Caller = {0}: Error translating text {1} into language {2}: {3}>>{4}", caller, textItem, languageCode, ex?.Message, ex?.InnerException?.Message);
                    }
                    catch (Exception ex)
                    {
                        ApplicationViewModel.Log.ErrorFormat(GetType().Name, 108, nameof(TranslateUserText), "Caller = {0}: Error translating text {1} into language {2}: {3}>>{4}", caller, textItem, languageCode, ex?.Message, ex?.InnerException?.Message);
                    }
                }
            }
            return TokenReplace(defaultText);
        }
    }
}
