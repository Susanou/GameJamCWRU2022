using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject MenuMusic;

    public void startGame()
    {
        SceneManager.LoadScene(1); // 1 should be the number of the castle scene
        Time.timeScale = 1f;
        //MenuMusic.SetActive(false);
    }

    public void credits()
    {
        SceneManager.LoadScene(2);
        //MenuMusic.SetActive(false);
    }

    public void startMenu()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        SceneManager.LoadScene(0); // 1 should be the number of the castle scene
        //MenuMusic.SetActive(false);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    private void EndScene(){
        SceneManager.LoadScene("End Scene");
    }
}
