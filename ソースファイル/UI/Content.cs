using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Content : MonoBehaviour
{

    [SerializeField] Color m_normal;
    [SerializeField] Color m_highLight;
    [SerializeField] Color m_select;
    public bool isSelecting = false;
    // Update is called once per frame
    void Update()
    {
        Image image = transform.GetChild(1).GetComponent<Image>();
        image.color = (EventSystem.current.currentSelectedGameObject == transform.GetChild(0).gameObject) ? image.color : new Color(1, 0, 0, 0);
    }

    public void OnClick()
    {
        //PreviewUIへの情報表示
        PreviewManager.Instance.ShowStatus(gameObject);
        //選択中のコンテンツのInsertBarを表示する
        transform.GetChild(1).GetComponent<Image>().color = new Color(1, 0, 0, 1);
        //プレビューを変更する
        JsonEditor.Instance.g_Preview = new Achievement();
        //g_Previewに選択されたコンテンツをコピー
        AchievementManager.Instance.CopyAchieve(JsonEditor.Instance[gameObject.name], JsonEditor.Instance.g_Preview);
        //Edit用に選択されたコンテンツのインスタンスをg_Selectedに保存
        JsonEditor.Instance.g_Selected = JsonEditor.Instance[gameObject.name];
        Button button = GetComponent<Button>();
        
    }

}
