using System.Linq;
using System.Collections.Generic;
using UnityEngine;

//<<<�����Ӂ� �V���O���g���ł�>>>
public class UIManager
{
    //UIManager�̓V���O���g���ō쐬
    private static UIManager m_instance;
    //�V���O���g�����������₷���̂Ŗ����K���������܂��B
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

    //�V�[���ɉ�����UI�̐����ς�邽�߃��X�g���̗p
    private List<GameObject> m_contents = new List<GameObject>();

    //m_contents��private�Z�b�^�[
    private GameObject Contents
    {
        set
        {
            if (m_contents.Contains(value))
            {
                Debug.LogError($"GameObject�u{value.name}�v�͊��Ƀ��X�g�ɑ��݂��Ă��܂��B");
            }
            else
            {
                m_contents.Add(value);
            }
        }
    }

    //m_contents�̃C���f�N�T�錾(�C���f�b�N�X�ԍ��Ŏ擾����ꍇ)
    public GameObject this[int index]
    {
        get
        {
            if(index >= m_contents.Count)
            {
                Debug.LogError($"����{index}�̓��X�g�̗e�ʂ𒴂��Ă��܂�");
            }
            else if (m_contents[index] == null)
            {
                Debug.LogError($"�C���f�b�N�X�ԍ�{index}�ɑΉ������I�u�W�F�N�g�����݂��܂���B");
                return null;
            }
            return m_contents[index];
        }
    }

    //m_contents�̃C���f�N�T�錾(�I�u�W�F�N�g�̖��O�������̓^�O���Ŏ擾����ꍇ)
    public GameObject this[string name]
    {
        get
        {
            //LINQ�ƃ����_����p���ă��X�g�ɖ��O�A�������̓^�O���uname�v�̃I�u�W�F�N�g��T��
            GameObject obj = m_contents.FirstOrDefault(x => x.name == name || x.CompareTag(name));
            if (obj == null)
            {
                Debug.LogError($"���O�A�܂��̓^�O��\"{name}\"�̃I�u�W�F�N�g�����X�g�ɑ��݂��܂���B");
            }
            return obj;
        }
    }

    public void Init()
    {
        GameObject Canvas = GameObject.Find("Canvas");
        //�Ή�����V�[���̂��ׂĂ�UI�����X�g�ɒǉ�����
        foreach (Transform obj in Canvas.transform)
        {
            Contents = obj.gameObject;
            Debug.Log($"UIManager included {obj.name}");
        }

    }

    public void Uninit()
    {
        //���X�g�̗v�f�̑S�폜
        m_contents.Clear();
    }

}
