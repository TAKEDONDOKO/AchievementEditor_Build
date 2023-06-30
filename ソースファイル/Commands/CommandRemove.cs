public class CommandRemove : Command
{
    private int m_index;
    public CommandRemove()
    {
        m_achieve = m_je[m_je.g_Preview.Description];
    }

    public override void Execute()
    {
        //���X�g����m_achieve���폜
        m_je.achievements.Remove(m_achieve);
        //�Y���A�`�[�u�����g�ȍ~�̃��X�g�̃C���f�b�N�X�ԍ����ЂƂÂ����̂ł���ɍ��킹��Number���ς���
        for (int i = m_achieve.Number; i < JsonEditor.Instance.achievements.Count;i++) JsonEditor.Instance[i].Number--;
        //���X�gUI�̕\���X�V
        ContentList.Instance.DisplayUpdate();

    }
    public override void Undo()
    {
        //m_achieve�����X�g�ɒǉ�
        m_je.achievements.Insert(m_achieve.Number,m_achieve);
        //�Y���A�`�[�u�����g�ȍ~�̃��X�g�̃C���f�b�N�X�ԍ����ЂƂÂ����̂ł���ɍ��킹��Number���ς���
        for (int i = m_achieve.Number + 1; i < JsonEditor.Instance.achievements.Count; i++) JsonEditor.Instance[i].Number++;
        JsonEditor.Instance.g_Preview = new Achievement();
        //���X�gUI�̕\���X�V
        ContentList.Instance.DisplayUpdate();
    }
}
