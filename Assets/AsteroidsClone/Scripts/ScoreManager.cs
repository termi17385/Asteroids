using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText, currentScoreText, highScoreText;
    [SerializeField] private GameObject prefab, p_prefab, deathMenu;
    [SerializeField] private Transform content, p_content;
    [SerializeField] private bool disabled = true;
    
    private List<GameObject> livesObjects = new List<GameObject>();
    private List<HighScore> highScores = new List<HighScore>();
    [SerializeField] private int lives;
    public int CurrentScore => score;
    
    public string pName = "";
    private int score;
    private int highScore;
    
    [System.Serializable]
    public struct HighScore
    {
        public string name;
        public int data;
    }

    public void YourName(string _name) => pName = _name;
    private void Awake()
    {
        GameManager.ScoreEvent += ScoreSystem;
        GameManager.DeathEvent += LoseLife;
        GameManager.DeathEvent += DisplayPlayerLives;
        DisplayPlayerLives();
    }
    private void Start() => DisplayScores(score);
    private void OnDisable() => GameManager.ScoreEvent -= ScoreSystem;
    
    private void ScoreSystem(Transform _spawnPosition, int _score)
    {
        var obj = Resources.Load<GameObject>($"ScoreEffects/score(+{_score})");
        DisplayScores((score += _score));
        
        Instantiate(obj, _spawnPosition.position, quaternion.identity);
        Debug.Log($"scORE: {_score}");
    }

    #region Scoring
    private void LoadScores()
    {
        var scoreData = SaveSystem.instance.LoadData().scoreData;
        highScores.Clear(); // make sure list is cleared

        if(scoreData != null)
        {
            // add the loaded scores to the list            
            foreach (var data in scoreData)
            {
                highScores.Add(new HighScore()
                {
                    name = data.name,
                    data = data.highScore
                });
            }
        }
    }

    private void SaveScores(HighscoreData _data, string _currentName, int _highScore)
    {
        var hData = _data;
        hData.SaveData(_currentName, _highScore);
        SaveSystem.instance.SaveData(hData);
    }
    #endregion

    //private void Update() => DisplayPlayerLives();
    private void LoseLife()
    {
        lives -= 1;
        if (lives > 0) StartCoroutine(SpawnPlayer());   
        if (lives <= 0) StartCoroutine(GameOver());
    }
    private void DisplayScores(int _score) => scoreText.text = $"{_score:D8}";
    private void DisplayPlayerLives()
    {
        // destroys the lives in the UI if the list is
        // greater then the amount of lives the player has
        if (livesObjects.Count > lives);
        {
            for (int i = livesObjects.Count - 1; i >= 0; i--)
            {
                var obj = livesObjects[i];
                livesObjects.RemoveAt(i);
                Destroy(obj);
                if (livesObjects.Count <= lives) break;
            }
        }
        
        // spawns lives
        if(livesObjects.Count < lives)
            for (int i = 0; i < lives; i++)
            {
                var obj = Instantiate(prefab, content);
                livesObjects.Add(obj);            
            }
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSecondsRealtime(2f);
        GameManager.ScoreEvent -= ScoreSystem;
        GameManager.DeathEvent -= LoseLife;
        GameManager.DeathEvent -= DisplayPlayerLives;
            
        Cursor.lockState = CursorLockMode.None;
        lives = 0;
        
        if(!disabled) LoadScores();
            
        deathMenu.SetActive(true);
        Debug.Log("GameOver");
        currentScoreText.text = $"CurrentScore:{CurrentScore:D8}";

        bool nameExists = false;
        if (SaveSystem.instance != null)
        {
            foreach (var data in highScores)
            {
                if (data.name == pName)
                {
                    nameExists = true;
                    highScore = CurrentScore >= data.data ? CurrentScore : data.data;
                    break;
                }
                else nameExists = false;
            }

            if (!nameExists) highScore = CurrentScore;

            highScoreText.text = $"HighScore:{highScore:D8}";
            if(!disabled) SaveScores(SaveSystem.instance.LoadData(), pName, highScore);
        }
    }
    private IEnumerator SpawnPlayer()
    {
        yield return new WaitForSecondsRealtime(4f);
        var player = Instantiate(p_prefab, p_content);
        player.SetActive(true);
    }
}
