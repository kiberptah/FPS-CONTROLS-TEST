using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoRunner : MonoBehaviour
{
    public static CoRunner instance;

    private void Awake()
    {
        if (CoRunner.instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
