using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;


public class Elevator : MonoBehaviour
{
    [SerializeField] List<float> floorsHeight = new List<float>();
    List<Vector3> floorsCoord = new List<Vector3>();
    [SerializeField] Transform elevatorCabin;
    [SerializeField] float elevatorSpeed = 10f;

    IEnumerator elevator;

    [SerializeField] UnityEvent elevatorStart;
    [SerializeField] UnityEvent elevatorStop;


    //Vector3 destination;

    private void Awake()
    {
        for (int i = 0; i < floorsHeight.Count; ++i)
        {
            floorsCoord.Add(new Vector3(0, floorsHeight[i], 0));
        }
        //destination = elevatorCabin.transform.localPosition;
        
    }
    public void ChangeFloor(int _floor)
    {
        //destination = floorsCoord[_floor];
        if (elevator != null)
        {
            StopCoroutine(elevator);
        }
        
        elevator = MoveElevator(_floor);
        StartCoroutine(elevator);
    }

    private void FixedUpdate()
    {

        /*if (Vector3.Distance(elevatorCabin.transform.localPosition, destination) > 0.01f)
        {
            elevatorCabin.transform.Translate((destination - elevatorCabin.transform.localPosition).normalized * Time.deltaTime * elevatorSpeed);
        }
        if (Vector3.Distance(elevatorCabin.transform.localPosition, destination) < 0.01f && Vector3.Distance(elevatorCabin.transform.localPosition, destination) != 0)
        {
            elevatorCabin.transform.localPosition = destination;
        }*/
    }

    IEnumerator MoveElevator(int _floor)
    {
        elevatorStart?.Invoke();

        while (Vector3.Distance(elevatorCabin.transform.localPosition, floorsCoord[_floor]) > 0.1f)
        {
            Vector3 offset = elevatorCabin.transform.localPosition + (floorsCoord[_floor] - elevatorCabin.transform.localPosition).normalized * elevatorSpeed;
            //elevatorCabin.transform.Translate((floorsCoord[_floor] - elevatorCabin.transform.localPosition).normalized * Time.deltaTime * elevatorSpeed);
            elevatorCabin.transform.localPosition = Vector3.Lerp(elevatorCabin.transform.localPosition, 
                offset, 0.1f);

            yield return new WaitForFixedUpdate();

        }
        if (Vector3.Distance(elevatorCabin.transform.localPosition, floorsCoord[_floor]) < 0.1f && Vector3.Distance(elevatorCabin.transform.localPosition, floorsCoord[_floor]) != 0)
        {
            elevatorCabin.transform.localPosition = floorsCoord[_floor];
        }

        elevatorStop?.Invoke();
        //yield return new WaitForFixedUpdate();
        yield return null;
    }
}
