using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class EventDirector : MonoBehaviour
{
    public static Action<Transform, float> someHeal;
    public static Action<Transform> someDeath;
    public static Action<Transform, Transform, Vector3, float> someAttack;
    public static Action<Transform, Transform, Vector3, float> somePrepAttack;

    public static Action<Transform, float> updateHealth;

    public static Action<string> showInteractUI;
    public static Action hideInteractUI;

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
