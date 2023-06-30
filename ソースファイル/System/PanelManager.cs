using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//<<<�����Ӂ� �V���O���g���ł�>>>
public class PanelManager
{

    //PanelManager�̓V���O���g���ō쐬
    private static PanelManager m_instance;
    //�V���O���g�����������₷���̂Ŗ����K���������܂��B
    public static PanelManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new PanelManager();
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
            if (m_contents == null)
            {
                Debug.LogError($"m_contents ���X�g�� null �ł��B");
                return null;
            }

            if (index < 0 || index >= m_contents.Count)
            {
                Debug.LogError($"IndexOutOfRangeException: m_contents[{index}] �����݂��܂���B");
                return null;
            }

            if (m_contents[index] == null)
            {
                Debug.LogError($"m_contents{index}�̒��g��null�ł��B");
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
        GameObject Canvas = GameObject.Find("MainPanel");
        //�Ή�����V�[���̂��ׂĂ�UI�����X�g�ɒǉ�����
        foreach (Transform obj in Canvas.transform)
        {
            Contents = obj.gameObject;
            Debug.Log($"PanelManager included {obj.name}");
        }
    }

    public void Uninit()
    {
        //���X�g�̗v�f�̑S�폜
        m_contents.Clear();
    }
}
