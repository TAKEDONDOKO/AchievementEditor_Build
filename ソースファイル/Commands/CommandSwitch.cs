
public class CommandSwitch : Command
{
    //元の位置のNumberを保存
    private int m_initialNumber;
    private int m_afterNumber;


    public CommandSwitch()
    {
        //ここ難しい...
        m_achieve = m_je.g_Selected;
    }
}
