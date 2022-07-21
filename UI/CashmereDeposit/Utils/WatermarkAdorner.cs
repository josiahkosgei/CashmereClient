// Decompiled with JetBrains decompiler
// Type: CashmereDeposit.Utils.WatermarkAdorner
// Assembly: CashmereDeposit, Version=6.6.5.7, Culture=neutral, PublicKeyToken=null
// MVID: 7BC9C8AC-6829-47FD-BEA6-5232B37B7616
// Assembly location: C:\DEV\maniwa\bak\New folder\6.0 - Demo\CashmereDeposit.exe

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace CashmereDeposit.Utils
{
    internal class WatermarkAdorner : Adorner
    {
        private readonly ContentPresenter contentPresenter;

        public WatermarkAdorner(UIElement adornedElement, object watermark)
          : base(adornedElement)
        {
            IsHitTestVisible = false;
            this.contentPresenter = new ContentPresenter();
            this.contentPresenter.Content = watermark;
            this.contentPresenter.Opacity = 0.5;
            ContentPresenter contentPresenter = this.contentPresenter;
            Thickness thickness1 = Control.Margin;
            double left1 = thickness1.Left;
            thickness1 = Control.Padding;
            double left2 = thickness1.Left;
            double left3 = left1 + left2;
            thickness1 = Control.Margin;
            double top1 = thickness1.Top;
            thickness1 = Control.Padding;
            double top2 = thickness1.Top;
            double top3 = top1 + top2;
            Thickness thickness2 = new Thickness(left3, top3, 0.0, 0.0);
            contentPresenter.Margin = thickness2;
            if (Control is ItemsControl && !(Control is ComboBox))
            {
                this.contentPresenter.VerticalAlignment = VerticalAlignment.Center;
                this.contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
            }
            SetBinding(VisibilityProperty, (BindingBase)new Binding("IsVisible")
            {
                Source = (object)adornedElement,
                Converter = (IValueConverter)new BooleanToVisibilityConverter()
            });
        }

        protected override int VisualChildrenCount => 1;

        private Control Control => (Control)AdornedElement;

        protected override Visual GetVisualChild(int index) => (Visual)contentPresenter;

        protected override Size MeasureOverride(Size constraint)
        {
            contentPresenter.Measure(Control.RenderSize);
            return Control.RenderSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            contentPresenter.Arrange(new Rect(finalSize));
            return finalSize;
        }
    }
}
