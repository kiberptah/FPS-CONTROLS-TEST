using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player singleton;
    void Awake()
    {
        if (Player.singleton != null)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
        }
        
    }
}
