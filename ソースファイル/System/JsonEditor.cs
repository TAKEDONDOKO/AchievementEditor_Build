using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;


public class JsonEditor : SingletonMonoBehaviour<JsonEditor>
{
    //リストのSubjectをスクロールバーに表示するためのプレハブ
    [SerializeField]
    GameObject m_ContentPrefab;
    //多分消すけど一応今選択しているコンテンツのリスト番号(コマンド履歴とは違うやつだから注意!!)
    public int g_CurrentIndex;
    //コマンド実行履歴を管理するためのリスト
    List<Command> m_history = new List<Command>();
    //現在のコマンドインデックスを示す変数
    public int m_current = 0;
    //アチーブメントを管理するリスト
    public List<Achievement> achievements;
    //画面右側のInputField等での情報を扱うための変数
    public Achievement g_Preview = new Achievement();
    //Edit用に選択されたアチーブメントを保存する
    public Achievement g_Selected = null;
    //Jsonファイルにアクセスするためのデータパス
    static string filePath;
    //変更点があるかどうかの判断フラグ
    public static bool isChange = false;

    //インデクサー共
    public Achievement this[int index]
    {
        get
        {
            if (achievements == null)
            {
                Debug.LogError($"achievements リストが null です。");
                return null;
            }

            if (index < 0 || index >= achievements.Count)
            {
                Debug.LogError($"IndexOutOfRangeException: achievements[{index}] が存在しません。");
                return null;
            }
            if (achievements[index] == null)
            {
                Debug.LogError($"achievements[{index}]の中身はnullです。");
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
        //DontDestroyはしない
        PanelManager.Instance.Init();

        //リストの初期化
        achievements = new List<Achievement>();

        //パスの設定
        filePath = Path.Combine(Application.dataPath, "Json", "AchieveList.json");

        //Jsonファイルの中身を格納するための変数
        string json;
        try
        {
            //Jsonファイルの中身をstring型に格納
            json = System.IO.File.ReadAllText(filePath);
            if (json == "{}") return; //新規作成した状態そのままだった場合、returnする
        }
        catch (System.IO.FileNotFoundException)
        {
            //ファイルが指定したパスに存在しない場合の例外処理
            //ここでファイルを新規作成
            System.IO.File.WriteAllText(filePath, "{}");
            //Jsonファイルの中身をstring型に格納
            json = System.IO.File.ReadAllText(filePath);
            //ここから下の処理は既にJsonに"Achievement"が存在している場合にのみ動くのでスキップする
            return;
        }

        //stringに置きなおしたJsonをAchievement型リストにしてぶっこむ
        achievements = JsonUtility.FromJson<List<Achievement>>(json);
        //パーズする
        var obj = JObject.Parse(json);

        //リストに片っ端からAddする(これが初期状態になるのでここはコマンドは使わない)
        foreach (var x in obj["Achievement"].Children())
        {
            achievements.Add(x.ToObject<Achievement>());
        }

        //リストUIの表示更新
        ContentList.Instance.DisplayUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        //Redo、Undoの実装
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Undo");
            Undo();
        }
        else if(Input.GetKeyDown(KeyCode.Y))
        {
            Redo();
        }

        //終了シーケンスの開始
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance["ExitPanel"].SetActive(true);
            UIManager.Instance["ExitPanel"].GetComponent<ExitSequence>().ReadyPanel();
        }
        
    }

    //コンテンツの編集
    public void EditContent()
    {
        if (g_Selected != null && !AchievementManager.Instance.CheckSameContent(g_Preview, g_Selected))
        {
            if (g_Preview.Subject == null ||
                g_Preview.Subject == "" ||
                g_Preview.Description == null ||
                g_Preview.Description == "")
                goto EDIT_ERROR;
            //全要素を検索して選択された要素以外でDescriptionが同じアチーブメントが存在していればエラーを渡す。(Descriptionはシングルトンが望ましいため)
            foreach (Achievement achieve in achievements) if (achieve != g_Selected && achieve.Description == g_Preview.Description) goto ERROR_DESCRIPTION;
            CommandExecute(new CommandEdit());
            g_Preview = new Achievement();
            return;
        }
    EDIT_ERROR:
        ErrorSender.Instance.SetError("アチーブメントの編集に失敗しました。アチーブメントが選択されていないか内容が変更されていない可能性があります。");
        Debug.LogError("アチーブメントの編集に失敗しました。アチーブメントが選択されていないか内容が変更されていない可能性があります。");
        //プレビューのクリア
        PreviewManager.Instance.ClearPreview();
        //編集対象をnullにする
        g_Selected = null;
        return;
    ERROR_DESCRIPTION:
        ErrorSender.Instance.SetError("アチーブメントの編集に失敗しました。変更後のDescriptionが既に他の要素に適用されています。");
        Debug.LogError("アチーブメントの編集に失敗しました。変更後のDescriptionが既に他の要素に適用されています。");
        //プレビューのクリア
        PreviewManager.Instance.ClearPreview();
        //編集対象をnullにする
        g_Selected = null;
        return;
    }

