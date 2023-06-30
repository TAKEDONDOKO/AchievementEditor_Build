using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ContentList : SingletonMonoBehaviour<ContentList>
{

    //スクロールビューに表示する用のプレハブ
    [SerializeField]
    private GameObject m_contentPrefab;
    [SerializeField]
    private PreviewManager m_PM;

    //選択中のコンテンツを管理するためのリスト
    public List<Achievement> g_Selects = new List<Achievement>();

    private int m_baseIndex;

    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    //コンテンツの追加
    public void AddContent()
    {
        GameObject obj = Instantiate(m_contentPrefab);
        obj.transform.SetParent(this.transform);
        obj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "・" + JsonEditor.Instance.g_Preview.Subject;
    }

    //JsonEditor.Instance.achievementsの中身を全て表示する
    //対象のリストの中身が増減、位置変更した場合にこのメソッドを呼び出し表示更新する
    public void DisplayUpdate()
    {
        //子オブジェクトを一旦すべて破棄する
        foreach(Transform obj in this.transform)
        {
            Destroy(obj.gameObject);
        }
        foreach(Achievement achieve in JsonEditor.Instance.achievements)
        {
            //自身の子オブジェクトとして新しいオブジェクトを生成
            GameObject obj = Instantiate(m_contentPrefab, this.transform);
            //生成したプレハブのテキストをSubjectと同じものに変更
            obj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "・" + achieve.Subject;
            //一応見やすいようにオブジェクトの名前を変更
            obj.name = achieve.Subject;
        }
        m_PM.ClearPreview();
    }

    public void ClearSelects()
    {
        //ここにボタンの選択状態のカラー解除

        //リストのクリア
        g_Selects.Clear();
    }

    //Ctrl押しながら
    public void AddSelectsSingle(int Number)
    {
        //リストに要素を追加
        g_Selects.Add(JsonEditor.Instance[Number]);
        //ここにボタンの選択状態カラー設定
    }

    //Shift押しながら
    public void AddSelectsMultiple(int Number)
    {
        //要素数が0であれば真そうでなければ偽
        bool flg = g_Selects.Count == 0;

        //一括選択時の開始地点
        int baseIndex = flg ? 0 : g_Selects[0].Number;

        //for文内の第三ステートメントでインクリメントとデクリメントどちらもあり得るので変数で管理
        int direction = flg || baseIndex < Number ? 1 : -1;

        //これまでの条件をもとにfor分で一括選択
        for( int i = baseIndex; Number - i != 0 ;i += direction) g_Selects.Add(JsonEditor.Instance[i]);
        
        //ここにボタンの選択状態カラー設定
    }

    //Ctrlを押しながら要素をクリックしたときなどに使用
    public void RemoveSelects(int Number)
    {
        //ここにボタンの選択状態のカラー解除

        //リストから削除
        g_Selects.RemoveAt(Number);
    }

}
