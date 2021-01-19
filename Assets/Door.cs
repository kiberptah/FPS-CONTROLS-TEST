using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, ICanChangeState
{
    
    Vector3 closePosition;
    
    //[SerializeField] [Range(0, 1)] float speed;
    [SerializeField] Vector3 openPosition = new Vector3(0, 2, 0);
    [SerializeField] Transform movingPart;

    IEnumerator open;
    IEnumerator close;

    [SerializeField] bool isOpen = false;

    private void Awake()
    {
        open = OpenDoor();
        close = CloseDoor();


        if (movingPart == null)
        {
            foreach (Transform ch in transform)
            {
                if (ch.name == "Door_MovingPart")
                {
                    movingPart = ch;
                }
            }
            
        }

        closePosition = Vector3.zero;

        if (Vector3.Distance(movingPart.transform.localPosition, openPosition) < Vector3.Distance(movingPart.transform.localPosition, closePosition))
        {
            isOpen = true;
        }
    }

    private void Start()
    {

    }

    public void ChangeState()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            StartCoroutine(OpenDoor());
        }
        else
        {
            StartCoroutine(CloseDoor());
        }
    }

    public void Open()
    {
        isOpen = true;
        StartCoroutine(OpenDoor());
    }
    public void Close()
    {
        isOpen = false;
        StartCoroutine(CloseDoor());
    }

    IEnumerator OpenDoor()
    {
        //StopCoroutine(CloseDoor());

        while (isOpen && Vector3.Distance(movingPart.transform.localPosition, openPosition) > 0.01f)
        {
            //Debug.Log("DOOR OPEN!");

            movingPart.transform.localPosition = Vector3.Lerp(movingPart.transform.localPosition, openPosition, 0.1f);
            //movingPart.transform.localPosition = openPosition;
            yield return null;

        }
        yield return null;
    }
    IEnumerator CloseDoor()
    {
        //StopCoroutine(OpenDoor());

        while (!isOpen && Vector3.Distance(movingPart.transform.localPosition, closePosition) > 0.01f)
        {

            //Debug.Log("DOOR CLOSE!");

            movingPart.transform.localPosition = Vector3.Lerp(movingPart.transform.localPosition, closePosition, 0.1f);
            //movingPart.transform.localPosition = closePosition;
            yield return null;

        }
        yield return null;
    }
}
