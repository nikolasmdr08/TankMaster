using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenuController : MonoBehaviour
{
    //TODO: TP2 - Fix - Not set in scene
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetLevel()
    {
        string _currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(_currentSceneName);
    }
}
