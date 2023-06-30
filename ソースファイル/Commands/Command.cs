abstract public class Command
{
    //おそらく全コマンドで使うであろう変数をここに宣言
    protected JsonEditor m_je = JsonEditor.Instance;
    protected Achievement m_achieve = null;

    ~Command() { }
    virtual public void Execute() { }
    virtual public void Undo() { }
}
