public class CommandEdit : Command
{
    //変更前の内容を保存するための変数
    private Achievement m_BeforeAchieve = null;
    //変更後の内容を保存するための変数
    private Achievement m_AfterAchieve = null;

    public CommandEdit()
    {
        m_BeforeAchieve = new Achievement();
        m_achieve = m_je.g_Selected;
        //m_BeforeAchieveに内容をコピー
        AchievementManager.Instance.CopyAchieve(m_achieve, m_BeforeAchieve);
    }

    public override void Execute()
    {
        //該当要素を上書き
        if (m_AfterAchieve == null)
        {
            AchievementManager.Instance.CopyAchieve(m_je.g_Preview, m_achieve);
            m_AfterAchieve = new Achievement();
            AchievementManager.Instance.CopyAchieve(m_achieve, m_AfterAchieve);
        }
        else
        {
            AchievementManager.Instance.CopyAchieve(m_AfterAchieve, m_achieve);
        }

        //リストUIの表示更新
        ContentList.Instance.DisplayUpdate();
    }

    public override void Undo()
    {
        //該当要素を上書き
        AchievementManager.Instance.CopyAchieve(m_BeforeAchieve, m_achieve);
        //リストUIの表示更新
        ContentList.Instance.DisplayUpdate();
    }
}
