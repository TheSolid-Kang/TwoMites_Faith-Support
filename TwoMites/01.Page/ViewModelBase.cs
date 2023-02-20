using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TwoMites_Engine._00.CEngine_TwoMites;

namespace TwoMites._01.Page
{
  public class ViewModelBase : IDisposable, INotifyPropertyChanged
  {
    public ViewModelBase()
    {
      initialize();
    }
    ~ViewModelBase()
    {
      Dispose();
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected CEngine_TwoMites? m_engineTwoMites { get; set; }
    protected virtual bool ThrowOnInvalidPropertyName { get; private set; }
    private void initialize()
    {
      App? app = Application.Current as App;
      m_engineTwoMites = app.m_engineTwoMites;
    }

    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    private void VerifyPropertyName(string? propertyName)
    {
      if (TypeDescriptor.GetProperties(this)[propertyName] == null)
      {
        string msg = "Invalid property name: " + propertyName;

        if (this.ThrowOnInvalidPropertyName)
          throw new Exception(msg);
        else
          Debug.Fail(msg);
      }
    }

    protected virtual void Load()
    {

    }

    protected virtual void UnLoad()
    {

    }



    protected virtual void NotifyPropertyChanged(string? propertyName = null)
    {
      try
      {
        this.VerifyPropertyName(propertyName);
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      } catch (Exception _e) {
        MessageBox.Show(_e.Message);
      }
    }

    public virtual void Dispose()
    {
      GC.SuppressFinalize(this);
    }
  }
}
