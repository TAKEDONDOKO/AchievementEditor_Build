using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;


//Jsonファイルを読み込んだのち、この型にぶち込む
[System.Serializable]
public class Achievement
{
    public string Subject = null;
    public int Number = 0;
    public string Description = null;
    public bool Secret = false;
    public bool Clear = false;
    public bool ClearThisGame = false;
    public int Score = 0;
}

[System.Serializable]
public class Achievements
{
    public List<Achievement> Achievement = new List<Achievement>();
}

//<<<※注意※　シングルトン>>>
public class AchievementManager : SingletonMonoBehaviour<AchievementManager>
{

    //Jsonファイルにアクセスするためのデータパス
    static string filePath;

    public List<Achievement> achievements { get; private set; }

    //achievements用のインデクサ
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
            return achievements.FirstOrDefault(x => x.Description == tex);
        }
    }

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
        //いまはとりあえずミッションそのものを実装したいのでJSONを使わずにアチーブメントを用意する

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

        json = System.IO.File.ReadAllText(Application.dataPath + "/Json/AchieveList.json");

        Debug.Log(json);
        

        achievements = JsonUtility.FromJson<List<Achievement>>(json);

        var obj = JObject.Parse(json);

        
        foreach (var x in obj["Achievement"].Children())
        {
            achievements.Add(x.ToObject<Achievement>());
        }

        foreach(Achievement x in achievements)
        {
            Debug.Log(x.Subject);
        }


        SceneManager.sceneLoaded += ResetClear;
    }

    //アチーブメントを取得したときの処理
    public void GetAchieve(int index)
    {
        if (!this[index].Clear)
        {
            this[index].Clear = true;
            //GameObject.Find("StageManager").GetComponent<ScoreManager>().AddScore(this[index].Score);
        }
        //MissionBoard.Instance.SetTexts();
    }


    //アチーブメントのクリア状況をクリアする(アチーブメントの取得がゲームの得点となるためクリア状況をクリアする)
    void ResetClear(Scene scene,LoadSceneMode mode)
    {
        if (scene.name == "Stage1")
        {
            foreach (Achievement achieve in achievements)
            {
                achieve.Clear = false;
            }
            //MissionBoard.Instance.SetTexts();

        }

    }

    //アチーブメントの内容をコピーするメソッド。これを呼ぶとアドレスの違う中身が同じアチーブメントが作れる
    //第一引数：コピー元、第二引数：コピー先
    public void CopyAchieve(Achievement Base,Achievement Subject)
    {
        Subject.Subject = Base.Subject;
        Subject.Description = Base.Description;
        Subject.Score = Base.Score;
        Subject.Secret = Base.Secret;
    }

    //二つの引数のアチーブメントを比較して、内容がすべて同じ場合trueを返す
    public bool CheckSameContent(Achievement Achieve1,Achievement Achieve2)
    {
        if (
            Achieve1.Subject == Achieve2.Subject &&
            Achieve1.Description == Achieve2.Description &&
            Achieve1.Score == Achieve2.Score &&
            Achieve1.Secret == Achieve2.Secret
            )
        {
            return true;
        }
        else return false;
    }
}
