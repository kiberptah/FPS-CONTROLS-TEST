using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_Teleportation : MonoBehaviour
{

    [SerializeField]
    List<GameObject> teleportPosition = new List<GameObject>();
    void Update()
    {
        TeleportToPosition();
    }

    void TeleportToPosition()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            transform.position = teleportPosition[1].transform.position;
        if (Input.GetKeyDown(KeyCode.F2))
            transform.position = teleportPosition[2].transform.position;
        if (Input.GetKeyDown(KeyCode.F3))
            transform.position = teleportPosition[3].transform.position;
        if (Input.GetKeyDown(KeyCode.F4))
            transform.position = teleportPosition[4].transform.position;
        if (Input.GetKeyDown(KeyCode.F5))
            transform.position = teleportPosition[5].transform.position;
        if (Input.GetKeyDown(KeyCode.F6))
            transform.position = teleportPosition[6].transform.position;
        if (Input.GetKeyDown(KeyCode.F7))
            transform.position = teleportPosition[7].transform.position;
        if (Input.GetKeyDown(KeyCode.F8))
            transform.position = teleportPosition[8].transform.position;
        if (Input.GetKeyDown(KeyCode.F9))
            transform.position = teleportPosition[9].transform.position;
        if (Input.GetKeyDown(KeyCode.F10))
            transform.position = teleportPosition[10].transform.position;
        if (Input.GetKeyDown(KeyCode.F11))
            transform.position = teleportPosition[11].transform.position;
        if (Input.GetKeyDown(KeyCode.F12))
            transform.position = teleportPosition[12].transform.position;

    }
}
