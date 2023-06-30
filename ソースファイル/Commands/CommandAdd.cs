public class CommandAdd : Command
{
    public CommandAdd()
    {
        m_achieve = m_je.g_Preview;
    }

    override public void Execute()
    {
        //最後尾に追加するのでList.Countがインデックス番号となる。
        JsonEditor.Instance.g_Preview.Number = JsonEditor.Instance.achievements.Count;
        //m_achieveのNumberを追加前のリストの要素数に合わせる
        m_achieve.Number = JsonEditor.Instance.achievements.Count;
        //m_achieveをリストに追加
        m_je.achievements.Add(m_achieve);
        JsonEditor.Instance.g_Preview = new Achievement();
        //リストUIの表示更新
        ContentList.Instance.DisplayUpdate();
    }
    public override void Undo()
    {
        //リストからm_achieveを削除
        m_je.achievements.Remove(m_achieve);
        //JsonEditor.Instance.g_Preview = m_achieve;
        //リストUIの表示更新
        ContentList.Instance.DisplayUpdate();
    }
}
