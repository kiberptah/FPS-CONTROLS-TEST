using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public static GameDirector instance;
    void Awake()
    {
        if (GameDirector.instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }  
    }




    private void OnEnable()
    {
        EventDirector.player_Death += PlayerDeath;
    }

    private void OnDisable()
    {
        EventDirector.player_Death -= PlayerDeath;
    }




    void PlayerDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneDirector.instance.ChangeScene("RestartScreen");
    }

}
