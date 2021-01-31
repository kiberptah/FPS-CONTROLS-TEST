using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealStation : MonoBehaviour, IInteractable
{
    public float healAmount = 100f;

    //public float maxRange => 100f;

    //private const float maxRange = 100f;

    public void OnHoverStart(Transform _interactor)
    {
        //Debug.Log("READY TO INTERACT");
        EventDirector.showInteractUI("Heal");
    }


    public void OnHoverEnd(Transform _interactor)
    {
        EventDirector.hideInteractUI();
    }

        
    public void OnInteract(Transform _interactor)
    {
        //Debug.Log("DO INTERACT");
        EventDirector.someHeal(_interactor, healAmount);
    }
}
