using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChildrenManager : MonoBehaviour
{
    //�A�^�b�`���ꂽ�I�u�W�F�N�g�̎q�I�u�W�F�N�g��S�Ċi�[���邽�߂̃��X�g
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
            if (index >= m_contents.Count)
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

    //m_contents�̃C���f�N�T�錾(�I�u�W�F�N�g�̖��O�Ŏ擾����ꍇ)
    public GameObject this[string name]
    {
        get
        {
            //LINQ�ƃ����_����p���ă��X�g�ɖ��O���uname�v�̃I�u�W�F�N�g��T��
            GameObject obj = m_contents.FirstOrDefault(x => x.name == name);
            if (obj == null)
            {
                Debug.LogError($"���O�A�܂��̓^�O��\"{name}\"�̃I�u�W�F�N�g�����X�g�ɑ��݂��܂���B");
            }
            return obj;
        }
    }

    public void Awake()
    {
        //���g�̎q�I�u�W�F�N�g��S�Ċi�[
        foreach(Transform obj in this.transform) m_contents.Add(obj.gameObject);   
    }

    public void Uninit()
    {
        m_contents.Clear();
    }
}
