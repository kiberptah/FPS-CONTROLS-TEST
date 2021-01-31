using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class OnEnterTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent triggerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == Player.singleton.transform)
        {
            triggerEntered?.Invoke();
        }
    }
}
