using Caliburn.Micro;
using System;
using System.Windows;

namespace CashmereDeposit.Models.Forms
{
    public class FormItem : Screen
    {
        private string name;
        private string label;
        private string _value;
        private string errorMessage;
        private Visibility valueLabelTextboxIsVisible;
        private Visibility valueTextboxIsVisible;

        public Form Form { get; set; }

        public string id { get; set; }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<string>>)(() => Name));
            }
        }

        public string Label
        {
            get => label;
            set
            {
                label = value;
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<string>>)(() => Label));
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<string>>)(() => Value));
            }
        }

        public int InputDataType { get; set; }

        public string ToolTipText { get; set; }

        public string HintText { get; set; }

        public string FullInstructionText { get; set; }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<string>>)(() => ErrorMessage));
            }
        }

        public Guid? DBReferenceItem { get; set; }

        public string ClientSideValidationID { get; set; }

        public bool IsRequired { get; internal set; }

        public Visibility ValueLabelTextboxIsVisible
        {
            get => valueLabelTextboxIsVisible;
            set
            {
                valueLabelTextboxIsVisible = value;
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<Visibility>>)(() => ValueLabelTextboxIsVisible));
            }
        }

        public Visibility ValueTextboxIsVisible
        {
            get => valueTextboxIsVisible;
            set
            {
                valueTextboxIsVisible = value;
                NotifyOfPropertyChange((System.Linq.Expressions.Expression<Func<Visibility>>)(() => ValueTextboxIsVisible));
            }
        }

        public override string ToString() => Name + " = " + Value;
    }
}
