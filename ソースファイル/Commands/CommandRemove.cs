public class CommandRemove : Command
{
    private int m_index;
    public CommandRemove()
    {
        m_achieve = m_je[m_je.g_Preview.Description];
    }

    public override void Execute()
    {
        //リストからm_achieveを削除
        m_je.achievements.Remove(m_achieve);
        //該当アチーブメント以降のリストのインデックス番号がひとつづつずれるのでそれに合わせてNumberも変える
        for (int i = m_achieve.Number; i < JsonEditor.Instance.achievements.Count;i++) JsonEditor.Instance[i].Number--;
        //リストUIの表示更新
        ContentList.Instance.DisplayUpdate();

    }
    public override void Undo()
    {
        //m_achieveをリストに追加
        m_je.achievements.Insert(m_achieve.Number,m_achieve);
        //該当アチーブメント以降のリストのインデックス番号がひとつづつずれるのでそれに合わせてNumberも変える
        for (int i = m_achieve.Number + 1; i < JsonEditor.Instance.achievements.Count; i++) JsonEditor.Instance[i].Number++;
        JsonEditor.Instance.g_Preview = new Achievement();
        //リストUIの表示更新
        ContentList.Instance.DisplayUpdate();
    }
}
