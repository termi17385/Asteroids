using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class HighscoreData
{
    [Serializable]
    public struct ScoreData
    {
        public string name;
        public int highScore;
    }

    public List<ScoreData> scoreData = new List<ScoreData>();
    public void SaveData(string _name, int _data)
    {
        var index = 0;
        if (Contains(_name, out index))
        {
            var score = scoreData[index];
            score.highScore = _data;
            scoreData[index] = score;
        }
        else
        {
            scoreData.Add(new ScoreData()
            {
                name = _name,
                highScore = _data
            });
        }
    }

    private bool Contains(string _key, out int _index)
    {
        for (int i = 0; i < scoreData.Count; i++)
            if (scoreData[i].name == _key)
            {
                _index = i;
                return true;
            }

        _index = 0;
        return false;
    }
}
public class HighscoreSaveSystem : MonoBehaviour
{
    //private HighscoreData hSData;
    //private int highscore;
    public static HighscoreSaveSystem Instance;
    private void Awake() => Instance = this;
    
    public void SaveData(HighscoreData _hSData)
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/Highscore.dat", FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            formatter.Serialize(fs, _hSData);
        }
        catch (SerializationException e)
        {
            Debug.LogError("Failed to save. Reason:" + e);
            throw;
        }
        finally
        {
            fs.Close();
        }
    }
    public void CreateDefaultFile()
    {
        if (File.Exists(Application.persistentDataPath + "/Highscore.dat")) return;
        
        var data = new HighscoreData();
        data.SaveData("Asteroid", 5);
        SaveData(data);
    }
    public HighscoreData LoadData()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/Highscore.dat", FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            return (HighscoreData) formatter.Deserialize(fs);
        }
        catch (SerializationException e)
        {
            Debug.LogError("Failed to load. Reason:" + e);
            return null;
        }
        finally
        {
            fs.Close();
        }
    }
}
