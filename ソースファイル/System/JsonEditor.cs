using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;


public class JsonEditor : SingletonMonoBehaviour<JsonEditor>
{
    //���X�g��Subject���X�N���[���o�[�ɕ\�����邽�߂̃v���n�u
    [SerializeField]
    GameObject m_ContentPrefab;
    //�����������ǈꉞ���I�����Ă���R���e���c�̃��X�g�ԍ�(�R�}���h�����Ƃ͈Ⴄ������璍��!!)
    public int g_CurrentIndex;
    //�R�}���h���s�������Ǘ����邽�߂̃��X�g
    List<Command> m_history = new List<Command>();
    //���݂̃R�}���h�C���f�b�N�X�������ϐ�
    public int m_current = 0;
    //�A�`�[�u�����g���Ǘ����郊�X�g
    public List<Achievement> achievements;
    //��ʉE����InputField���ł̏����������߂̕ϐ�
    public Achievement g_Preview = new Achievement();
    //Edit�p�ɑI�����ꂽ�A�`�[�u�����g��ۑ�����
    public Achievement g_Selected = null;
    //Json�t�@�C���ɃA�N�Z�X���邽�߂̃f�[�^�p�X
    static string filePath;
    //�ύX�_�����邩�ǂ����̔��f�t���O
    public static bool isChange = false;

    //�C���f�N�T�[��
    public Achievement this[int index]
    {
        get
        {
            if (achievements == null)
            {
                Debug.LogError($"achievements ���X�g�� null �ł��B");
                return null;
            }

            if (index < 0 || index >= achievements.Count)
            {
                Debug.LogError($"IndexOutOfRangeException: achievements[{index}] �����݂��܂���B");
                return null;
            }
            if (achievements[index] == null)
            {
                Debug.LogError($"achievements[{index}]�̒��g��null�ł��B");
                return null;
            }
            else
            {
                return achievements[index];
            }
        }
    }
    public Achievement this[string tex]
    {
        get
        {
            return achievements.FirstOrDefault(x => x.Description == tex || x.Subject == tex);
        }
    }


    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroy�͂��Ȃ�
        PanelManager.Instance.Init();

        //���X�g�̏�����
        achievements = new List<Achievement>();

        //�p�X�̐ݒ�
        filePath = Path.Combine(Application.dataPath, "Json", "AchieveList.json");

        //Json�t�@�C���̒��g���i�[���邽�߂̕ϐ�
        string json;
        try
        {
            //Json�t�@�C���̒��g��string�^�Ɋi�[
            json = System.IO.File.ReadAllText(filePath);
            if (json == "{}") return; //�V�K�쐬������Ԃ��̂܂܂������ꍇ�Areturn����
        }
        catch (System.IO.FileNotFoundException)
        {
            //�t�@�C�����w�肵���p�X�ɑ��݂��Ȃ��ꍇ�̗�O����
            //�����Ńt�@�C����V�K�쐬
            System.IO.File.WriteAllText(filePath, "{}");
            //Json�t�@�C���̒��g��string�^�Ɋi�[
            json = System.IO.File.ReadAllText(filePath);
            //�������牺�̏����͊���Json��"Achievement"�����݂��Ă���ꍇ�ɂ̂ݓ����̂ŃX�L�b�v����
            return;
        }

        //string�ɒu���Ȃ�����Json��Achievement�^���X�g�ɂ��ĂԂ�����
        achievements = JsonUtility.FromJson<List<Achievement>>(json);
        //�p�[�Y����
        var obj = JObject.Parse(json);

        //���X�g�ɕЂ��[����Add����(���ꂪ������ԂɂȂ�̂ł����̓R�}���h�͎g��Ȃ�)
        foreach (var x in obj["Achievement"].Children())
        {
            achievements.Add(x.ToObject<Achievement>());
        }

