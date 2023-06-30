using UnityEngine;
using UnityEngine.UI;

public class ExitSequence : MonoBehaviour
{

    bool m_finalCheck = false;

    // Start is called before the first frame update
    void Start()
    {
        ReadyPanel();
    }

    public void OnClickExitWithSave()
    {
        //Json�t�@�C���̍X�V�����s
        JsonEditor.Instance.ApplyToJson();
        //�G�f�B�^�̏I��
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif

    }

    public void OnClickExit()
    {
        if(JsonEditor.isChange && !m_finalCheck)
        {
            //�ŏI�m�F��ʂɈڍs
            GetComponent<ChildrenManager>()[0].GetComponent<Text>().text = "�ۑ�����Ă��Ȃ��ύX������܂�\n�{���ɏI�����܂����H";
            m_finalCheck = true;
        }
        else
        {
            //�G�f�B�^�̏I��
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }
    }


    public void OnClickBack()
    {
        //�G�f�B�^��ʂɖ߂�
        m_finalCheck = false;
        GetComponent<ChildrenManager>()["ExitWithSave"].SetActive(false);
        GetComponent<ChildrenManager>()["Exit"].GetComponentInChildren<Text>().text = "�I��";
        GetComponent<ChildrenManager>()[0].GetComponent<Text>().text = "�G�f�B�^���I�����܂����H";
        gameObject.SetActive(false);

    }

    public void ReadyPanel()
    {
        //�ύX�_�������ԂŃV�[�P���X���J�n�����ꍇ�ȉ��̕ύX��������
        if (JsonEditor.isChange)
        {
            GetComponent<ChildrenManager>()["ExitWithSave"].SetActive(true);
            GetComponent<ChildrenManager>()["Exit"].GetComponentInChildren<Text>().text = "�ۑ������I��";
        }
    }

}
