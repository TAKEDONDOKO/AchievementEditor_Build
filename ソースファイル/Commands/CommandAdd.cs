public class CommandAdd : Command
{
    public CommandAdd()
    {
        m_achieve = m_je.g_Preview;
    }

    override public void Execute()
    {
        //�Ō���ɒǉ�����̂�List.Count���C���f�b�N�X�ԍ��ƂȂ�B
        JsonEditor.Instance.g_Preview.Number = JsonEditor.Instance.achievements.Count;
        //m_achieve��Number��ǉ��O�̃��X�g�̗v�f���ɍ��킹��
        m_achieve.Number = JsonEditor.Instance.achievements.Count;
        //m_achieve�����X�g�ɒǉ�
        m_je.achievements.Add(m_achieve);
        JsonEditor.Instance.g_Preview = new Achievement();
        //���X�gUI�̕\���X�V
        ContentList.Instance.DisplayUpdate();
    }
    public override void Undo()
    {
        //���X�g����m_achieve���폜
        m_je.achievements.Remove(m_achieve);
        //JsonEditor.Instance.g_Preview = m_achieve;
        //���X�gUI�̕\���X�V
        ContentList.Instance.DisplayUpdate();
    }
}
