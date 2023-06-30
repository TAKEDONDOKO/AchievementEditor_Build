using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorSender : SingletonMonoBehaviour<ErrorSender>
{
    [Header("�т�������܂ł̕b��")]
    [SerializeField] float m_fadeSecond = 0;
    //�q�I�u�W�F�N�g�������i�[
    GameObject[] m_objs = new GameObject[3];
    //�C�[�W���O�֐��ɗp����o�ߎ���
    float m_time = 0;

    Coroutine m_activeCoroutine;
    private void Awake()
    {
        //���ɃC���X�^���X�����݂��Ă��邩�̃`�F�b�N
        if(this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        //�z��Ɏq�I�u�W�F�N�g�������Ă���
        int i = 0;
        foreach (Transform obj in transform) m_objs[i++] = obj.gameObject;
    }

    public void SetError(string message)
    {
        //���s���̃R���[�`�������݂���ꍇ�A�����j������
        if (m_activeCoroutine != null)
        {
            StopCoroutine(m_activeCoroutine);
            m_activeCoroutine = null;
        }
        //�R���[�`���̊J�n
        m_activeCoroutine = StartCoroutine(SendError(message));
    }

    IEnumerator SendError(string message)
    {
        //�e�L�X�g�̃Z�b�g
        m_objs[2].GetComponent<Text>().text = message;

        //���l���ő��
        m_objs[0].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        m_objs[1].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        m_objs[2].GetComponent<Text>().color = new Color(1, 1, 1, 1);
        m_time = 0;

        //�����̊Ԃ͂�����ƕ\��������
        yield return new WaitForSeconds(1.0f);

        //�t�F�[�h�̊J�n
        while(m_objs[0].GetComponent<Image>().color.a > 0)
        {
            float alpha = 1.0f - CircOut(m_time, m_fadeSecond);
            //0.01�ȉ��ɂȂ�����؂�̂�
            if (alpha <= 0.01) alpha = 0;
            m_objs[0].GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            m_objs[1].GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            m_objs[2].GetComponent<Text>().color = new Color(1, 1, 1, alpha);
            m_time += Time.deltaTime;
            yield return null;
        }

        yield break;
    }

    //�C�[�W���O�֐�"https://qiita.com/pixelflag/items/e5ddf0160781170b671b"���Q�l�T�C�g
    static float CircOut(float t, float totaltime)
    {
        t = t / totaltime - 1;
        return Mathf.Sqrt(1 - t * t);
    }
}
