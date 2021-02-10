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

    public static Action<float> player_updateHealth;
    public static Action<float> player_TakeDamage;
    public static Action<float> player_Heal;
    public static Action player_Death;
    public static Action<float> player_ChangeResistance;
    public static Action<Vector3> player_firing;
    public static Action<bool, bool, Vector3> eventPlayerMoving;
    public static Action<Vector3> globalSpeedEvent;


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
