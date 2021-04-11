using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
    public void Button()
    {
        Invoke("GameContinue", .1f);
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene("HJY");
    }
}
