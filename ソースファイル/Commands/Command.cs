abstract public class Command
{
    //�����炭�S�R�}���h�Ŏg���ł��낤�ϐ��������ɐ錾
    protected JsonEditor m_je = JsonEditor.Instance;
    protected Achievement m_achieve = null;

    ~Command() { }
    virtual public void Execute() { }
    virtual public void Undo() { }
}
