using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void Button()
    {
        Invoke("Startgame", .1f);
    }
    public void GameStart()
    {
        SceneManager.LoadScene("Loading");
    }

}