    //コンテンツの追加
    public void AddContent()
    {
        if (g_Preview.Subject == null ||
            g_Preview.Subject == "" ||
            g_Preview.Description == null || 
            g_Preview.Description == "")
        {
            ErrorSender.Instance.SetError("アチーブメントを追加するのに十分な要素を満たしていません。SubjectとDescriptionを確認してください。");
            Debug.LogError("アチーブメントを追加するのに十分な要素を満たしていません。SubjectとDescriptionを確認してください。");
            return;
        }
        foreach(Achievement achieve in achievements)
        {
            if(achieve.Description == g_Preview.Description)
            {
                ErrorSender.Instance.SetError("既に同じDescriptionが他のアチーブメントに存在しています。Descriptionはかぶらないように設定してください。");
                Debug.LogError("既に同じDescriptionが他のアチーブメントに存在しています。Descriptionはかぶらないように設定してください。");
                goto RETURN_ERROR;
            }
            if ( AchievementManager.Instance.CheckSameContent(achieve,g_Preview))
            {
                ErrorSender.Instance.SetError("このアチーブメントは既にリスト内に存在しています。最低限1つのパラメータを変更してください。");
                Debug.LogError("このアチーブメントは既にリスト内に存在しています。最低限1つのパラメータを変更してください。");
                goto RETURN_ERROR;
            }
        }
        Debug.Log("add");
        CommandExecute(new CommandAdd());
        return;

        RETURN_ERROR:
        g_Preview = new Achievement();
        //プレビューのクリア
        PreviewManager.Instance.ClearPreview();
        //編集対象をnullにする
        g_Selected = null;
        return;

    }

    //コンテンツの削除
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
        ErrorSender.Instance.SetError("このアチーブメントは既にリストに存在しません。");
        Debug.LogError("このアチーブメントは既にリストに存在しません。");
        //編集対象をnullに
        g_Selected = null;
    }

    void CommandExecute(Command pCommand)
    {
        //何か変更があればフラグが立つ
        if (!isChange)
        {
            isChange = true;
            //Applyボタンのアクティブをtrueにする
            PanelManager.Instance["Editor"].GetComponent<ChildrenManager>()["Button_Apply"].SetActive(true);
        }
        //m_current以降のコマンド履歴を削除する(Undo対策)
        m_history.RemoveRange(m_current, m_history.Count - m_current);
        //コマンドの追加
        m_history.Add(pCommand);
        //コマンドの実行
        m_history.Last().Execute();
        //カレントコマンドを最新のものにする
        m_current = m_history.Count;
        //編集対象をnullに
        g_Selected = null;
    }

    public void Redo()
    {
        //カレントコマンドがリストの最後尾出ない場合に実行
        if(m_current != m_history.Count)
        {
            m_history[m_current++].Execute();
            //編集対象をnullに
            g_Selected = null;
        }
    }
    public void Undo()
    {

        //カレントコマンドがリストの最初出ない場合に実行
        if (m_current > 0)
        {
            //何か変更があればフラグが立つ
            if (!isChange)
            {
                isChange = true;
                //Applyボタンのアクティブをtrueにする
                PanelManager.Instance["Editor"].GetComponent<ChildrenManager>()["Button_Apply"].SetActive(true);
            }
            m_history[--m_current].Undo();
            //編集対象をnullに
            g_Selected = null;
        }
    }

    public void ApplyToJson()
    {
        //isChangeをfalseにする
        isChange = false;
        //Applyボタンのアクティブをfalseにする
        PanelManager.Instance["Editor"].GetComponent<ChildrenManager>()["Button_Apply"].SetActive(false);

        Achievements data = new Achievements();
        data.Achievement = achievements;
        // Jsonファイルに書き込む
        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
        File.WriteAllText(filePath, json);
    }
}
