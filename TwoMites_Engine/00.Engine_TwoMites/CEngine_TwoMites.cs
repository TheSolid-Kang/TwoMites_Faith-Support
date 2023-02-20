

using Engine._03.CFRPMgr;
using TwoMites_Engine._03.Mgr;

namespace TwoMites_Engine._00.CEngine_TwoMites
{
  public class CEngine_TwoMites : IDisposable
  {
    public CEngine_TwoMites()
    {
      initialize();
    }
    ~CEngine_TwoMites()
    {

    }

    private void initialize()
    {

    }
    public CBibleMgr m_bible_mgr => CBibleMgr.m_pInstance;
    public CTheWordMgr m_the_word_mgr => CTheWordMgr.m_pInstance;
    public CFellowshipMgr m_fellowship_mgr => CFellowshipMgr.m_pInstance;

    //이거랑 같은 의미다.
    public CFTPMgr m_ftp_mgr => CFTPMgr.get_instance();
    public void Dispose()
    {
    }



  }
}