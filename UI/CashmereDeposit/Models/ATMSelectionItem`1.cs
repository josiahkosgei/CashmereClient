
// Type: CashmereDeposit.Models.ATMSelectionItem`1




using System;
using System.Windows.Media.Imaging;
using Cashmere.Library.Standard.Utilities;

namespace CashmereDeposit.Models
{
  public class ATMSelectionItem<T>
  {
    public BitmapImage Image { get; set; }

    public string ImageContent { get; set; }

    public string SelectionText { get; set; }

    public T Value { get; set; }

    public ATMSelectionItem(string imageContent, string selectionText, T value)
    {
      ImageContent = imageContent.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory).Replace("{ResourceDir}", "pack://application:,,,");
      Image = new BitmapImage(ImageContent.ToURI());
      SelectionText = selectionText;
      Value = value;
    }

    public ATMSelectionItem(BitmapImage image, string selectionText, T value)
    {
      Image = image;
      SelectionText = selectionText;
      Value = value;
    }
  }
}
