using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class EventDirector : MonoBehaviour
{
    public static Action<float> someHeal;
    public static Action<Transform> someDeath;
    public static Action<Transform, float> updateHealth;
    void Awake()
    {
        if (EventDirector.instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public static EventDirector instance;
    /*public delegate void myDelegate(float amount);
    public static event myDelegate playerHeal;*/
}
