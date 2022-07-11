﻿
// Type: CashmereDeposit.ViewModels.FormListViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CashmereDeposit.Interfaces;

namespace CashmereDeposit.ViewModels
{
  public class FormListViewModel : Conductor<Screen>, IShell
  {
    private FormListItem selected;
    private string _nextCaption = "Next";
    private string _backCaption = "Back";

    public IDepositorForm Form { get; set; }

    public List<FormListItem> FieldList { get; set; }

    public string FormErrorText { get; set; }

    public FormListItem SelectedFieldList
    {
        get { return selected; }
        set
      {
        selected = value;
        NotifyOfPropertyChange(nameof (SelectedFieldList));
        Form.SelectFormField(value);
      }
    }

    public FormListViewModel(IDepositorForm form)
    {
      Form = form;
      FieldList = form.GetFields();
      FormErrorText = Form.GetErrors();
    }

    public string NextCaption
    {
        get { return _nextCaption; }
        set
      {
        _nextCaption = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => NextCaption));
      }
    }

    public async void Save()
    {
      string str = await Form.SaveForm();
      if (str == null)
        Form.FormClose(true);
      else
        FormErrorText = str;
    }

    public string BackCaption
    {
        get { return _backCaption; }
        set
      {
        _backCaption = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => BackCaption));
      }
    }

    public void Back()
    {
        Form.FormClose(false);
    }
  }
}