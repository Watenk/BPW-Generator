using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("lvl01");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("tutorial");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
