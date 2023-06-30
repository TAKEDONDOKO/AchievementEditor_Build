using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChildrenManager : MonoBehaviour
{
    //アタッチされたオブジェクトの子オブジェクトを全て格納するためのリスト
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
            if (index >= m_contents.Count)
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

    //m_contentsのインデクサ宣言(オブジェクトの名前で取得する場合)
    public GameObject this[string name]
    {
        get
        {
            //LINQとラムダ式を用いてリストに名前が「name」のオブジェクトを探す
            GameObject obj = m_contents.FirstOrDefault(x => x.name == name);
            if (obj == null)
            {
                Debug.LogError($"名前、またはタグが\"{name}\"のオブジェクトがリストに存在しません。");
            }
            return obj;
        }
    }

    public void Awake()
    {
        //自身の子オブジェクトを全て格納
        foreach(Transform obj in this.transform) m_contents.Add(obj.gameObject);   
    }

    public void Uninit()
    {
        m_contents.Clear();
    }
}
