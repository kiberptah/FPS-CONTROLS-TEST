using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class InteractiveSwitch : MonoBehaviour, IInteractable
{
    //[SerializeField] List<Transform> mechanisms = new List<Transform>();
    [SerializeField] UnityEvent onButtonPress;

    public void OnHoverStart(Transform interactor)
    {
        EventDirector.showInteractUI("Interact");
        //EventDirector.test();
    }
    public void OnHoverEnd(Transform interactor)
    {
        EventDirector.hideInteractUI();
    }

    

    public void OnInteract(Transform interactor)
    {
        onButtonPress?.Invoke();
        /*foreach (Transform m in mechanisms)
        {
            m.GetComponent<ICanChangeState>().ChangeState();
        }*/
    }
}
