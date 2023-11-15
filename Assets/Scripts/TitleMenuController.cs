using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _MenuControls;
    [SerializeField] private GameObject _Title;

    public void PlayGame()
    {
        //TODO: TP2 - Fix - Hardcoded value/s
        SceneManager.LoadScene("Level1");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToControlsMenu()
    {
        _Title.SetActive(false);
        _MenuControls.SetActive(true);
    }

    public void ReturnToTitle()
    {
        _MenuControls.SetActive(false);
        _Title.SetActive(true);
    }
}
