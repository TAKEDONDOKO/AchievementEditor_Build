using System.Linq;
using System.Collections.Generic;
using UnityEngine;

//<<<※注意※ シングルトンです>>>
public class UIManager
{
    //UIManagerはシングルトンで作成
    private static UIManager m_instance;
    //シングルトンだし扱いやすいので命名規則無視します。
    public static UIManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new UIManager();
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
            if(index >= m_contents.Count)
            {
                Debug.LogError($"引数{index}はリストの容量を超えています");
            }
            else if (m_contents[index] == null)
            {
                Debug.LogError($"インデックス番号{index}に対応したオブジェクトが存在しません。");
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
        GameObject Canvas = GameObject.Find("Canvas");
        //対応するシーンのすべてのUIをリストに追加する
        foreach (Transform obj in Canvas.transform)
        {
            Contents = obj.gameObject;
            Debug.Log($"UIManager included {obj.name}");
        }

    }

    public void Uninit()
    {
        //リストの要素の全削除
        m_contents.Clear();
    }

}
