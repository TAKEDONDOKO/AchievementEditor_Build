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
        //PreviewUI�ւ̏��\��
        PreviewManager.Instance.ShowStatus(gameObject);
        //�I�𒆂̃R���e���c��InsertBar��\������
        transform.GetChild(1).GetComponent<Image>().color = new Color(1, 0, 0, 1);
        //�v���r���[��ύX����
        JsonEditor.Instance.g_Preview = new Achievement();
        //g_Preview�ɑI�����ꂽ�R���e���c���R�s�[
        AchievementManager.Instance.CopyAchieve(JsonEditor.Instance[gameObject.name], JsonEditor.Instance.g_Preview);
        //Edit�p�ɑI�����ꂽ�R���e���c�̃C���X�^���X��g_Selected�ɕۑ�
        JsonEditor.Instance.g_Selected = JsonEditor.Instance[gameObject.name];
        Button button = GetComponent<Button>();
        
    }

}
