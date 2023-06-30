using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewManager : SingletonMonoBehaviour<PreviewManager>
{
    Achievement achieve;
    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    //Subject�̕ύX
    public void OnChangeSubject(InputField str)
    {
        Debug.Log(str);
        //�R�}���h�̎��s�ɂ��C���X�^���X���ω����Ă���\��������̂Ŗ���擾����
        JsonEditor.Instance.g_Preview.Subject = str.text;
        Debug.Log(JsonEditor.Instance.g_Preview.Subject);
    }

    //Description�̕ύX
    public void OnChangeDescription(InputField str)
    {
        //�R�}���h�̎��s�ɂ��C���X�^���X���ω����Ă���\��������̂Ŗ���擾����
        JsonEditor.Instance.g_Preview.Description = str.text;
    }

    //Score�̕ύX
    public void OnChangeScore(InputField str)
    {
        JsonEditor.Instance.g_Preview.Score = int.Parse(str.text);
    }

    //Secret�̕ύX
    public void OnChangeSecret()
    {
        JsonEditor.Instance.g_Preview.Secret = !JsonEditor.Instance.g_Preview.Secret;
    }

    public void ClearPreview()
    {
        int i = 0;
        for(; i < 3; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<InputField>().text = null;
        }

        transform.GetChild(i).GetComponent<Toggle>().isOn = false;

        JsonEditor.Instance.g_Preview = new Achievement();

    }

    //���X�gUI�v���n�u�̃{�^�������p���\�b�h
    public void ShowStatus(GameObject obj) {
        Debug.Log(obj.name);
        Achievement achieve = JsonEditor.Instance[obj.name];
        Debug.Log(achieve.Description);
        transform.GetChild(0).GetChild(0).GetComponent<InputField>().text = achieve.Subject;
        transform.GetChild(1).GetChild(0).GetComponent<InputField>().text = achieve.Description;
        transform.GetChild(2).GetChild(0).GetComponent<InputField>().text = achieve.Score.ToString();
        transform.GetChild(3).GetComponent<Toggle>().isOn = achieve.Secret;

    }
}
