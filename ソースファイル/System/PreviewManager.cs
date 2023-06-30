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


    //Subjectの変更
    public void OnChangeSubject(InputField str)
    {
        Debug.Log(str);
        //コマンドの実行によりインスタンスが変化している可能性があるので毎回取得する
        JsonEditor.Instance.g_Preview.Subject = str.text;
        Debug.Log(JsonEditor.Instance.g_Preview.Subject);
    }

    //Descriptionの変更
    public void OnChangeDescription(InputField str)
    {
        //コマンドの実行によりインスタンスが変化している可能性があるので毎回取得する
        JsonEditor.Instance.g_Preview.Description = str.text;
    }

    //Scoreの変更
    public void OnChangeScore(InputField str)
    {
        JsonEditor.Instance.g_Preview.Score = int.Parse(str.text);
    }

    //Secretの変更
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

    //リストUIプレハブのボタン押下用メソッド
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
