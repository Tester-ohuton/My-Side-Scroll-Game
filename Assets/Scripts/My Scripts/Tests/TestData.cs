using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Data
{
    public string player_name;
    public int player_level;
    public int HP;          // HP
    public int SPD;         // �X�s�[�h
    public int ATK;         // �U����
    public int DEF;         // �h���
    public int LUK;         // �^
    public int jumpSpeed;   //�W�����v�X�s�[�h
    public float gravity;   //�d��
}
public class TestData : MonoBehaviour
{
    void Start()
    {
        Data saveData = new Data();      //1

        saveData.player_name = "���݂�����";      //2
        saveData.player_level = 1;               //2

        string a = JsonUtility.ToJson(saveData); //3

        Debug.Log(a);                            //4
    }
    //public SaveData savedata;
    public void Save(Data saveData)
    {
        StreamWriter writer;

        string jsonstr = JsonUtility.ToJson(saveData);

        writer = new StreamWriter(Application.dataPath + "/savedata.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }
    public Data Load()
    {
        if (File.Exists(Application.dataPath + "/savedata.json"))
        {
            string datastr = "";
            StreamReader reader;
            reader = new StreamReader(Application.dataPath + "/savedata.json");
            datastr = reader.ReadToEnd();
            reader.Close();

            return JsonUtility.FromJson<Data>(datastr);
        }

        Data saveData = new Data();
        saveData.HP=100;
        saveData.SPD=20;
        return saveData;
    }
    // Start is called before the first frame update
    public void PushSaveButton()
    {
        Data saveData = new Data();
        saveData.HP=150;
        saveData.SPD=10;
        Save(saveData);
    }

    // Update is called once per frame
    public void PushLoadButton()
    {
        Data saveData = Load();
        Debug.Log(saveData.HP);
        Debug.Log(saveData.SPD);
    }
}
