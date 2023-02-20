using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TwoMites_Engine._00.CEngine_TwoMites;

namespace TwoMites
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public App()
    {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(App));
      initialize();
    }
    private void initialize()
    {
      m_engineTwoMites = new CEngine_TwoMites();
    }
    public CEngine_TwoMites m_engineTwoMites { get; set; }
  }

}
