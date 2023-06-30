using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ContentList : SingletonMonoBehaviour<ContentList>
{

    //�X�N���[���r���[�ɕ\������p�̃v���n�u
    [SerializeField]
    private GameObject m_contentPrefab;
    [SerializeField]
    private PreviewManager m_PM;

    //�I�𒆂̃R���e���c���Ǘ����邽�߂̃��X�g
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

    //�R���e���c�̒ǉ�
    public void AddContent()
    {
        GameObject obj = Instantiate(m_contentPrefab);
        obj.transform.SetParent(this.transform);
        obj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "�E" + JsonEditor.Instance.g_Preview.Subject;
    }

    //JsonEditor.Instance.achievements�̒��g��S�ĕ\������
    //�Ώۂ̃��X�g�̒��g�������A�ʒu�ύX�����ꍇ�ɂ��̃��\�b�h���Ăяo���\���X�V����
    public void DisplayUpdate()
    {
        //�q�I�u�W�F�N�g����U���ׂĔj������
        foreach(Transform obj in this.transform)
        {
            Destroy(obj.gameObject);
        }
        foreach(Achievement achieve in JsonEditor.Instance.achievements)
        {
            //���g�̎q�I�u�W�F�N�g�Ƃ��ĐV�����I�u�W�F�N�g�𐶐�
            GameObject obj = Instantiate(m_contentPrefab, this.transform);
            //���������v���n�u�̃e�L�X�g��Subject�Ɠ������̂ɕύX
            obj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "�E" + achieve.Subject;
            //�ꉞ���₷���悤�ɃI�u�W�F�N�g�̖��O��ύX
            obj.name = achieve.Subject;
        }
        m_PM.ClearPreview();
    }

    public void ClearSelects()
    {
        //�����Ƀ{�^���̑I����Ԃ̃J���[����

        //���X�g�̃N���A
        g_Selects.Clear();
    }

    //Ctrl�����Ȃ���
    public void AddSelectsSingle(int Number)
    {
        //���X�g�ɗv�f��ǉ�
        g_Selects.Add(JsonEditor.Instance[Number]);
        //�����Ƀ{�^���̑I����ԃJ���[�ݒ�
    }

    //Shift�����Ȃ���
    public void AddSelectsMultiple(int Number)
    {
        //�v�f����0�ł���ΐ^�����łȂ���΋U
        bool flg = g_Selects.Count == 0;

        //�ꊇ�I�����̊J�n�n�_
        int baseIndex = flg ? 0 : g_Selects[0].Number;

        //for�����̑�O�X�e�[�g�����g�ŃC���N�������g�ƃf�N�������g�ǂ�������蓾��̂ŕϐ��ŊǗ�
        int direction = flg || baseIndex < Number ? 1 : -1;

        //����܂ł̏��������Ƃ�for���ňꊇ�I��
        for( int i = baseIndex; Number - i != 0 ;i += direction) g_Selects.Add(JsonEditor.Instance[i]);
        
        //�����Ƀ{�^���̑I����ԃJ���[�ݒ�
    }

    //Ctrl�������Ȃ���v�f���N���b�N�����Ƃ��ȂǂɎg�p
    public void RemoveSelects(int Number)
    {
        //�����Ƀ{�^���̑I����Ԃ̃J���[����

        //���X�g����폜
        g_Selects.RemoveAt(Number);
    }

}
