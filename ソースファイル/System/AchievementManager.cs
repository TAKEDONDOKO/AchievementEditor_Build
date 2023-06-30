using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;


//Json�t�@�C����ǂݍ��񂾂̂��A���̌^�ɂԂ�����
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

//<<<�����Ӂ��@�V���O���g��>>>
public class AchievementManager : SingletonMonoBehaviour<AchievementManager>
{

    //Json�t�@�C���ɃA�N�Z�X���邽�߂̃f�[�^�p�X
    static string filePath;

    public List<Achievement> achievements { get; private set; }

    //achievements�p�̃C���f�N�T
    public Achievement this[int index]
    {
        get
        {
            if (achievements == null)
            {
                Debug.LogError($"achievements ���X�g�� null �ł��B");
                return null;
            }

            if (index < 0 || index >= achievements.Count)
            {
                Debug.LogError($"IndexOutOfRangeException: achievements[{index}] �����݂��܂���B");
                return null;
            }
            if (achievements[index] == null)
            {
                Debug.LogError($"achievements[{index}]�̒��g��null�ł��B");
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
        //���܂͂Ƃ肠�����~�b�V�������̂��̂������������̂�JSON���g�킸�ɃA�`�[�u�����g��p�ӂ���

        achievements = new List<Achievement>();

        //�p�X�̐ݒ�
        filePath = Path.Combine(Application.dataPath, "Json", "AchieveList.json");

        //Json�t�@�C���̒��g���i�[���邽�߂̕ϐ�
        string json;
        try
        {
            //Json�t�@�C���̒��g��string�^�Ɋi�[
            json = System.IO.File.ReadAllText(filePath);
            if (json == "{}") return; //�V�K�쐬������Ԃ��̂܂܂������ꍇ�Areturn����
        }
        catch (System.IO.FileNotFoundException)
        {
            //�t�@�C�����w�肵���p�X�ɑ��݂��Ȃ��ꍇ�̗�O����
            //�����Ńt�@�C����V�K�쐬
            System.IO.File.WriteAllText(filePath, "{}");
            //Json�t�@�C���̒��g��string�^�Ɋi�[
            json = System.IO.File.ReadAllText(filePath);
            //�������牺�̏����͊���Json��"Achievement"�����݂��Ă���ꍇ�ɂ̂ݓ����̂ŃX�L�b�v����
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

    //�A�`�[�u�����g���擾�����Ƃ��̏���
    public void GetAchieve(int index)
    {
        if (!this[index].Clear)
        {
            this[index].Clear = true;
            //GameObject.Find("StageManager").GetComponent<ScoreManager>().AddScore(this[index].Score);
        }
        //MissionBoard.Instance.SetTexts();
    }


    //�A�`�[�u�����g�̃N���A�󋵂��N���A����(�A�`�[�u�����g�̎擾���Q�[���̓��_�ƂȂ邽�߃N���A�󋵂��N���A����)
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

    //�A�`�[�u�����g�̓��e���R�s�[���郁�\�b�h�B������ĂԂƃA�h���X�̈Ⴄ���g�������A�`�[�u�����g������
    //�������F�R�s�[���A�������F�R�s�[��
    public void CopyAchieve(Achievement Base,Achievement Subject)
    {
        Subject.Subject = Base.Subject;
        Subject.Description = Base.Description;
        Subject.Score = Base.Score;
        Subject.Secret = Base.Secret;
    }

    //��̈����̃A�`�[�u�����g���r���āA���e�����ׂē����ꍇtrue��Ԃ�
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
