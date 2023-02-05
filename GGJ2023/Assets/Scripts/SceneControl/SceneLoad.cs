using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    public void LoadScene(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);

    }
    public void Quit()
    {
        Debug.Log("Qiut");
        UnityEngine.Application.Quit();
    }
}
