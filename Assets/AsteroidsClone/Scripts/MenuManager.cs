using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Start() => Screen.SetResolution(900, 900, true);
    
    public void LoadGameScene() => SceneManager.LoadScene("GameScene");
    public void LoadMenuScene() => SceneManager.LoadScene("MainMenuScene");
    public void QuitGame() => Application.Quit();
}
