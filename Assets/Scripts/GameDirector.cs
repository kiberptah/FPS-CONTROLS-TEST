using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public static GameDirector instance;
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

    private void OnEnable()
    {
        EventDirector.someDeath += CheckDeath;
    }

    void Update()
    {
        
    }

    void CheckDeath(Transform ddd)
    {
        if (ddd == Player.singleton.transform)
        {
            PlayerDeath();
        }
    }

    void PlayerDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneDirector.instance.ChangeScene("RestartScreen");
    }

    private void OnDisable()
    {
        EventDirector.someDeath -= CheckDeath;
    }
}
