
// Type: CashmereDeposit.ViewModels.FormListItemType

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System;

namespace CashmereDeposit.ViewModels
{
  [Flags]
  public enum FormListItemType
  {
    NONE = 0,
    ALPHANUMERIC = 1,
    NUMERIC = 2,
    COMBOBOX = 4,
    PASSWORD = 8,
    TEXTBOX = 16, // 0x00000010
    LISTBOX = 32, // 0x00000020
    ALPHAPASSWORD = PASSWORD | ALPHANUMERIC, // 0x00000009
    NUMERICPASSWORD = PASSWORD | NUMERIC, // 0x0000000A
    ALPHATEXTBOX = TEXTBOX | ALPHANUMERIC, // 0x00000011
    NUMERICTEXTBOX = TEXTBOX | NUMERIC, // 0x00000012
  }
}
