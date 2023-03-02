using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
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
        public CEngine_TwoMites m_engineTwoMites { get; set; }
        private Mutex mutex;
        private void initialize()
        {
            m_engineTwoMites = new CEngine_TwoMites();
        }
        //프로세스 런 가드
        //https://afsdzvcx123.tistory.com/entry/WPF-WPF-%EC%A4%91%EB%B3%B5-%EC%8B%A4%ED%96%89-%EB%B0%A9%EC%A7%80-%ED%95%98%EB%8A%94%EB%B2%95
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string mutexName = "program";
            bool createNew = false;

            mutex = new Mutex(true, mutexName, out createNew);

            if( false == createNew )
            {
                System.Windows.Forms.MessageBox.Show("이미 실행중입니다.");
                Shutdown();
            }

        }
    }

}
