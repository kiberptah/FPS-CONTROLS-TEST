using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealStation : MonoBehaviour, IInteractable
{
    public float healAmount = 100f;

    //public float maxRange => 100f;

    //private const float maxRange = 100f;

    public void OnHoverStart()
    {
        //Debug.Log("READY TO INTERACT");
    }


    public void OnHoverEnd()
    {
        //Debug.Log("CANT INTERACT");
    }

        
    public void OnInteract(Transform _interactor)
    {
        //Debug.Log("DO INTERACT");
        EventDirector.someHeal(healAmount);
    }
}
