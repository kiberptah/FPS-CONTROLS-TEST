using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody))]

public class Elevator : MonoBehaviour
{


    [SerializeField] List<float> floorsHeight = new List<float>();
    List<Vector3> floorsCoord = new List<Vector3>();
    [SerializeField] Transform elevatorCabin;

    [Range(0, 10)]
    [SerializeField] float elevatorSpeed = 10f;

    IEnumerator elevator;

    [SerializeField] UnityEvent elevatorStart;
    [SerializeField] UnityEvent elevatorStop;


    bool isElevatorMoving = false;
    int destinationFloor;

    Vector3 destination;
    Vector3 offset;

    //Vector3 destination;

    private void Awake()
    {
        elevatorSpeed = Mathf.Clamp(elevatorSpeed * 0.1f, 0, 10);

        for (int i = 0; i < floorsHeight.Count; ++i)
        {
            floorsCoord.Add(new Vector3(transform.localPosition.x, floorsHeight[i], transform.localPosition.z));
        }
        //destination = elevatorCabin.transform.localPosition;

    }
    public void ChangeFloor(int _floor)
    {

        /*if (elevator != null)
        {
            StopCoroutine(elevator);
        }
        
        elevator = MoveElevator(_floor);
        StartCoroutine(elevator);*/


        destinationFloor = _floor;
        destination = transform.parent.TransformPoint(floorsCoord[destinationFloor]);
        offset = (destination - elevatorCabin.transform.position).normalized * elevatorSpeed;


        isElevatorMoving = true;
        elevatorStart?.Invoke();


    }
    public void StopElevator()
    {
        isElevatorMoving = false;
    }

    private void FixedUpdate()
    {
        ElevatorMovement();
    }

    void ElevatorMovement()
    {
        if (isElevatorMoving == true)
        {

            if (Vector3.Distance(elevatorCabin.transform.localPosition, floorsCoord[destinationFloor]) > 0.1f)
            {
                if (Vector3.Distance(transform.position + offset, destination) > Vector3.Distance(transform.position, destination)
                    || transform.position == destination)
                {
                    //elevatorCabin.transform.localPosition = floorsCoord[destinationFloor];
                    transform.position = destination;

                    //Debug.Log(Time.time + " elevator adjust position");
                    //rb.MovePosition(destination);
                    isElevatorMoving = false;
                    elevatorStop?.Invoke();
                }
                else
                {
                    //rb.MovePosition(transform.position + offset);
                    transform.Translate(offset);
                    //Debug.Log(Time.time + " elevator moving");

                }
            }
        }
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
