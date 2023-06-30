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
        //Jsonファイルの更新を実行
        JsonEditor.Instance.ApplyToJson();
        //エディタの終了
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
            //最終確認画面に移行
            GetComponent<ChildrenManager>()[0].GetComponent<Text>().text = "保存されていない変更があります\n本当に終了しますか？";
            m_finalCheck = true;
        }
        else
        {
            //エディタの終了
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }
    }


    public void OnClickBack()
    {
        //エディタ画面に戻る
        m_finalCheck = false;
        GetComponent<ChildrenManager>()["ExitWithSave"].SetActive(false);
        GetComponent<ChildrenManager>()["Exit"].GetComponentInChildren<Text>().text = "終了";
        GetComponent<ChildrenManager>()[0].GetComponent<Text>().text = "エディタを終了しますか？";
        gameObject.SetActive(false);

    }

    public void ReadyPanel()
    {
        //変更点がある状態でシーケンスを開始した場合以下の変更を加える
        if (JsonEditor.isChange)
        {
            GetComponent<ChildrenManager>()["ExitWithSave"].SetActive(true);
            GetComponent<ChildrenManager>()["Exit"].GetComponentInChildren<Text>().text = "保存せず終了";
        }
    }

}
