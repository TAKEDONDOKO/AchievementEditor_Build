using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//<<<※注意※ シングルトンです>>>
public class PanelManager
{

    //PanelManagerはシングルトンで作成
    private static PanelManager m_instance;
    //シングルトンだし扱いやすいので命名規則無視します。
    public static PanelManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new PanelManager();
            }
            return m_instance;
        }
    }

    //シーンに応じてUIの数も変わるためリストを採用
    private List<GameObject> m_contents = new List<GameObject>();

    //m_contentsのprivateセッター
    private GameObject Contents
    {
        set
        {
            if (m_contents.Contains(value))
            {
                Debug.LogError($"GameObject「{value.name}」は既にリストに存在しています。");
            }
            else
            {
                m_contents.Add(value);
            }
        }
    }

    //m_contentsのインデクサ宣言(インデックス番号で取得する場合)
    public GameObject this[int index]
    {
        get
        {
            if (m_contents == null)
            {
                Debug.LogError($"m_contents リストが null です。");
                return null;
            }

            if (index < 0 || index >= m_contents.Count)
            {
                Debug.LogError($"IndexOutOfRangeException: m_contents[{index}] が存在しません。");
                return null;
            }

            if (m_contents[index] == null)
            {
                Debug.LogError($"m_contents{index}の中身はnullです。");
                return null;
            }

            return m_contents[index];
        }
    }

    //m_contentsのインデクサ宣言(オブジェクトの名前もしくはタグ名で取得する場合)
    public GameObject this[string name]
    {
        get
        {
            //LINQとラムダ式を用いてリストに名前、もしくはタグが「name」のオブジェクトを探す
            GameObject obj = m_contents.FirstOrDefault(x => x.name == name || x.CompareTag(name));
            if (obj == null)
            {
                Debug.LogError($"名前、またはタグが\"{name}\"のオブジェクトがリストに存在しません。");
            }
            return obj;
        }
    }

    public void Init()
    {
        GameObject Canvas = GameObject.Find("MainPanel");
        //対応するシーンのすべてのUIをリストに追加する
        foreach (Transform obj in Canvas.transform)
        {
            Contents = obj.gameObject;
            Debug.Log($"PanelManager included {obj.name}");
        }
    }

    public void Uninit()
    {
        //リストの要素の全削除
        m_contents.Clear();
    }
}
