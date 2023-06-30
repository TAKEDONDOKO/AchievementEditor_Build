using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorSender : SingletonMonoBehaviour<ErrorSender>
{
    [Header("帯が消えるまでの秒数")]
    [SerializeField] float m_fadeSecond = 0;
    //子オブジェクトたちを格納
    GameObject[] m_objs = new GameObject[3];
    //イージング関数に用いる経過時間
    float m_time = 0;

    Coroutine m_activeCoroutine;
    private void Awake()
    {
        //既にインスタンスが存在しているかのチェック
        if(this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        //配列に子オブジェクトを代入していく
        int i = 0;
        foreach (Transform obj in transform) m_objs[i++] = obj.gameObject;
    }

    public void SetError(string message)
    {
        //実行中のコルーチンが存在する場合、それを破棄する
        if (m_activeCoroutine != null)
        {
            StopCoroutine(m_activeCoroutine);
            m_activeCoroutine = null;
        }
        //コルーチンの開始
        m_activeCoroutine = StartCoroutine(SendError(message));
    }

    IEnumerator SendError(string message)
    {
        //テキストのセット
        m_objs[2].GetComponent<Text>().text = message;

        //α値を最大に
        m_objs[0].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        m_objs[1].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        m_objs[2].GetComponent<Text>().color = new Color(1, 1, 1, 1);
        m_time = 0;

        //少しの間はっきりと表示させる
        yield return new WaitForSeconds(1.0f);

        //フェードの開始
        while(m_objs[0].GetComponent<Image>().color.a > 0)
        {
            float alpha = 1.0f - CircOut(m_time, m_fadeSecond);
            //0.01以下になったら切り捨て
            if (alpha <= 0.01) alpha = 0;
            m_objs[0].GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            m_objs[1].GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            m_objs[2].GetComponent<Text>().color = new Color(1, 1, 1, alpha);
            m_time += Time.deltaTime;
            yield return null;
        }

        yield break;
    }

    //イージング関数"https://qiita.com/pixelflag/items/e5ddf0160781170b671b"←参考サイト
    static float CircOut(float t, float totaltime)
    {
        t = t / totaltime - 1;
        return Mathf.Sqrt(1 - t * t);
    }
}
