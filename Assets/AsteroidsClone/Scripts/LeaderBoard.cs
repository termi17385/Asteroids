using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    #region Variables
    [SerializeField, Tooltip("What we want to spawn")] private GameObject prefab; 
    [SerializeField, Tooltip("Where to spawn it and set as parent")] private Transform content;
    #endregion
    
    private void Start()
   {
       HighscoreSaveSystem.Instance.CreateDefaultFile();
       LoadLeaderBoard();
   }
    /// <summary>
    /// Handles displaying the leaderboard on the main menu
    /// </summary>
    private void LoadLeaderBoard()
    {
        // sorts the data by descending order
        var data = HighscoreSaveSystem.Instance.LoadData();
        var sortedList = data.scoreData
            .OrderByDescending(_x => _x.highScore);
        
        int i = 1;
        // loops through the sorted list
        foreach (var sData in sortedList)
        {
            // makes sure that only the top 10 scores are displayed
            if(i > 10) break;
            else SpawnTile(i, sData.highScore, sData.name);                   // spawns all the other scores
            i++; // increments i
        }
    }
    /// <summary>
    /// Spawns a tile with the correct score and name
    /// </summary>
    private void SpawnTile(int _pos, int _score, string _name)
    {
        var obj = Instantiate(prefab, content);

        obj.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = _score.ToString("D8");
        obj.transform.Find("Place").GetComponent<TextMeshProUGUI>().text = _pos.ToString();
        obj.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = _name;
    }
}
