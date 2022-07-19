using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CashmereDeposit.Models.Forms
{
    public class Form : Screen
    {
        private string id;
        private string name;
        private string errorMessage;
        private string fullInstructionText;

        public string ID
        {
            get => id;
            set
            {
                id = value;
                NotifyOfPropertyChange(() => ID);
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public List<FormItem> FormItems { get; set; }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        public string FullInstructionText
        {
            get => fullInstructionText;
            set
            {
                fullInstructionText = value;
                NotifyOfPropertyChange(() => FullInstructionText);
            }
        }

        public Dictionary<string, ClientSideValidation> ClientSideValidation { get; set; }
    }
}
