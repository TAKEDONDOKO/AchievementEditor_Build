public class CommandEdit : Command
{
    //�ύX�O�̓��e��ۑ����邽�߂̕ϐ�
    private Achievement m_BeforeAchieve = null;
    //�ύX��̓��e��ۑ����邽�߂̕ϐ�
    private Achievement m_AfterAchieve = null;

    public CommandEdit()
    {
        m_BeforeAchieve = new Achievement();
        m_achieve = m_je.g_Selected;
        //m_BeforeAchieve�ɓ��e���R�s�[
        AchievementManager.Instance.CopyAchieve(m_achieve, m_BeforeAchieve);
    }

    public override void Execute()
    {
        //�Y���v�f���㏑��
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

        //���X�gUI�̕\���X�V
        ContentList.Instance.DisplayUpdate();
    }

    public override void Undo()
    {
        //�Y���v�f���㏑��
        AchievementManager.Instance.CopyAchieve(m_BeforeAchieve, m_achieve);
        //���X�gUI�̕\���X�V
        ContentList.Instance.DisplayUpdate();
    }
}
