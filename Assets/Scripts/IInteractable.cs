using UnityEngine;

public interface IInteractable
{
    //float maxRange { get; }

    void OnHoverStart();
    void OnInteract(Transform interactor);
    void OnHoverEnd();

}