        //���X�gUI�̕\���X�V
        ContentList.Instance.DisplayUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        //Redo�AUndo�̎���
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Undo");
            Undo();
        }
        else if(Input.GetKeyDown(KeyCode.Y))
        {
            Redo();
        }

        //�I���V�[�P���X�̊J�n
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance["ExitPanel"].SetActive(true);
            UIManager.Instance["ExitPanel"].GetComponent<ExitSequence>().ReadyPanel();
        }
        
    }

    //�R���e���c�̕ҏW
    public void EditContent()
    {
        if (g_Selected != null && !AchievementManager.Instance.CheckSameContent(g_Preview, g_Selected))
        {
            if (g_Preview.Subject == null ||
                g_Preview.Subject == "" ||
                g_Preview.Description == null ||
                g_Preview.Description == "")
                goto EDIT_ERROR;
            //�S�v�f���������đI�����ꂽ�v�f�ȊO��Description�������A�`�[�u�����g�����݂��Ă���΃G���[��n���B(Description�̓V���O���g�����]�܂�������)
            foreach (Achievement achieve in achievements) if (achieve != g_Selected && achieve.Description == g_Preview.Description) goto ERROR_DESCRIPTION;
            CommandExecute(new CommandEdit());
            g_Preview = new Achievement();
            return;
        }
    EDIT_ERROR:
        ErrorSender.Instance.SetError("�A�`�[�u�����g�̕ҏW�Ɏ��s���܂����B�A�`�[�u�����g���I������Ă��Ȃ������e���ύX����Ă��Ȃ��\��������܂��B");
        Debug.LogError("�A�`�[�u�����g�̕ҏW�Ɏ��s���܂����B�A�`�[�u�����g���I������Ă��Ȃ������e���ύX����Ă��Ȃ��\��������܂��B");
        //�v���r���[�̃N���A
        PreviewManager.Instance.ClearPreview();
        //�ҏW�Ώۂ�null�ɂ���
        g_Selected = null;
        return;
    ERROR_DESCRIPTION:
        ErrorSender.Instance.SetError("�A�`�[�u�����g�̕ҏW�Ɏ��s���܂����B�ύX���Description�����ɑ��̗v�f�ɓK�p����Ă��܂��B");
        Debug.LogError("�A�`�[�u�����g�̕ҏW�Ɏ��s���܂����B�ύX���Description�����ɑ��̗v�f�ɓK�p����Ă��܂��B");
        //�v���r���[�̃N���A
        PreviewManager.Instance.ClearPreview();
        //�ҏW�Ώۂ�null�ɂ���
        g_Selected = null;
        return;
    }

    //�R���e���c�̒ǉ�
    public void AddContent()
    {
        if (g_Preview.Subject == null ||
            g_Preview.Subject == "" ||
            g_Preview.Description == null || 
            g_Preview.Description == "")
        {
            ErrorSender.Instance.SetError("�A�`�[�u�����g��ǉ�����̂ɏ\���ȗv�f�𖞂����Ă��܂���BSubject��Description���m�F���Ă��������B");
            Debug.LogError("�A�`�[�u�����g��ǉ�����̂ɏ\���ȗv�f�𖞂����Ă��܂���BSubject��Description���m�F���Ă��������B");
            return;
        }
        foreach(Achievement achieve in achievements)
        {
            if(achieve.Description == g_Preview.Description)
            {
                ErrorSender.Instance.SetError("���ɓ���Description�����̃A�`�[�u�����g�ɑ��݂��Ă��܂��BDescription�͂��Ԃ�Ȃ��悤�ɐݒ肵�Ă��������B");
                Debug.LogError("���ɓ���Description�����̃A�`�[�u�����g�ɑ��݂��Ă��܂��BDescription�͂��Ԃ�Ȃ��悤�ɐݒ肵�Ă��������B");
                goto RETURN_ERROR;
            }
            if ( AchievementManager.Instance.CheckSameContent(achieve,g_Preview))
            {
                ErrorSender.Instance.SetError("���̃A�`�[�u�����g�͊��Ƀ��X�g���ɑ��݂��Ă��܂��B�Œ��1�̃p�����[�^��ύX���Ă��������B");
                Debug.LogError("���̃A�`�[�u�����g�͊��Ƀ��X�g���ɑ��݂��Ă��܂��B�Œ��1�̃p�����[�^��ύX���Ă��������B");
                goto RETURN_ERROR;
            }
        }
        Debug.Log("add");
        CommandExecute(new CommandAdd());
        return;

        RETURN_ERROR:
        g_Preview = new Achievement();
        //�v���r���[�̃N���A
        PreviewManager.Instance.ClearPreview();
        //�ҏW�Ώۂ�null�ɂ���
        g_Selected = null;
        return;

    }

    //�R���e���c�̍폜
    public void RemoveContent()
    {
        foreach(Achievement achieve in achievements)
        {
            if(AchievementManager.Instance.CheckSameContent(achieve,g_Preview))
            {
                Debug.Log("remove");
                CommandExecute(new CommandRemove());
                return;
            }
        }
        ErrorSender.Instance.SetError("���̃A�`�[�u�����g�͊��Ƀ��X�g�ɑ��݂��܂���B");
        Debug.LogError("���̃A�`�[�u�����g�͊��Ƀ��X�g�ɑ��݂��܂���B");
        //�ҏW�Ώۂ�null��
        g_Selected = null;
    }

    void CommandExecute(Command pCommand)
    {
        //�����ύX������΃t���O������
        if (!isChange)
        {
            isChange = true;
            //Apply�{�^���̃A�N�e�B�u��true�ɂ���
            PanelManager.Instance["Editor"].GetComponent<ChildrenManager>()["Button_Apply"].SetActive(true);
        }
        //m_current�ȍ~�̃R�}���h�������폜����(Undo�΍�)
        m_history.RemoveRange(m_current, m_history.Count - m_current);
        //�R�}���h�̒ǉ�
        m_history.Add(pCommand);
        //�R�}���h�̎��s
        m_history.Last().Execute();
        //�J�����g�R�}���h���ŐV�̂��̂ɂ���
        m_current = m_history.Count;
        //�ҏW�Ώۂ�null��
        g_Selected = null;
    }

    public void Redo()
    {
        //�J�����g�R�}���h�����X�g�̍Ō���o�Ȃ��ꍇ�Ɏ��s
        if(m_current != m_history.Count)
        {
            m_history[m_current++].Execute();
            //�ҏW�Ώۂ�null��
            g_Selected = null;
        }
    }
    public void Undo()
    {

        //�J�����g�R�}���h�����X�g�̍ŏ��o�Ȃ��ꍇ�Ɏ��s
        if (m_current > 0)
        {
            //�����ύX������΃t���O������
            if (!isChange)
            {
                isChange = true;
                //Apply�{�^���̃A�N�e�B�u��true�ɂ���
                PanelManager.Instance["Editor"].GetComponent<ChildrenManager>()["Button_Apply"].SetActive(true);
            }
            m_history[--m_current].Undo();
            //�ҏW�Ώۂ�null��
            g_Selected = null;
        }
    }

    public void ApplyToJson()
    {
        //isChange��false�ɂ���
        isChange = false;
        //Apply�{�^���̃A�N�e�B�u��false�ɂ���
        PanelManager.Instance["Editor"].GetComponent<ChildrenManager>()["Button_Apply"].SetActive(false);

        Achievements data = new Achievements();
        data.Achievement = achievements;
        // Json�t�@�C���ɏ�������
        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
        File.WriteAllText(filePath, json);
    }
}
