using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor.SceneManagement;

public class SceneDirector : MonoBehaviour
{

    public static SceneDirector instance;
    void Awake()
    {
        if (Player.singleton != null)
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
        EditorSceneManager.LoadScene(sceneName);
    }


}
