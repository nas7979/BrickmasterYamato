using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stop : MonoBehaviour
{
    public void Button()
    {
        Invoke("StopButton", .1f);
    }

    public void GameStop()
    {
        SceneManager.LoadScene("Title");
    }
}
