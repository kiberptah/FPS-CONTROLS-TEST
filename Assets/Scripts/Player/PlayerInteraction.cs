using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float range;
    Camera mainCamera;
    IInteractable currentTarget;

    void Awake()
    {
        mainCamera = Camera.main;
    }



    // Update is called once per frame
    void Update()
    {
        RaycastForInteractable();
        Interact();
    }

    void RaycastForInteractable()
    {
        RaycastHit hit;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, range))
        {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();

            if (interactable != null) // if hit interactable
            {
                //if (hit.distance <= interactable.maxRange) // if in range
                if (true)
                {
                    if (interactable == currentTarget)
                    {
                        return;
                    }
                    else
                    {
                        if (currentTarget != null)
                        {
                            currentTarget.OnHoverEnd(transform);
                            currentTarget = interactable;
                            currentTarget.OnHoverStart(transform);
                            return;
                        }
                        else
                        {
                            currentTarget = interactable;
                            currentTarget.OnHoverStart(transform);
                            return;
                        }
                    }
                }
                else // if not in range
                {
                    if (currentTarget != null)
                    {
                        currentTarget.OnHoverEnd(transform);
                        currentTarget = null;
                        return;
                    }
                }
            }
            else // if doesn't hit interactable
            {
                if (currentTarget != null)
                {
                    currentTarget.OnHoverEnd(transform);
                    currentTarget = null;
                    return;
                }
            }
        }
        else // if doesnt hit at all
        {
            if (currentTarget != null)
            {
                currentTarget.OnHoverEnd(transform);
                currentTarget = null;
                return;
            }
        }
    }

    void Interact()
    {
        if (Input.GetButtonDown("INTERACT"))
        {
            currentTarget?.OnInteract(transform); // IF NOT NULL???
        }
    }
}
