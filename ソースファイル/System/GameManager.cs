using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Title = 0,
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5,
    Menu,
    Result
}

public class GameManager : SingletonMonoBehaviour<GameManager>
{

    //現在のシーンを格納する変数インスタンスを取得せずとも参照できるようにstaticで宣言
    public static GameState _nowState { private set; get; }
    //UIManagerのシングルトンインスタンスを取得
    private static UIManager m_um;

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        m_um = UIManager.Instance;
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void LoadScene(GameState state)
    {
        Debug.Log(m_um);
        m_um.Uninit();
        SceneManager.LoadScene(state.ToString());
        _nowState = state;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"現在のシーン : {scene.name}");
        m_um.Init();
    }

}
