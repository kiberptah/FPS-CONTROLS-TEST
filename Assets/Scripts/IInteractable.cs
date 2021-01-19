using UnityEngine;

public interface IInteractable
{
    //float maxRange { get; }

    void OnHoverStart(Transform _interactor);
    void OnInteract(Transform interactor);
    void OnHoverEnd(Transform _interactor);

}
