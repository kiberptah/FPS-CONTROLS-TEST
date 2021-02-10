using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SceneDirector : MonoBehaviour
{

    public static SceneDirector instance;
    void Awake()
    {
        if (SceneDirector.instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void ChangeScene(string sceneName)
    {
        //Debug.Log("change scene?");
        SceneManager.LoadScene(sceneName);
    }


}
