using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{

    public void Button()
    {
        Invoke("CreditBack", .1f);
    }
    public void CreditBack()
    {
		ObjectManager.Instance.Clear();
        SceneManager.LoadScene("Title");
    }
}
