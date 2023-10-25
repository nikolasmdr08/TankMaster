using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenuController : MonoBehaviour
{
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ResetLevel()
    {
        string _currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(_currentSceneName);
    }
}
